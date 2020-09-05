using System.Threading.Tasks;
using FluentAssertions;
using Shared;
using Xunit;

namespace Tests.IntegrationTests.InMemoryProjections
{
    public sealed class CreatedUsersViewProjectionTests : ProjectionTests
    {
        [Fact]
        public async Task when_no_events_are_applied_view_exists()
        {
            var optional = await CreatedUsersViewProjection.Fetch(string.Empty);
            optional.HasValue.Should().BeTrue();
        }

        [Fact]
        public async Task when_no_events_are_applied_view_returns_empty_list()
        {
            var optional = await CreatedUsersViewProjection.Fetch(string.Empty);
            optional.Value.Users.Should().BeEmpty();
        }
        
        [Fact]
        public async Task when_user_is_created_it_is_present_in_view()
        {
            await Apply(new UserEvents.UserCreated("JohnDoe"));
            
            var optional = await CreatedUsersViewProjection.Fetch(string.Empty);
            
            optional.Value.Users.Should().Contain("JohnDoe");
        } 
        
        [Fact]
        public async Task when_same_user_is_created_two_times_it_is_not_duplicated_in_view()
        {
            await Apply(new UserEvents.UserCreated("JohnDoe"));
            await Apply(new UserEvents.UserCreated("JohnDoe"));
            
            var optional = await CreatedUsersViewProjection.Fetch(string.Empty);
            
            optional.Value.Users.Should().ContainSingle("JohnDoe");
        }
        
        [Fact]
        public async Task when_two_users_are_created_both_are_present_in_view()
        {
            await Apply(new UserEvents.UserCreated("JohnDoe"));
            await Apply(new UserEvents.UserCreated("JaneDoe"));
            
            var optional = await CreatedUsersViewProjection.Fetch(string.Empty);
            
            optional.Value.Users.Should().Contain("JohnDoe", "JaneDoe");
        }
    }
}