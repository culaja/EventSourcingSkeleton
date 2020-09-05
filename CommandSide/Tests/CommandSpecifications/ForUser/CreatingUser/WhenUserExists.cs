using System;
using System.Collections.Generic;
using Domain;
using Domain.ForUser;
using Domain.ForUser.Commands;
using FluentAssertions;
using Framework;
using Framework.Commanding;
using static Domain.Errors.General;
using static Shared.UserEvents;
using static Tests.Values;

namespace Tests.CommandSpecifications.ForUser.CreatingUser
{
    public class WhenUserExists : CommandSpecificationFor<CreateUser>
    {
        protected override IReadOnlyList<IDomainEvent> WhenGiven => Events(
            JohnDoeUserCreated);
        protected override CreateUser AfterExecuting => new CreateUser(JohnDoeUserId);
        protected override ICommandHandler By() => StubbedCommandHandlerContainerUsing(Store);

        protected override IReadOnlyList<Action> Outcome => Is(
            () => Result.Should().BeFailureWith(AggregateAlreadyInStore(JohnDoeUserId)),
            () => ProducedEvents.Should().NotContain(EventOf<UserCreated>()));
    }
}