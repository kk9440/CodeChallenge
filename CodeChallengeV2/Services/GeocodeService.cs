using System.Net.Http;
using System.Threading.Tasks;
using CodeChallengeV2.Models;
using Newtonsoft.Json.Linq;

namespace CodeChallengeV2.Services
{
    public class GeocodeService : IGeocodeService
    {
        static readonly HttpClient client = new HttpClient();
        public async Task<string> FindCountryCode(GeocodePayload payload)
        {
            var requestStr = $"http://api.geonames.org/findNearbyPlaceNameJSON?lat={payload.Lat}&lng={payload.Long}&username=kk9440";

            HttpResponseMessage response = (await client.GetAsync(requestStr)).EnsureSuccessStatusCode();
            var parsedContent = JObject.Parse(await response.Content.ReadAsStringAsync());
            return (string) parsedContent["geonames"][0]["countryCode"];
        }
    }
}
