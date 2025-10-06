
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Command
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<PostAgreggate> _eventSourcingHandler;

        public CommandHandler(IEventSourcingHandler<PostAgreggate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandlerAsync(NewPostCommand command)
        {
            var aggregate = new PostAgreggate(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(EditMessageCommand command)
        {
            var aggregate= await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(LikePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();
            await _eventSourcingHandler.SaveAsync(aggregate);

        }

        public async Task HandlerAsync(AddCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comment, command.UserName);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(EditCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment,command.UserName);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(RemoveCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.CommmentId, command.UserName);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(DeletePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.UserName);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}
