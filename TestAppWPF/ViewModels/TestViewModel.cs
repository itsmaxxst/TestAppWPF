using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestAppWPF.BaseModels;
using TestAppWPF.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TestAppWPF.ViewModels
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public ICommand OpenSelectedTestWindowCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand DeleteCommand { get; }
        private Test _test;
        public TestViewModel(Test test)
        {
            _test = test;
            OpenSelectedTestWindowCommand = new RelayCommand(OpenSelectedTestWindow);
            NextCommand = new RelayCommand(Next);
            DeleteCommand = new RelayCommand(DeleteSelectedTests);
        }

        public int Id
        {
            get { return _test.Id; }
            set
            {
                if (_test.Id != value)
                {
                    _test.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get { return _test.Name; }
            set
            {
                if (_test.Name != value)
                {
                    _test.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private int _selectedTestId;

        public int SelectedTestId
        {
            get { return _selectedTestId; }
            set
            {
                if (_selectedTestId != value)
                {
                    _selectedTestId = value;
                    OnPropertyChanged(nameof(SelectedTestId));
                }
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OpenSelectedTestWindow(object parameter)
        {
            if (parameter is int selectedTestId)
            {
                SelectedTestId = selectedTestId;
                var viewModel = SelectedTestWindowViewModel.CreateFromDatabase(SelectedTestId);
                var selectedTestWindow = new SelectedTestWindow(viewModel);
                // Closing the previous window
                if (System.Windows.Application.Current.MainWindow != null)
                {
                    System.Windows.Application.Current.MainWindow.Close();
                }
                // SelectedTestWindow becomes the new MainWindow
                System.Windows.Application.Current.MainWindow = selectedTestWindow;
                selectedTestWindow.Show();
            }
        }
        private void Next(object parameter)
        {
            //Save to Db Test
            SaveTestData();
            var newWindow = new AddInfoWindow();

            //Closing the previus window
            if (System.Windows.Application.Current.MainWindow != null)
            {
                System.Windows.Application.Current.MainWindow.Close();
            }

            //AddInfoWindow becomes the new MainWindow
            System.Windows.Application.Current.MainWindow = newWindow;
            newWindow.Show();
        }
        private void SaveTestData()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                using (var dbContext = new Context())
                {
                    //Creating a new Test
                    Test newTest = new Test();
                    newTest.Name = Name;
                    dbContext.Tests.Add(newTest);
                    dbContext.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Please enter your question.", "Warning");
                if (System.Windows.Application.Current?.Windows.Count > 0)
                {
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        window.Close();
                    }
                }
            }
        }

        public ObservableCollection<Test> _tests;
        public ObservableCollection<Test> Tests
        {
            get { return _tests; }
            set
            {
                if (_tests != value)
                {
                    _tests = value;
                    OnPropertyChanged(nameof(Tests));
                }
            }
        }
        public void LoadTests()
        {
            using (var dbContext = new Context())
            {
                Tests = new ObservableCollection<Test>(dbContext.Tests.ToList());
            }
        }

        private void DeleteSelectedTests(object parameter)
        {
            var selectedTests = Tests.Where(t => t.IsSelected).ToList();

            if (selectedTests.Any())
            {
                using (var context = new Context())
                {
                    foreach (var test in selectedTests)
                    {
                        // Load not null questions
                        context.Entry(test).Collection(t => t.Questions).Load();

                        // Delete related results
                        var resultsToDelete = context.Results.Where(r => r.TestId == test.Id);
                        context.Results.RemoveRange(resultsToDelete);

                        // Delete related questions and answers
                        foreach (var question in test.Questions)
                        {
                            var answersToDelete = context.Answers.Where(a => a.QuestionId == question.Id);
                            context.Answers.RemoveRange(answersToDelete);
                        }
                        context.Questions.RemoveRange(test.Questions);

                        // Delete the test
                        context.Tests.Remove(test);

                        // Remove from collection
                        Tests.Remove(test);
                    }

                    context.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("No tests selected for deletion.", "Delete");
            }
        }

    }
}
