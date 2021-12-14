using Plugin.Geolocator;
using Projekt_Inzynierski.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Projekt_Inzynierski.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            getCurrentPosition();
            BindingContext = new AboutViewModel();
        }

        public async void getCurrentPosition()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();
                CurrentLocationMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(1)));
            }
            catch(Exception e)
            {
                await Shell.Current.DisplayAlert("Alert", "Włącz moduł GPS do poprawnego działania aplikacji", "OK");
            }
        }
    }
}