using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Scheduler;

namespace ExamPortal.ViewModels
{
    public class ExamCalendarViewModel : BaseViewModel
    {
        public ExamCalendarViewModel() : base()
        {
            InitializeExamDates();
            InitializeExamDetails();
            InitializeExamAppointments();

        }

        private ObservableCollection<DateTime> examDates;
        private ObservableCollection<CalendarEvent> examDetails;
        private ObservableCollection<SchedulerAppointment> examAppointments;
        private ObservableCollection<MockData.Exam> examsForSelectedDate;
        private DateTime selectedDate;
        private bool isExamDetailsVisible;

        public ObservableCollection<DateTime> ExamDates
        {
            get => examDates;
            set => SetProperty(ref examDates, value);
        }

        public ObservableCollection<CalendarEvent> ExamDetails
        {
            get => examDetails;
            set => SetProperty(ref examDetails, value);
        }

        public ObservableCollection<MockData.Exam> ExamsForSelectedDate
        {
            get => examsForSelectedDate;
            set => SetProperty(ref examsForSelectedDate, value);
        }

        public DateTime SelectedDate
        {
            get => selectedDate;
            set => SetProperty(ref selectedDate, value);
        }

        public bool IsExamDetailsVisible
        {
            get => isExamDetailsVisible;
            set => SetProperty(ref isExamDetailsVisible, value);
        }

        public ObservableCollection<SchedulerAppointment> ExamAppointments
        {
            get => examAppointments;
            set => SetProperty(ref examAppointments, value);
        }

        private void InitializeExamDates()
        {
            ExamDates = new ObservableCollection<DateTime>();

            foreach (var exam in MockData.Exams)
            {
                if (!ExamDates.Contains(exam.Date.Date))
                {
                    ExamDates.Add(exam.Date.Date);
                }
            }
        }

        private void InitializeExamAppointments()
        {
            ExamAppointments = new ObservableCollection<SchedulerAppointment>();

            if (MockData.Exams == null || MockData.Exams.Count == 0)
            {
                return;
            }

            foreach (var exam in MockData.Exams)
            {
                var appointment = new SchedulerAppointment
                {
                    StartTime = exam.Date,
                    EndTime = exam.Date.AddHours(2), 
                    Subject = exam.Subject,
                    IsAllDay = false,
                    Notes = $"{exam.Course} - {exam.Venue}\n{exam.Description}",   
                    Background = new SolidColorBrush(GetSubjectColor(exam.Subject))
                };
                ExamAppointments.Add(appointment);
            }
        }

        private Color GetSubjectColor(string subject)
        {
            var hash = subject.GetHashCode();
            var random = new Random(hash);

            return Color.FromRgb(
                (byte)(random.Next(100, 200)),
                (byte)(random.Next(100, 200)),
                (byte)(random.Next(100, 200))
            );
        }



        private void InitializeExamDetails()
        {
            ExamDetails = new ObservableCollection<CalendarEvent>();

            foreach (var exam in MockData.Exams)
            {
                var calendarEvent = new CalendarEvent
                {
                    Date = exam.Date,
                    Subject = exam.Subject,
                    Course = exam.Course,
                    Time = exam.Time,
                    Venue = exam.Venue,
                    Description = exam.Description
                };
                ExamDetails.Add(calendarEvent);
            }
        }

        public void ShowExamsForDate(DateTime date)
        {
            Console.WriteLine($"ShowExamsForDate called with: {date}");

            SelectedDate = date;
            ExamsForSelectedDate.Clear();

            // Find exams for the selected date
            var examsForDate = MockData.Exams.Where(exam => exam.Date.Date == date.Date).ToList();

            Console.WriteLine($"Found {examsForDate.Count} exams for {date}");

            foreach (var exam in examsForDate)
            {
                ExamsForSelectedDate.Add(exam);
            }

            IsExamDetailsVisible = ExamsForSelectedDate.Count > 0;
            Console.WriteLine($"IsExamDetailsVisible: {IsExamDetailsVisible}");
        }


        public class CalendarEvent
        {
            public DateTime Date { get; set; }
            public string Subject { get; set; }
            public string Course { get; set; }
            public string Time { get; set; }
            public string Venue { get; set; }
            public string Description { get; set; }
        }
    }
}