using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppWPF.Models;

namespace TestAppWPF.ViewModels
{
    public class AnswerViewModel: INotifyPropertyChanged
    {
        //Get info from Db 
        private Answer _answer;

        //Constructor
        public AnswerViewModel(Answer answer)
        {
            _answer = answer;
            AnswerStatus = _answer.AnswerStatus;
        }

        public int Id
        {
            get { return _answer.Id; } //Current value
            set //New value
            {
                if (_answer.Id != value) //Checks if the old value is different from the new one
                {
                    _answer.Id = value; //Sets new value instead of the old value
                    OnPropertyChanged(nameof(Id)); //Notifies that the old value has been replaced by the new one
                }
            }
        }

        public int TestId
        {
            get { return _answer.TestId; }
            set
            {
                if (_answer.TestId != value)
                {
                    _answer.TestId = value;
                    OnPropertyChanged(nameof(TestId));
                }
            }
        }

        public int QuestionId
        {
            get { return _answer.QuestionId; }
            set
            {
                if (_answer.QuestionId != value)
                {
                    _answer.QuestionId = value;
                    OnPropertyChanged(nameof(QuestionId));
                }
            }
        }

        public string AnswerText
        {
            get { return _answer.AnswerText; }
            set
            {
                if (_answer.AnswerText != value)
                {
                    _answer.AnswerText = value;
                    OnPropertyChanged(nameof(AnswerText));
                }
            }
        }

        private string _answerStatus;
        public string AnswerStatus
        {
            get { return _answerStatus; }
            set
            {
                if (_answerStatus != value)
                {
                    _answerStatus = value;
                    OnPropertyChanged(nameof(AnswerStatus));
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
        //Init interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
