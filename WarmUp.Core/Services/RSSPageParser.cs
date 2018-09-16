using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WarmUp.Core.Models;
using WarmUp.Core.Services.Interfaces;

namespace WarmUp.Core.Services
{
    public class RSSPageParser : IRSSPageParser
    {
        public ICollection<RSSChannel> ReadFromPageUrl(string url)
        {
            var hw = new HtmlWeb();
            var htmlDocument = hw.Load(url);

            var listOfRssHtmlElements = htmlDocument
                .GetElementbyId("panel_R")
                .Descendants("a")
                .Where(
                    x => x.Attributes.Contains("rel")
                         &&
                         x.GetAttributeValue("rel", "") == "nofollow");

            var channelsList = listOfRssHtmlElements.Select(x => new RSSChannel { Link = x.GetAttributeValue("href", "") }).ToList();

            var filteredChannelList = new List<RSSChannel>();

            foreach (var channel in channelsList)
            {
                try
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(channel.Link);
                    webRequest.Timeout = 500;
                    webRequest.AllowAutoRedirect = false;
                    HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                    filteredChannelList.Add(channel);
                }
                catch (System.Net.WebException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            foreach (var rssChannel in filteredChannelList)
            {
                Console.WriteLine(rssChannel.Link);
            }
            return filteredChannelList;
        }
    }
}