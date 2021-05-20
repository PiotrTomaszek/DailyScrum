using DailyScrum.Models.Database;
using DailyScrum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
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

                // test
                var timeToEnd = TeamModel.DailyMeeting.Date.AddMinutes(1);

                //tutaj powinno zaczac sie odliczanie 
                SetUpTimer(new TimeSpan(timeToEnd.Hour, timeToEnd.Minute,timeToEnd.Second), DbUser.TeamMember.Name);
            }
        }
        
        public async Task EndDailyMeeting()
        {
            TeamModel.IsDailyStarted = false;
            if (!TeamModel.IsDailyStarted)
            {
                _dailyRepository.EndDailyMeeting(TeamModel.DailyMeeting.DailyMeetingId);

                foreach (var item in _userRepository.GetAllTeamMebers(DbUser.TeamMember.Name))
                {
                    var conn = _connectedUsers.Where(x => x.Value.Id == item.Id).FirstOrDefault().Key;
                    await SetEnabledOptions();

                    await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("EndDaily");
                }
            }
        }

        public async Task GetDailyOptions()
        {
            var model = TeamModel;

            await Clients.Caller.SendAsync("EnabledOptions", model.IsDailyStarted, DbUser.TeamRole.Name);
        }

        public async Task SetEnabledOptions()
        {
            foreach (var item in TeamModel.UsersList)
            {
                var conn = _connectedUsers.Where(x => x.Value.Id == item.Id).FirstOrDefault().Key;

                if (conn != null)
                {
                    await Clients.Client(conn).SendAsync("EnabledOptions", TeamModel.IsDailyStarted, item.TeamRole.Name);
                }
            }
        }


        // to jest chyba ok

        public async Task EnableSubmitButton()
        {
            if (TeamModel != null)
            {
                if (!_postRepository.HasAlreadyPosted(DbUser.UserName, TeamModel.DailyMeeting?.DailyMeetingId))
                {
                    await Clients.Caller.SendAsync("EnableSubmitPostButton", TeamModel.IsDailyStarted, true);
                }
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
        }

        public async Task GetAllPosts()
        {
            if (TeamModel?.DailyMeeting != null)
            {
                var posts = _postRepository.GetAllPost(TeamModel.DailyMeeting.DailyMeetingId);

                foreach (var post in posts)
                {
                    var fullname = $"{ post.FromUser.LastName} {post.FromUser.FirstName}";

                    await Clients.Caller.SendAsync("SendDailyPost", fullname,
                        post.FirstQuestion, post.SecondQuestion, post.ThirdQuestion,
                        post.Date.ToShortTimeString(), post.FromUser.Id,
                        post.FromUser.PhotoPath ?? "no-avatar.jpg");
                }
            }
        }

        public async Task SendPost(string yesterday, string today, string problem)
        {
            Problem problemHold = null;
            DailyPost dailyPost = null;

            if (TeamModel != null)
            {
                // dodanie postu do bazy
                dailyPost = _postRepository.CreateDailyPost(yesterday, today, problem, DbUser, TeamModel.DailyMeeting, DateTime.Now);
                problemHold = _problemRepository.CreateProblem(DbUser.TeamMember.TeamId, TeamModel.DailyMeeting.DailyMeetingId, DbUser.Id, problem);

                await EnableSubmitButton();
            }

            var master = _userRepository.FindScrumMaster(DbUser.TeamMember.TeamId).Id;

            var SMconnectionId = _connectedUsers
                .Where(x => x.Value.Id == master)
                .FirstOrDefault().Key;

            await Clients.Client(SMconnectionId).SendAsync("SendProblem", UserFullName, DbUser.Id, problemHold.Description, DateTime.Now.ToShortTimeString(), problemHold.ProblemId, DbUser.PhotoPath);

            await Clients.Group(DbUser.TeamMember.Name).SendAsync("SendDailyPost", UserFullName, dailyPost.FirstQuestion, dailyPost.SecondQuestion, dailyPost.ThirdQuestion, dailyPost.Date.ToShortTimeString(), DbUser.Id, DbUser.PhotoPath ?? "no-avatar.jpg");
        }
    }
}
