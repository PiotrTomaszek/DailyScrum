using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using DailyScrum.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DailyScrumContext _context;

        public PostRepository(DailyScrumContext context)
        {
            _context = context;
        }

        public DailyPost CreateDailyPost(string first, string second, string third, ApplicationUser fromUser, DailyMeeting daily, DateTime date)
        {
            var newPost = new DailyPost
            {
                FirstQuestion = first,
                SecondQuestion = second,
                ThirdQuestion = third,
                FromUser = _context.Users.Find(fromUser.Id),
                Date = date,
                Meeting = _context.DailyMeetings.Find(daily.DailyMeetingId)
            };

            _context.DailyPosts.Add(newPost);
            _context.SaveChanges();

            return newPost;
        }

        public List<DailyPost> GetAllPost(int meetingId)
        {
            var posts = _context.DailyPosts
                .Include(z => z.Meeting)
                .Include(u => u.FromUser)
                .Where(x => x.Meeting.DailyMeetingId == meetingId)
                .ToList();

            return posts;
        }

        public bool HasAlreadyPosted(string userName, int? meetingId)
        {
            if(meetingId == null)
            {
                return false;
            }

            var hasPosted = _context.DailyPosts
                .Include(user => user.FromUser)
                .Include(meeting => meeting.Meeting)
                .Where(x => x.FromUser.UserName.Equals(userName) && x.Meeting.DailyMeetingId == meetingId)
                .FirstOrDefault();

            var result = hasPosted == null ? false : true;

            return result;
        }
    }
}
