using DailyScrum.Models.Database;
using System;

namespace DailyScrum.Models.DbLogic
{
    public interface IDatabaseTeam
    {
        Team GetTeam(string name);
        void CreateTeam(string name, DateTime dailyTime, string creatorName);
        void DeleteTeam(string name);
        //void UpdateTeam(string name, DateTime dailyTime);
    }
}
