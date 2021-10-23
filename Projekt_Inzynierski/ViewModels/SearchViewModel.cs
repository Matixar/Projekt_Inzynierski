using Projekt_Inzynierski.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel()
        {
            SearchCommand = new Command(OnSearch);
        }

        public Command SearchCommand { get; }

        private async void OnSearch()
        {
            
        }
    }
}