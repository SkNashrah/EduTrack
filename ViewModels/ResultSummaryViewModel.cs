
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamPortal.ViewModels
{
    public class ResultSummaryViewModel : BaseViewModel
    {
        public ResultSummaryViewModel() : base()
        {
            MockData.InitializeData();
            LoadCourses();
            LoadStudentMarksheets();
        }

        private ObservableCollection<StudentMarksheet> studentMarksheets;
        private ObservableCollection<string> courses;
        private string selectedCourse;
        private string grade;
        private string passRate;
        private int totalStudents;
        private int passedStudents;
        private double overallPercentage;
        private bool isPass;

        public ObservableCollection<string> Courses
        {
            get => courses;
            set => SetProperty(ref courses, value);
        }
        public string SelectedCourse
        {
            get => selectedCourse;
            set
            {
                SetProperty(ref selectedCourse, value);
                LoadStudentMarksheets();
            }
        }
        public ObservableCollection<StudentMarksheet> StudentMarksheets
        {
            get => studentMarksheets;
            set => SetProperty(ref studentMarksheets, value);
        }
        public double OverallPercentage
        {
            get => overallPercentage;
            set => SetProperty(ref overallPercentage, value);
        }
        public string Grade
        {
            get => grade;
            set => SetProperty(ref grade, value);
        }
        public bool IsPass
        {
            get => isPass;
            set => SetProperty(ref isPass, value);
        }
        public int TotalStudents
        {
            get => totalStudents;
            set => SetProperty(ref totalStudents, value);
        }
        public int PassedStudents
        {
            get => passedStudents;
            set => SetProperty(ref passedStudents, value);
        }

        public string PassRate
        {
            get => passRate;
            set => SetProperty(ref  passRate, value);
        }

        
        

        private void LoadCourses()
        {
            Courses = MockData.Courses;
            SelectedCourse = null;
        }

        private void LoadStudentMarksheets()
        {
            List<MockData.Student> courseStudents;

            if (SelectedCourse == null || SelectedCourse == "All")
            {
                courseStudents = MockData.Students.ToList();
            }
            else
            {
                courseStudents = MockData.Students
                    .Where(s => s.Course == SelectedCourse)
                    .ToList();
            }

                

            var marksheets = new List<StudentMarksheet>();

            foreach (var student in courseStudents)
            {
                var studentResults = MockData.Results
                    .Where(r => r.StudentName == student.Name && (SelectedCourse == null || SelectedCourse == "All" || r.Course == SelectedCourse) )
                    .ToList();
                
                if(studentResults.Count == 5)
                {
                    var marksheet = new StudentMarksheet
                    {
                        StudentName = student.Name,
                        Course = student.Course,
                        Year = student.Year,
                        Semester = student.Semester,
                        Subjects = new ObservableCollection<SubjectResult>()
                    };

                    double  totalObtained = 0;
                    double totalMarks = 0;
                    int failedSubjects = 0;

                    foreach (var result in studentResults)
                    {
                        marksheet.Subjects.Add(new SubjectResult
                        {
                            SubjectName = result.Subject,
                            MarksObtained = result.MarksObtained,
                            TotalMarks = result.TotalMarks,
                            Grade = result.Grade,
                            IsPass = result.IsPass
                        });
                        totalObtained += result.MarksObtained;
                        totalMarks += result.TotalMarks;

                        if (!result.IsPass)
                        {
                            failedSubjects++;
                        }
                    }

                    marksheet.TotalMarksObtained = totalObtained;
                    marksheet.TotalMarks = totalMarks;
                    marksheet.Percentage = (totalObtained / totalMarks) * 100 ;
                    marksheet.OverallGrade = GetGrade(marksheet.Percentage);
                    marksheet.IsPass = marksheet.Percentage >= 40;

                    if (failedSubjects == 0)
                    {
                        marksheet.OverallStatus = "PASS";
                        marksheet.StatusColor = "Green";
                    }
                    else if (failedSubjects <= 2) 
                    {
                        marksheet.OverallStatus = "ATKT";
                        marksheet.StatusColor = "Orange";
                        marksheet.IsPass = false; 
                    }
                    else 
                    {
                        marksheet.OverallStatus = "FAIL";
                        marksheet.StatusColor = "Red";
                        marksheet.IsPass = false;
                    }

                    marksheets.Add(marksheet);
                }
            }

            StudentMarksheets = new ObservableCollection<StudentMarksheet>(marksheets.OrderByDescending(m => m.Percentage));

            CalculateOverallStatistics();
        }


        private void CalculateOverallStatistics()
        {
            if (StudentMarksheets == null || StudentMarksheets.Count == 0)
            {
                OverallPercentage = 0;
                Grade = "N/A";
                IsPass = false;
                TotalStudents = 0;
                PassedStudents = 0;
                PassRate = "0%";
                return;
            }


            TotalStudents = StudentMarksheets.Count;
            PassedStudents = StudentMarksheets.Count(m => m.Percentage >= 40);

            if (TotalStudents > 0)
            {
                PassRate = $"{((double)PassedStudents / TotalStudents * 100):F1}%";
            }
            else
            {
                PassRate = "0%";
            }
           
            double totalPercentage = 0;

            foreach (var marksheet in StudentMarksheets)
            {
                totalPercentage += marksheet.Percentage;
            }

            OverallPercentage = totalPercentage / TotalStudents;
            Grade = GetGrade(OverallPercentage);
            IsPass = OverallPercentage >= 40;
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
    }

    public class StudentMarksheet 
    {
        public string StudentName { get; set; }
        public string Course { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
        public ObservableCollection<SubjectResult> Subjects { get; set; }
        public double TotalMarksObtained { get; set; }
        public double TotalMarks { get; set; }
        public double Percentage { get; set; }
        public string OverallGrade { get; set; }
        public bool IsPass { get; set; }
        public string OverallStatus { get; set; }
        public string StatusColor { get; set; } // "Green" for Pass, "Red" for Fail
    }

    public class SubjectResult
    {
        public string SubjectName { get; set; }
        public double MarksObtained { get; set; }
        public double TotalMarks { get; set; }
        public string Grade { get; set; }
        public bool IsPass { get; set; }
    }
}
