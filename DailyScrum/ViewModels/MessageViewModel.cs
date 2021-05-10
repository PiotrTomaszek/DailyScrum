using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.ViewModels
{
    public class MessageViewModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        Notification,
        Message
    }
}
