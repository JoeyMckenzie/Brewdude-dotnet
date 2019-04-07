using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Brewdude.Persistence;
using Newtonsoft.Json;

namespace Brewdude.Web.Tests.Common
{
    public class Utilities
    {
        public static void InitializeDbContexts(BrewdudeDbContext context, BrewdudeDbIdentityContext identityContext)
        {
            BrewdudeDbInitializer.Initialize(context, identityContext);
        }
        
        public static StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public static async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

    }
}