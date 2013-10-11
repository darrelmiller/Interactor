using System.Collections.Generic;
using InteractorLib.Interfaces;

namespace InteractorLib.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Links = new List<Link>();
        }

        public List<Link> Links { get; set; }
    }
}
