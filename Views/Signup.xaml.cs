using ExamPortal.ViewModels;

namespace ExamPortal;

public partial class Signup : ContentPage
{
	public Signup()
	{
		InitializeComponent();
		BindingContext = new SignupViewModel();
	}
}