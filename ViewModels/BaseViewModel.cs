using MyMauiApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamPortal.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel(bool createProfileVm = true)
        {
            if (createProfileVm)
            {
                ProfileVm = new ProfileViewModel();
                ShowProfileCommand = new Command(() => ProfileVm.Show());
            }

            NavigateToDashboardCommand = new Command(Dashboard);
            NavigateToCalendarCommand = new Command(Calendar);
            NavigateToTimeTableCommand = new Command(TimeTable);
            NavigateToResultSummaryCommand = new Command(ResultSummary);
            NavigateToAddExamCommand = new Command(AddExam);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private ProfileViewModel profileVm;
        
        public ProfileViewModel ProfileVm
        {
            get => profileVm;
            set => SetProperty(ref profileVm, value);
        }
        public ICommand NavigateToDashboardCommand { get; set; }
        public ICommand NavigateToProfileCommand { get; set; }
        public ICommand NavigateToCalendarCommand { get; set; }
        public ICommand NavigateToTimeTableCommand { get; set; }
        public ICommand NavigateToResultSummaryCommand { get; set; }
        public ICommand NavigateToAddExamCommand { get; set; }
        public ICommand ShowProfileCommand { get; }

        private void Dashboard()
        {
            Application.Current.MainPage.Navigation.PushAsync(new Dashboard());
        }

        
        private void Calendar()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ExamCalendar());
        }
        private void TimeTable()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TimeTable());
        }
        private void ResultSummary()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ResultSummary());
        }
        private void AddExam()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AddExamSchedule());
        }
        protected void Notify([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            Notify(propName);
            return true;
        }
    }
}
