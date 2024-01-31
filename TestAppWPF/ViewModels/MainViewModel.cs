using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestAppWPF.BaseModels;
using TestAppWPF.Models;

namespace TestAppWPF.ViewModels
{
    public class MainViewModel
    {
        public ICommand OpenTestWindowCommand { get; }
        public ICommand OpenAddWindowCommand { get; }
        public ICommand OpenAllTestsWindowCommand { get; }
        public ICommand OpenResultWindowCommand { get; }
        public ICommand OpenTestsWindowCommand { get; }
        public UserViewModel UserVM { get; } = new UserViewModel(new User());
        public ObservableCollection<Role> Roles { get; set; } = new ObservableCollection<Role>();

        public MainViewModel()
        {
            OpenTestWindowCommand = new RelayCommand(OpenTestWindow);
            OpenAllTestsWindowCommand = new RelayCommand(OpenAllTestsWindow);
            OpenAddWindowCommand = new RelayCommand(OpenAddWindow);
            OpenResultWindowCommand = new RelayCommand(OpenResultWindow);
            OpenTestsWindowCommand = new RelayCommand(OpenTestsWindow);
            UserVM = new UserViewModel(new User());
            LoadRoles();
        }
        private void OpenTestWindow(object parameter)
        {
            var newWindow = new TestWindow(UserVM);

            //Closing the previus window
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }

            //TestWindow becomes the new MainWindow
            Application.Current.MainWindow = newWindow;
            newWindow.Show();

        }
        private void OpenTestsWindow(object parameter)
        {
            var newWindow = new TestsWindow();

            //Closing the previus window
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }

            //TestWindow becomes the new MainWindow
            Application.Current.MainWindow = newWindow;
            newWindow.Show();

        }
        private void OpenResultWindow(object parameter)
        {
            var newWindow = new ResultWindow();

            //Closing the previus window
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }

            //TestWindow becomes the new MainWindow
            Application.Current.MainWindow = newWindow;
            newWindow.Show();

        }
        private void OpenAllTestsWindow(object parameter)
        {
            var newWindow = new AllTestsWindow();
            
            //Closing the previus window
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }

            //AllTestsWindow becomes the new MainWindow
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
        }
        private void OpenAddWindow(object parameter)
        {
            var newWindow = new AddWindow();

            //Closing the previus window
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }

            //AllTestsWindow becomes the new MainWindow
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
        }

        public List<QuestionViewModel> GetQuestionsFromDatabase()
        {
            using (var dbContext = new Context())
            {
                // LINQ for DB
                return dbContext.Questions
                    .Include(q => q.Answers) // Upload questions with their answers
                    .Select(q => new QuestionViewModel(q))
                    .ToList();
            }
        }
        private void LoadRoles()
        {
            using (var dbContext = new Context())
            {
                Roles = new ObservableCollection<Role>(dbContext.Roles.Select(r => new Role { Id = r.Id, Name = r.Name }).ToList());
            }
        }
    }
}
