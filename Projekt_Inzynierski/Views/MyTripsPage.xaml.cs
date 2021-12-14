using Projekt_Inzynierski.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt_Inzynierski.Views
{
   
    public partial class MyTripsPage : ContentPage
    {
        TripsViewModel _viewModel;

        public MyTripsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new TripsViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            return true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
