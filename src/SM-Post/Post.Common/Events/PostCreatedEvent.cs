using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Common.Events
{
    public class PostCreatedEvent:BaseEvent
    {
        public PostCreatedEvent() : base(nameof(PostCreatedEvent))
        {
            
        }
        public string Author { get; set; } = string.Empty;
        public string Message { get; set; }= string.Empty;
        public DateTime DatePosted { get; set; }

    }
}
