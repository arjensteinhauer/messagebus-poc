using MB.Client.Desktop.App.EventHandlers;
using MB.Client.Desktop.App.Proxies;
using MB.Manager.Message1.Interface.V1;
using System;
using System.Windows;
using EchoRequest = MB.Client.Gateway.Service.Contracts.EchoRequest;
using IAmAliveRequest = MB.Client.Gateway.Service.Contracts.IAmAliveRequest;
using Message1AmAliveEventData = MB.Manager.Message1.Interface.V1.AmAliveEventData;
using Message1EchoedEventData = MB.Manager.Message1.Interface.V1.EchoedEventData;
using Message2AmAliveEventData = MB.Manager.Message2.Interface.V1.AmAliveEventData;
using Message2EchoedEventData = MB.Manager.Message2.Interface.V1.EchoedEventData;
using OneWayRequest = MB.Client.Gateway.Service.Contracts.OneWayRequest;
using RequestResponseRequest = MB.Client.Gateway.Service.Contracts.RequestResponseRequest;

namespace MB.Client.Desktop.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMessage1Service _message1Service;
        private readonly IMessage2Service _message2Service;
        private readonly IMessage1EventHandler _message1EventHandler;
        private readonly IMessage2EventHandler _message2EventHandler;
        private readonly IEventsService _eventsService;

        public MainWindow(
            IMessage1Service message1Service,
            IMessage2Service message2Service,
            IEventsService eventsService,
            IMessage1EventHandler message1EventHandler,
            IMessage2EventHandler message2EventHandler)
        {
            // save the provided instances
            _message1Service = message1Service;
            _message2Service = message2Service;
            _eventsService = eventsService;
            _message1EventHandler = message1EventHandler;
            _message2EventHandler = message2EventHandler;

            // event handlers
            _message1EventHandler.OnEchoed += Message1EventHandler_OnEchoed;
            _message1EventHandler.OnProcessedOneWayCommand += Message1EventHandler_OnProcessedOneWayCommand;
            _message1EventHandler.OnAmAlive += Message1EventHandler_OnAmAlive;

            _message2EventHandler.OnEchoed += Message2EventHandler_OnEchoed;
            _message2EventHandler.OnAmAlive += Message2EventHandler_OnAmAlive;

            // initialize xaml form conponents
            InitializeComponent();
        }

        private async void Message1EventHandler_OnEchoed(object sender, Message1EchoedEventData eventData)
        {
            // synchronize with the UI thread (because we're about to update the UI)
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                Message1AsyncEchoResult.Text = eventData.Result;
            });
        }

        private async void Message1EventHandler_OnProcessedOneWayCommand(object sender, ProcessedOneWayCommandEventData eventData)
        {
            // synchronize with the UI thread (because we're about to update the UI)
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                Message1AsyncOneWayResult.Text = eventData.Result;
            });
        }

        private async void Message1EventHandler_OnAmAlive(object sender, Message1AmAliveEventData eventData)
        {
            // synchronize with the UI thread (because we're about to update the UI)
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                Message1AsyncResult.Text = eventData.Message;
            });
        }

        private async void Message2EventHandler_OnEchoed(object sender, Message2EchoedEventData eventData)
        {
            // synchronize with the UI thread (because we're about to update the UI)
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                Message2AsyncEchoResult.Text = eventData.Result;
            });
        }

        private async void Message2EventHandler_OnAmAlive(object sender, Message2AmAliveEventData eventData)
        {
            // synchronize with the UI thread (because we're about to update the UI)
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                Message2AsyncResult.Text = eventData.Message;
            });
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // subscribe to SignalR events
                await _message1EventHandler.SubscribeOnEvents().ConfigureAwait(false);
                await _message2EventHandler.SubscribeOnEvents().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string errorText = GetExceptionMessageText(ex);
                MessageBox.Show(errorText);
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // unsubscribe from SignalR
                await _message1EventHandler.Disconnect().ConfigureAwait(false);
                await _message2EventHandler.Disconnect().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string errorText = GetExceptionMessageText(ex);
                MessageBox.Show(errorText);
            }
        }

        private async void SendMessage1Echo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Message1Response.Text = "";
                Message1AsyncEchoResult.Text = "";

                var request = new EchoRequest { Input = Message1EchoMessage.Text };

                string result = await _message1Service.Echo(request, _message1EventHandler.ConnectionId).ConfigureAwait(false);

                // synchronize with the UI thread (because we're about to update the UI)
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    Message1Response.Text = result;
                });
            }
            catch (Exception ex)
            {
                string errorText = GetExceptionMessageText(ex);
                MessageBox.Show(errorText);
            }
        }

        private async void SendMessage1OneWayCommand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Message1OneWayCommandResponse.Text = "";
                Message1AsyncOneWayResult.Text = "";

                var request = new OneWayRequest { Input = Message1CommandMessage.Text };

                string result = await _message1Service.OneWay(request, _message1EventHandler.ConnectionId).ConfigureAwait(false);

                // synchronize with the UI thread (because we're about to update the UI)
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    Message1OneWayCommandResponse.Text = result;
                });
            }
            catch (Exception ex)
            {
                string errorText = GetExceptionMessageText(ex);
                MessageBox.Show(errorText);
            }
        }

        private async void SendMessage1RequestResponse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Message1RequestResponseResponse.Text = "";

                var request = new RequestResponseRequest { Input = Message1RequestResponseRequestMessage.Text };

                string result = await _message1Service.RequestResponse(request, _message1EventHandler.ConnectionId).ConfigureAwait(false);

                // synchronize with the UI thread (because we're about to update the UI)
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    Message1RequestResponseResponse.Text = result;
                });
            }
            catch (Exception ex)
            {
                string errorText = GetExceptionMessageText(ex);
                MessageBox.Show(errorText);
            }
        }

        private async void SendMessage2Echo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Message2Response.Text = "";
                Message2AsyncEchoResult.Text = "";

                var request = new EchoRequest { Input = Message2EchoMessage.Text };

                string result = await _message2Service.Echo(request, _message2EventHandler.ConnectionId).ConfigureAwait(false);

                // synchronize with the UI thread (because we're about to update the UI)
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    Message2Response.Text = result;
                });
            }
            catch (Exception ex)
            {
                string errorText = GetExceptionMessageText(ex);
                MessageBox.Show(errorText);
            }
        }

        private async void PublishIAmAliveMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EventsIAmAliveResponse.Text = "";
                Message1AsyncResult.Text = "";
                Message2AsyncResult.Text = "";

                var request = new IAmAliveRequest { Input = EventsIAmAliveMessage.Text };

                string result = await _eventsService.IAmAlive(request, _message1EventHandler.ConnectionId, _message2EventHandler.ConnectionId).ConfigureAwait(false);

                // synchronize with the UI thread (because we're about to update the UI)
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    EventsIAmAliveResponse.Text = result;
                });
            }
            catch (Exception ex)
            {
                string errorText = GetExceptionMessageText(ex);
                MessageBox.Show(errorText);
            }
        }

        /// <summary>
        /// Gets the error message from the provided exception.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>The error message.</returns>
        private string GetExceptionMessageText(Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errorMessage += String.Format("\r\nInner exception:\r\n{0}", GetExceptionMessageText(ex.InnerException));
            }

            return errorMessage;
        }
    }
}
