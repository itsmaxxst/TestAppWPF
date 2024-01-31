using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppWPF.Models;
using Microsoft.Win32;
using System.Windows;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using TestAppWPF.BaseModels;
using System.Diagnostics;

namespace TestAppWPF.ViewModels
{
    public class ResultViewModel : INotifyPropertyChanged
    {
        private Result _result;
        private bool _isSelected;
        public ICommand DownloadCommand { get; }
        public ICommand DeleteCommand { get; }

        public ResultViewModel()
        {
            DownloadCommand = new RelayCommand(Download);
            DeleteCommand = new RelayCommand(Delete);
        }
        public int Id
        {
            get { return _result.Id; } //Current value
            set //New value
            {
                if (_result.Id != value) //Checks if the old value is different from the new one
                {
                    _result.Id = value; //Sets new value instead of the old value
                    OnPropertyChanged(nameof(Id)); //Notifies that the old value has been replaced by the new one
                }
            }
        }
        public int UserId
        {
            get { return _result.UserId; }
            set
            {
                if (_result.UserId != value)
                {
                    _result.UserId = value;
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }

        public int TestId
        {
            get { return _result.TestId; }
            set
            {
                if (_result.TestId != value)
                {
                    _result.TestId = value;
                    OnPropertyChanged(nameof(TestId));
                }
            }
        }

        public int Score
        {
            get { return _result.Score; }
            set
            {
                if (_result.Score != value)
                {
                    _result.Score = value;
                    OnPropertyChanged(nameof(Score));
                }
            }
        }
        public ObservableCollection<Result> _results;
        public ObservableCollection<Result> Results
        {
            get { return _results; }
            set
            {
                if (_results != value)
                {
                    _results = value;
                    OnPropertyChanged(nameof(Results));
                }
            }
        }
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
        public void LoadResults()
        {
            using (var dbContext = new Context())
            {
                Results = new ObservableCollection<Result>(
                    dbContext.Results
                        .Include(r => r.User)
                        .Include(r => r.Test)
                        .ToList()
                );
            }
        }
        private void Download(object parameter)
        {
            Debug.WriteLine("Download method called.");
            var selectedResults = Results.Where(r => r.IsSelected).ToList();
            if (selectedResults.Any())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        // Write header
                        csv.WriteField("UserId");
                        csv.WriteField("FirstName");
                        csv.WriteField("SecondName");
                        csv.WriteField("TestName");
                        csv.WriteField("TestScore");
                        csv.NextRecord();

                        // Write data
                        foreach (var result in selectedResults)
                        {
                            csv.WriteField(result.UserId);
                            csv.WriteField(result.User.FirstName);
                            csv.WriteField(result.User.SecondName);
                            csv.WriteField(result.Test.Name);
                            csv.WriteField(result.Score);
                            csv.NextRecord();
                        }
                    }
                    MessageBox.Show("Download complete!", "Download");
                }
            }
            else
            {
                MessageBox.Show("No results selected for download.", "Download");
            }
        }

        private void Delete(object parameter)
        {
            var selectedResults = Results.Where(r => r.IsSelected).ToList();

            if (selectedResults.Any())
            {
                using (var dbContext = new Context())
                {
                    //Delete from Db
                    dbContext.Results.RemoveRange(selectedResults);
                    dbContext.SaveChanges();

                    //Delete from collection
                    foreach (var result in selectedResults)
                    {
                        Results.Remove(result);
                    }
                }

                MessageBox.Show("Selected results deleted.", "Delete");
            }
            else
            {
                MessageBox.Show("No results selected for deletion.", "Delete");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
