using DailyScrum.Extensions;
using DailyScrum.Models.Database;
using DailyScrum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        public TeamViewModel TeamModel
        {
            get
            {
                _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var teamModel);
                return teamModel;
            }
        }

        public async Task StartDailyMeeting()
        {
            if (!TeamModel.IsDailyStarted)
            {
                TeamModel.IsDailyStarted = true;
                TeamModel.DailyMeeting = _dailyRepository.CreateDailyMeeting(DbUser.TeamMember.TeamId);

                await SetEnabledOptions();

                await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("StartDaily");
                await Clients.Group(DbUser.TeamMember.Name).SendAsync("ResetDailyBoard");
                await Clients.Group(DbUser.TeamMember.Name).SendAsync("EnableSubmitPostButton", TeamModel.IsDailyStarted);

                TeamModel.MeetingStartingTime = TeamModel.DailyMeeting.Date;

                await DisplayStartTime();
                await SetUpTimer();
            }
        }

        public async Task EndDailyMeeting()
        {
            TeamModel.IsDailyStarted = false;

            if (!TeamModel.IsDailyStarted)
            {
                _dailyRepository.EndDailyMeeting(TeamModel.DailyMeeting.DailyMeetingId);

                // po co ja tutaj iteruje?
                foreach (var item in TeamModel.UsersList)
                {
                    var conn = _connectedUsers.Where(x => x.Value.Id == item.Id).FirstOrDefault().Key;
                    await SetEnabledOptions();

                    if (conn != null)
                    {
                        await Clients.Client(conn).SendAsync("EndDaily");
                        await Clients.Client(conn).SendAsync("EnableSubmitPostButton", TeamModel.IsDailyStarted);
                    }
                }

                await DisableTimer();
            }
        }


        public async Task AddDailyOptions()
        {
            if (_userRepository.CheckIfScrumMaster(DbUser.UserName))
            {
                await Clients.Caller.SendAsync("GenScrumMasterOptions");
            }
            else
            {
                await Clients.Caller.SendAsync("GenDevOptions");
            }

            var time = new DateTime();

            if (TeamModel.DailyMeeting?.Date == null)
            {
                // usunieto to to universal time bo zakladam ze w bazie bedzie juz ten czas
                time = _teamRepository.GetDailyTime(DbUser.TeamMember.Name);
            }
            else
            {
                time = TeamModel.DailyMeeting.Date.ToUniversalTime();
            }

            await Clients.Caller.SendAsync("DisplayStartTime", time);
        }

        public async Task GetDailyOptions()
        {
            if (TeamModel == null)
            {
                return;
            }

            await Clients.Caller.SendAsync("EnabledOptions", TeamModel.IsDailyStarted, DbUser.TeamRole?.Name);
        }


        public async Task SetEnabledOptions()
        {
            foreach (var item in TeamModel.UsersList)
            {
                var connenctionID = _connectedUsers.Where(x => x.Value.Id == item.Id).FirstOrDefault().Key;

                if (connenctionID != null)
                {
                    await Clients.Client(connenctionID).SendAsync("EnabledOptions", TeamModel.IsDailyStarted, item.TeamRole?.Name);
                }
            }
        }

        public async Task EnableSubmitButton()
        {
            if (TeamModel == null)
            {
                return;
            }

            if (_postRepository.HasAlreadyPosted(DbUser.UserName, TeamModel.DailyMeeting?.DailyMeetingId))
            {
                await Clients.Caller.SendAsync("EnableSubmitPostButton", TeamModel.IsDailyStarted, true);
            }
            else
            {
                await Clients.Caller.SendAsync("EnableSubmitPostButton", TeamModel.IsDailyStarted, false);
            }
        }


        public async Task GetAllPosts()
        {
            if (TeamModel?.DailyMeeting != null)
            {
                var posts = _postRepository.GetAllPost(TeamModel.DailyMeeting.DailyMeetingId);

                foreach (var post in posts)
                {
                    var fullname = $"{ post.FromUser.LastName} {post.FromUser.FirstName}";

                    var photo = "https://avios.pl/wp-content/uploads/2018/01/no-avatar.png";
                    if (post.FromUser.PhotoPath != null)
                    {
                        photo = DbUser.PhotoPath;
                    }

                    await Clients.Caller.SendAsync("SendDailyPost", fullname,
                        post.FirstQuestion, post.SecondQuestion, post.ThirdQuestion,
                        post.Date.ToShortTimeString(), post.FromUser.Id,
                        photo);
                }
            }
        }

        public async Task SendPost(string yesterday, string today, string problem)
        {
            Problem problemHold = null;
            DailyPost dailyPost = null;

            if (TeamModel != null)
            {
                today = today.ReplaceHTMLTags();
                problem = problem.ReplaceHTMLTags();
                yesterday = yesterday.ReplaceHTMLTags();

                dailyPost = _postRepository.CreateDailyPost(yesterday, today, problem, DbUser, TeamModel.DailyMeeting, DateTime.Now);
                problemHold = _problemRepository.CreateProblem(DbUser.TeamMember.TeamId, TeamModel.DailyMeeting.DailyMeetingId, DbUser.Id, problem);

                await EnableSubmitButton();
            }

            var master = _userRepository.FindScrumMaster(DbUser.TeamMember.TeamId).Id;

            var SMconnectionId = _connectedUsers
                .Where(x => x.Value.Id == master)
                .FirstOrDefault().Key;

            await Clients.Client(SMconnectionId).SendAsync("SendProblem", UserFullName, DbUser.Id, problemHold.Description, DateTime.Now.ToShortTimeString(), problemHold.ProblemId, DbUser.PhotoPath);
            await AddNotification("problem");

            await Clients.Group(DbUser.TeamMember.Name).SendAsync("SendDailyPost", UserFullName, dailyPost.FirstQuestion, dailyPost.SecondQuestion, dailyPost.ThirdQuestion, dailyPost.Date.ToShortTimeString(), DbUser.Id, GetUserPhoto);
            await AddNotification("daily");
        }
    }
}
