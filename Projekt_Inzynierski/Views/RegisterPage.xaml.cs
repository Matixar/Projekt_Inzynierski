using Projekt_Inzynierski.Models;
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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();

            BindingContext = new RegisterViewModel();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            if (!EntryPassword.Text.Equals(EntryRepeatPassword.Text))
            {
                await DisplayAlert("Błąd", "Wprowadzone hasła się różnią", "OK");
            }
            else if (!LabelValidEmail.IsVisible && EntryEmail.Text != null &&
                !LabelValidName.IsVisible && EntryName.Text != null &&
                !LabelValidSurname.IsVisible && EntrySurname.Text != null &&
                !LabelValidPassword.IsVisible)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                await DisplayAlert("Błąd", "Niepoprawnie wprowadzone dane", "OK");
            }
        }
        
    }
}