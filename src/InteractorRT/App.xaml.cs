using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227
using InteractorLib.Interfaces;
using InteractorLib.ResponseHandlers;
using InteractorLib.ViewModels;
using XBrowser.Views;
using XBrowser.zFakeStuff;

namespace Interactor
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application, IShell
    {
        HttpClient _httpClient;
        List<IHttpResponseHandler> _responseHandlers = new List<IHttpResponseHandler>();
        private Frame _rootFrame;


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            _httpClient = new HttpClient(new FakeServer())
            {
                BaseAddress = new Uri("http://myapi.com/")
            };

            _responseHandlers.Add(new PlainTextHandler(this));
            _responseHandlers.Add(new HtmlHandler(this));
            _responseHandlers.Add(new HalHandler(this, _httpClient));
            
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (_rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                _rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = _rootFrame;
            }

            if (_rootFrame.Content == null)
            {
                
                var link = new Link()
                {
                    Relation = "home",
                    Target = new Uri("hal", UriKind.Relative)
                };

                _httpClient.GetAsync(link.Target).ContinueWith(t => HandleResponse(link,t.Result), TaskScheduler.FromCurrentSynchronizationContext());

            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async void HandleResponse(Link link, HttpResponseMessage result)
        {
            foreach (var httpResponseHandler in _responseHandlers)
            {
                var response = await httpResponseHandler.HandleAsync(link, result);
                if (response == null) break;
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        public void Show(string message)
        {
           
            _rootFrame.Navigate(typeof(PlainTextView), message);
        }

        public void ShowHtml(string page)
        {
            _rootFrame.Navigate(typeof(HtmlView), page);
        }

        public void ShowHal(HalViewModel viewModel)
        {
            _rootFrame.Navigate(typeof(HalView), viewModel);
        }
    }
}
