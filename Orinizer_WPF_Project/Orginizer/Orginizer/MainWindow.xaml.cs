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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Drawing.Text;
using System.Threading;
using System.Reflection;
using System.IO;
using System.ComponentModel;

namespace Orginizer
{
    public partial class MainWindow : Window
    {
        public string message;
        private static int start = 0;
        private static int start2 = 0;
        public static int CurrentMonth = DateTime.Now.Month;
        public static int CurrentYear = DateTime.Now.Year;
        private static DockPanel[,] dock = new DockPanel[8, 8];
        private static List<Note> list_note = new List<Note>();
        private static Note Selected_Note;
        private static List<Diary> list_diary = new List<Diary>();
        public static System.Windows.Forms.NotifyIcon notifyIcon1 = new System.Windows.Forms.NotifyIcon();
        public static BackgroundWorker backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
        public static Image image = new Image();
        public static System.Windows.Forms.ContextMenuStrip contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
        public static System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        public static System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
        private void ShowDayNames(Label Mo, Label Tu, Label We, Label Th, Label Fr, Label Sa, Label Su)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            Mo.Content = dtfi.GetShortestDayName(DayOfWeek.Monday);
            Tu.Content = dtfi.GetShortestDayName(DayOfWeek.Tuesday);
            We.Content = dtfi.GetShortestDayName(DayOfWeek.Wednesday);
            Th.Content = dtfi.GetShortestDayName(DayOfWeek.Thursday);
            Fr.Content = dtfi.GetShortestDayName(DayOfWeek.Friday);
            Sa.Content = dtfi.GetShortestDayName(DayOfWeek.Saturday);
            Su.Content = dtfi.GetShortestDayName(DayOfWeek.Sunday);
        }
        private void ShowMonthNamesAndCurrentYear(int mounth, int year, Label label)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            label.Content = dtfi.MonthNames[mounth - 1];
            label.Content += " " + CurrentYear;
        }
        private void DeleteCalendar()
        {

            int c = 0;
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    grid1.Children.Remove(dock[j, i]);
                }
            }
        }
        private void FillCalendar(int CurrentYear, int CurrentMonth)
        {
            int[,] a = new int[7, 7];

            int c = 1;

            DateTime dt = new DateTime(CurrentYear, CurrentMonth, 1);
            switch (dt.DayOfWeek.ToString())
            {
                case "Monday":
                    start = 0;
                    break;
                case "Tuesday":
                    start = 1;
                    break;

                case "Wednesday":
                    start = 2;
                    break;
                case "Thursday":
                    start = 3;
                    break;
                case "Friday":
                    start = 4;
                    break;
                case "Saturday":
                    start = 5;
                    break;

                case "Sunday":
                    start = 6;
                    break;
            }
            start2 = start + 1;

            for (int j = 1; j < 7; j++)
            {
                for (int i = start; i < 7; i++)
                {
                    if (c > DateTime.DaysInMonth(CurrentYear, CurrentMonth))
                    { break; }
                    a[j, i] = c;
                    dock[j, i] = new DockPanel();
                    dock[j, i].LastChildFill = true;


                    Label label1 = new Label();
                    label1.Content = c;
                    label1.HorizontalAlignment = HorizontalAlignment.Center;
                    dock[j, i].Children.Add(label1);
                    dock[j, i].MouseDown += Dock_MouseDown;
                    DockPanel.SetDock(label1, Dock.Top);



                    grid1.Children.Add(dock[j, i]);
                    Grid.SetRow(dock[j, i], j);
                    Grid.SetColumn(dock[j, i], i);
                    c++;

                }
                start = 0;

            }

        }

        private void Reset()
        {
            Exchange_button.IsEnabled = false;
            Text_textBox.Text = "";
            Time_textBox.Text = "";
            Notify_checkBox.IsChecked = false;
        }
        public MainWindow()
        {
            InitializeComponent();

            #region Calendar initialization 
            FillCalendar(CurrentYear, CurrentMonth);
            ShowDayNames(Mo, Tu, We, Th, Fr, Sa, Su);
            ShowMonthNamesAndCurrentYear(CurrentMonth, CurrentYear, label3);

            int a = (DateTime.Now.Day + start2) % 8;
            int b = (DateTime.Now.Day + start2) / 8 + 1;
            dock[b, a].Background = Brushes.LightGray;
            #endregion

            #region Task tab's item initialization
            Date_label.Content = DateTime.Now.ToShortDateString();
            DataWorking.GetData(out list_note, "data.xml");
            for (int i = 0; i < list_note.Count; i++)
            {
                if ((list_note[i].date < DateTime.Now) && (list_note[i].state == "В процессе") && (list_note[i].login == AuthorizationWindow.user.name) && (list_note[i].notify == true))
                {
                    list_note[i].state = "Не выполнено";
                    DataWorking.WriteData(list_note, "data.xml");
                }
            }
            TableWorking.FillTable(Date_label, list_note, dataGrid);

            State_comboBox.Items.Add("В процессе");
            State_comboBox.Items.Add("Завершено");
            #endregion

            #region Diary tab's item initialization
            InstalledFontCollection ifc = new InstalledFontCollection();
            System.Drawing.FontFamily[] families = ifc.Families;
            for (int i = 0; i < families.Length; i++)
            {
                Label lb = new Label();
                lb.Content = families[i].Name;
                lb.FontFamily = new FontFamily(families[i].Name);
                comboBox1.Items.Add(lb);

            }
            comboBox1.Text = "Times New Roman";

            int[] FontSizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            for (int i = 0; i < FontSizes.Length; i++)
                comboBox2.Items.Add(FontSizes[i]);
            comboBox2.Text = "14";

            comboBox3.ItemsSource = typeof(Colors).GetProperties();

            try
            {
                Diary.Load(richTextBox, Date_label);
            }
            catch (Exception ex) { richTextBox.Document.Blocks.Clear(); }
            #endregion

            #region NotifyIcon initialization
            System.Drawing.Icon icon = new System.Drawing.Icon(@"G:\Orinizer_WPF_Project\Orginizer\Orginizer\Images\calendar.ico");
            notifyIcon1.Icon = icon;
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipClicked += new System.EventHandler(notifyIcon1_BalloonTipClicked);
            #endregion

            #region BackgroundWorker initialization
            backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerAsync();
            #endregion

            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] 
            {
                toolStripMenuItem1,
                toolStripMenuItem2
            });
            contextMenuStrip1.Visible = true;
            contextMenuStrip1.Size = new System.Drawing.Size(138, 48);

            toolStripMenuItem1.Text = "Завершено";
            toolStripMenuItem1.Click += new System.EventHandler(toolStripMenuItem1_Click);
            contextMenuStrip1.Size = new System.Drawing.Size(138, 48);

            toolStripMenuItem2.Text = "Отложить";
            toolStripMenuItem2.Click += new System.EventHandler(toolStripMenuItem2_Click);
            toolStripMenuItem2.Size = new System.Drawing.Size(137, 22);

        }

        private void grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Note_Parent note = (Note_Parent)e.Row.DataContext;

            if (note.state == "Не выполнено")
            {
                e.Row.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(System.Windows.Forms.Control.MousePosition);
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataWorking.GetData(out list_note,"data.xml");
            this.Show();
            Note note = new Note();
            DateTime dt = note.date;
            DateTime.TryParse(DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":00", out dt);
            note.date = dt;
            note.text = message;
            note.notify = true;
            note.state = "В процессе";
            note.login = AuthorizationWindow.user.name;
            list_note.Remove(note);// удаляем из списка
            note.state = "Завершено";
            note.notify = false;
            list_note.Add(note);
            File.WriteAllText("data.xml", string.Empty);// очищаем файл
            DataWorking.WriteData(list_note, "data.xml");
            TableWorking.RefreshTable(Date_label, list_note, dataGrid);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Show();
            // делаем редактирование заметки
            Note note = new Note();
            DateTime dt = note.date;
            DateTime.TryParse(DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":00", out dt);
            note.date = dt;
            note.text = message;
            note.state = "В процессе";
            note.notify = true;
            //monthCalendar1.SelectionStart = new DateTime(note.date.Year, note.date.Month, note.date.Day);
            Date_label.Content = note.date.ToShortDateString();
            Time_textBox.Text = note.date.ToShortTimeString().Split(':')[0] + ":" + note.date.ToShortTimeString().Split(':')[1];
            Text_textBox.Text = note.text;
            button4.IsEnabled = true;
            Notify_checkBox.IsChecked = true;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            TableWorking.time(backgroundWorker1, list_note);
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            notifyIcon1.ShowBalloonTip(1000, "New note", e.UserState as string, System.Windows.Forms.ToolTipIcon.Info);
            message = e.UserState as string;
        }
        private void Dock_MouseDown(object sender, MouseButtonEventArgs e) // clicked some date on the calendar
        {
            #region Changing selected element on the calendar
            DockPanel d = (DockPanel)sender;
            for (int j = 1; j < 7; j++)
            {
                for (int i = start; i < 7; i++)
                {
                    if (dock[j, i] != null)
                        dock[j, i].Background = Brushes.White;
                }
            }
            d.Background = Brushes.LightGray;
            #endregion

            #region Ovveride all label, table ect
            Label label = (Label)d.Children[0];
            DateTime dt = new DateTime(CurrentYear, CurrentMonth, Convert.ToInt32(label.Content));
            Date_label.Content = dt.ToShortDateString();

            DataWorking.GetData(out list_note, "data.xml");
            TableWorking.FillTable(Date_label, list_note, dataGrid);
            #endregion

            #region Loading diary
            try
            {
                Diary.Load(richTextBox, Date_label);
            }
            catch (Exception ex) { richTextBox.Document.Blocks.Clear(); }
            #endregion

        }

        private void left_gif_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentMonth == 1)
            {
                CurrentYear--;
                CurrentMonth = 12;
            }
            else
                CurrentMonth--;
            DeleteCalendar();
            FillCalendar(CurrentYear, CurrentMonth);
            ShowMonthNamesAndCurrentYear(CurrentMonth, CurrentYear, label3);
        }// show on the calendar previous month 

        private void right_gif_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentMonth == 12)
            {
                CurrentYear++;
                CurrentMonth = 1;
            }
            else
                CurrentMonth++;
            DeleteCalendar();
            FillCalendar(CurrentYear, CurrentMonth);
            ShowMonthNamesAndCurrentYear(CurrentMonth, CurrentYear, label3);
        }// show on the calendar next month

        private void button_Click(object sender, RoutedEventArgs e) // add new note
        {
            Note.Add(list_note, Date_label, Time_textBox, Text_textBox, dataGrid, Notify_checkBox, State_comboBox);
            Reset();
        }

        private void button1_Click(object sender, RoutedEventArgs e) // edit some note
        {
            if (Selected_Note == null)
                return;
            Time_textBox.Text = Selected_Note.date.ToShortTimeString();
            Text_textBox.Text = Selected_Note.text;
            Exchange_button.IsEnabled = true;
            State_comboBox.Text = Selected_Note.state;
            Notify_checkBox.IsChecked = Selected_Note.notify;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Note.Delete(list_note, Date_label, dataGrid, Selected_Note);
        } // delete some note

        private void dataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Reset();

            Note_Parent note1 = dataGrid.SelectedItem as Note_Parent;
            if (note1 == null)
                return;
            Note note2 = new Orginizer.Note();
            note2.date = note1.date;
            note2.state = note1.state;
            note2.text = note1.text;
            note2.login = AuthorizationWindow.user.name;
            note2.notify = note1.notify;
            Selected_Note = note2;
        }// ????????????????????????????????What is it??????????

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Note.Delete(list_note, Date_label, dataGrid, Selected_Note);
            Note.Add(list_note, Date_label, Time_textBox, Text_textBox, dataGrid, Notify_checkBox, State_comboBox);
            Reset();
        } // exchange some note


        bool b = false;
        bool i = false;
        bool u = false;
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (b == false)
            {
                richTextBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
                b = true;
            }
            else
            {
                richTextBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
                b = false;
            }

        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (i == false)
            {
                richTextBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                i = true;
            }
            else
            {
                richTextBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
                i = false;
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Label l = comboBox1.SelectedValue as Label;
            richTextBox.Selection.ApplyPropertyValue(FontFamilyProperty, new FontFamily(l.Content.ToString()));
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int str = Convert.ToInt32(comboBox2.SelectedValue);
            richTextBox.Selection.ApplyPropertyValue(FontSizeProperty, Convert.ToDouble(str));
        }

        private void comboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color selectedColor = (Color)(comboBox3.SelectedItem as PropertyInfo).GetValue(null, null);
            //------ERROR !!!!! при смене перемещении курсора на другой цвет текста цвет в comboBox не меняется !!!!!
            //----------------ERROR------------------
            richTextBox.Selection.ApplyPropertyValue(ForegroundProperty, new SolidColorBrush(selectedColor));
        }

        private void button_Left(object sender, RoutedEventArgs e)
        {
            richTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            richTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            richTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }

        private void UnderLine_Click(object sender, RoutedEventArgs e)
        {
            if (b == false)
            {
                richTextBox.Selection.ApplyPropertyValue(TextBlock.TextDecorationsProperty, TextDecorations.Underline);
                b = true;
            }
            else
            {
                richTextBox.Selection.ApplyPropertyValue(TextBlock.TextDecorationsProperty, null);
                b = false;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Diary.Save(richTextBox, Date_label);
        }

        private void comboBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox4.SelectedIndex == 0)
            {
                System.Windows.Forms.OpenFileDialog f = new System.Windows.Forms.OpenFileDialog();
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Canvas can = new Canvas();
                    BitmapImage img = new BitmapImage(new Uri(f.FileName));
                    Image im = new Image();
                    im.Source = img;
                    can.Children.Add(im);
                    im.Height = 300;
                    im.Width = 300;
                    image = im;

                    Paragraph para = new Paragraph();
                    para.Inlines.Add(can);
                    richTextBox.Document.Blocks.Add(para);
                    richTextBox.ScrollToEnd();
                    richTextBox.Focus();
                    //image.Loaded += delegate
                    //{
                    //    AdornerLayer al = AdornerLayer.GetAdornerLayer(image);
                    //    if (al != null)
                    //    {
                    //        al.Add(new ResizingAdorner(image));
                    //    }
                    //};
                }
            }
            comboBox4.SelectedIndex = -1;
        }



    }

}
