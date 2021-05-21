using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        public async Task DeleteTeam()
        {
            var members = TeamModel.UsersList;

            _teamRepository.DeleteTeam(DbUser.TeamMember.Name);

            foreach (var item in members)
            {
                // na sztuke dziala
                var member = _connectedUsers
                    .Where(x => x.Value.UserName == item.UserName)
                    .FirstOrDefault();

                await Clients.Client(member.Key).SendAsync("RefreshNoTeam");
            }
        }

    }
}
