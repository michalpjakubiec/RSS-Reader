using WarmUp.Core.Models;

namespace WarmUp.Core.DB.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<RSSChannel> RSSChannel { get; set; }
        IRepository<RSSItem> RSSItem { get; set; }
        void Commit();

    }
}