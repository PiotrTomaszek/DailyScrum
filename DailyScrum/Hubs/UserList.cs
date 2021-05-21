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
        private async Task HandleNewTeam()
        {
            if (!_connectedTeams.ContainsKey(DbUser.TeamMember?.Name))
            {
                var teamMates = await _dbContext.Users
                    .Include(x => x.TeamMember)
                    .Include(r => r.TeamRole)
                    .Where(a => a.TeamMember.Name.Equals(DbUser.TeamMember.Name)).OrderBy(order => order.LastName).ToListAsync();

                var teamModel = new TeamViewModel
                {
                    UsersList = teamMates,
                    TeamMemberCount = teamMates.Count(),
                    UsersOnline = Enumerable.Repeat(false, teamMates.Count()).ToList(),
                    UsersNotification = Enumerable.Repeat(new NotificationViewModel(), teamMates.Count()).ToList(),
                    //Messages = new List<MessageViewModel>(),
                    //DailyPosts = new List<DailyPostViewModel>(),
                    //MeetingStartingTime = new TimeSpan(11, 40, 00),
                    IsDailyStarted = false
                };

                _connectedTeams.Add(DbUser.TeamMember.Name, teamModel);
            }
        }

        private async Task ShowTeamName(string teamName)
        {
            await Clients.Caller.SendAsync("DisplayTeamName", teamName);
        }

        private async Task HandleTeamMemberNumber(int operation)
        {
            //porblem bo nie ma zespolu

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
            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

            int index = 0;
            foreach (var item in model.UsersOnline)
            {
                var id = model.UsersList[index].Id;
                var isOnline = item;

                await Clients.Caller.SendAsync("SetUserStatus", id, item);

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
            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

            foreach (var item in model.UsersList)
            {
                var photo = item.PhotoPath == null ? "no-avatar.jpg" : item.PhotoPath;

                await Clients.Caller.SendAsync("UserConnected", $"{item.FirstName} {item.LastName}", item.Email, item.Id, photo, item.TeamRole?.Name);
            }
        }
        public async Task UpdateUserList(string teamName)
        {
            _connectedTeams.TryGetValue(teamName, out TeamViewModel model);
            await Clients.Group(teamName).SendAsync("UpdateUserList", model.ConnectedUsersCount, model.TeamMemberCount);
        }

        public async Task GenerateConnectedUsers(string teamName)
        {
            _connectedTeams.TryGetValue(teamName, out TeamViewModel model);
            await Clients.Caller.SendAsync("GenerateUserCounter", model.ConnectedUsersCount, model.TeamMemberCount);
        }
    }
}
