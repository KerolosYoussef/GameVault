﻿
namespace Ordering.Domain.Abstractions
{
    public class Aggregate<TId> : Entity<TId>, IAggregate<TId>
    {
        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));
            _domainEvents.Add(domainEvent);
        }
        public IDomainEvent[] ClearDomainEvents()
        {
            IDomainEvent[] domainEvents = _domainEvents.ToArray();
            _domainEvents.Clear();
            return domainEvents;
        }
    }
}
