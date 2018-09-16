using System.Collections.Generic;

namespace WarmUp.Core.Model
{
    public interface IRSSChannel
    {
        string Title { get; set; }
        string Description { get; set; }
        string Link { get; set; }
        List<RSSItem> RSSItemsList { get; set; }
    }
}