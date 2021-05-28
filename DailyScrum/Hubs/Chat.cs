using DailyScrum.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        public string UserFullName => $"{DbUser.LastName} {DbUser.FirstName}";

        public string GetUserPhoto
        {
            get
            {
                var photo = "https://avios.pl/wp-content/uploads/2018/01/no-avatar.png";
                if (DbUser.PhotoPath != null)
                {
                    photo = DbUser.PhotoPath;
                }
                return photo;
            }
        }

        public async Task GetMessages()
        {
            if (TeamModel == null)
            {
                return;
            }

            var messages = _messageRepository.GetMessageHistory(DbUser.TeamMember.Name);

            foreach (var item in messages)
            {
                if (item.FromUser?.Id == DbUser.Id)
                {
                    await Clients.Caller.SendAsync("GenerateShowSentMessage", UserFullName, item.Content, item.Date);
                }
                else
                {
                    var person = TeamModel.UsersList.Where(x => x.Id == item.FromUser?.Id).FirstOrDefault();

                    var photo = "https://avios.pl/wp-content/uploads/2018/01/no-avatar.png";
                    if (person?.PhotoPath != null)
                    {
                        photo = person.PhotoPath;
                    }

                    await Clients.Caller.SendAsync("GenerateSendMessageToGroup", $"{person?.LastName} {person?.FirstName}", item?.Content, item?.Date, photo);
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

            var mes = _messageRepository.CreateNewMessage(DbUser.TeamMember.TeamId, message, DbUser.Id, DateTime.UtcNow);

            await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("SendMessageToGroup", UserFullName, mes.Content, mes.Date, GetUserPhoto);
            await Clients.Caller.SendAsync("ShowSentMessage", UserFullName, mes.Content, mes.Date);

            await AddNotification("chat");
        }
    }
}
