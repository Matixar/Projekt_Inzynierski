﻿using Projekt_Inzynierski.Services;
using Projekt_Inzynierski.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt_Inzynierski
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<MockDataTrips>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
