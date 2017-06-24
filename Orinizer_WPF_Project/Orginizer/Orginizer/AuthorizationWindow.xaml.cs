using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Orginizer
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public static User user;
        public static List<User> user_list = new List<User>();
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            user = new User();
            user.name = textBox1.Text;
            user.password = Sha_512.GetHash(textBox2.Password);
            user.islocal = (bool)radioButton1.IsChecked;

            if (user.islocal = true)
            {
                Authorize.Local_Authorize(out user_list, user);
            }
            else
            {
                // Authorize.Online_Authorize(out user_list, user);
            }
        }

        private void label3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegistrationWindow form = new RegistrationWindow();
            form.Show();
        }
    }
}
