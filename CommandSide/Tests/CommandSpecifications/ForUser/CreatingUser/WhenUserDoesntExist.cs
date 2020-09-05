using System;
using System.Collections.Generic;
using Domain.ForUser.Commands;
using FluentAssertions;
using Framework;
using Framework.Commanding;
using static Tests.Values;

namespace Tests.CommandSpecifications.ForUser.CreatingUser
{
    public class WhenUserDoesntExist : CommandSpecificationFor<CreateUser>
    {
        protected override IReadOnlyList<IDomainEvent> WhenGiven => NoEvents;
        protected override CreateUser AfterExecuting => new CreateUser(JohnDoeUserId);
        protected override ICommandHandler By() => StubbedCommandHandlerContainerUsing(Store);

        protected override IReadOnlyList<Action> Outcome => Is(
            () => Result.Should().BeSuccess(),
            () => ProducedEvents.Should().Contain(JohnDoeUserCreated)
        );
    }
}