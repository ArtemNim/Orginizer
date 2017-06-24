using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Threading;

namespace Orginizer
{
    class TableWorking
    {

        private static List<Note_Parent> New_Note_list = new List<Note_Parent>();
        public static void FillTable(Label label2, List<Note> list_note, DataGrid dataGridView)// заполняем таблицу
        {
            New_Note_list = new List<Note_Parent>();
            DateTime date;
            DateTime.TryParse(label2.Content.ToString(), out date);//lable2.Text
            for (int i = 0; i < list_note.Count; i++)
            {
                if ((list_note[i].date.ToString().Split(' ')[0] == date.ToShortDateString()) && (list_note[i].login == AuthorizationWindow.user.name))
                {
                    New_Note_list.Add( new Note_Parent {date= list_note[i].date,text= list_note[i].text, state = list_note[i].state,notify = list_note[i].notify });
                }
            }
            dataGridView.ItemsSource = New_Note_list;
        }
        public static void RefreshTable(Label label2, List<Note> list_note, DataGrid dataGridView1)
        {
            DataWorking.GetData(out list_note, "data.xml");
            TableWorking.FillTable(label2, list_note, dataGridView1);

        }//перезагрузить таблицу

        public static void time(System.ComponentModel.BackgroundWorker backgroundWorker1, List<Note> list_note)
        {
            DateTime now = DateTime.Now;
            backgroundWorker1.WorkerSupportsCancellation = true;
            while (true)
            {
                DataWorking.GetData(out list_note, "data.xml");
                for (int i = 0; i < list_note.Count; i++)
                {
                    if ((list_note[i].date.Day == DateTime.Now.Day) && (list_note[i].date.Hour == DateTime.Now.Hour) && (list_note[i].date.Minute == DateTime.Now.Minute) && (list_note[i].state == "В процессе") && (list_note[i].login == AuthorizationWindow.user.name)&&(list_note[i].notify == true))
                    {
                        backgroundWorker1.WorkerReportsProgress = true;
                        backgroundWorker1.ReportProgress(0, list_note[i].text);
                        break;
                    }
                   
                }
                Thread.Sleep(10000);

            }

        }
    }
}
