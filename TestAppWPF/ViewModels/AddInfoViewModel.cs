using System;
using System.Collections.Generic;
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
    public class AddInfoViewModel : INotifyPropertyChanged
    {
        public Test _test;
        public string _questionText;
        private string _answerText1;
        private string _answerText2;
        private string _answerText3;
        private string _answerText4;
        private bool _isAnswer1Checked;
        private bool _isAnswer2Checked;
        private bool _isAnswer3Checked;
        private bool _isAnswer4Checked;
        private string _answerStatus1 = "False";
        private string _answerStatus2 = "False";
        private string _answerStatus3 = "False";
        private string _answerStatus4 = "False";
        public bool IsAnswer1Checked
        {
            get { return _isAnswer1Checked; }
            set
            {
                if (_isAnswer1Checked != value)
                {
                    _isAnswer1Checked = value;
                    OnPropertyChanged(nameof(IsAnswer1Checked));
                    AnswerStatus1 = value ? "True" : "False";
                    Debug.WriteLine($"Answer 1 Status: {AnswerStatus1}");
                }
            }
        }
        public bool IsAnswer2Checked
        {
            get { return _isAnswer2Checked; }
            set
            {
                if (_isAnswer2Checked != value)
                {
                    _isAnswer2Checked = value;
                    OnPropertyChanged(nameof(IsAnswer2Checked));
                    AnswerStatus2 = value ? "True" : "False";
                    Debug.WriteLine($"Answer 2 Status: {AnswerStatus2}");
                }
            }
        }
        public bool IsAnswer3Checked
        {
            get { return _isAnswer3Checked; }
            set
            {
                if (_isAnswer3Checked != value)
                {
                    _isAnswer3Checked = value;
                    OnPropertyChanged(nameof(IsAnswer3Checked));
                    AnswerStatus3 = value ? "True" : "False";
                    Debug.WriteLine($"Answer 3 Status: {AnswerStatus3}");
                }
            }
        }
        public bool IsAnswer4Checked
        {
            get { return _isAnswer4Checked; }
            set
            {
                if (_isAnswer4Checked != value)
                {
                    _isAnswer4Checked = value;
                    OnPropertyChanged(nameof(IsAnswer4Checked));
                    AnswerStatus4 = value ? "True" : "False";
                    Debug.WriteLine($"Answer 4 Status: {AnswerStatus4}");
                }
            }
        }

        public string AnswerText1
        {
            get { return _answerText1; }
            set
            {
                if (_answerText1 != value)
                {
                    _answerText1 = value;
                    OnPropertyChanged(nameof(AnswerText1));
                }
            }
        }

        public string AnswerText2
        {
            get { return _answerText2; }
            set
            {
                if (_answerText2 != value)
                {
                    _answerText2 = value;
                    OnPropertyChanged(nameof(AnswerText2));
                }
            }
        }

        public string AnswerText3
        {
            get { return _answerText3; }
            set
            {
                if (_answerText3 != value)
                {
                    _answerText3 = value;
                    OnPropertyChanged(nameof(AnswerText3));
                }
            }
        }

        public string AnswerText4
        {
            get { return _answerText4; }
            set
            {
                if (_answerText4 != value)
                {
                    _answerText4 = value;
                    OnPropertyChanged(nameof(AnswerText4));
                }
            }
        }
        public string AnswerStatus1
        {
            get { return _answerStatus1; }
            set
            {
                if (_answerStatus1 != value)
                {
                    _answerStatus1 = value;
                    OnPropertyChanged(nameof(AnswerStatus1));
                }
            }
        }
        public string AnswerStatus2
        {
            get { return _answerStatus2; }
            set
            {
                if (_answerStatus2 != value)
                {
                    _answerStatus2 = value;
                    OnPropertyChanged(nameof(AnswerStatus2));
                }
            }
        }
        public string AnswerStatus3
        {
            get { return _answerStatus3; }
            set
            {
                if (_answerStatus3 != value)
                {
                    _answerStatus3 = value;
                    OnPropertyChanged(nameof(AnswerStatus3));
                }
            }
        }
        public string AnswerStatus4
        {
            get { return _answerStatus4; }
            set
            {
                if (_answerStatus1 != value)
                {
                    _answerStatus4 = value;
                    OnPropertyChanged(nameof(AnswerStatus4));
                }
            }
        }

        public string QuestionText
        {
            get { return _questionText; }
            set
            {
                if (_questionText != value)
                {
                    _questionText = value;
                    OnPropertyChanged(nameof(QuestionText));
                }
            }
        }
        public ICommand AddAnswersCommand { get; }
        public ICommand AddQuestionCommand { get; }
        public ICommand FinishCommand { get; }

        public AddInfoViewModel()
        {
            AddQuestionCommand = new RelayCommand(AddQuestionWithAnswers);
            FinishCommand = new RelayCommand(Finish);
        }
        private void Finish(object parameter) 
        {
            MessageBox.Show("Your test has been saved");
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }
        }
        private void AddQuestionWithAnswers(object parameter)
        {
            using (var dbContext = new Context())
            {
                // Get biggest Id for Test/Question
                var maxTestId = dbContext.Tests.Max(t => t.Id);

                // Add new question
                var newQuestion = new Question { TestId = maxTestId, QuestionText = QuestionText };
                dbContext.Questions.Add(newQuestion);
                dbContext.SaveChanges();  // Save changes to get the generated Id

                // Use the generated Id for answers
                var newAnswer1 = new Answer { TestId = maxTestId, QuestionId = newQuestion.Id, AnswerText = AnswerText1, AnswerStatus = AnswerStatus1 };
                var newAnswer2 = new Answer { TestId = maxTestId, QuestionId = newQuestion.Id, AnswerText = AnswerText2, AnswerStatus = AnswerStatus2 };
                var newAnswer3 = new Answer { TestId = maxTestId, QuestionId = newQuestion.Id, AnswerText = AnswerText3, AnswerStatus = AnswerStatus3 };
                var newAnswer4 = new Answer { TestId = maxTestId, QuestionId = newQuestion.Id, AnswerText = AnswerText4, AnswerStatus = AnswerStatus4 };

                // Add answers to Db
                dbContext.Answers.AddRange(newAnswer1, newAnswer2, newAnswer3, newAnswer4);
                dbContext.SaveChanges();

                //Clears the TextBoxs
                QuestionText = string.Empty;
                AnswerText1 = string.Empty;
                AnswerText2 = string.Empty;
                AnswerText3 = string.Empty;
                AnswerText4 = string.Empty;

                // Uncheck RadioButton values
                IsAnswer1Checked = false;
                IsAnswer2Checked = false;
                IsAnswer3Checked = false;
                IsAnswer4Checked = false;

                //Sets the AnswerStatus to false by default
                _answerStatus1 = "False";
                _answerStatus2 = "False";
                _answerStatus3 = "False";
                _answerStatus4 = "False";
            }
            MessageBox.Show("Your question has been added to the test");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
 }
