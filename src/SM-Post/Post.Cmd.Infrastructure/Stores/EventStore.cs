using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.Extensions.Logging;
using Post.Cmd.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        public EventStore(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }
        public async Task<List<BaseEvent>> GetEventAsync(Guid aggregateId)
        {
           var eventStream= await _eventStoreRepository.FindByAggregateById(aggregateId);
            if (eventStream == null || !eventStream.Any())
            {
                throw new AggregateNotFoundException("Incorrect Post Id provided");
            }
            return eventStream.OrderBy(x => x.Version).Select(x=>x.EventData).ToList();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateById(aggregateId);
            if (expectedVersion != -1 && eventStream[^1].Version !=expectedVersion)
            {
                throw new AggregateNotFoundException("Incorrect Post Id provided");
            }
            var version = expectedVersion;
            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType=@event.GetType().Name;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(PostAgreggate),
                    EventData = @event,

                };
              await  _eventStoreRepository.SaveAsync(eventModel);
            }
        }
    }
}
