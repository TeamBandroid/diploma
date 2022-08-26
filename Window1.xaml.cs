using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public class Phone: INotifyPropertyChanged
    {
        public string Name { get; set; }
        public  int Number;
        static bool access;
        public string videoPath2;
        static double width;
        public ListBox List;
        public  double Width { 
            get { return (width); } 
            set {
                width = value-35;   
                OnPropertyChanged("Width");
                }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public string picturePath { get; set; }
        public Label Lable;
        public MediaElement back;
        public TextBox instruction;
        StackPanel Pull_setting;
        Button Start;
        public bool clicking = false;
        public Label Name_id;
        int intExercise1Id;
        public Label Gogen;
        StackPanel Settings;
        ScrollViewer SettingsScrol;
        StackPanel tz_instruction;
        public Phone(object[] mas, ref Label Title, ref MediaElement back,  ref TextBox instruction, ref Button Start, ref ListBox  Listu, ref StackPanel Pull_setting, ref StackPanel Settings, ref Label Name_id,  int intExercise1Id, ref Label Gogen, ref  ScrollViewer SettingsScrol, ref StackPanel tz_instruction)
        {
            this.tz_instruction = tz_instruction;
            this.SettingsScrol = SettingsScrol;
            this.Gogen = Gogen;
            this.intExercise1Id = intExercise1Id;
            this.Name_id = Name_id;
            this.Settings = Settings;
            this.Pull_setting = Pull_setting;
            Number = (int)mas[0];
            access = (bool)mas[1];
            videoPath2 = (string)mas[4];
            picturePath = (string)mas[2];
            this.Start = Start;
            Name = (string)mas[3];
            Lable = Title;
            this.back = back;
            this.instruction = instruction;
            Width = Listu.Width;
            List = Listu;

        }
        public void SQLcommand()
        {
            var sqlConnection = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Валерия\\Desktop\\Курсовая\\Новая папка\\не сломанаая\\BodyBasics-WPF\\Database1.mdf; Integrated Security = True");
            sqlConnection.Open();


            SqlCommand sql = new SqlCommand("Select * From [tbldata] where intWorkerId =" + intExercise1Id.ToString() + "and intExercise1Id =" + Number.ToString(), sqlConnection);
            var r = sql.ExecuteReader();
            while (r.Read())
            {
                Gogen.Content = r["charDescription"].ToString();
            }
            sqlConnection.Close();
        }

        private ICommand clic;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand Clic

        {

            get
            {
                return clic ?? (clic = new RelayCommand(() =>
                {

                    if (access)
                    {
                        //фон
                        back.Visibility = Visibility.Hidden;
                        Name_id.Visibility = Visibility.Hidden;
                        Lable.Content = Name;
                        Start.Visibility = Visibility.Visible;
                        //инструкция
                        instruction.Text =videoPath2;
                        instruction.Visibility = Visibility.Visible;
                        SettingsScrol.Visibility = Visibility.Visible;
                        SQLcommand();
                        Settings.Visibility = Visibility.Visible;
                        Pull_setting.Visibility = Visibility.Hidden;
                        tz_instruction.Visibility = Visibility.Hidden;
                        if (Number == 2)
                        {
                            tz_instruction.Visibility = Visibility.Visible;
                            Settings.Visibility = Visibility.Hidden;
                        }
                        if (Number == 3)
                            {
                            Pull_setting.Visibility = Visibility.Visible;
                            }

                     
                       var flag = "Уровень провален! время вышло!";
                         StreamWriter file = new StreamWriter(new FileStream("point.txt", FileMode.Create));
                        file.WriteLine(flag);
                        file.Close();
                        foreach (var t in List.Items)
                        {
                            if (t.GetType() == typeof(Phone))
                            {
                                var j = (Phone)t;
                                if (j.clicking == true)
                                    j.clicking = false;
                            }
                        }
                        clicking = true;
                    }


                }
              ));

            }
        }
    }
   
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
       {
        //описание упражнений
        public String[] mast = new string[4] { @"Упражнение 1:
Пациент стоит/ сидит прямо, рука вытянута вперед.
Пациент совершает поворот рукой по окружности не выходя за рамки.

Настройки упражнения:
1.Cкоростных режима.Скорость увеличиваем постепенно до тех пор, пока пациент не почувствует легкую дурноту муть. 
2.Рабочая рука",

        @"Упражнение 2:
Пациент стоит/сидит прямо, рука вытянута вперед.
Пациент совершает повороты головы направо / налево, но до такой степени, что бы экран монитора оставался в поле зрения пациента. 
При этом он «ловит» рукой появляющиеся в горизонтальной и вертикальной плоскости визуальные стимулы.

Настройки упражнения:
1.Cкоростных режима.Скорость увеличиваем постепенно до тех пор, пока пациент не почувствует легкую дурноту муть. 
2.Рабочая рука",
        @"Упражнение 3:
Повторяются упражнения первой группы во время выполнения пациентом  приставных шагов во фронтальной плоскости ( шаг вправо, шаг влево).  Выполнение элемента фланговая походка  с одновременным «захватом визуального стимула» проводится  для  формирования физиологической постуральной стратегии шага.
Пациент стоит прямо. Рука вытянута вперед. Делает приставной шаг вправо, влево и «ловит» появляющийся  по горизонтали и вертикали визуальный стимул. ",
        @"Упражнение 4:
Инструкция по выполнению:
Поза стоя или сидя. Голова прямо. Рабочая рука вытянута вперед  до уровня глаза.
Голова пациента остается неподвижной. Задача вытянутой рукой «словить» фрукт.
Настройки упражнения:
1.Cкоростных режима.Скорость увеличиваем постепенно до тех пор, пока пациент не почувствует легкую дурноту муть. 
2.Рабочая рука

Варианты услажнения упражнения:
1.Выполнение упражения с наклоненной головой к плечу в сторону поражения.
2.Выполнение упражения с наклоненной головой к плечу в сторону противоположную стороне поражения.
3.Голова наклонена вперед, настолько, что бы экран монитора оставался в поле зрения пациента. 
4.Голова отклоняется назад, настолько, что бы монитор оставался в поле зрения и выполняется упражнение
Упражнение противопоказано для выполнения пациентам с не разрешившимся отолитиазом заднего полукружного канала.
5.Во время медленного наклона головы к плечу, наклона вперед / запрокидывания головы. "
    };
        // названия упражнений 
        public string[] names = new string[4] { "Круговое движение рукой", "Плавные повороты головой ", " Приставной шаг с рукой ", " Предметы " };
  
        public ObservableCollection<Phone> Stack { get; set; }
        public string LevelNum;

        int Id = 0;
        public void  SQLcommand()
        {
            var sqlConnection = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Валерия\\Desktop\\Курсовая\\Новая папка\\не сломанаая\\BodyBasics-WPF\\Database1.mdf; Integrated Security = True");
            sqlConnection.Open();
            
            SqlCommand sql = new SqlCommand("Select * From [Table] where Id ="+Id.ToString(), sqlConnection);
            var r = sql.ExecuteReader();

            InitializeComponent();
            while (r.Read())
            {
                Name_id.Content = r["Name"].ToString();
            }
            sqlConnection.Close();
            sqlConnection.Open();
        }

        public Window1(int id)
        {
            this.Top = 0;
            this.Width = System.Windows.SystemParameters.WorkArea.Width;
            this.Height = System.Windows.SystemParameters.WorkArea.Height;
            this.Left = 0;
            Id = id;
            SQLcommand();
            B_NO.IsChecked = true;

            var videoPath =Directory.GetCurrentDirectory().Replace("\\bin\\AnyCPU\\Debug", "");
            back.Source = new Uri(videoPath + @"\Images\Frame2.png");
            back.Play();

            Stack = new ObservableCollection<Phone>();
       
            List.Width = System.Windows.SystemParameters.WorkArea.Width/9;
            var mas = new object[4];
          
            for (var i = 0; i < 4; i++)
            {
              // номер упражнения от одного 0, ... 1, картинка 2, название 3, текст 4
                    mas[i] = new object[] { i, true,  videoPath + @"\Images\video" + i + ".jpg", names[i],  mast[i] };
                
            }

            for (var i = 0; i < 4; i++)
                Stack.Add(new Phone((object[])mas[i], ref Title, ref back,  ref instruction, ref Start,ref List, ref Pull_setting, ref Settings, ref Name_id, id,ref Gogen, ref SettingsScrol, ref tz_instruction));
            List.ItemsSource = Stack;
        
            var fase = face.Children;
            foreach(var i in fase)
            {
                if (i.GetType() != typeof(Label))
                {
                    var tt = (RadioButton)i;
                    var NAme_contect= tt.Name;
                    var yy = (StackPanel)tt.Content;
                    foreach(var J in yy.Children)
                    {
                        if (J.GetType() == typeof(Image))
                        {
                            var j = (Image)J;
                            BitmapImage bi3 = new BitmapImage();
                            bi3.BeginInit();
                            bi3.UriSource = new Uri(videoPath +@"\Images"+ @"\"+NAme_contect + ".png");
                            bi3.EndInit();
                            j.Stretch = Stretch.Fill;
                            j.Width = 150;
                            j.Height = 100;
                            j.Source = bi3;
                        }
                      
                    }
                    
                }
              
            }
        }
      
    
        Window taskWindow;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            taskWindow = null;
            int a = 0;
            foreach(var t in List.Items)
            {
                if (t.GetType() == typeof(Phone))
                {
                    var Phone_1 = (Phone)t;
                    if(Phone_1.clicking == true)
                        a = Phone_1.Number;
                }
            } 
            var hd = double.Parse(numbers_hard.Content.ToString());  
            hd = Math.Round(hd);
           
            switch (a)
            {  
                case 0:
                StreamWriter filee = new StreamWriter(new FileStream("hard_3.txt", FileMode.Create));
                filee.WriteLine(hd.ToString());
                filee.Close();
                taskWindow = new MainWindow(Id, side);
                    break;
                case 1:
                taskWindow = new Window2(Id);
                    break;
                case 2:
                    // прямо по тз
                if(GG_1.IsChecked==true)
                taskWindow = new Window3(Id,true);
                // по кругу по диплому
                if (GG_2.IsChecked == true)
                    taskWindow = new Window3(Id,false);
                  break;
                case 3:
                    StreamWriter fiile = new StreamWriter(new FileStream("hard_3.txt", FileMode.Create));
                    fiile.WriteLine(hd.ToString());
                    fiile.Close();
                    taskWindow = new Window4(Id, side);
                    break;

            }
          
            taskWindow.Owner = this;
            taskWindow.ShowDialog();
            StreamReader file = new StreamReader(new FileStream("point.txt", FileMode.Open));
            var flag = file.ReadLine();
            file.Close();
            Title.Content = flag;
           
        }
        
        void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            List.Width = this.Width / 9;
          foreach(var i in List.Items)
         {
             var u = (Phone)i;
             u.Width = List.Width;
          }
                   
        }
  
        private void RadioButton_Face2(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            StreamWriter file = new StreamWriter(new FileStream("face.txt", FileMode.Create));
            file.WriteLine("fase2.png");
            file.Close();
        }
        private void RadioButton_Face1(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            StreamWriter file = new StreamWriter(new FileStream("face.txt", FileMode.Create));
            file.WriteLine("fase1.png");
            file.Close();
        }

        private void RadioButton_Checked_Hend_1(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            StreamWriter file = new StreamWriter(new FileStream("Hend.txt", FileMode.Create));
            file.WriteLine("HandRight");
            file.Close();
        }
        private void RadioButton_Checked_Hend_2(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            StreamWriter file = new StreamWriter(new FileStream("Hend.txt", FileMode.Create));
            file.WriteLine("HandLeft");
            file.Close();
        }
        private void Button_Skor_2(object sender, RoutedEventArgs e)
        { 
            var hd = double.Parse(numbers_hard.Content.ToString());
            if (hd != 1)
            {
                hd--;
                numbers_hard.Content = hd.ToString();
            }
          
        }
        private void Button_Skor(object sender, RoutedEventArgs e)
        {
           var hd = double.Parse(numbers_hard.Content.ToString());

            hd = hd < 3 ? (hd+1) : (hd);

            numbers_hard.Content = hd.ToString();
        }
        private void Tz_1(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            StreamWriter file = new StreamWriter(new FileStream("Tz.txt", FileMode.Create));
            file.WriteLine("False");
            file.Close();
        }
        private void Tz_2(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            StreamWriter file = new StreamWriter(new FileStream("Tz.txt", FileMode.Create));
            file.WriteLine("True");
            file.Close();
        }
        string side;
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            for(var yp =0; yp< instruction.Text.Length;yp++)
                instruction.LineUp();
            
            // приводим отправителя к элементу типа RadioButton
            RadioButton radioButton = (RadioButton)sender;
            int ebanina = 0;
            int y = 0;
            foreach (var t in List.Items)
            {
                if (t.GetType() == typeof(Phone))
                {
                    var j = (Phone)t;
                    if (j.clicking == true)
                        ebanina = j.Number;
                    break;
                }
            }


            if (ebanina == 3) { y = 1; }

            if ((string)radioButton.Content == (string)"Да")
            {
                Vravo.IsChecked = true;
                
            }
            if ((string)radioButton.Content == (string)"Вправо")
            {
                Yess.IsChecked = true;
            }
            if ((string)radioButton.Content == (string)"Влево")
            {
                Yess.IsChecked = true;
            }
            if ((string)radioButton.Content==(string)"Нет")
            {
                if(y==1)

                    instruction.Text = @"Упражнение 4:
Инструкция по выполнению:
Поза стоя или сидя. Голова прямо. Рабочая рука вытянута вперед  до уровня глаза.
Голова пациента остается неподвижной. Задача вытянутой рукой «словить» предмет.
Настройки упражнения:
1.Cкоростных режима.Скорость увеличиваем постепенно до тех пор, пока пациент не почувствует легкую дурноту муть. 
2.Рабочая рука

Варианты услажнения упражнения:
1.Выполнение упражения с наклоненной головой к плечу в сторону поражения.
2.Выполнение упражения с наклоненной головой к плечу в сторону противоположную стороне поражения.
3.Голова наклонена вперед, настолько, что бы экран монитора оставался в поле зрения пациента. 
4.Голова отклоняется назад, настолько, что бы монитор оставался в поле зрения и выполняется упражнение
Упражнение противопоказано для выполнения пациентам с не разрешившимся отолитиазом заднего полукружного канала.
5.Во время медленного наклона головы к плечу, наклона вперед / запрокидывания головы. ";

                Vravo.IsChecked = false;
                Levo.IsChecked = false; 
            }
            else
            {
                if (y == 1)

                    instruction.Text = @"Упражнение 4:
Инструкция по выполнению: 
Рука , соответственно стороне поражения, вытянута вперед. 
Голова прямо. Взгляд прямо. Начинается движение предметов друг за другом по горизонтали.  Когда предмет меняет цвет его нужно схватить.
 
Настройки упражнения:
1. Cкоростных режима.  Скорость увеличиваем постепенно до тех пор, пока пациент не почувствует легкую дурноту муть. 
2. Рабочая рука
3. Сторона стимула 

Варианты услажнения упражнения:
1. Рука , соответственно стороне поражения, вытянута вперед. Начинается движение предметов друг за другом по горизонтали в сторону поражения.  
2. Вариант выполнения этого же упражнения с вытянутой рукой  со стороны, противоположной стороне поражения. Начинается движение предметов  друг за другом по горизонтали в сторону поражения.  
3. Рука вытянута вперед.  Направление визуального стимула – к стороне поражения. Пациент поворачивает голову  в сторону поражения/сторону  противоположную стороне поражения. Во время поворота головы необходимо «словить рукой выделенный визуальный стимул»
4. в положении стоя с наклоном головы в сторону поражения 15, в сторону противоположную стороне поражения 16, с наклоном головы вперед настолько, что бы монитор оставался в поле зрения. С наклоном головы вперед, назад настолько, что бы экран  монитора оставался в поле зрения. 19,20,21 – выполнение упражнений 15,16,17 с вытянутой рукой противоположной  стороне поражения.
5. выполнение этого упражнения на подушке и поролоновом коврике с целью уменьшения проприоцептивного влияния . Уменьшение потока  проприоцептивной  афферинтации в условиях измененного ВОР и подачи динамического визуального стимула способствует, в том числе, и стимуляции остаточной вестибулярной функции.
Выполнение упражнений с наклонами /запрокидыванием головы вперед назад противопоказаны пациентам с не разрешившимся отолитиазом заднего полукружного канала. .
 ";

                B_NO.IsChecked = false;
             
            }
            if (radioButton.IsChecked == true)
            {if ((string)radioButton.Content == "Да")
                    side = "Вправо";
                else
                    side = (string)radioButton.Content;
      
            }
        }

    }  
}
