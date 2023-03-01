using IPTV_Parser.Models;

namespace IPTV_Parser.Services
{
    public interface IParserService
    {
        Task<IEnumerable<ChannelModel>> GetChannelsAsync(string url);
    }
}
