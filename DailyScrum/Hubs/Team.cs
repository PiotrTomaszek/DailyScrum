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
        public async Task UpdatePhoto()
        {
            var user = TeamModel.UsersList.FirstOrDefault(x => x.UserName.Equals(DbUser.UserName));

            if (user != null)
            {
                user.PhotoPath = _userRepository.GetUserPhotoPath(user.UserName);
            }

            await MemberHandler();
        }

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

            // co to jest?
            //await Clients.Group(DbUser.TeamMember.Name).SendAsync("UpdateUser");

            await Clients.Caller.SendAsync("DisplayChangeRoleSucces");

            await MemberHandler();
        }

        public async Task RemoveTeamMember(string userName)
        {
            if (userName.Equals(DbUser.UserName))
            {
                await Clients.Caller.SendAsync("ShowRemoveMessage", "Nie mozesz usunac samego siebie!");
                return;
            }

            var member = _teamRepository.RemoveTeamMember(userName);

            if (member == null)
            {
                await Clients.Caller.SendAsync("ShowRemoveMessage", "Nie ma takiego w zespole!");
                return;
            }

            var dummy = TeamModel.UsersList.FirstOrDefault(x => x.UserName.Equals(member?.UserName));

            if (dummy == null)
            {
                await Clients.Caller.SendAsync("ShowRemoveMessage", "Nie ma takiego w zespole!");
                return;
            }

            var index = TeamModel.UsersList.IndexOf(dummy);

            TeamModel.UsersList.Remove(dummy);
            TeamModel.UsersOnline.RemoveAt(index);
            TeamModel.UsersNotification.RemoveAt(index);
            TeamModel.TeamMemberCount = TeamModel.UsersList.Count;

            TeamModel.ConnectedUsersCount = TeamModel.UsersOnline.Count(x => x == true);

            await Clients.Caller.SendAsync("ShowRemoveMessage", "Sukces! Usunieto uczestnika tego spotkania.");

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

        public async Task AddTeamMember(string userName)
        {
            if (userName.Equals(DbUser.UserName))
            {
                await Clients.Caller.SendAsync("ShowAddMessage", "Dodaj innych a nie siebie.");
                return;
            }

            if (!_userRepository.CheckIfExists(userName))
            {
                await Clients.Caller.SendAsync("ShowAddMessage", "Taki user nie istnieje.");
                return;
            }

            if (_userRepository.CheckIfHasTeam(userName))
            {
                await Clients.Caller.SendAsync("ShowAddMessage", "Ten zawodnik juz ma team.");
                return;
            }

            var member = _teamRepository.AddNewTeamMember(userName, DbUser.TeamMember.Name);

            TeamModel.UsersList.Add(member);
            TeamModel.UsersOnline.Add(false);
            TeamModel.UsersNotification.Add(new NotificationViewModel());
            TeamModel.TeamMemberCount = TeamModel.UsersList.Count;

            await Clients.Caller.SendAsync("ShowAddMessage", "Sukces!!!");

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
                var photo = item.PhotoPath == null ? "https://avios.pl/wp-content/uploads/2018/01/no-avatar.png" : item.PhotoPath;

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

                if (member.Key != null)
                {
                    await Clients.Client(member.Key).SendAsync("RefreshNoTeam");
                }
            }

            // test usuwania z pamieci modelu zepsolu i chyba dziala
            _connectedTeams.Remove(DbUser.TeamMember.Name);
        }
    }
}
