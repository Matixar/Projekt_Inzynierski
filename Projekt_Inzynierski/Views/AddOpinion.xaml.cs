﻿using Projekt_Inzynierski.ViewModels;
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
    public partial class AddOpinion : ContentPage
    {
        public AddOpinion()
        {
            InitializeComponent();

            BindingContext = new AddOpinionViewModel();
        }


    }
}