using CQRS.Core.Command;

namespace Post.Cmd.Api.Command
{
    public class EditCommentCommand:BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
