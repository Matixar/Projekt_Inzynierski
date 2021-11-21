using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_Inzynierski.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private string email;
        private string password;

        private bool emailValid;
        private bool passwordEmptyValid;
        private bool nameValid;
        private bool surnameValid;

        public bool EmailValid
        {
            get => emailValid;
            set
            {
                emailValid = value;
                OnPropertyChanged();
            }
        }
        public bool PasswordEmptyValid
        {
            get => passwordEmptyValid;
            set
            {
                passwordEmptyValid = value;
                OnPropertyChanged();
            }
        }
        public bool NameValid
        {
            get => nameValid;
            set
            {
                nameValid = value;
                OnPropertyChanged();
            }
        }
        public bool SurnameValid
        {
            get => surnameValid;
            set
            {
                surnameValid = value;
                OnPropertyChanged();
            }
        }

        public string Email { get => email; set => SetProperty(ref email, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
    }
}
