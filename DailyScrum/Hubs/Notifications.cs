using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        public async Task AddNotification(string notiFrom)
        {
            foreach (var item in TeamModel.UsersNotification)
            {
                switch (notiFrom)
                {
                    case "daily":
                        {
                            item.MeetingNotify = true;
                            await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("Notification", notiFrom, "meetingBellNotifyDaily");
                            break;
                        }
                    case "chat":
                        {
                            item.ChatNotify = true;
                            await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("Notification", notiFrom, "meetingBellNotifyChat");
                            break;
                        }
                    case "problem":
                        {
                            item.ProblemNotify = true;
                            await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("Notification", notiFrom, "meetingBellNotifyProblem");
                            break;
                        }
                    case null:
                    default:
                        break;
                }
            }
        }

        public async Task RemoveNotification(string from)
        {
            var index = TeamModel.UsersList
                .IndexOf(TeamModel.UsersList.Where(x => x.Email.Equals(DbUser.Email))
                .FirstOrDefault());

            var usersNotifications = TeamModel.UsersNotification.ElementAt(index);

            switch (from)
            {
                case "daily":
                    {
                        usersNotifications.MeetingNotify = false;
                        break;
                    }
                case "chat":
                    {
                        usersNotifications.ChatNotify = false;
                        break;
                    }
                case "problem":
                    {
                        usersNotifications.ProblemNotify = false;
                        break;
                    }
                case null:
                default:
                    break;
            }
        }

        public async Task DisplayNotifications()
        {
            var index = TeamModel.UsersList
                .IndexOf(TeamModel.UsersList.Where(x => x.Email.Equals(DbUser.Email))
                .FirstOrDefault());

            var usersNotifications = TeamModel.UsersNotification.ElementAt(index);

            if (usersNotifications.MeetingNotify)
            {
                await Clients.Caller.SendAsync("Notification", "daily", "meetingBellNotifyDaily");
            }

            if (usersNotifications.ChatNotify)
            {
                await Clients.Caller.SendAsync("Notification", "chat", "meetingBellNotifyChat");
            }

            if (usersNotifications.ProblemNotify)
            {
                await Clients.Caller.SendAsync("Notification", "problem", "meetingBellNotifyProblem");
            }
        }
    }
}
