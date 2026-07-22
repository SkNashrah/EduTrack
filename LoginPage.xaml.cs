using ExamPortal.ViewModels;

namespace ExamPortal;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel();
    }
}