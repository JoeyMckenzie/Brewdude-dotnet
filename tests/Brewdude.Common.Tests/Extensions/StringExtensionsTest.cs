using System;
using System.IO;
using Brewdude.Common.Extensions;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Brewdude.Common.Tests.Extensions
{
    public class StringExtensionsTest
    {
        private readonly string _validJson;

        public StringExtensionsTest()
        {
            var filePath = Environment.CurrentDirectory + "/Extensions/userLoginStub.json";
            using (var streamReader = new StreamReader(filePath))
            {
                var rawJsonString = streamReader.ReadToEnd();
                _validJson = JsonConvert.SerializeObject(rawJsonString);
            }
        }

        [Fact]
        public void IsValidJson_GivenProperlyStructuredJsonString_ReturnsTrue()
        {
            // Act
            var result = _validJson.IsValidJson();

            // Assert
            result.ShouldBeOfType<bool>();
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsValidJson_GivenAnyNonJsonFormattedString_ReturnsFalse()
        {
            // Arrange
            const string someRandomString = "No stairway? Denied!";
            const string anotherRandomString = "Pardon me, do you have any Grey Poupon?";
            const string yetAnotherRandomString = "{hellllllllllllllllllllooooooooooooooooo from the outsiiiiiiiiiiiide}";
            
            // Act
            var resultSomeRandomString = someRandomString.IsValidJson();
            var resultAnotherRandomString = anotherRandomString.IsValidJson();
            var resultYetAnotherRandomString = yetAnotherRandomString.IsValidJson();
            
            // Assert
            resultSomeRandomString.ShouldBeOfType<bool>();
            resultAnotherRandomString.ShouldBeOfType<bool>();
            resultYetAnotherRandomString.ShouldBeOfType<bool>();
            
            resultSomeRandomString.ShouldBeFalse();
            resultAnotherRandomString.ShouldBeFalse();
            resultYetAnotherRandomString.ShouldBeFalse();
        }
    }
}