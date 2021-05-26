using DailyScrum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
      

        private async Task ShowTeamName(string teamName)
        {
            await Clients.Caller.SendAsync("DisplayTeamName", teamName);
        }

        private async Task HandleTeamMemberNumber(int operation)
        {
            if (DbUser.TeamMember?.Name != null)
            {
                var team = _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var teamModel);

                if (team)
                {
                    teamModel.ConnectedUsersCount += operation;
                }

                await UpdateUserList(DbUser.TeamMember?.Name);
            }

        }

        private async Task GetAllUsersStatus()
        {
            var model = TeamModel;

            int index = 0;
            foreach (var item in model.UsersOnline)
            {
                var id = model.UsersList[index].Id;
                var isOnline = item;

                await Clients.Caller.SendAsync("SetUserStatus", id, item);

                index++;
            }
        }

        private async Task SetAllUsersStatusForTeam()
        {
            var model = TeamModel;

            int index = 0;
            foreach (var item in model.UsersOnline)
            {
                var id = model.UsersList[index].Id;
                var isOnline = item;

                await Clients.Group(DbUser.TeamMember.Name).SendAsync("SetUserStatus", id, item);

                index++;
            }
        }

        private async Task SetUserStatus(bool isOnline)
        {
            var isOK = _connectedUsers.TryGetValue(Context.ConnectionId, out var connUser);

            if (isOK)
            {
                _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

                var index = model.UsersList.IndexOf(model.UsersList.Where(x => x.Email.Equals(connUser.Email)).FirstOrDefault());

                if (isOnline)
                {
                    model.UsersOnline[index] = true;
                }
                else
                {
                    model.UsersOnline[index] = false;
                }
            }

            await Clients.Group(DbUser.TeamMember.Name).SendAsync("SetUserStatus", DbUser.Id, isOnline);
        }

        private async Task GenerateUserList()
        {
            var model = TeamModel;

            foreach (var item in model.UsersList)
            {
                var photo = item.PhotoPath == null ? "https://avios.pl/wp-content/uploads/2018/01/no-avatar.png" : item.PhotoPath;

                await Clients.Caller.SendAsync("UserConnected", $"{item.FirstName} {item.LastName}", item.Email, item.Id, photo, item.TeamRole?.Name);
            }
        }
        public async Task UpdateUserList(string teamName)
        {
            var model = TeamModel;
            await Clients.Group(teamName).SendAsync("UpdateUserList", model.ConnectedUsersCount, model.TeamMemberCount);
        }

        public async Task GenerateConnectedUsers(string teamName)
        {
            var model = TeamModel;
            await Clients.Caller.SendAsync("GenerateUserCounter", model.ConnectedUsersCount, model.TeamMemberCount);
        }
    }
}
