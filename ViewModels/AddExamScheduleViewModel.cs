using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExamPortal.ViewModels;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Popup;
using System.ComponentModel;

namespace ExamPortal.ViewModels 
{
    public class AddExamScheduleViewModel : BaseViewModel
    {
        private string  subject;
        private string course;
        private int year;
        private int semester;
        private DateTime date = DateTime.Today;
        private TimeSpan time = DateTime.Now.TimeOfDay;
        private string venue;
        private string description;

        private ObservableCollection<string> courses;
        private ObservableCollection<int> years;
        private ObservableCollection<int> semesters;
        private ObservableCollection<string> venues;

        

        private SnackbarViewModel snackbarVm;
        public string Subject
        {
            get => subject;
            set => SetProperty(ref subject, value);
        }

        public string SelectedCourse
        {
            get => course;
            set => SetProperty(ref course, value);
        }
        public int SelectedYear
        {
            get => year;
            set => SetProperty(ref year, value);
        }
        public int SelectedSemester
        {
            get => semester;
            set => SetProperty(ref semester, value);
        }
        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }
        public TimeSpan Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }
        public string SelectedVenue
        {
            get => venue;
            set => SetProperty(ref venue, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public ObservableCollection<string> Courses
        {
            get => courses;
            set => SetProperty(ref courses, value);
        }
        public ObservableCollection<int> Years
        {
            get => years;
            set => SetProperty(ref years, value);
        }
        public ObservableCollection<int> Semesters
        {
            get => semesters;
            set => SetProperty(ref semesters, value);
        }
        public ObservableCollection<string> Venues
        {
            get => venues;
            set => SetProperty(ref venues, value);
        }

        
        

        public ICommand AddExamScheduleCommand { get; }
        public ICommand ClosePopupCommand { get; }

        public AddExamScheduleViewModel()
        {
            snackbarVm = new SnackbarViewModel();
            Courses = MockData.Courses;
            Years = new ObservableCollection<int> { 1, 2, 3, 4 };
            Semesters = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            Venues = new ObservableCollection<string>
            {
                 "Room 101", "Room 202", "Room 303", "Room 404", "Room 505",
                "Room 606", "Room 707", "Room 808", "Room 909", "Room 010",
                "Room 111", "Room 212", "Room 313"
            };

            AddExamScheduleCommand = new  Command(async () => await AddExamSchedule());
        }

        private async Task AddExamSchedule()
        {
            if (string.IsNullOrWhiteSpace(Subject) ||
               string.IsNullOrWhiteSpace(SelectedCourse) ||
               SelectedYear == 0 ||
               SelectedSemester == 0 ||
               string.IsNullOrWhiteSpace(SelectedVenue))
            {
                PopupViewModel.ShowConfirmationPopup("Error", "Please fill in all required fields.", null);
                return;
            }

            var timeDateTime = DateTime.Today.Add(Time);
            var formattedTime = timeDateTime.ToString("hh:mm tt");

            var newExam = new MockData.Exam
            {
                Subject = Subject,
                Course = SelectedCourse,
                Year = SelectedYear,
                Semester = SelectedSemester,
                Date = Date,
                Time = formattedTime,
                Venue = SelectedVenue,
                Description = Description
            };

            MockData.Exams.Add(newExam);


            await Application.Current.MainPage.Navigation.PushAsync(new ExamCalendar());

            await Task.Delay(500);
            await snackbarVm.ShowSnackbar("Exam Schedule Added Successfully!");

            ClearForm();
        }

        
      

        private void ClearForm()
        {
            Subject = string.Empty;
            SelectedCourse = null;
            SelectedYear = 0;
            SelectedSemester = 0;
            Date = DateTime.Today;
            Time = DateTime.Now.TimeOfDay;
            SelectedVenue = null;
            Description = string.Empty;
        }
    }
}
