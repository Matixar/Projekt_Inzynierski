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
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            this.BindingContext = new SearchViewModel();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var client = new OpenApiService.OpenApiService("https://travelapp-api.azurewebsites.net/", new System.Net.Http.HttpClient());
            var user = client.UsersAllAsync().Result.First();
            
            DisplayAlert("test", user.Email, "OK");
            
            
        }
    }
}