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
        public async Task UpdateUserInUserList()
        {

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

        public async Task RemoveTeamMember()
        {
            // do poprawy

            //var member = _teamRepository.AddNewTeamMember(userName, DbUser.TeamMember.Name);

            //var dummy = TeamModel.UsersList.FirstOrDefault(x => x.UserName.Equals(member.UserName));

            //TeamModel.UsersList.Remove(dummy);
            //TeamModel.UsersOnline.Remove(false);


            //TeamModel.UsersNotification.Add(new NotificationViewModel());
            //TeamModel.TeamMemberCount = TeamModel.UsersList.Count;
        }

        public async Task AddTeamMember(string userName)
        {
            var member = _teamRepository.AddNewTeamMember(userName, DbUser.TeamMember.Name);

            TeamModel.UsersList.Add(member);
            TeamModel.UsersOnline.Add(false);
            TeamModel.UsersNotification.Add(new NotificationViewModel());
            TeamModel.TeamMemberCount = TeamModel.UsersList.Count;

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
