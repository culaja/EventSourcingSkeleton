using System.Collections.Generic;
using System.Linq;
using Framework;

namespace ApplicationServices.ProjectionDefinitions.ForUser
{
    public sealed class CreatedUsersView : ValueObject
    {
        private readonly HashSet<string> _usersHashSet;

        private CreatedUsersView(HashSet<string> usersHashSet)
        {
            _usersHashSet = usersHashSet;
        }
        

        public IReadOnlyList<string> Users => _usersHashSet.ToList();
        
        public static CreatedUsersView New() => new CreatedUsersView(new HashSet<string>());

        public CreatedUsersView AddUser(string userId) => 
            new CreatedUsersView(new HashSet<string>(_usersHashSet) {userId});

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var item in _usersHashSet) yield return item;
        }
    }
}