using Projekt_Inzynierski.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Projekt_Inzynierski.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}