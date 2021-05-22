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
        public async Task UpdateUserData(string firstName, string lastName, string phone)
        {
            var user = TeamModel.UsersList.FirstOrDefault(x => x.UserName.Equals(DbUser.UserName));

            if (user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.PhoneNumber = phone;
            }

            await MemberHandler();
        }


        public async Task ChangeMemberRole(string roleName)
        {
            var temp = TeamModel.UsersList.FirstOrDefault(x => x.UserName.Equals(DbUser.UserName));

            if (temp == null)
            {
                return;
            }

            var role = _userRepository.SetTeamRole(DbUser.UserName, roleName);

            temp.TeamRole = role;

            await Clients.Group(DbUser.TeamMember.Name).SendAsync("UpdateUser");

            await Clients.Caller.SendAsync("DisplayChangeRoleSucces");
            //Update list
        }

        public async Task RemoveTeamMember(string userName)
        {
            // do poprawy
            if (userName.Equals(DbUser.UserName))
            {
                return;
            }

            var member = _teamRepository.RemoveTeamMember(userName);

            //tutaj dodalem warunek
            if (member == null)
            {
                return;
            }

            var dummy = TeamModel.UsersList.FirstOrDefault(x => x.UserName.Equals(member?.UserName));

            if (dummy == null)
            {
                return;
            }

            var index = TeamModel.UsersList.IndexOf(dummy);

            TeamModel.UsersList.Remove(dummy);
            TeamModel.UsersOnline.RemoveAt(index);
            TeamModel.UsersNotification.RemoveAt(index);
            TeamModel.TeamMemberCount = TeamModel.UsersList.Count;

            TeamModel.ConnectedUsersCount = TeamModel.UsersOnline.Count(x => x == true);

            var findId = _connectedUsers.FirstOrDefault(x => x.Value.Id.Equals(dummy.Id)).Key;

            if (findId != null)
            {
                await Clients.Client(findId).SendAsync("RefreshNoTeam");
            }

            await RefreshUsersStatus();

            await MemberHandler();
        }

        public async Task RefreshUsersStatus()
        {
            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

            int index = 0;
            foreach (var item in model.UsersOnline)
            {
                var id = model.UsersList[index].Id;
                var isOnline = item;

                await Clients.Group(DbUser.TeamMember.Name).SendAsync("SetUserStatus", id, item);

                index++;
            }
        }

        public async Task AddTeamMember(string userName)
        {
            if (userName.Equals(DbUser.UserName))
            {
                return;
            }

            if (_userRepository.CheckIfHasTeam(userName))
            {
                return;
            }

            var member = _teamRepository.AddNewTeamMember(userName, DbUser.TeamMember.Name);

            TeamModel.UsersList.Add(member);
            TeamModel.UsersOnline.Add(false);
            TeamModel.UsersNotification.Add(new NotificationViewModel());
            TeamModel.TeamMemberCount = TeamModel.UsersList.Count;

            // tutaj sprawdzenie czy nie jest zalogowany a jak to to przelaczenie mu wikoku
            var findId = _connectedUsers.FirstOrDefault(x => x.Value.Id.Equals(member.Id)).Key;

            if (findId != null)
            {
                await Clients.Client(findId).SendAsync("RefreshAddToTeam");
            }

            await MemberHandler();
        }

        public async Task MemberHandler()
        {
            await Clients.Group(DbUser.TeamMember.Name).SendAsync("DisposeUserList");
            await Clients.Group(DbUser.TeamMember.Name).SendAsync("UpdateUserList", TeamModel.ConnectedUsersCount, TeamModel.UsersList.Count);

            foreach (var item in TeamModel.UsersList)
            {
                var photo = item.PhotoPath == null ? "no-avatar.jpg" : item.PhotoPath;

                await Clients.Group(DbUser.TeamMember.Name)
                    .SendAsync("UserConnected", $"{item.FirstName} {item.LastName}", item.Email, item.Id, photo, item.TeamRole?.Name);
            }

            await SetAllUsersStatusForTeam();
        }

        public async Task DeleteTeam()
        {
            var members = TeamModel.UsersList;

            _teamRepository.DeleteTeam(DbUser.TeamMember.Name);

            foreach (var item in members)
            {
                // na sztuke dziala
                var member = _connectedUsers
                    .Where(x => x.Value.UserName == item.UserName)
                    .FirstOrDefault();

                await Clients.Client(member.Key).SendAsync("RefreshNoTeam");
            }
        }
    }
}
