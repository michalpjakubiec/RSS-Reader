using System.Collections.Generic;
using WarmUp.Core.Models.Interface;

namespace WarmUp.Core.Models
{
    public class RSSChannel : IRSS
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RSSItem> RSSItems { get; set; }

        public RSSChannel()
        {
            RSSItems = new HashSet<RSSItem>();
        }
    }
}
