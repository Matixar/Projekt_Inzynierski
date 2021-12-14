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

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            return true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", new System.Net.Http.HttpClient());
            
            
            
            
        }
    }
}