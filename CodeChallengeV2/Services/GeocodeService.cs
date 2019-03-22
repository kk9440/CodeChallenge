using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CodeChallengeV2.Models;
using Newtonsoft.Json.Linq;

namespace CodeChallengeV2.Services
{
    public class GeocodeService : IGeocodeService
    {
        public Task<string> FindCountryCode(GeocodePayload payload)
        {
            string requestStr = "http://api.geonames.org/findNearbyPlaceNameJSON?lat="+payload.Lat+"&lng="+payload.Long+"&username=kk9440";
            WebRequest request = WebRequest.Create(requestStr);  
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();  
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);  
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();  

            JObject jo = JObject.Parse(responseFromServer);
            response.Close();

            return Task.FromResult((string)jo["geonames"][0]["countryCode"]);
        }
    }
}
