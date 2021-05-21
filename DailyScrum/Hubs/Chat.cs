using DailyScrum.Extensions;
using DailyScrum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        public string UserFullName => $"{DbUser.LastName} {DbUser.FirstName}";

        public async Task GetMessages()
        {
            if (TeamModel == null)
            {
                return;
            }

            var messages = _messageRepository.GetMessageHistory(DbUser.TeamMember.Name);

            foreach (var item in messages)
            {
                if (item.FromUser.Id == DbUser.Id)
                {
                    await Clients.Caller.SendAsync("ShowSentMessage", UserFullName, item.Content, item.Date.ToShortTimeString());
                }
                else
                {
                    var person = TeamModel.UsersList.Where(x => x.Id == item.FromUser.Id).FirstOrDefault();

                    await Clients.Caller.SendAsync("SendMessageToGroup", $"{person.LastName} {person.FirstName}", item.Content, item.Date.ToShortTimeString(), person.PhotoPath);
                }
            }
        }

        public async Task SendMessage(string message)
        {
            if (TeamModel == null)
            {
                return;
            }

            message = message.ReplaceHTMLTags();

            var mes = _messageRepository.CreateNewMessage(DbUser.TeamMember, message, DbUser, DateTime.Now);

            await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("SendMessageToGroup", UserFullName, mes.Content, mes.Date.ToShortTimeString(), DbUser.PhotoPath);
            await Clients.Caller.SendAsync("ShowSentMessage", UserFullName,mes.Content, mes.Date.ToShortTimeString());

            await AddNotification("chat");
        }
    }
}
