using DailyScrum.Models.Database;
using System;
using System.Collections.Generic;

namespace DailyScrum.Repository
{
    public interface IMessageRepository
    {
        List<Message> GetMessageHistory(string teamName);
        Message CreateNewMessage(int teamId, string content, string fromUserId, DateTime date);
    }
}
