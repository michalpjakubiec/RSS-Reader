using System.Collections.Generic;
using WarmUp.Core.Models;

namespace WarmUp.Core.Services.Interfaces
{
    public interface IRSSPageParser
    {
        ICollection<RSSChannel> ReadFromPageUrl(string url);

    }
}