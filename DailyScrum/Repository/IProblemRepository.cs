using DailyScrum.Models.Database;
using System.Collections.Generic;

namespace DailyScrum.Repository
{
    public interface IProblemRepository
    {
        IEnumerable<Problem> GetAllActiveProblems(int teamId);
    }
}
