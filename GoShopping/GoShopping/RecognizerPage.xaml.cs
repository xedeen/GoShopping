using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Yandex.SpeechKit;
using Yandex.SpeechKit.UI;

namespace GoShopping
{
    public partial class RecognizerPage : PhoneApplicationPage
    {

        private string Result;
        public RecognizerPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            Result = string.Empty;
            RecognizerView.StartRecognition("ru-RU", LanguageModel.General);

            base.OnNavigatedTo(args);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs args)
        {
            RecognizerView.CancelRecognition();

            var mainPage = (args.Content as MainPage);
            if (null != mainPage)
                mainPage.RetrievedText = Result;


            base.OnNavigatedFrom(args);
        }

        private void RecognizerViewFinished(object sender, RecognitionFinishedEventArgs args)
        {
            Result = args.Result;
            NavigationService.GoBack();
        }
    }
}