using Microsoft.Maui.Controls;
using Syncfusion.Maui.Popup;
using ExamPortal.ViewModels;

namespace MyMauiApp;

public partial class AddExamSchedule : ContentPage
{
	public AddExamSchedule()
	{
        InitializeComponent();

        this.BindingContext = new AddExamScheduleViewModel();   
    }
}