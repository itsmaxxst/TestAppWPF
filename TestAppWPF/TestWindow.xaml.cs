using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestAppWPF.Models;
using TestAppWPF.ViewModels;

namespace TestAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow(UserViewModel userVM)
        {
            InitializeComponent();
            DataContext = userVM;
            DataContext = new MainViewModel();
            InitializeUserData();
            userVM.SaveUser();
            UpdateButtonsEnabled();
        }
        private void UpdateButtonsEnabled()
        {
            //Init methods
            bool isUserAdmin = IsUserAdmin();
            bool isRegularUser = IsRegularUser();
            //Setting buttons availability
            btnAddTest.IsEnabled = !isUserAdmin;
            btnResults.IsEnabled = !isUserAdmin;
            btnTests.IsEnabled = !isUserAdmin;
            btnAllTests.IsEnabled = !isRegularUser;
        }
        private bool IsUserAdmin()
        {
            using (var dbContext = new Context())
            {
                // Find User with biggest User.Id and User.RoleId==2
                int? maxUserIdWithRoleId2 = dbContext.Users
                     .Where(u => u.RoleId == 2)
                     .Select(u => (int?)u.Id)
                     .OrderByDescending(id => id)
                     .FirstOrDefault();

                int currentUserId = GetMaxUserId();
                return currentUserId == maxUserIdWithRoleId2;
            }
        }

        private int GetMaxUserId()
        {
            using (var dbContext = new Context())
            {
                //Find biggest User Id
                int maxUserId = dbContext.Users.Max(u => u.Id);
                return maxUserId;
            }
        }
        private bool IsRegularUser()
        {
            using (var dbContext = new Context())
            {
                int? maxUserIdWithRoleId1 = dbContext.Users
                    .Where(u => u.RoleId == 1)
                    .Select(u => (int?)u.Id)
                    .OrderByDescending(id => id)
                    .FirstOrDefault();

                int currentUserId = GetMaxUserId();

                // Пользователь существует и его RoleId == 1
                return maxUserIdWithRoleId1.HasValue && currentUserId == maxUserIdWithRoleId1.Value;
            }
        }
        private void InitializeUserData()
        {
            using (var dbContext = new Context())
            {
                var userWithMaxId = dbContext.Users.OrderByDescending(u => u.Id).FirstOrDefault();
                if (userWithMaxId != null)
                {
                    UserFullNameLabel.Content = $"{userWithMaxId.FirstName} {userWithMaxId.SecondName}";
                    var userRole = dbContext.Roles.FirstOrDefault(r => r.Id == userWithMaxId.RoleId);
                    if (userRole != null)
                    {
                        RoleLabel.Content = $"{userRole.Name}";
                    }
                }
            }
        }
    }
}