using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ExamPortal.ViewModels;

namespace ExamPortal.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel() : base()
        {
            UpcomingExams = new ObservableCollection<MockData.Exam>();
            RecentResults = new ObservableCollection<ResultWithColor>();
            PassFailDatas = new ObservableCollection<PassFailData>();
            GradeDistributions = new ObservableCollection<GradeDistribution>();
            CoursePerformances = new ObservableCollection<CoursePerformance>();

            CustomBrushes = new ObservableCollection<Brush>
            {
                new SolidColorBrush(Color.FromArgb("#BCEACF")), 
                new SolidColorBrush(Color.FromArgb("#FBCB7F")), 
                new SolidColorBrush(Color.FromArgb("#FBA1A0"))  
            };



            LoadUpcomingExams();
            LoadRecentResults();
            CalculateOverallStatistics();
            GenerateChartData();

        }

        private ObservableCollection<MockData.Exam> upcomingExams;
        private ObservableCollection<ResultWithColor> recentResults;
        private ObservableCollection<PassFailData> passFailDatas;
        private ObservableCollection<GradeDistribution> gradeDistributions;
        private ObservableCollection<CoursePerformance> coursePerformances;
        private ObservableCollection<Brush> customBrushes;
        private double overallPercentage;
        private string overallgrade;
        private int totalExams;
        private int totalStudents;
        private int passedStudents;
        private string passRate;
        private string studentPassRate;


        public ObservableCollection<MockData.Exam> UpcomingExams
        {
            get => upcomingExams;
            set => SetProperty(ref upcomingExams, value);
        }

        public ObservableCollection<ResultWithColor> RecentResults
        {
            get => recentResults;
            set => SetProperty(ref recentResults, value);
        }

        public ObservableCollection<PassFailData> PassFailDatas
        {
            get => passFailDatas;
            set => SetProperty(ref  passFailDatas, value);
        }

        public ObservableCollection<GradeDistribution> GradeDistributions
        {
            get => gradeDistributions;
            set => SetProperty(ref  gradeDistributions, value);
        }

        public ObservableCollection<CoursePerformance> CoursePerformances
        {
            get => coursePerformances;
            set => SetProperty(ref coursePerformances, value);
        }

        public ObservableCollection<Brush> CustomBrushes
        {
            get => customBrushes;
            set => SetProperty(ref customBrushes, value);
        }

        public double OverallPercentage
        {
            get => overallPercentage;
            set => SetProperty(ref overallPercentage, value);
        }

        public string OverallGrade
        {
            get => overallgrade;
            set => SetProperty(ref overallgrade, value);
        }
        public int TotalExams
        {
            get => totalExams;
            set => SetProperty(ref totalExams, value);
        }
        public int TotalStudents
        {
            get => totalStudents;
            set => SetProperty(ref  totalStudents, value);
        }
        public int PassedStudents
        {
            get => passedStudents;
            set => SetProperty(ref passedStudents, value);
        }
        public string PassRate
        {
            get => passRate;
            set => SetProperty(ref passRate, value);
        }

        public string StudentPassRate
        {
            get => studentPassRate;
            set => SetProperty(ref studentPassRate, value);
        }
        


        private void LoadUpcomingExams()
        {
            if(MockData.Exams == null || MockData.Exams.Count == 0)
            {
                UpcomingExams = new ObservableCollection<MockData.Exam>();
                return;
            }

            var upcoming = MockData.Exams
                .Where(e => e.Date >= DateTime.Today)
                .OrderBy(e => e.Date)
                .Take(5)
                .ToList();

            UpcomingExams =new ObservableCollection<MockData.Exam> (upcoming);
        }

        private void LoadRecentResults()
        {
            if (MockData.Results == null || MockData.Results.Count == 0)
            {
                RecentResults = new ObservableCollection<ResultWithColor>();
                return;
            }

            var recent = MockData.Results
                .OrderByDescending(r => r.MarksObtained)
                .Take(5)
                .Select(r => new ResultWithColor
                {
                    StudentName = r.StudentName,
                    Subject = r.Subject,
                    Course = r.Course,
                    MarksObtained = r.MarksObtained,
                    TotalMarks = r.TotalMarks,
                    Grade = r.Grade,
                    IsPass = r.IsPass,
                    //Percentage = r.Percentage,
                    //PassFailStatus = r.PassFailStatus,
                    StatusColor = r.IsPass ? Colors.Green : Colors.Red
                })
                .ToList();

            RecentResults = new ObservableCollection<ResultWithColor>(recent);
        }

        public void CalculateOverallStatistics()
        {
            if(MockData.Results == null || MockData.Results.Count == 0)
            {
                SetDefaultValues();
                return;
            }

            double totalMarks = 0;
            double obtainedMarks = 0;

            TotalStudents = MockData.Results
                .Select(r => r.StudentName)
                .Distinct()
                .Count();
            TotalExams = MockData.Results.Count;

            PassedStudents =MockData.Results.Count(r => r.IsPass);

            foreach(var result in MockData.Results)
            {
                totalMarks += result.TotalMarks;
                obtainedMarks += result.MarksObtained;
            }

            if(totalMarks > 0 )
            {
                OverallPercentage = (obtainedMarks / totalMarks) * 100;
            }
            else
            {
                OverallPercentage = 0;
            }

            OverallGrade = GetGrade(OverallPercentage);

            if (TotalExams > 0)
            {
                PassRate = $"{((double)PassedStudents / TotalExams * 100):F1}% Pass Rate";
            }
            else
            {
                PassRate = "0% Pass Rate"; 
            }

            if (MockData.Students != null && MockData.Students.Count > 0)
            {
                int studentsPassed = MockData.Students.Count(s => s.OverallStatus == "PASS");
                StudentPassRate = $"{((double)studentsPassed / MockData.Students.Count * 100):F1}%";
            }
            else
            {
                StudentPassRate = "0%";
            }
        }
        public void GenerateChartData()
        {
            if (MockData.Results == null || MockData.Results.Count == 0)
            {
                PassFailDatas.Clear();
                PassFailDatas.Add(new PassFailData { Category = "Pass", Count = 0 });
                PassFailDatas.Add(new PassFailData { Category = "ATKT", Count = 0 });
                PassFailDatas.Add(new PassFailData { Category = "Fail", Count = 0 });

                GradeDistributions.Clear();
                CoursePerformances.Clear();
                return;
            }

            int passCount = 0;
            int atktCount = 0;
            int failCount = 0;

            foreach (var student in MockData.Students)
            {
                var studentResults = MockData.Results.Where(r => r.StudentName == student.Name).ToList();

                if (!studentResults.Any())
                {
                    failCount++;
                    continue;
                }

                int failedSubjects = studentResults.Count(r => !r.IsPass);

                if (failedSubjects == 0)
                    passCount++;
                else if (failedSubjects <= 2)
                    atktCount++;
                else
                    failCount++;
            }

            // FIX: Update existing collection instead of creating new one
            PassFailDatas.Clear();
            PassFailDatas.Add(new PassFailData { Category = "Pass", Count = passCount });
            PassFailDatas.Add(new PassFailData { Category = "ATKT", Count = atktCount });
            PassFailDatas.Add(new PassFailData { Category = "Fail", Count = failCount });

            // FIX: Update existing collections (don't create new ones)
            var gradeGroups = MockData.Results
                .GroupBy(r => r.Grade)
                .Select(g => new GradeDistribution
                {
                    Grade = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Grade)
                .ToList();

            GradeDistributions.Clear();
            foreach (var item in gradeGroups)
            {
                GradeDistributions.Add(item);
            }

            var coursePerformanceData = MockData.Results
                .GroupBy(r => r.Course)
                .Select(g => new CoursePerformance
                {
                    Course = g.Key,
                    StudentCount = g.Select(r => r.StudentName).Distinct().Count(),
                    PassCount = g.Count(r => r.IsPass),
                    Percentage = g.Average(r => r.Percentage)
                })
                .OrderByDescending(c => c.Percentage)
                .ToList();

            CoursePerformances.Clear();
            foreach (var item in coursePerformanceData)
            {
                CoursePerformances.Add(item);
            }
        }

        private void SetDefaultValues()
        {
            OverallPercentage = 0;
            OverallGrade = "N/A";
            TotalStudents = 0;
            PassedStudents = 0;
            PassRate = "0%";
            StudentPassRate = "0%";
        }

        private string GetGrade(double percentage)
        {
            return percentage switch
            {
                >= 90 => "A+",
                >= 80 => "A",
                >= 70 => "B+",
                >= 60 => "B",
                >= 50 => "C",
                >= 40 => "D",
                _ => "F",
            };
        }
        public class PassFailData
        {
            public string Category { get; set; }
            public int Count { get; set; }
        }
        public class GradeDistribution
        {
            public string Grade { get; set; }
            public int Count { get; set; }
        }
        public class ResultWithColor : MockData.Result
        {
            public Color StatusColor { get; set; }
        }
        public class CoursePerformance
        {
            public string Course { get; set; }
            public double Percentage { get; set; }
            public int StudentCount { get; set; }
            public int PassCount { get; set; }

            public string PassRate => $"{((double)PassCount / StudentCount * 100):F1}%";
            
        }
    }
}
