using System;
using System.Data.Entity.Validation;
using WarmUp.Core.DB.Interfaces;
using WarmUp.Core.Models;

namespace WarmUp.Core.DB
{
    public class UnitOfWrok : IUnitOfWork
    {
        private RSSContext _context;
        public IRepository<RSSChannel> RSSChannel { get; set; }
        public IRepository<RSSItem> RSSItem { get; set; }

        public UnitOfWrok(RSSContext context)
        {
            _context = context;
        }

        public UnitOfWrok(RSSContext context, IRepository<RSSChannel> rssChannel, IRepository<RSSItem> rssItem)
        {
            _context = context;
            RSSChannel = rssChannel;
            RSSItem = rssItem;
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}