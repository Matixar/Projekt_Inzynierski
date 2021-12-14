using Newtonsoft.Json;
using OpenApiService;
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
                !LabelValidPassword.IsVisible)
            {
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", new System.Net.Http.HttpClient());
                var register = new RegisterDto();
                register.Email = EntryEmail.Text;
                register.Password = EntryPassword.Text;
                try
                {
                    var result = await client.RegisterAsync(register);
                    saveName();
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                catch(ApiException error)
                {
                    await DisplayAlert("Błąd", JsonConvert.DeserializeObject<ErrorResponse>(error.Response).error, "OK");
                }
            }
            else
            {
                await DisplayAlert("Błąd", "Niepoprawnie wprowadzone dane", "OK");
            }
        }

        private async void saveName()
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var login = await client.LoginAsync(new OpenApiService.LoginDto()
                {
                    Email = EntryEmail.Text,
                    Password = EntryPassword.Text
                });

                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", login.Token);

                client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var body = new OpenApiService.EditMemberDto()
                {
                    Name = EntryName.Text
                };
                var result = await client.UsersAsync(body);
                await AppShell.Current.DisplayAlert("Potwierdzenie", "Zarejestrowano pomyślnie", "OK");
            }
            catch (OpenApiService.ApiException e)
            {
                await AppShell.Current.DisplayAlert("Błąd", "Błąd podczas logowania " + e.Message, "OK");
            }
        }


    }
}