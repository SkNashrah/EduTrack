using ExamPortal.ViewModels;

namespace ExamPortal;

public partial class Dashboard : ContentPage
{
	public Dashboard()
	{
		MockData.InitializeData();

		InitializeComponent();
		BindingContext = new DashboardViewModel();
    }
}