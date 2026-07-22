using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ExamPortal.ViewModels;

namespace ExamPortal.ViewModels
{
    public class SnackbarViewModel : BaseViewModel
    {
        public async Task ShowSnackbar(string message, string hexColor = "#4CAF50", int duration = 3000)
        {
            try
            {
                // Ensure we're on main thread
                if (Application.Current?.Dispatcher.IsDispatchRequired ?? false)
                {
                    Application.Current.Dispatcher.Dispatch(async () =>
                    {
                        await ShowSnackbarInternal(message, hexColor, duration);
                    });
                }
                else
                {
                    await ShowSnackbarInternal(message, hexColor, duration);
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Info", message, "OK");
            }
        }

        private async Task ShowSnackbarInternal(string message, string hexColor, int duration)
        {
            var options = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb(hexColor),
                TextColor = Colors.White,
                CornerRadius = new CornerRadius(12),
                Font = Microsoft.Maui.Font.SystemFontOfSize(14, FontWeight.Bold)
            };

            await Snackbar.Make(
                message: message,
                duration: TimeSpan.FromMilliseconds(duration),
                visualOptions: options
            ).Show();
        }
    }
}