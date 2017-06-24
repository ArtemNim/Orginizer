using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Orginizer
{
    class DataWorking
    {
        public static void GetData(out List<Note> list_note, string path)// получаем данные, имеющиеся в файле
        {
            XmlSerializer x = new XmlSerializer(typeof(List<Note>));
            Stream s = File.OpenRead(path);
            list_note = (List<Note>)x.Deserialize(s);
            s.Close();
        }

        public static void GetData(out List<User> list_note, string path)// получаем данные, имеющиеся в файле
        {
            list_note = new List<User>();
            XmlSerializer x = new XmlSerializer(typeof(List<User>));
            Stream s = File.OpenRead(path);
            list_note = (List<User>)x.Deserialize(s);
            s.Close();
        }

        public static void GetData(out List<Diary> list_note, string path)// получаем данные, имеющиеся в файле
        {
            list_note = new List<Diary>();
            XmlSerializer x = new XmlSerializer(typeof(List<Diary>));
            Stream s = File.OpenRead(path);
            list_note = (List<Diary>)x.Deserialize(s);
            s.Close();
        }
      
        public static void WriteData(List<Note> list_note, string path)// записываем данные в файл
        {
            XmlSerializer x = new XmlSerializer(typeof(List<Note>));
            Stream st = File.OpenWrite(path);
            x.Serialize(st, list_note);
            st.Close();

        }
        public static void WriteData(List<User> list_note, string path)// записываем данные в файл
        {
            XmlSerializer x = new XmlSerializer(typeof(List<User>));
            Stream st = File.OpenWrite(path);
            x.Serialize(st, list_note);
            st.Close();
        }

       

    }
}
