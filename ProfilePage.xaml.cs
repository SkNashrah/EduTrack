using ExamPortal.ViewModels;
using Microsoft.Maui.Controls;

namespace ExamPortal
{
    public partial class ProfilePage : ContentView
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is BaseViewModel baseViewModel)
            {
                baseViewModel.ProfileVm.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(ProfileViewModel.IsVisible))
                    {
                        if (baseViewModel.ProfileVm.IsVisible)
                            ShowPanel();
                        else
                            HidePanel();
                    }
                };
            }
        }

        public async void ShowPanel()
        {
            backgroundOverlay.IsVisible = true;
            profilePanel.IsVisible = true;
            await Task.WhenAll(
                backgroundOverlay.FadeTo(1, 250),
                profilePanel.TranslateTo(0, 0, 250)
            );
        }

        public async void HidePanel()
        {
            await Task.WhenAll(
                backgroundOverlay.FadeTo(0, 250),
                profilePanel.TranslateTo(profilePanel.Width, 0, 250)
            );
            profilePanel.IsVisible = false;
            backgroundOverlay.IsVisible = false;
        }

        private void OnBackgroundTapped(object sender, EventArgs e)
        {
            if (BindingContext is BaseViewModel baseViewModel)
                baseViewModel.ProfileVm.Hide();
        }

        private void OnSwipeToHide(object sender, SwipedEventArgs e)
        {
            if (BindingContext is BaseViewModel baseViewModel)
                baseViewModel.ProfileVm.Hide();
        }
    }
}