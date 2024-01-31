using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TestAppWPF.BaseModels;
using TestAppWPF.Models;

namespace TestAppWPF.ViewModels
{
    public class QuestionViewModel : INotifyPropertyChanged
    {
        private Question _question;
        public QuestionViewModel(Question question)
        {
            _question = question;
            Answers = _question.Answers.Select(a =>
            {
                var answerViewModel = new AnswerViewModel(a);
                Console.WriteLine($"AnswerStatus in QuestionViewModel: {answerViewModel.AnswerStatus}");
                return answerViewModel;
            }).ToList();
        }

        public int Id
        {
            get { return _question.Id; }
            set
            {
                if (_question.Id != value)
                {
                    _question.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public int TestId
        {
            get { return _question.TestId; }
            set
            {
                if (_question.TestId != value)
                {
                    _question.TestId = value;
                    OnPropertyChanged(nameof(TestId));
                }
            }
        }

        public string QuestionText
        {
            get { return _question.QuestionText; }
            set
            {
                if (_question.QuestionText != value)
                {
                    _question.QuestionText = value;
                    OnPropertyChanged(nameof(QuestionText));
                }
            }
        }

        public List<AnswerViewModel> Answers
        {
            get { return _answers; }
            set
            {
                if (_answers != value)
                {
                    _answers = value;
                    OnPropertyChanged(nameof(Answers));

                    // Events for every answer
                    foreach (var answer in _answers)
                    {
                        answer.PropertyChanged += AnswerPropertyChanged;
                    }
                }
            }
        }

        private AnswerViewModel _selectedAnswer;

        public AnswerViewModel SelectedAnswer
        {
            get { return _selectedAnswer; }
            set
            {
                if (_selectedAnswer != value)
                {
                    _selectedAnswer = value;
                    OnPropertyChanged(nameof(SelectedAnswer));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<AnswerViewModel> _answers;

        // Answers change
        private void AnswerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AnswerViewModel.IsSelected))
            {
                OnPropertyChanged(nameof(SelectedAnswer));
            }
        }

    }
}
