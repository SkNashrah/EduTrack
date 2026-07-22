using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamPortal.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private bool isVisible;
        private string userName;
        private string userRole;
        private string profileImage;

        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string UserRole
        {
            get => userRole;
            set => SetProperty(ref userRole, value);
        }

        public string ProfileImage
        {
            get => profileImage;
            set => SetProperty(ref profileImage, value);
        }

        public ICommand ShowCommand { get; }
        public ICommand HideCommand { get; }
        public ICommand LogoutCommand { get; }

        public ProfileViewModel() : base(createProfileVm: false)
        {
            UserName = "John Doe";
            UserRole = "Administrator";
            ProfileImage = "user_profile.png";

            ShowCommand = new Command(Show);
            HideCommand = new Command(Hide);
            LogoutCommand = new Command(Logout);
        }

        public void Show()
        {
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        private async void Logout()
        {
            PopupViewModel.ShowConfirmationPopup(
                "Logout",
                "Are you sure you want to logout?",
                async () => {
                    Hide();
                    await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
                    var snackbar = new SnackbarViewModel();
                    snackbar.ShowSnackbar("Logged out successfully");
                }
            );
        }
    }
}