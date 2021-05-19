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
        // pobieranie wszystkich aktulanych problemów - z bazy



        // dodawanie real time nowych problemów oraz zapis do bazy


    }
}
