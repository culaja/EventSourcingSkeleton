using Framework.Commanding;

namespace Domain.ForUser
{
    public sealed class UserId : AggregateId<User>
    {
        private UserId(string name) : base(name)
        {
        }
        
        public static UserId Of(string name) => new UserId(name);
    }
}