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
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
            BindingContext = new UserViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            return true;
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            EntryName.IsEnabled = true;
            SwitchGender.IsEnabled = true;
            EntryDescription.IsEnabled = true;
            ButtonEdit.IsVisible = false;
            ButtonSave.IsVisible = true;
            ButtonCancel.IsVisible = true;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            EntryName.IsEnabled = false;
            SwitchGender.IsEnabled = false;
            EntryDescription.IsEnabled = false;
            ButtonEdit.IsVisible = true;
            ButtonSave.IsVisible = false;
            ButtonCancel.IsVisible = false;
        }
    }
}