using System.Linq;
using Brewdude.Jwt;
using Shouldly;
using Xunit;

namespace Brewdude.Security.Tests
{
    public class BrewdudeScopesTest
    {
        [Fact]
        public void GetAllScopes_WhenCalled_ReturnsSetOfAllScopes()
        {
            // Arrange
            var brewdudeScopes = new BrewdudeScopes();
            
            // Act
            var scopes = brewdudeScopes.GetAllScopes().ToList();

            // Assert
            scopes.ShouldNotBeEmpty();
            scopes.Count.ShouldBe(8);
        }
        
        [Fact]
        public void GetReadScopes_WhenCalled_ReturnsSetOfReadScopes()
        {
            // Arrange
            var brewdudeScopes = new BrewdudeScopes();
            
            // Act
            var readScopes = brewdudeScopes.GetReadScopes().ToList();

            // Assert
            readScopes.ShouldNotBeEmpty();
            readScopes.Count.ShouldBe(4);
            foreach (var scope in readScopes)
                scope.ShouldNotContain("write");
        }
        
        [Fact]
        public void GetWriteScopes_WhenCalled_ReturnsSetOfWriteScopes()
        {
            // Arrange
            var brewdudeScopes = new BrewdudeScopes();
            
            // Act
            var writeScopes = brewdudeScopes.GetWriteScopes().ToList();

            // Assert
            writeScopes.ShouldNotBeEmpty();
            writeScopes.Count.ShouldBe(4);
            foreach (var scope in writeScopes)
                scope.ShouldNotContain("read");
        }
    }
}