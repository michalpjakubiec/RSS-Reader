using System.Collections.Generic;
using System.Linq;
using WarmUp.Core.DB.Interfaces;
using WarmUp.Core.Models;
using WarmUp.Core.Services.Interfaces;
using WarmUp.Core.ViewModels;

namespace WarmUp.Core
{
    public class RSSService : IRSSService
    {
        private IUnitOfWork _uow;
        private readonly string _url;
        private IRSSPageParser _pageParser;
        private IRSSReader _rssReader;

        public RSSService(IRSSPageParser pageParser, IRSSReader rSSReader, IUnitOfWork uow, string url)
        {
            _pageParser = pageParser;
            _rssReader = rSSReader;
            _uow = uow;
            _url = url;
        }

        public ICollection<RSSChannelViewModel> GetAllRSS()
        {
            UpadteData();
            var channels = _uow.RSSChannel.All().ToList();
            var channelsVM = new List<RSSChannelViewModel>();
            foreach (var channel in channels)
            {
                var rssItemList = new List<RSSItemViewModel>(channel.RSSItems.Select(y => new RSSItemViewModel
                {
                    Link = y.Link,
                    Title = y.Title,
                    Description = y.Description,
                    Author = y.Author,
                    Image = y.Image,
                    PublishDate = y.PublishDate
                }));
                channelsVM.Add(
                     new RSSChannelViewModel
                     {
                         Title = channel.Title,
                         Description = channel.Description,
                         Link = channel.Link,
                         RSSItems = rssItemList
                     });
            }
            return channelsVM;
        }

        public void ParsePageAndSaveRSSChannels()
        {
            var pagerrChannels = _pageParser
                .ReadFromPageUrl(_url)
                .Where(x => !_uow.RSSChannel.All().Any(y => y.Link == x.Link));
            foreach (var channel in pagerrChannels)
            {
                _uow.RSSChannel.Add(channel);
            }
        }

        public void ReadRSSItemsFromChannelAndSaveItToDb()
        {
            var channels = _uow.RSSChannel.All().ToList();
            foreach (var channel in channels)
            {
                channel.RSSItems = _rssReader
                    .GetRSSItemsFromRSSChannel(channel)
                    .Where(item => channel.RSSItems.All(exItem => exItem.Link != item.Link)).ToList();
                _uow.RSSChannel.Update(channel);
            }

            UpdateChannelTitle(channels);

            _uow.Commit();
        }

        private void UpdateChannelTitle(List<RSSChannel> channels)
        {
            for (int i = 0; i < channels.Count; i++)
                channels[i] = _rssReader.GetRSSChannelInfo(channels[i]);
        }

        public void UpadteData()
        {
            ParsePageAndSaveRSSChannels();
            ReadRSSItemsFromChannelAndSaveItToDb();
        }
    }
}
