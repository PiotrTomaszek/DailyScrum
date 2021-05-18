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

        public void CreateDailyMeeting(int teamId)
        {
            throw new NotImplementedException();
        }

        public List<DailyMeeting> GetAllMeetings(string userName)
        {
            var teamId = _userRepository.GetTeamId(userName);

            var meetings = _context.DailyMeetings
                .Include(a => a.Team)
                .Where(x => x.Team.TeamId == teamId)
                .ToList();

            return meetings;
        }

        public DailyMeeting GetMeeting(string userName, int key)
        {
            var teamId = _userRepository.GetTeamId(userName);

            var meeting = _context.DailyMeetings
                .Include(a => a.Team)
                .Where(x => x.Team.TeamId == teamId && x.DailyMeetingId == key)
                .FirstOrDefault();

            return meeting;
        }
    }
}
