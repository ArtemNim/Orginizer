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

namespace Orginizer
{
    public class Note:Note_Parent
    {
        [XmlElement]
        public new DateTime date { get; set; }
        [XmlElement]
        public string text { get; set; }
        [XmlElement]
        public string state { get; set; }
        [XmlElement]
        public string login { get; set; }
        [XmlElement]
        public bool notify { get; set; }
        public override bool Equals(Object obj)
        {
            bool rez = false;
            if (obj != null)
            {
                Note n = obj as Note;
                if ((date == n.date) && (text == n.text))
                    rez = true;
            }


            return rez;
        }

        public static void Add(List<Note> list_note, Label label2, TextBox textBox1, TextBox textBox2, DataGrid dataGrid, CheckBox checkBox1, ComboBox comboBox1)
        {
            try
            {
                Note note = new Note();
                DateTime dt;
                DateTime.TryParse(label2.Content + " " + textBox1.Text, out dt);
                note.date = dt;
                note.text = textBox2.Text;
                note.state = comboBox1.Text;
                note.login = AuthorizationWindow.user.name;
                note.notify = (bool)checkBox1.IsChecked;
                DataWorking.GetData(out list_note, "data.xml");
                list_note.Add(note);//добавляет в список
                DataWorking.WriteData(list_note, "data.xml");
                TableWorking.RefreshTable(label2, list_note, dataGrid);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }// добавляем заметку

        public static void Delete(List<Note> list_note, Label label2, DataGrid dataGridView1, Note Selected_Note)
        {
            
            Note note = new Note();
            DateTime dt;
            DateTime.TryParse(Selected_Note.date.ToString(), out dt);
            note.date = dt;
            note.text = Selected_Note.text;
            DataWorking.GetData(out list_note, "data.xml");
            list_note.Remove(note);// удаляем из списка
            File.WriteAllText("data.xml", string.Empty);// очищаем файл
            DataWorking.WriteData(list_note, "data.xml");
            TableWorking.FillTable(label2,list_note,dataGridView1);
            }//удаляем заметку
    }
}
