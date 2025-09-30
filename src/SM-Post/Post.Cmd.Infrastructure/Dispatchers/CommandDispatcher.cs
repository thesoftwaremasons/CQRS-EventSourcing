using CQRS.Core.Command;
using CQRS.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();
        public void RegisterHandler<T>(Func<T,Task> handler) where T : BaseCommand
        {
            if(_handlers.ContainsKey(typeof(T)))
            {
                throw new IndexOutOfRangeException($"Handler for command {typeof(T).Name} is already registered");
            }
            _handlers.Add(typeof(T),x=>handler((T)x));
        }

        public async Task SendAsync(BaseCommand command)
        {
          if(_handlers.TryGetValue(command.GetType(), out var handler))
            {
                await handler(command);
            }
            else
            {
                throw new ArgumentNullException(nameof(handler),"No command handler was registered");
            }
        }
    }
}
