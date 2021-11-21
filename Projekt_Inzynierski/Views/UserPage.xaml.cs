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
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            EntryDateOfBirth.IsEnabled = true;
            EntryName.IsEnabled = true;
            SwitchGender.IsEnabled = true;
            EntryDescription.IsEnabled = true;
            EntryAvatar.IsEnabled = true;
            ButtonEdit.IsVisible = false;
            ButtonSave.IsVisible = true;
            ButtonCancel.IsVisible = true;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            EntryDateOfBirth.IsEnabled = false;
            EntryName.IsEnabled = false;
            SwitchGender.IsEnabled = false;
            EntryDescription.IsEnabled = false;
            EntryAvatar.IsEnabled = false;
            ButtonEdit.IsVisible = true;
            ButtonSave.IsVisible = false;
            ButtonCancel.IsVisible = false;
        }
    }
}