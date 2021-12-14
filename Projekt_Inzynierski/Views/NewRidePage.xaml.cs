using Projekt_Inzynierski.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt_Inzynierski.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRidePage : ContentPage
    {
        NewTripViewModel _viewmodel;
        public NewRidePage()
        {
            InitializeComponent();

            _viewmodel = new NewTripViewModel();

            BindingContext = _viewmodel;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewmodel.OnAppearing();

            CarPicker.ItemDisplayBinding = new Binding()
            {
                StringFormat = "Nr rejestracyjny: {0}",
                Path = "RegistrationNumber"
            };
            

        }

    }

}