using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;

namespace ExamPortal.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);
            NavigateToSignupCommand = new Command(ToSignup);
            snackbarViewModel = new SnackbarViewModel();
        }

        private string username;
        private string password;
        private readonly SnackbarViewModel snackbarViewModel;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);

        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public ICommand LoginCommand { get; }
        public ICommand NavigateToSignupCommand { get; }

        private async void OnLogin()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await PopupViewModel.ShowConfirmationPopup("Error", "Please fill in all fields.", () => { });
                return;
            }
            else if (Password.Length < 6)
            {
                await snackbarViewModel.ShowSnackbar("Password must be at least 6 characters long.", "#f44336");
            }
            else
            {
                await Task.Delay(500);
                await snackbarViewModel.ShowSnackbar("Login successful!", "#4CAF50");
                await Application.Current.MainPage.Navigation.PushAsync(new Dashboard());
            }
        }

        private void ToSignup()
        {
            Application.Current.MainPage.Navigation.PushAsync(new Signup());
        }
    }
}
