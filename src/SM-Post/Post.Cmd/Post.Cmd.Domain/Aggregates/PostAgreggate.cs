using CQRS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAgreggate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }
        public PostAgreggate()
        {

        }
        public PostAgreggate(Guid id,string  author,string message)
        {
            RaiseEvent(new Post.Common.Events.PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now
            });
        }
        public void Apply(Post.Common.Events.PostCreatedEvent @event)
        {
            _id = @event.Id;
            _author = @event.Author;
            _active = true;
        }
    }
}