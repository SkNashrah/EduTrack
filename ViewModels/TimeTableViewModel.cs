using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExamPortal.ViewModels;

namespace ExamPortal.ViewModels
{
    public class TimeTableViewModel : BaseViewModel
    {
        public TimeTableViewModel()
        {
            var upcomingExams= MockData.Exams.OrderBy(e => e.Date)
                .Where(e => e.Date > DateTime.Today)
                .OrderBy(e => e.Date)
                .ToList();

            Exams = new ObservableCollection<MockData.Exam>(upcomingExams);
            FilteredExams = new ObservableCollection<MockData.Exam>(upcomingExams);

            Courses = new ObservableCollection<string>(upcomingExams.Select(e => e.Course).Distinct());
            Years = new ObservableCollection<int>(upcomingExams.Select(e => e.Year).Distinct());
            
            SelectedCourse = null;
            SelectedYear = 0;

            ClearFiltersCommand = new Command(ClearFilters);
        }

        private ObservableCollection<MockData.Exam> exams;
        private ObservableCollection<MockData.Exam> filteredExams;
        private ObservableCollection<string> courses;
        private ObservableCollection<int> years;
        private string selectedCourse;
        private int selectedYear;

        public ObservableCollection<MockData.Exam> Exams
        {
            get => exams;
            set => SetProperty(ref exams, value);
        }
        public ObservableCollection<MockData.Exam> FilteredExams
        {
            get => filteredExams;
            set => SetProperty(ref filteredExams, value);
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

        public string SelectedCourse
        {
            get => selectedCourse;
            set
            {
                SetProperty(ref selectedCourse, value);
                FilterExams();
            }
        }
        public int SelectedYear
        {
            get => selectedYear;
            set
            {
                SetProperty(ref selectedYear, value);
                FilterExams();
            }
        }   
        public ICommand ClearFiltersCommand { get; set; }

        private void FilterExams()
        {
            var filtered = Exams.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedCourse))
            {
                filtered = filtered.Where(e => e.Course == SelectedCourse);
            }
            if (SelectedYear != 0)
            {
                filtered = filtered.Where(e => e.Year == SelectedYear);
            }
           

            filtered = filtered.OrderBy(e => e.Date);

            FilteredExams = new ObservableCollection<MockData.Exam>(filtered);
        }

        private void ClearFilters()
        {
            SelectedCourse = null;
            SelectedYear = 0;

            FilteredExams = new ObservableCollection<MockData.Exam>(Exams);
        }

    }
}
