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


        public async Task EnableSubmitButton()
        {
            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var teamModel);

            if (teamModel != null)
            {
                var find = teamModel.DailyPosts.Where(x => x.FromUser == DbUser.Id).FirstOrDefault();

                if (find == null)
                {
                    await Clients.Caller.SendAsync("EnableSubmitPostButton");
                }
            }
        }

        public async Task EndDailyMeeting()
        {
            var teamModel = TeamModel;

                teamModel.IsDailyStarted = false;
            if (!teamModel.IsDailyStarted)
            {

                foreach (var item in teamModel.UsersList)
                {
                    var conn = _connectedUsers.Where(x => x.Value.Id == item.Id).FirstOrDefault().Key;
                    await SetEnabledOptions();
                }

                //await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("EndDaily");
            }
        }

        public async Task GetDailyOptions()
        {
            var model = TeamModel;

            await Clients.Caller.SendAsync("EnabledOptions", model.IsDailyStarted, DbUser.TeamRole.Name);
        }

        public async Task StartDailyMeeting()
        {
            var teamModel = TeamModel;

            if (!teamModel.IsDailyStarted)
            {
                teamModel.IsDailyStarted = true;

                await SetEnabledOptions();

                await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("StartDaily");
            }
        }

        public async Task SetEnabledOptions()
        {
            var teamModel = TeamModel;

            foreach (var item in teamModel.UsersList)
            {
                var conn = _connectedUsers.Where(x => x.Value.Id == item.Id).FirstOrDefault().Key;
                await Clients.Client(conn).SendAsync("EnabledOptions", teamModel.IsDailyStarted, item.TeamRole.Name);
            }

        }

        public async Task AddDailyOptions()
        {
            if (DbUser.TeamRole.RoleId == 1)
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
            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var teamModel);

            if (teamModel != null)
            {
                foreach (var item in teamModel.DailyPosts)
                {
                    var person = teamModel.UsersList.Where(x => x.Id == item.FromUser).FirstOrDefault();

                    await Clients.Caller.SendAsync("SendDailyPost", $"{person.LastName} {person.FirstName}", item.FirstQuestion, item.SecondQuestion, item.ThirdQuestion, item.Date.ToShortTimeString(), item.FromUser, person.PhotoPath ?? "no-avatar.jpg");
                }
            }
        }

        public async Task SendPost(string yesterday, string today, string problem)
        {
            var time = DateTime.UtcNow.ToShortTimeString();

            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var teamModel);

            if (teamModel != null)
            {
                var newDailyPost = new DailyPostViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstQuestion = yesterday,
                    SecondQuestion = today,
                    ThirdQuestion = problem,
                    FromUser = DbUser.Id,
                    Date = DateTime.UtcNow
                };

                teamModel.DailyPosts.Add(newDailyPost);
            }

            await Clients.Group(DbUser.TeamMember.Name).SendAsync("SendDailyPost", UserFullName, yesterday, today, problem, time, DbUser.Id, DbUser.PhotoPath ?? "no-avatar.jpg");
        }
    }
}
