using CleanCodeTest.Infrastucture.Adapters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;


namespace CleanCodeTest.Infrastucture.Services
{
    internal class MarvelApiService
    {
        string _timeStamp;
        string _hash;
        string _token;
        HttpClient _client;
        string _marvelUrl;

        public MarvelApiService(string timeStamp, string hash, string token, HttpClient client)
        {
            _timeStamp = timeStamp;
            _hash = hash;
            _token = token;
            _client = client;
            _marvelUrl = "https://gateway.marvel.com:443/v1/public";
        }

        public ComicApiMarvelDTO getComicByID(string id)
        {
            string url = $"{_marvelUrl}/comics/{id}?ts={_timeStamp}&apikey={_token}&hash={_hash}";
            HttpResponseMessage response = _client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            string json = response.Content.ReadAsStringAsync().Result;
            dynamic jsonResponse = JsonConvert.DeserializeObject(json);
            dynamic results = jsonResponse.data.results[0];

            string description = results.description;
            string title = results.title;
            string cover = results.thumbnail.path + "." + results.thumbnail.extension;

            if (description == null || description == "")
            {
                JArray textObjectsArray = results.textObjects;
                if (textObjectsArray.Count > 0)
                {
                    description = results.textObjects[0].text;
                }
                else
                {
                    if (results.variantDescription == "")
                    {
                        description = "No description available";
                    }
                    else
                    {
                        description = results.variantDescription;
                    }
                }
            }

            return new ComicApiMarvelDTO
            {
                id = id,
                title = title,
                description = description,
                cover = cover
            };
            
        }
    }
}
