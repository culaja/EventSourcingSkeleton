using System;
using System.Linq.Expressions;
using ApplicationServices;
using Domain.ForUser;
using Framework;
using Framework.Commanding;
using Ports;
using Shared;

namespace Tests
{
    public static class Values
    {
        public static ICommandHandler StubbedCommandHandlerContainerUsing(IStore store) => 
            CommandHandlerContainerBuilder.New()
                .Use(store)
                .Build();
        
        public static IDomainEventHandler StubbedDomainEventHandlerContainerUsing(IStore store) =>
            DomainEventHandlerContainerBuilder.New()
                .Use(store)
                .Build();
        
        public static readonly UserId JohnDoeUserId = UserId.Of("JohnDoe");
        
        public static readonly UserEvents.UserCreated JohnDoeUserCreated = new UserEvents.UserCreated(JohnDoeUserId);
        
        public static Expression<Func<IDomainEvent, bool>> EventOf<T>() =>
            e => e.GetType() == typeof(T);
    }
}