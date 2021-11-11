using Projekt_Inzynierski.ViewModels;
using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Projekt_Inzynierski
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(TripDetailPage), typeof(TripDetailPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnAboutItemClicked(object sender, EventArgs e)
        {
            await DisplayAlert("O aplikacji","Aplikacja mobilna do organizacji wspólnych przejazdów. Projekt inżynierski.\n\n@Sylwester Kot, 2021","Anuluj");
        }
    }
}
