﻿using FeedMeDaddy.Core;
using FeedMeDaddy.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        public string Description { get; set; }

        public HomeViewModel()
        {
            var ctx = new FeedMeDaddyContext();
        }
    }
}
