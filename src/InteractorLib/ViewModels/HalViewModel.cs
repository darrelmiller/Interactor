using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using InteractorLib.Interfaces;

namespace InteractorLib.ViewModels
{
    public class HalViewModel
    {
        private readonly HttpClient _httpClient;
        private XDocument _doc;

        public string Stylesheet { get; set; }

        public HalViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
            // Command commands to LO links.
            Commands = new Dictionary<string, ICommand>();
            var xCommand = new LinkCommand(httpClient);
            Commands.Add("PressMeCommand",xCommand);   

        }

        public Dictionary<string,ICommand>  Commands { get; set; }

        public object this[string path]
        {
            get
            {
                return _doc.Element("resource").Element(path).Value;
            }
            set
            {
                _doc.Element("resource").Element(path).Value = value.ToString();
            }
            
    }

        public async Task Load(Stream stream)
        {
            _doc = XDocument.Load(stream);

            // Load Stylesheet
            var link = GetStylesheetLink();
            if (link != null)
            {
                var response = await _httpClient.GetAsync(link.Target);
                Stylesheet = await response.Content.ReadAsStringAsync();
            }

        }

        private Link GetStylesheetLink()
        {
            return new Link()
            {
                Relation = "Stylesheet",
                Target = new Uri("hal_stylesheet", UriKind.Relative)
            };
        }
    }
}
