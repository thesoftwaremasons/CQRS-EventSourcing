
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Api.Command;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

namespace Post.Cmd.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
            builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            builder.Services.AddScoped<IEventStore, EventStore>();
            builder.Services.AddScoped<IEventSourcingHandler<PostAgreggate>, EventSourcingHandler>();
            builder.Services.AddScoped<ICommandHandler, CommandHandler>();

            var commandHandler=builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();

            var dispatcher = new CommandDispatcher();
            dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandlerAsync);
            dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandlerAsync);
            dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandlerAsync);
            dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandlerAsync);
            dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandlerAsync);
            dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandlerAsync);
            dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandlerAsync);

            builder.Services.AddSingleton<ICommandDispatcher>(_=> dispatcher);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
