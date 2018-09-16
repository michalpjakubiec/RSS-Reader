using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using WarmUp.Core.Models;
using WarmUp.Core.Services.Interfaces;

namespace WarmUp.Core.Services
{
    public class RSSReader : IRSSReader
    {
        public ICollection<RSSItem> GetRSSItemsFromRSSChannel(RSSChannel channel)
        {
            try
            {
                var rssChannel = XElement.Load(channel.Link);

                var q = rssChannel.Descendants("item").Select(x =>
                    new RSSItem
                    {
                        Link = x.Element("link")?.Value,
                        Description = x.Element("description")?.Value,
                        Title = x.Element("title")?.Value,
                        Author = x.Element("author")?.Value,
                        PublishDate = DateTime.Parse(x.Element("pubDate")?.Value),
                        Image = x.Element("image")?.Value,
                    });
                return q.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<RSSItem>();
            }
        }

        public RSSChannel GetRSSChannelInfo(RSSChannel channel)
        {
            try
            {
                try
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(channel.Link);
                    webRequest.Timeout = 500;
                    webRequest.AllowAutoRedirect = false;
                    HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                    var rssChannel = XElement.Load(channel.Link);
                    var channelNode = rssChannel.Descendants("channel").First();

                    channel.Title = channelNode.Element("title")?.Value;
                    channel.Description = channelNode.Element("description")?.Value;
                    channel.Link = channelNode.Element("link")?.Value;
                }
                catch (System.Net.WebException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                channel.Title = "RSS Down";
                channel.Description = e.Message;
            }

            return channel;
        }
    }
}