using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace Orginizer
{
   
    public class Diary
    {

        public static void Load(RichTextBox richTextBox, Label label1) 
        {
            TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (FileStream fs = new FileStream(@"diary\" + label1.Content + ".rtf", FileMode.Open))
            {
                doc.Load(fs, DataFormats.Rtf);
            }
        }
        public static void Save(RichTextBox richTextBox, Label label1)
        {
            TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (FileStream fs = File.Create(@"diary\" + label1.Content + ".rtf"))
            {
                doc.Save(fs, DataFormats.Rtf);
            }
        }
    }
}
