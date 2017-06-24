using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Orginizer
{
    class Authorize
    {
        public static void Local_Authorize(out List<User> user_list, User user)
        {
            bool a = false;
            DataWorking.GetData(out user_list, "users.xml");
            for (int i = 0; i < user_list.Count; i++)
            {
                if ((user_list[i].name == user.name) && (user_list[i].password == user.password))
                {
                    a = true;
                    MessageBox.Show("Выполнен успешный вход");
                    MainWindow f = new Orginizer.MainWindow();
                    f.Show();
                    break;
                }
            }
            if (a == false) MessageBox.Show("Неверный логин или пароль.");
        }
        //public static void Online_Authorize(out List<User> user_list, User user)
        //{

        //}
    }
}
