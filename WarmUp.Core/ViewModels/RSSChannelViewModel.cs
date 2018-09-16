using System.Collections.Generic;
using WarmUp.Core.Models;
using WarmUp.Core.Models.Interface;

namespace WarmUp.Core.ViewModels
{
    public class RSSChannelViewModel : IRSS
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<RSSItemViewModel> RSSItems { get; set; }

    }
}