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
    public class UserViewModel : INotifyPropertyChanged
    {
        private User _user;
        private Role _selectedRole;

        public UserViewModel(User user)
        {
            _user = user;
        }

        public int Id
        {
            get { return _user.Id; }
            set
            {
                if (_user.Id != value)
                {
                    _user.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string FirstName
        {
            get { return _user.FirstName; }
            set
            {
                if (_user.FirstName != value)
                {
                    _user.FirstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }
        public string SecondName
        {
            get { return _user.SecondName; }
            set
            {
                if (_user.SecondName != value)
                {
                    _user.SecondName = value;
                    OnPropertyChanged(nameof(SecondName));
                }
            }
        }
        public int RoleId
        {
            get { return _user.RoleId; }
            set
            {
                if (_user.RoleId != value)
                {
                    _user.RoleId = value;
                    OnPropertyChanged(nameof(RoleId));
                }
            }
        }
        public Role SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;
                    OnPropertyChanged(nameof(SelectedRole));
                    RoleId = _selectedRole?.Id ?? 0;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Save user to DB
        public void SaveUser()
        {
            using (var context = new Context())
            {
                var selectedRole = context.Roles.Find(RoleId);
                if (selectedRole != null)
                {
                    _user.Role = selectedRole;
                }
                else
                {
                    MessageBox.Show("Selected role not found.", "Warning");
                    return;
                }

                if (_user.Id == 0)
                {
                    context.Users.Add(_user);
                }
                else
                {
                    var existingUser = context.Users.Find(_user.Id);
                    if (existingUser != null)
                    {
                        context.Entry(existingUser).CurrentValues.SetValues(_user);
                    }
                }

                context.SaveChanges();
            }
        }
    }

}
