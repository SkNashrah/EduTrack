using ExamPortal.ViewModels;

namespace ExamPortal;

public partial class TimeTable : ContentPage
{
	public TimeTable()
	{
		InitializeComponent();
		BindingContext = new TimeTableViewModel();
    }
}