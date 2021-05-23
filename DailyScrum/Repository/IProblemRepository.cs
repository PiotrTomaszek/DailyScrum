using DailyScrum.Models.Database;
using System.Collections.Generic;

namespace DailyScrum.Repository
{
    public interface IProblemRepository
    {
        Problem CreateProblem(int teamId, int meetingId, string userId, string problemContent);
        IEnumerable<Problem> GetAllActiveProblems(int teamId);

        void CompleteProblem(int id);

    }
}
