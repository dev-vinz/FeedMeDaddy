using FeedMeDaddy.Core;
using FeedMeDaddy.Model;
using FeedMeDaddy.Services;
using FeedMeDaddy.Services.Database;
using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.ViewModel
{
    class AddRecipeViewModel : ObservableObject
    {
        public Services.DataContracts.UnitWeight[] unitList { get; set; }
        public AddRecipeModel AddRecipeModel { get; set; }

        public AddRecipeViewModel()
        {
            LoadUnits();
            
        }

        private void LoadUnits()
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();

            IEnumerable<Services.DataContracts.UnitWeight> units = db.UnitWeight.Fetch();
            unitList = new Services.DataContracts.UnitWeight[units.Count()];
            Array.Copy(units.ToArray(), unitList, units.Count());

            db.Dispose();

            AddRecipeModel = new AddRecipeModel(unitList);

            
        }
    }
}
