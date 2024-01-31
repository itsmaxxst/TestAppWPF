using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestAppWPF.BaseModels;
using TestAppWPF.Models;

namespace TestAppWPF.ViewModels
{
    public class AllTestsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TestViewModel> _tests;

        public ObservableCollection<TestViewModel> Tests
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

        public AllTestsViewModel()
        {
            LoadTests();
        }

        private void LoadTests()
        {
            using (var context = new Context())
            {
                var testsFromDb = context.Tests.ToList();
                Tests = new ObservableCollection<TestViewModel>(testsFromDb.Select(test => new TestViewModel(test)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
