using System.ComponentModel.DataAnnotations;

namespace ExamPortal.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
        public SignupViewModel() 
        {
            SignupCommand = new Command(OnSignup);
            NavigateToLoginCommand = new Command(ToLogin);
            snackbarVm = new SnackbarViewModel();
        }

        private string username;
        private string password;
        private string confirmPassword;
        private string email;
        private string emailError;
        private string passwordError;
        private string confirmPasswordError;

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
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string EmailError
        {
            get => emailError;
            set => SetProperty(ref emailError, value);
        }
        public string PasswordError
        {
            get => passwordError;
            set => SetProperty(ref passwordError, value);
        }
        public string ConfirmPasswordError
        {
            get => confirmPasswordError;
            set => SetProperty(ref confirmPasswordError, value);
        }
        public Command SignupCommand { get; }
        public Command NavigateToLoginCommand { get; }

        private readonly SnackbarViewModel snackbarVm;

        private async void OnSignup()
        {
            EmailError = string.Empty;
            PasswordError = string.Empty;
            ConfirmPasswordError = string.Empty;

            bool isValid = true;
            string errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await PopupViewModel.ShowConfirmationPopup("Error", "Please fill in all required fields.", () => { });
                return;
            }

            if (!IsValidEmail(Email))
            {
                EmailError = "Please enter a valid email address.";
                isValid = false;
                errorMessage = "Please enter a valid email address.";
            }

            if (Password.Length < 6)
            {
                PasswordError = "Password must be at least 6 characters long.";
                isValid = false;
                errorMessage = "Password must be at least 6 characters long.";
            }

            if (Password != ConfirmPassword)
            {
                ConfirmPasswordError = "Passwords do not match.";
                isValid = false;
                errorMessage = "Passwords do not match.";
            }

            if (!isValid)
            {
                await snackbarVm.ShowSnackbar(errorMessage, "#F44336");
                return;
            }

            try
            {
                await Task.Delay(500);
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                await snackbarVm.ShowSnackbar("Signup successful! Please log in.", "#4CAF50");
            }
            catch (Exception ex)
            {
                await PopupViewModel.ShowConfirmationPopup("Error", "An error occurred during signup. Please try again.", () => { });
            }
        }

        private async void ToLogin()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                await PopupViewModel.ShowConfirmationPopup("Error", "Unable to navigate to login page.", () => { });
            }
        }

        private bool IsValidEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                return false;

            var emailValidator = new EmailAddressAttribute();
            return emailValidator.IsValid(email);
        }
    }
}
