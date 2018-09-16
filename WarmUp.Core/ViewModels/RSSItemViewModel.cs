using System;
using WarmUp.Core.Models.Interface;

namespace WarmUp.Core.ViewModels
{
    public class RSSItemViewModel : IRSS
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string Image { get; set; }

    }
}