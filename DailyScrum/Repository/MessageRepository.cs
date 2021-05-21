using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using DailyScrum.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DailyScrum.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DailyScrumContext _context;

        public MessageRepository(DailyScrumContext context)
        {
            _context = context;
        }

        public Message CreateNewMessage(int teamId, string content, string fromUserId, DateTime date)
        {
            var team = _context.Teams
                .Include(s => s.Members)
                .ThenInclude(w => w.TeamRole)
                .Where(t => t.TeamId == teamId)
                .FirstOrDefault();

            var newMessage = new Message
            {
                //Team = team,
                Content = content,
                Date = date,
                //FromUser = from
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            newMessage.Team = team;
            _context.SaveChanges();

            newMessage.FromUser = _context.Users.Find(fromUserId);
            _context.SaveChanges();

            return newMessage;
        }

        public List<Message> GetMessageHistory(string teamName)
        {
            var messages = _context.Messages
                .Include(x => x.Team)
                .Include(z => z.FromUser)
                .Where(a => a.Team.Name.Equals(teamName))
                .OrderByDescending(order => order.Date)
                .Take(10)
                .OrderBy(order => order.Date)
                .ToList();

            return messages;
        }
    }
}
