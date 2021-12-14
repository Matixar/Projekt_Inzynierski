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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendlistPage : ContentPage
    {
        FriendsViewModel _viewModel;

        public FriendlistPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new FriendsViewModel();
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
