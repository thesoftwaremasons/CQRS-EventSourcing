using CQRS.Core.Domain;
using CQRS.Core.Messages;
using Post.Common.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public void EditMessage(string message)
        {
            if (_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException($"The value of {nameof(message)} cannot be null or empty. Provide a valid {nameof(message)}");
            }
            RaiseEvent(new MessageUpdatedEvent { Id = _id, Message = message });
        }
        public void Apply(MessageUpdatedEvent @event)
        {
            _id=@event.Id;
        }
        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post");
            }
            RaiseEvent(new PostLikedEvent
            {
                Id = _id,

            });
        }
        public void Apply(PostLikedEvent @event)
        {
            _id=@event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post");
            }
            if (string.IsNullOrEmpty(comment))
            {
                throw new ArgumentNullException($"The value of {nameof(comment)} cannot be null or empty. Provide a valid {nameof(comment)}");
            }
            RaiseEvent(new CommentAddedEvent
            {
                Id = _id,
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now,
            });
        }
        public void Apply(CommentAddedEvent @event)
        {

            _id = @event.Id;
            _comments.Add(@event.Id,new Tuple<string,string>(@event.Comment,@event.Username));
        }

        public void EditComment(Guid commentId,string comment, string username)
        {

            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit an inactive post");
            }
            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user");
            }
            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                Username = username,
                EditDate = DateTime.Now,

            });
        }
        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.Id, new Tuple<string, string>(@event.Comment, @event.Username));
        }
        public void RemoveComment(Guid commentId, string userName)
        {
            if (_active)
            {
                throw new InvalidOperationException("You cannot remove a comment  of an inactive post");
            }
            if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user");
            }
            RaiseEvent(new CommentRemovedEvent
            {
                Id = _id,
                CommentId = commentId,
            });
        }
        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.Id);
        }

        public void DeletePost(string userName)
        {
            if (_active)
            {
                throw new InvalidOperationException("Th epost have already been removed");
            }

            if (!_author.Equals(userName,StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user");
            }
            RaiseEvent(new PostRemovedEvent
            {
                Id = _id,
            });
        }
        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }

    }
}