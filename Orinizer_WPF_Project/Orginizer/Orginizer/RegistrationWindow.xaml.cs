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
using System.Security.Cryptography;
using System.Net.Mail;
using Orginizer;

namespace Orginizer
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public static List<User> user_list = new List<User>();
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool name = true;
            bool email = true;

            try
            {
                MailAddress m = new MailAddress(textBox2.Text);
               
                if (textBox3.Password == textBox4.Password)
                {
                    User user = new User();
                    user.email = textBox2.Text;
                    user.name = textBox1.Text;
                    user.password = Sha_512.GetHash(textBox3.Password);
                    user.islocal = (bool)radioButton1.IsChecked;

                    DataWorking.GetData(out user_list, "users.xml");

                    for (int i = 0; i < user_list.Count; i++)
                    {
                        if (user_list[i].name == user.name)
                          name = false;
                        if (user_list[i].email == user.email)
                            email = false;
                    }
                       

                    
                    if (name == true && email  == true)
                    {
                        user_list.Add(user);
                        DataWorking.WriteData(user_list, "users.xml");
                        MessageBox.Show("Учетная запись была создана");
                    }
                    if(name == false)
                    {
                        MessageBox.Show("Это имя уже занято");
                    }
                    if (email == false)
                        MessageBox.Show("Этот email уже занят");
                    
                }
                else
                    MessageBox.Show("Пароли не совпадают");
            }
            catch { MessageBox.Show("Некорректный email."); }
        }

    }
    public class Sha_512
    {
        public static string GetHash(string pass)
        {
            byte[] data = Encoding.Default.GetBytes(pass);
            HashAlgorithm ha = HashAlgorithm.Create("SHA-512");
            byte[] hash_code = ha.ComputeHash(data);
            string hash = "";
            for (int i = 0; i < hash_code.Length; i++)
                hash += hash_code[i] + " ";
            return hash;
        }
    }
    public class User
    {
        public string name;
        public string email;
        public string password;

        public bool islocal;

    }
}
