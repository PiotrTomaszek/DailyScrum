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

        public async Task GetAllMessages()
        {
            var teamModel = TeamModel;

            //if (teamModel != null)
            //{
            //    foreach (var item in teamModel.Messages)
            //    {
            //        if (item.From == DbUser.Id)
            //        {
            //            await Clients.Caller.SendAsync("ShowSentMessage", UserFullName, item.Content, item.Date.ToShortTimeString());
            //        }
            //        else
            //        {
            //            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

            //            var person = model.UsersList.Where(x => x.Id == item.From).FirstOrDefault();

            //            await Clients.Caller.SendAsync("SendMessageToGroup", $"{person.LastName} {person.FirstName}", item.Content, item.Date.ToShortTimeString(), person.PhotoPath);
            //        }
            //    }
            //}
        }

        public async Task SendMessage(string message)
        {
            var teamModel = TeamModel;

            if (teamModel == null)
            {
                return;
            }

            message = message.ReplaceHTMLTags();

            //var newMessage = new MessageViewModel
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Content = message,
            //    From = DbUser.Id,
            //    Date = DateTime.UtcNow
            //};

            //teamModel.Messages.Add(newMessage);


            await Clients.OthersInGroup(DbUser.TeamMember.Name).SendAsync("SendMessageToGroup", UserFullName, message, DateTime.UtcNow.ToShortTimeString(), DbUser.PhotoPath);
            await Clients.Caller.SendAsync("ShowSentMessage", UserFullName, message, DateTime.UtcNow.ToShortTimeString());

        }
    }
}
