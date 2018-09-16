using System.Collections.Generic;
using WarmUp.Core.Models;

namespace WarmUp.Core.Services.Interfaces
{
    public interface IRSSReader
    {
        ICollection<RSSItem> GetRSSItemsFromRSSChannel(RSSChannel channel);
        RSSChannel GetRSSChannelInfo(RSSChannel channel);
    }
}