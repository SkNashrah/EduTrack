using ExamPortal.ViewModels;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Scheduler;

namespace ExamPortal;

public partial class ExamCalendar : ContentPage
{
    private ExamCalendarViewModel viewModel;

    public ExamCalendar()
    {
        InitializeComponent();
        viewModel = new ExamCalendarViewModel();
        BindingContext = viewModel;

        // Add tap gesture to the scheduler
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnSchedulerTapped;
        Scheduler.GestureRecognizers.Add(tapGesture);
    }

    private void OnSchedulerTapped(object sender, TappedEventArgs e)
    {
        // This is a fallback method if the built-in events don't work
        // You might need to implement custom logic to determine the tapped date
        Console.WriteLine("Scheduler tapped - using fallback method");

        // For now, let's just show today's exams as a demonstration
        viewModel.ShowExamsForDate(DateTime.Today);
    }

    private void Scheduler_SelectionChanged(object sender, SchedulerSelectionChangedEventArgs e)
    {
        if (e.NewValue is DateTime selectedDate)
        {
            viewModel.ShowExamsForDate(selectedDate);
        }
    }

    private void Scheduler_Tapped(object sender, SchedulerTappedEventArgs e)
    {
        if (e.Date != null)
        {
            viewModel.ShowExamsForDate(e.Date.Value);
        }
    }
}