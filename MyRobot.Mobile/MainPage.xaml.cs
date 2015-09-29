using Microsoft.AspNet.SignalR.Client;
using MyRobot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyRobot.Mobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        HubConnection hubConnection;
        IHubProxy controlHubProxy;
        public MainPage()
        {
            this.InitializeComponent();
            ButtonGo.ClickMode = ClickMode.Press;
            ButtonGo.PointerReleased += ButtonGo_PointerReleased;
            ButtonBack.ClickMode = ClickMode.Press;
            ButtonBack.PointerReleased += ButtonBack_PointerReleased;
            ButtonLeft.ClickMode = ClickMode.Press;
            ButtonLeft.PointerReleased += ButtonLeft_PointerReleased;
            ButtonRight.ClickMode = ClickMode.Press;
            ButtonRight.PointerReleased += ButtonRight_PointerReleased;
        }

        private void ButtonRight_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            controlHubProxy.Invoke("SendCommand", new object[] { Command.NoOp });


        }

        private void ButtonLeft_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            controlHubProxy.Invoke("SendCommand", new object[] { Command.NoOp });
        }

        private void ButtonBack_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            controlHubProxy.Invoke("SendCommand", new object[] { Command.NoOp });
        }

        private void ButtonGo_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            controlHubProxy.Invoke("SendCommand", new object[] { Command.NoOp });
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            hubConnection = new HubConnection("http://myrobotservice.azurewebsites.net/");
            hubConnection.StateChanged += HubConnection_StateChanged;
            controlHubProxy = hubConnection.CreateHubProxy("ControlHub");

            hubConnection.Start().Wait();

            base.OnNavigatedTo(e);

        }

        private async void HubConnection_StateChanged(StateChange obj)
        {
            await TextBlockStatus.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlockStatus.Text += string.Format("{0}->{1} at {2}\n", obj.OldState, obj.NewState, DateTime.Now);
            });
        }
  

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            controlHubProxy.Invoke("SendCommand", new object[] { Command.Go });
            TextBlockStatus.Text += string.Format("{0}\n", DateTime.Now.Millisecond);
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            controlHubProxy.Invoke("SendCommand", new object[] { Command.Left });
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            controlHubProxy.Invoke("SendCommand", new object[] { Command.Right });
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            controlHubProxy.Invoke("SendCommand", new object[] { Command.Back });
        }
    }
}
