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
        private static Dictionary<string, NotificationViewModel> _usersNotifications = new Dictionary<string, NotificationViewModel>();

        public async Task AddNotification(string notiFrom)
        {
            foreach (var item in _usersNotifications)
            {
                switch (notiFrom)
                {
                    case "Daily":
                        {
                            item.Value.MeetingNotify = true;
                            break;
                        }
                    case "Chat":
                        {
                            item.Value.ChatNotify = true;
                            break;
                        }
                    case "Problem":
                        {
                            item.Value.ProblemNotify = true;
                            break;
                        }
                    case null:
                    default:
                        break;
                }
            }

            await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("NotifyUsers", notiFrom, "test");
        }


    }
}
