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
        // tutaj do poprawy bo bedzie dla wszystkich zepsolow
        //private static Dictionary<string, NotificationViewModel> _usersNotifications = new Dictionary<string, NotificationViewModel>();

        public async Task AddNotificationSystemToUser()
        {
            if (!_usersNotifications.ContainsKey(DbUser.UserName))
            {
                _usersNotifications.Add(DbUser.UserName, new NotificationViewModel());
            }
        }

        public async Task RemoveNotificationSystemToUser()
        {
            _usersNotifications.Remove(DbUser.UserName);
        }

        public async Task AddNotification(string notiFrom)
        {
            foreach (var item in TeamModel.UsersNotification)
            {
                switch (notiFrom)
                {
                    case "daily":
                        {
                            item.MeetingNotify = true;
                            break;
                        }
                    case "chat":
                        {
                            item.ChatNotify = true;
                            break;
                        }
                    case "problem":
                        {
                            item.Value.ProblemNotify = true;
                            break;
                        }
                    case null:
                    default:
                        break;
                }
            }

            await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("Notification", notiFrom, "meetingBellNotifyDaily");
        }

        public async Task RemoveNotification(string from)
        {
            var index = TeamModel.UsersList.IndexOf(TeamModel.UsersList.Where(x => x.Email.Equals(DbUser.Email)).FirstOrDefault());

            var usersNotifications = TeamModel.UsersNotification.ElementAt(index);

            switch (from)
            {
                case "caily":
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
            t

            foreach (var item in _usersNotifications.Where(x => x.Key.Equals(DbUser.TeamMember.Name)))
            {
                if (item.Value.ChatNotify)
                {
                    await Clients.Caller.SendAsync("Notification", "daily", "meetingBellNotifyDaily");
                }

                //...todo
            }
        }
    }
}
