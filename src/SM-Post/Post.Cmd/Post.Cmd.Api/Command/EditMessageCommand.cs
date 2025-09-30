using CQRS.Core.Command;

namespace Post.Cmd.Api.Command
{
    public class EditMessageCommand:BaseCommand
    {
        public string Message { get; set; } = string.Empty;
    }
}
