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

        public async Task AddScrumMasterOptions()
        {
            if (DbUser.TeamRole.RoleId == 1)
            {
                await Clients.Caller.SendAsync("EnableScrumMasterOptions");
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
