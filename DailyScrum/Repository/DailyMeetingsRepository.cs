using DailyScrum.Data;
using DailyScrum.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public class DailyMeetingsRepository : IDailyMeetingRepository
    {
        private readonly DailyScrumContext _context;
        private readonly IUserRepository _userRepository;

        public DailyMeetingsRepository(IUserRepository userRepository, DailyScrumContext context)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public DailyMeeting CreateDailyMeeting(int teamId)
        {
            var newMeeting = new DailyMeeting
            {
                Date = DateTime.UtcNow,
                HasFinished = false,
                Posts = new List<DailyPost>(),
                Problems = new List<Problem>(),
                Team = _context.Teams.Find(teamId)
            };

            _context.DailyMeetings.Add(newMeeting);
            _context.SaveChanges();

            return newMeeting;
        }

        public void EndDailyMeeting(int meetingId)
        {
            var meeting = _context.DailyMeetings
                .Where(x => x.DailyMeetingId == meetingId)
                .FirstOrDefault();

            meeting.HasFinished = true;

            _context.SaveChanges();
        }

        public DailyMeeting GetMeeting(string userName, int key)
        {
            var teamId = _userRepository.GetTeamId(userName);

            var meeting = _context.DailyMeetings
                .Include(a => a.Team)
                .Include(b => b.Posts)
                .ThenInclude(u => u.FromUser)
                .Where(x => x.Team.TeamId == teamId && x.DailyMeetingId == key)
                .FirstOrDefault();

            return meeting;
        }

        public List<DailyMeeting> GetAllMeetings(string userName)
        {
            var teamId = _userRepository.GetTeamId(userName);

            var meetings = _context.DailyMeetings
                .Include(a => a.Team)
                .Where(x => x.Team.TeamId == teamId)
                .OrderByDescending(t => t.Date)
                .ToList();

            return meetings;
        }

    }
}
