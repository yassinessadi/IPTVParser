using IPTV_Parser.Models;
using System.Text.RegularExpressions;

namespace IPTV_Parser.Services
{
    public class ParserService : IParserService
    {
        HttpClient _httpClient;
        public ParserService(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }
        public async Task<IEnumerable<ChannelModel>> GetChannelsAsync(string url)
        {
            List<ChannelModel> channels = new();
            var Url = new Uri(url);
            using (_httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = Url;
                var result = await _httpClient.GetAsync(url);
                if (!result.IsSuccessStatusCode)
                {
                    return channels;
                }
                var data = await result.Content.ReadAsStringAsync();
                string pattern = @"\btvg-name=""([^""]+)"".*\r?\n(https?\S+)";
                string patternLogo = @"\btvg-logo=""([^""]+)"".*\r?\n(https?\S+)";
                foreach (Match item in Regex.Matches(data, pattern))
                {
                    var All = item.Groups[0].Value;
                    var Img = Regex.Match(All, patternLogo);
                    //List of tv
                    var Logo = Img.Groups[1].Value;
                    var Name = item.Groups[1].Value;
                    var Link = item.Groups[2].Value;
                    channels.Add(new ChannelModel
                    {
                        Name = Name,
                        Link = Link,
                        Logo = Logo,
                    });
                }
            }
            return channels;
        }
    }
}
