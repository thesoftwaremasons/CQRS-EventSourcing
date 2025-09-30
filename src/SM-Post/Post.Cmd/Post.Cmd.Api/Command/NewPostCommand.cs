using CQRS.Core.Command;

namespace Post.Cmd.Api.Command
{
    public class NewPostCommand: BaseCommand
    {
        public string Author { get; set; }=string.Empty;
        public string Message { get; set; }=string.Empty;

    }
}
