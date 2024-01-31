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
    public class RoleViewModel: INotifyPropertyChanged
    {
        //Get info from Db 
        private Role _role;

        //Constructor
        public RoleViewModel(Role role)
        {
            _role = role;
        }

        public int Id
        {
            get { return _role.Id; } //Current value
            set //New value
            {
                if (_role.Id != value) //Checks if the old value is different from the new one
                {
                    _role.Id = value; //Sets new value instead of the old value
                    OnPropertyChanged(nameof(Id)); //Notifies that the old value has been replaced by the new one
                }
            }
        }

        public string Name
        {
            get { return _role.Name; } //Current value
            set //New value
            {
                if (_role.Name != value) //Checks if the old value is different from the new one
                {
                    _role.Name = value; //Sets new value instead of the old value
                    OnPropertyChanged(nameof(Name)); //Notifies that the old value has been replaced by the new one
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
