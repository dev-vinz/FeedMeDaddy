using FeedMeDaddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand PlanningViewCommand { get; set; }
        public RelayCommand RecipesViewCommand { get; set; }
        public RelayCommand ShoppingViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public PlanningViewModel PlanningVM { get; set; }
        public RecipesViewModel RecipesVM { get; set; }
        public ShoppingViewModel ShoppingVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            /*HomeVM = new HomeViewModel();
            PlanningVM = new PlanningViewModel();
            RecipesVM = new RecipesViewModel();
            ShoppingVM = new ShoppingViewModel();*/

            CurrentView = new HomeViewModel();

            // TODO
            // Potential Warning

            HomeViewCommand = new RelayCommand(o => {
                CurrentView = new HomeViewModel();
            });
            
            PlanningViewCommand = new RelayCommand(o => {
                CurrentView = new PlanningViewModel();
            });
            
            RecipesViewCommand = new RelayCommand(o => {
                CurrentView = new RecipesViewModel();
            });
            
            ShoppingViewCommand = new RelayCommand(o => {
                CurrentView = new ShoppingViewModel();
            });
        }

    }
}
