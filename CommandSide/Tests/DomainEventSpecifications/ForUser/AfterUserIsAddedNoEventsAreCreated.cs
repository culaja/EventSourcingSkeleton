using System;
using System.Collections.Generic;
using FluentAssertions;
using Framework;
using Framework.Commanding;
using Shared;
using static Tests.Values;

namespace Tests.DomainEventSpecifications.ForUser
{
    public class AfterUserIsAddedNoEventsAreCreated : DomainEventSpecificationFor<UserEvents.UserCreated>
    {
        protected override IReadOnlyList<IDomainEvent> WhenGiven => NoEvents;
        protected override UserEvents.UserCreated AfterHandling => JohnDoeUserCreated;
        protected override IDomainEventHandler By() => StubbedDomainEventHandlerContainerUsing(Store);

        protected override IReadOnlyList<Action> Outcome => Is(
            () => Result.Should().BeSuccess(),
            () => ProducedEvents.Should().BeEmpty());
    }
}