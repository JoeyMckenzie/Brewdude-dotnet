using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Brewdude.Jwt.Services;
using Brewdude.Persistence;
using Newtonsoft.Json;

namespace Brewdude.Web.Tests.Common
{
    public class Utilities
    {
        public static void InitializeDbContexts(BrewdudeDbContext context)
        {
            BrewdudeDbInitializer.Initialize(context);
        }
        
        public static StringContent GetRequestContent(object obj)
        {
            var requestStringContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return requestStringContent;
        }

        public static async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}