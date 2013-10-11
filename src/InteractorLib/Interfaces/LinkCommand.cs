using System;
using System.Net.Http;
using System.Windows.Input;

namespace InteractorLib.Interfaces
{
    public class LinkCommand : ICommand
    {
        private readonly HttpClient _httpClient;

        public LinkCommand(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Link Link { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // Need to get a link and follow it.
            _httpClient.SendAsync(Link.CreateRequest());
        }

        public event EventHandler CanExecuteChanged;
    }
}