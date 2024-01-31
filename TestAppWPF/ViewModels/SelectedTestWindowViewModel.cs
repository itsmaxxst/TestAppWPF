using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestAppWPF.BaseModels;
using TestAppWPF.Models;

namespace TestAppWPF.ViewModels
{
    public class SelectedTestWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<QuestionViewModel> _questions;
        public int _currentQuestionIndex;
        private int _correctAnswersCount;

        public ObservableCollection<QuestionViewModel> Questions
        {
            get { return _questions; }
            set
            {
                if (_questions != value)
                {
                    _questions = value;
                    OnPropertyChanged(nameof(Questions));
                }
            }
        }

        public QuestionViewModel CurrentQuestion => _currentQuestionIndex >= 0 && _currentQuestionIndex < _questions.Count? _questions[_currentQuestionIndex]: null;

        public ICommand SubmitAnswerCommand { get; }

        public SelectedTestWindowViewModel(List<Question> questions)
        {
            _questions = new ObservableCollection<QuestionViewModel>(questions.Select(q => new QuestionViewModel(q)));
            _currentQuestionIndex = -1;
            _correctAnswersCount = 0;
            SubmitAnswerCommand = new RelayCommand(SubmitAnswer);
            NextQuestion(); //First question
        }
        public int CorrectAnswersCount
        {
            get { return _correctAnswersCount; }
            set
            {
                if (_correctAnswersCount != value)
                {
                    _correctAnswersCount = value;
                    OnPropertyChanged(nameof(CorrectAnswersCount));
                }
            }
        }
        private void ShowTestResult()
        {
            MessageBox.Show($"Your result: {CorrectAnswersCount}/{_questions.Count} correct answers", "Test Result");
            double percentage = (double)CorrectAnswersCount / _questions.Count * 100;

            using (var dbContext = new Context())
            {
                // Get the User with the biggest Id
                var userWithMaxId = dbContext.Users.OrderByDescending(u => u.Id).FirstOrDefault();

                if (userWithMaxId != null)
                {
                    var result = new Result
                    {
                        UserId = userWithMaxId.Id,
                        Score = (int)percentage,
                        TestId = _questions.FirstOrDefault()?.TestId ?? 0
                    };
                    dbContext.Results.Add(result);
                    dbContext.SaveChanges();
                }
            }

            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }
        }
        public void NextQuestion(object parameter = null)
        {
            _currentQuestionIndex++;

            if (_currentQuestionIndex < _questions.Count)
            {
                OnPropertyChanged(nameof(CurrentQuestion));
                //Debug.WriteLine("NextQuestion executed");
            }
            else
            {
                ShowTestResult();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static SelectedTestWindowViewModel CreateFromDatabase(int selectedTestId)
        {
            using (var dbContext = new Context())
            {
                var questionsFromDatabase = dbContext.Questions
                    .Include(q => q.Answers)
                    .Where(q => q.TestId == selectedTestId)
                    .ToList();

                var viewModel = new SelectedTestWindowViewModel(questionsFromDatabase);

                return viewModel;
            }
        }
        public void SubmitAnswer(object parameter)
        {
            var selectedAnswer = CurrentQuestion?.SelectedAnswer;

            if (selectedAnswer != null)
            {
                Debug.WriteLine($"Selected Answer: {selectedAnswer.AnswerText}, AnswerStatus: {selectedAnswer.AnswerStatus}");

                if (selectedAnswer.AnswerStatus == "True")
                {
                    CorrectAnswersCount++;
                    Debug.WriteLine($"Correct! Current score: {CorrectAnswersCount}");
                }
                else
                {
                    Debug.WriteLine($"False. AnswerStatus: {selectedAnswer.AnswerStatus}");
                }

                Debug.WriteLine($"Current score: {CorrectAnswersCount}");
                NextQuestion();
            }
            else
            {
                Debug.WriteLine("Unable to find an answer.");
            }
        }
    }
}
