using System;
using Framework;
using Framework.Commanding;
using static Shared.UserEvents;

namespace Domain.ForUser
{
    public sealed class User : AggregateRoot
    {
        private UserId UserId => UserId.Of(Id.Id);
        
        public static User NewWith(UserId userId)
        {
            var user = new User();
            user.ApplyChange(new UserCreated(userId, userId));
            return user;
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case UserCreated userCreated: 
                    SetIdentity(UserId.Of(userCreated.AggregateId));
                    break;
                default:
                    throw new NotSupportedException($"{domainEvent.GetType().Name} is not supported in aggregate '{nameof(User)}'");
            }
        }
    }
}