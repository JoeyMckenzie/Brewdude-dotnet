using System.Collections.Generic;
using System.Globalization;

namespace Brewdude.Jwt
{
    public class BrewdudeScopes
    {
        public string ReadBeer { get; }
        public string WriteBeer { get; }
        public string ReadBrewery { get; }
        public string WriteBrewery { get; }
        public string ReadUserBeer { get; }
        public string WriteUserBeer { get; }
        public string ReadUserBrewery { get; }
        public string WriteUserBrewery { get; }

        public BrewdudeScopes()
        {
            // Construct scopes at runtime
            ReadBeer = "read:beer";
            WriteBeer = "write:beer";
            ReadBrewery = "read:brewery";
            WriteBrewery = "write:brewery";
            ReadUserBeer = "read:userbeer";
            WriteUserBeer = "write:userbeer";
            ReadUserBrewery = "read:userbrewery";
            WriteUserBrewery = "write:userbrewery";
        }

        public IEnumerable<string> GetAllScopes()
        {
            var scopes = new HashSet<string>();
            foreach (var scope in GetType().GetProperties())
            {
                scopes.Add((string) scope.GetValue(this));
            }

            return scopes;
        }

        public IEnumerable<string> GetReadScopes()
        {
            var readScopes = new HashSet<string>();
            foreach (var scope in GetType().GetProperties())
            {
                if (scope.Name.StartsWith("read", true, CultureInfo.InvariantCulture))
                    readScopes.Add((string) scope.GetValue(this));
            }

            return readScopes;
        }

        public IEnumerable<string> GetWriteScopes()
        {
            var writeScopes = new HashSet<string>();
            foreach (var scope in GetType().GetProperties())
            {
                if (scope.Name.StartsWith("write", true, CultureInfo.InvariantCulture))
                    writeScopes.Add((string) scope.GetValue(this));
            }

            return writeScopes;
        }

        public IEnumerable<string> GetUserScopes()
        {
            return new[]
            {
                ReadBeer,
                ReadBrewery,
                WriteUserBeer,
                WriteUserBrewery
            };
        }

        public IEnumerable<string> GetAdminUserScopes()
        {
            return GetAllScopes();
        }
    }
}