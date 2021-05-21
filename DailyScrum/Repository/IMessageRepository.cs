using DailyScrum.Areas.Identity.Data;
using DailyScrum.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public interface IMessageRepository
    {
        List<Message> GetMessageHistory(string teamName);
        Message CreateNewMessage(Team team, string content, ApplicationUser from, DateTime date);
    }
}
