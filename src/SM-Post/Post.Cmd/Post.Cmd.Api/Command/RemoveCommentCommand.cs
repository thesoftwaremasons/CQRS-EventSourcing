using CQRS.Core.Command;

namespace Post.Cmd.Api.Command
{
    public class RemoveCommentCommand:BaseCommand
    {
        public Guid CommmentId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
