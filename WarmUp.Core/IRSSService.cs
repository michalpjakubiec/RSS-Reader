using System.Collections.Generic;
using WarmUp.Core.ViewModels;

namespace WarmUp.Core
{
    public interface IRSSService
    {
        ICollection<RSSChannelViewModel> GetAllRSS();
        void ParsePageAndSaveRSSChannels();
        void ReadRSSItemsFromChannelAndSaveItToDb();
        void UpadteData();
    }
}