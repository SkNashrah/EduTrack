using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExamPortal.ViewModels
{
    public static class MockData
    {
        private static ObservableCollection<Exam> exams;
        private static ObservableCollection<Result> results;
        private static ObservableCollection<Student> students;
        private static ObservableCollection<string> courses;
        private static Random random = new Random();
        private static bool isInitialized = false;

        public static ObservableCollection<Exam> Exams
        {
            get => exams;
            set => exams = value;
        }

        public static ObservableCollection<Exam> UpcomingExams =>
            new ObservableCollection<Exam>(exams.Where(e => e.Date > DateTime.Today));

        public static ObservableCollection<Result> Results
        {
            get => results;
            set => results = value;
        }

        public static ObservableCollection<Student> Students
        {
            get => students;
            set => students = value;
        }

        public static ObservableCollection<string> Courses
        {
            get
            {
                if (courses == null)
                {
                    courses = new ObservableCollection<string>
                    {
                        "All",
                        "B.C.S",
                        "BSc CS",
                        "B.C.A",
                        "BSc",
                        "BA"
                    };
                }
                return courses;
            }
        }

        public static void InitializeData()
        {
            if (isInitialized)
            {
                CalculateStudentStatus();
                return;
            }

            InitializeStudents();
            InitializeExams();
            InitializeResults();
            CalculateStudentStatus();
            isInitialized = true;
        }

        private static void InitializeStudents()
        {
            students = new ObservableCollection<Student>
            {
                new Student
                {
                    StudentId = "STU001",
                    Name = "Alice Jhonson",
                    Course = "B.C.S",
                    Year = 2,
                    Semester = 1
                },
                new Student
                {
                    StudentId = "STU002",
                    Name = "Tyler Lockwood",
                    Course = "BSc",
                    Year = 3,
                    Semester = 5
                },
                new Student
                {
                    StudentId = "STU003",
                    Name = "Emily Cooper",
                    Course = "B.C.A",
                    Year = 4,
                    Semester = 8
                },
                new Student
                {
                    StudentId = "STU004",
                    Name = "Milli Holland",
                    Course = "B.C.S",
                    Year = 2,
                    Semester = 4
                },
                new Student
                {
                    StudentId = "STU005",
                    Name = "Emma Stark",
                    Course = "B.C.A",
                    Year = 3,
                    Semester = 5
                },
                new Student
                {
                    StudentId = "STU006",
                    Name = "Jenna Girbert",
                    Course = "BSc CS",
                    Year = 1,
                    Semester = 2
                },
                new Student
                {
                    StudentId = "STU007",
                    Name = "John Doe",
                    Course = "BA",
                    Year = 1,
                    Semester = 1
                }
            };
        }

        public static void InitializeExams()
        {
            exams = new ObservableCollection<Exam>();

            var subjectByCourse = new Dictionary<string, string[]>
    {
        {"B.C.S", new [] {"Data Structure", "Computer Networks", "Software Engineering", "Operating System", "DBMS"} },
        {"BSc CS", new[] { "Operating Systems", "Algorithms", "AI", "Data Structures", "Software Engineering" } },
        { "B.C.A", new[] { "Database Systems", "Web Development", "Data Structures", "Software Engineering", "Computer Networks" } },
        { "BSc", new[] { "Physics", "Chemistry", "Mathematics", "Biology", "Statistics" } },
        { "BA", new[] { "History", "Literature", "Psychology", "Sociology", "Economics" } }
    };

            foreach (var student in students)
            {
                if (subjectByCourse.ContainsKey(student.Course))
                {
                    var subjects = subjectByCourse[student.Course];

                    // Generate 5 past exams for each student
                    for (int i = 0; i < 5; i++)
                    {
                        var subject = subjects[i % subjects.Length]; // Cycle through subjects

                        exams.Add(new Exam
                        {
                            Course = student.Course,
                            Year = student.Year,
                            Semester = student.Semester,
                            Subject = subject,
                            Date = DateTime.Today.AddDays(-random.Next(30, 90)), // Past dates (30-90 days ago)
                            Time = "10:00 AM - 12:00 PM",
                            Venue = $"Room {random.Next(100, 300)}",
                            Description = $"{subject} Exam for {student.Course} Year {student.Year}",
                            IsUpcoming = false
                        });
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                var course = subjectByCourse.Keys.ElementAt(random.Next(subjectByCourse.Count));
                var subjects = subjectByCourse[course];
                var subject = subjects[random.Next(subjects.Length)];

                exams.Add(new Exam
                {
                    Course = course,
                    Year = random.Next(1, 5),
                    Semester = random.Next(1, 9),
                    Subject = subject,
                    Date = DateTime.Today.AddDays(random.Next(1, 60)), // Future date
                    Time = "10:00 AM - 12:00 PM",
                    Venue = $"Room {random.Next(100, 300)}",
                    Description = $"{subject} Exam for {course}",
                    IsUpcoming = true
                });
            }
        }

        private static void InitializeResults()
        {
            results = new ObservableCollection<Result>();

            var studentList = students.ToList();
            int atktCount = random.Next(1, 4);

            for (int i = 0; i < studentList.Count; i++)
            {
                var student = studentList[i];
                var studentExams = exams
                    .Where(e => e.Course == student.Course && e.Year == student.Year && e.Date < DateTime.Today)
                    .Take(5)
                    .ToList();

                string targetStatus;
                int targetFailCount;

                if (i == 0)
                {
                    targetStatus = "FAIL";
                    targetFailCount = 3;
                }
                else if (i <= atktCount)
                {
                    targetStatus = "ATKT";
                    targetFailCount = random.Next(1, 3);
                }
                else
                {
                    targetStatus = "PASS";
                    targetFailCount = 0;
                }

                int actualFailCount = 0;

                foreach (var exam in studentExams.Take(5))
                {
                    bool shouldFail = actualFailCount < targetFailCount;
                    double marks = shouldFail ? random.Next(0, 39) : random.Next(40, 101);

                    results.Add(new Result
                    {
                        StudentName = student.Name,
                        Course = student.Course,
                        Semester = student.Semester,
                        Subject = exam.Subject,
                        MarksObtained = marks,
                        TotalMarks = 100,
                        Grade = CalculateGrade(marks),
                        IsPass = !shouldFail
                    });

                    if (shouldFail) actualFailCount++;
                }

                student.OverallStatus = targetStatus;
            }
        }

        private static void CalculateStudentStatus()
        {
            if (students == null || results == null)
                return;

            foreach (var student in students)
            {
                var studentResults = results.Where(r => r.StudentName == student.Name).ToList();

                if (!studentResults.Any())
                {
                    student.OverallStatus = "N/A";
                    continue;
                }

                int failedCount = studentResults.Count(r => !r.IsPass);

                if (failedCount == 0)
                    student.OverallStatus = "PASS";
                else if (failedCount <= 2)
                    student.OverallStatus = "ATKT";
                else
                    student.OverallStatus = "FAIL";
            }
        }

        private static string CalculateGrade(double marks)
        {
            if (marks >= 90) return "A+";
            if (marks >= 80) return "A";
            if (marks >= 70) return "B+";
            if (marks >= 60) return "B";
            if (marks >= 50) return "C";
            if (marks >= 40) return "D";
            return "F";
        }

        public class Exam
        {
            public string Course { get; set; }
            public int Year { get; set; }
            public int Semester { get; set; }
            public string Subject { get; set; }
            public DateTime Date { get; set; }
            public string Time { get; set; }
            public string Venue { get; set; }
            public string Description { get; set; }
            public bool IsUpcoming { get; set; }
        }

        public class Student : BaseViewModel
        {
            private string studentId;
            private string name;
            private string course;
            private int year;
            private int semester;
            private string overallStatus;

            public string StudentId
            {
                get => studentId;
                set => SetProperty(ref studentId, value);
            }
            public string Name
            {
                get => name;
                set => SetProperty(ref name, value);
            }
            public string Course
            {
                get => course;
                set => SetProperty(ref course, value);
            }
            public int Year
            {
                get => year;
                set => SetProperty(ref year, value);
            }
            public int Semester
            {
                get => semester;
                set => SetProperty(ref semester, value);
            }
            public string OverallStatus
            {
                get => overallStatus;
                set => SetProperty(ref overallStatus, value);
            }
        }

        public class Result : BaseViewModel
        {
            private string studentName;
            private string course;
            private string subject;
            private int semester;
            private double marksObtained;
            private double totalMarks;
            private string grade;
            private bool isPass;

            public string StudentName
            {
                get => studentName;
                set => SetProperty(ref studentName, value);
            }
            public string Course
            {
                get => course;
                set => SetProperty(ref course, value);
            }
            public string Subject
            {
                get => subject;
                set => SetProperty(ref subject, value);
            }
            public int Semester
            {
                get => semester;
                set => SetProperty(ref semester, value);
            }
            public double MarksObtained
            {
                get => marksObtained;
                set => SetProperty(ref marksObtained, value);
            }
            public double TotalMarks
            {
                get => totalMarks;
                set => SetProperty(ref totalMarks, value);
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
            public double Percentage => TotalMarks > 0 ? (MarksObtained / TotalMarks) * 100 : 0;
            public string PassFailStatus => IsPass ? "Pass" : "Fail";
        }
    }
}