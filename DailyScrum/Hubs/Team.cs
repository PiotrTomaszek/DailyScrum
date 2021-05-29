using DailyScrum.Extensions;
using DailyScrum.Models;
using DailyScrum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
                    IsDailyStarted = false
                };

                _connectedTeams.Add(DbUser.TeamMember.Name, teamModel);
            }
        }

        public async Task UpdateUserData(string firstName, string lastName, string phone)
        {
            var user = TeamModel.UsersList.FirstOrDefault(x => x.UserName.Equals(DbUser.UserName));

            if (user != null)
            {
                user.FirstName = firstName.ReplaceHTMLTags();
                user.LastName = lastName.ReplaceHTMLTags();
                user.PhoneNumber = phone.ReplaceHTMLTags();
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

            if(LevensheimDistance.Compute(roleName, "Scrum Master") < 5 || LevensheimDistance.Compute(roleName, "SM") <= 2)
            {
                await Clients.Caller.SendAsync("ToastrInfoNotify", "Nazwa roli za bardzo zbliżona do roli Scrum Mastera!!!", "Błąd!");
                return;
            }

            var role = _userRepository.SetTeamRole(DbUser.UserName, roleName.ReplaceHTMLTags());

            temp.TeamRole = role;

            await Clients.Caller.SendAsync("ToastrNotify",  "Sukces!", "Zmieniłeś swoją rolę.");

            await MemberHandler();
        }

        public async Task RemoveTeamMember(string userName)
        {
            if (userName.Equals(DbUser.UserName))
            {
                //await Clients.Caller.SendAsync("ShowRemoveMessage", "Nie mozesz usunac samego siebie!");
                await Clients.Caller.SendAsync("ToastrInfoNotify", "Nie mozesz usunac samego siebie!", "Błąd!");
                return;
            }

            var member = _teamRepository.RemoveTeamMember(userName);

            if (member == null)
            {
                //await Clients.Caller.SendAsync("ShowRemoveMessage", "Nie ma takiego w zespole!");
                await Clients.Caller.SendAsync("ToastrInfoNotify", "Nie ma takiej osoby w zespole!", "Błąd!");
                return;
            }

            var dummy = TeamModel.UsersList.FirstOrDefault(x => x.UserName.Equals(member?.UserName));

            if (dummy == null)
            {
                //await Clients.Caller.SendAsync("ShowRemoveMessage", "Nie ma takiego w zespole!");
                await Clients.Caller.SendAsync("ToastrInfoNotify", "Nie ma takiej osoby w zespole!", "Błąd!");
                return;
            }

            var index = TeamModel.UsersList.IndexOf(dummy);

            TeamModel.UsersList.Remove(dummy);
            TeamModel.UsersOnline.RemoveAt(index);
            TeamModel.UsersNotification.RemoveAt(index);
            TeamModel.TeamMemberCount = TeamModel.UsersList.Count;

            TeamModel.ConnectedUsersCount = TeamModel.UsersOnline.Count(x => x == true);

          
            //await Clients.Caller.SendAsync("ShowRemoveMessage", "Sukces! Usunieto uczestnika tego spotkania.");

            var findId = _connectedUsers.FirstOrDefault(x => x.Value.Id.Equals(dummy.Id)).Key;

            if (findId != null)
            {
                await Clients.Client(findId).SendAsync("RefreshNoTeam");
            }

            await Clients.Caller.SendAsync("ToastrNotify", "Usunięto uczestnika tego spotkania!", "Sukces!");

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
                //await Clients.Caller.SendAsync("ShowAddMessage", "Dodaj innych a nie siebie.");
                await Clients.Caller.SendAsync("ToastrInfoNotify", "Nie możesz dodać samego siebie!", "Błąd!");
                return;
            }

            if (!_userRepository.CheckIfExists(userName))
            {
                //await Clients.Caller.SendAsync("ShowAddMessage", "Taki user nie istnieje.");
                await Clients.Caller.SendAsync("ToastrInfoNotify", "Taki użytkownik nie istnieje!", "Błąd!");
                return;
            }

            if (_userRepository.CheckIfHasTeam(userName))
            {
                //await Clients.Caller.SendAsync("ShowAddMessage", "Ten zawodnik juz ma team.");
                await Clients.Caller.SendAsync("ToastrInfoNotify", "Ta osoba już posiada zespół!", "Błąd!");
                return;
            }

            var member = _teamRepository.AddNewTeamMember(userName, DbUser.TeamMember.Name);

            TeamModel.UsersList.Add(member);
            TeamModel.UsersOnline.Add(false);
            TeamModel.UsersNotification.Add(new NotificationViewModel());
            TeamModel.TeamMemberCount = TeamModel.UsersList.Count;

            //await Clients.Caller.SendAsync("ShowAddMessage", "Sukces!!!");
            await Clients.Caller.SendAsync("ToastrNotify", "Dodano nowego uczestnika!", "Sukces!");

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
