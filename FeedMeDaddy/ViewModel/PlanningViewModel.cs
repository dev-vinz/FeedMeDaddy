using FeedMeDaddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.ViewModel
{
    class PlanningViewModel : ObservableObject
    {
        public string Description { get; set; }
        public string Toto { get; set; }

        public PlanningViewModel()
        {
            Description = "Hello World";
            Toto = "Ceci est un test";
        }
    }
}
