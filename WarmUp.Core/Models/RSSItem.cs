using System;
using WarmUp.Core.Models.Interface;

namespace WarmUp.Core.Models
{
    public class RSSItem : IRSS
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string Image { get; set; }
        public virtual RSSChannel RSSChannel { get; set; }
    }
}