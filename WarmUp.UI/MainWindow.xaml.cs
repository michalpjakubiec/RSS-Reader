using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WarmUp.Core;
using WarmUp.Core.DB;
using WarmUp.Core.Models;
using WarmUp.Core.Services;
using WarmUp.Core.ViewModels;

namespace WarmUp.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IRSSService _service;
        public MainWindow() {
            InitializeComponent();
            ShowChannels();
        }
        private void ShowChannels() {
            var context = new RSSContext();
            _service = new RSSService(
                new RSSPageParser(),
                new RSSReader(),
                new UnitOfWrok(context,
                    new Repository<RSSChannel>(context),
                    new Repository<RSSItem>(context)),
                "http://www.rss.lostsite.pl//index.php?rss=32");
            foreach (var item in _service.GetAllRSS()) {
                listOfChannels.Items.Add(item);
            }
        }
        private void ListViewItem_SelectedChannel(object sender, MouseButtonEventArgs e) {
            if (listOfItems.Items.Count != 0) {
                listOfItems.Items.Clear();
            }
            var channelToClick = sender as ListViewItem;
            if (channelToClick != null && channelToClick.IsSelected) {
                foreach (var item in (listOfChannels.SelectedItem as RSSChannelViewModel).RSSItems) {
                    listOfItems.Items.Add(item);
                }
            }
        }

        private void ListViewItem_SelectedItem(object sender, MouseButtonEventArgs e) {
            if (itemBox.Items.Count != 0) {
                itemBox.Items.Clear();
            }
            if (sender is ListViewItem itemToClick && itemToClick.IsSelected) {
                itemBox.Items.Add(listOfItems.SelectedItem as RSSItemViewModel);
            }
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ReloadRSS_button_Click(object sender, RoutedEventArgs e) {
            ShowChannels();
        }
    }
}
