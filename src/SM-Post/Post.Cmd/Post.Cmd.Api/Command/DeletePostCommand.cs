using CQRS.Core.Command;

namespace Post.Cmd.Api.Command
{
    public class DeletePostCommand:BaseCommand
    {
        public string UserName { get; set; } = string.Empty;
    }
}
