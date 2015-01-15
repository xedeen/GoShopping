using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using GoShopping.Code;
using GoShopping.Interactions;
using GoShopping.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GoShopping.Resources;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using Microsoft.Xna.Framework;

namespace GoShopping
{
  public partial class MainPage : PhoneApplicationPage
  {
    private ShoppingListViewModel _viewModel = ShoppingListViewModel.CreateOrLoadFromStorage();

    private InteractionManager _interactionManager = new InteractionManager();

    private PinchAddNewInteraction _pinchAddNewItemInteraction;

    //private AutoResetEvent _contactSearchEvt = new AutoResetEvent(false);
    //private Contact _foundContact;

    public string RetrievedText { get; set; }
    // Constructor
    public MainPage()
    {
      InitializeComponent();

      this.DataContext = _viewModel.Items;

      var dragReOrderInteraction = new DragReOrderInteraction(dragImageControl);
      dragReOrderInteraction.Initialise(todoList, _viewModel.Items);

      var swipeInteraction = new SwipeInteraction();
      swipeInteraction.Initialise(todoList, _viewModel.Items);

      var tapEditInteraction = new TapEditInteraction();
      tapEditInteraction.Initialise(todoList, _viewModel.Items);

      var addItemInteraction = new PullDownToAddNewInteraction(tapEditInteraction, pullDownItemInFront);
      addItemInteraction.Initialise(todoList, _viewModel.Items);

      _pinchAddNewItemInteraction = new PinchAddNewInteraction(tapEditInteraction, pullDownItemBehind);
      _pinchAddNewItemInteraction.Initialise(todoList, _viewModel.Items);

      _interactionManager.AddInteraction(swipeInteraction);
      _interactionManager.AddInteraction(dragReOrderInteraction);
      _interactionManager.AddInteraction(addItemInteraction);
      _interactionManager.AddInteraction(tapEditInteraction);
      _interactionManager.AddInteraction(_pinchAddNewItemInteraction);

      FrameworkDispatcher.Update();
    }

    // Sample code for building a localized ApplicationBar
    //private void BuildLocalizedApplicationBar()
    //{
    //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
    //    ApplicationBar = new ApplicationBar();

    //    // Create a new button and set the text value to the localized string from AppResources.
    //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
    //    appBarButton.Text = AppResources.AppBarButtonText;
    //    ApplicationBar.Buttons.Add(appBarButton);

    //    // Create a new menu item with the localized string from AppResources.
    //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
    //    ApplicationBar.MenuItems.Add(appBarMenuItem);
    //}
    private void Border_Loaded(object sender, RoutedEventArgs e)
    {
      _interactionManager.AddElement(sender as FrameworkElement);
    }

    private void RecognizeButton_OnClick(object sender, EventArgs e)
    {
      RetrievedText = string.Empty;
      _pinchAddNewItemInteraction.IsEnabled = false;
      NavigationService.Navigate(new Uri(@"/RecognizerPage.xaml", UriKind.Relative));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      ShoppingListViewModel.SaveAll(_viewModel);
      UpdateTile();
    }

    public void UpdateTile()
    {
      var iconicTile = FindTile();
      if (iconicTile != null)
      {
        iconicTile.Update(CreateIconicTileData());
      }
    }

    private ShellTile FindTile()
    {
      ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault();
      return shellTile;
    }

    private ShellTileData CreateIconicTileData()
    {
      var iconicTileData = new IconicTileData
      {
        Count = _viewModel.Items.Count(i => i.Completed == false),
        IconImage = new Uri("/Assets/Tiles/IconicTileMediumLarge.png", UriKind.Relative),
        SmallIconImage = new Uri("/Assets/Tiles/IconicTileSmall.png", UriKind.Relative)
      };
      return iconicTileData;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (!string.IsNullOrEmpty(RetrievedText))
      {
        ProcessRetrievedText();
      }
      _pinchAddNewItemInteraction.IsEnabled = true;

      base.OnNavigatedTo(e);
    }

    private void ClearButton_OnClick(object sender, EventArgs e)
    {
      if (MessageBox.Show("Вы действительно хотите очистить список?", "Подтверждение", MessageBoxButton.OKCancel) ==
          MessageBoxResult.OK)
        _viewModel.Items.Clear();
    }

    private void ProcessRetrievedText()
    {
      var source = RetrievedText.Trim().ToLowerInvariant();
      RetrievedText = string.Empty;
      var lex = LexCmd.FromString(source);

      if (null != lex)
      {
        switch (lex.CommandType)
        {
          case CommandType.Items:
            if (lex.Direction == CommandDirection.Add)
            {
              foreach (var newItem in lex.Items.Where(newItem => !_viewModel.Items.Any(item => item.Text.Equals(newItem, StringComparison.InvariantCultureIgnoreCase))))
              {
                _viewModel.Items.Add(new ShoppingItemViewModel(newItem));
              }
            }
            if (lex.Direction == CommandDirection.Remove)
            {
              foreach (var remItem in lex.Items)
              {
                var existing = _viewModel.Items.FirstOrDefault(item => item.Text.Equals(remItem, StringComparison.InvariantCultureIgnoreCase));
                if (null != existing)
                {
                  _viewModel.Items.Remove(existing);
                }
              }
            }
            break;
          case CommandType.SendMail:
            CreateEMail(lex.ContactName);
            break;
          case CommandType.SendMessage:
            CreateSms(lex.ContactName);
            break;
        }
      }
    }

    private void AboutButton_OnClick(object sender, EventArgs e)
    {
      _pinchAddNewItemInteraction.IsEnabled = false;
      NavigationService.Navigate(new Uri(@"/AboutPage.xaml", UriKind.Relative));
    }

    private void SMSSend_OnClick(object sender, EventArgs e)
    {
      CreateSms();
    }

    private void EmailSend_OnClick(object sender, EventArgs e)
    {
      CreateEMail();
    }

    private void ClipboardSend_OnClick(object sender, EventArgs e)
    {
      SendToClipBoard();
    }

    private void ClipboardGet_OnClick(object sender, EventArgs e)
    {
      if (!Clipboard.ContainsText())
      {
        MessageBox.Show("Ваш буфер обмена пуст!", "За покупками", MessageBoxButton.OK);
        return;
      }
        
      PasteBox.Text = string.Empty;
      PastePopup.IsOpen = true;
      PasteBox.Focus();
    }

    private void ButtonClipReplace_OnClick(object sender, RoutedEventArgs e)
    {
      _viewModel.Items.Clear();
      ButtonClipAdd_OnClick(sender, e);
    }

    private void ButtonClipAdd_OnClick(object sender, RoutedEventArgs e)
    {
      PastePopup.IsOpen = false;
      var items = TryParseClipboard(PasteBox.Text);
      if (null != items && items.Count > 0)
      {
        foreach (var item in items)
        {
          _viewModel.Items.Add(item);
        }
      }
    }

    private void ButtonClipCancel_OnClick(object sender, RoutedEventArgs e)
    {
      PastePopup.IsOpen = false;
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (this.PastePopup.IsOpen)
      {
        PastePopup.IsOpen = false;
        e.Cancel = true;
      }
    }

    /*private string SearchContacts(string name, bool bPhone)
    {
      var result = string.Empty;
      var cons = new Contacts();
      cons.SearchCompleted +=cons_SearchCompleted;
      cons.SearchAsync(name, FilterKind.None, null);
      if (_contactSearchEvt.WaitOne(3000))
      {
        if (null != _foundContact)
        {
          result = bPhone
            ? _foundContact.PhoneNumbers.Any()
              ? _foundContact.PhoneNumbers.First().PhoneNumber
              : string.Empty
            : _foundContact.EmailAddresses.Any()
              ? _foundContact.EmailAddresses.First().EmailAddress
              : string.Empty;
          _foundContact = null;
        }
      }
      return result;
    }*/

    //private void cons_SearchCompleted(object sender, ContactsSearchEventArgs e)
    //{
    //  _foundContact = e.Results.FirstOrDefault();
    //  _contactSearchEvt.Set();
    //}

    private List<ShoppingItemViewModel> TryParseClipboard(string text)
    {
      if (string.IsNullOrEmpty(text))
        return null;
      var list = new List<ShoppingItemViewModel>();

      foreach (var line in text.Split('\r'))
      {
        var subStr = line;
        if (!string.IsNullOrEmpty(line) && !line.Contains("Купить:"))
        {
          if (line.Contains("."))
          {
            var pos = line.IndexOf('.');
            if (pos < line.Length)
              subStr = line.Substring(pos + 1, line.Length - pos -1 );
          }
          list.Add(new ShoppingItemViewModel(subStr));
        }
      }
      return list;
    }


    private string CreateListText()
    {
      var text = string.Empty;
      var i = 0;
      foreach (var item in _viewModel.Items.Where(item => item.Completed == false && !string.IsNullOrEmpty(item.Text)))
      {
        ++i;
        text += string.Format("{0}. {1}\r\n", i, item.Text);
      }
      return i > 0 ? text : string.Empty;
    }

    private void SendToClipBoard()
    {
      var text = CreateListText();

      if (!string.IsNullOrEmpty(text))
      {
        Clipboard.SetText(text);
        MessageBox.Show("Ваш список отправлен в буфер обмена", "За покупками", MessageBoxButton.OK);
      }
      else
      {
        MessageBox.Show("Вам нечего отправить в буфер обмена", "Мы не посылаем пустые списки", MessageBoxButton.OK);
      }
    }


    private void CreateEMail(string address = null)
    {
      var text = CreateListText();

      if (!string.IsNullOrEmpty(text))
      {
        var emailTask = new EmailComposeTask
        {
          //To = SearchContacts(address, false),
          Subject = "Список покупок",
          Body = "Купить:\r\n" + text
        };
        emailTask.Show();
      }
      else
      {
        MessageBox.Show("Вам нечего послать по почте", "Мы не посылаем пустые списки", MessageBoxButton.OK);
      }
    }

    private void CreateSms(string address = null)
    {
      var text = CreateListText();
      if (!string.IsNullOrEmpty(text))
      {
        var smsTask = new SmsComposeTask
        {
          //To = SearchContacts(address,true),
          Body = "Купить:\r\n" + text
        };
        smsTask.Show();
      }
      else
      {
        MessageBox.Show("Вам нечего послать по СМС", "Мы не посылаем пустые списки", MessageBoxButton.OK);
      }
    }
  }
}