using CQRS.Core.Command;

namespace Post.Cmd.Api.Command
{
    public class AddCommentCommand:BaseCommand
    {
        public string Comment { get; set; }
        public string UserName { get;set; }
    }
}
