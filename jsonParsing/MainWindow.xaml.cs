using System;
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
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using Microsoft.VisualBasic;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Win32;
using static System.Collections.Specialized.BitVector32;

namespace jsonParsing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //TB1.Width = SystemParameters.PrimaryScreenWidth; // Ширина экрана компьютера


            actualWith = Convert.ToDouble(Wind.Width); // Ширина экрана компьютера

            //TB1.Width = Convert.ToDouble(MainWindow.ActualWidthProperty); // Ширина экрана компьютера

            var directory = AppDomain.CurrentDomain.BaseDirectory;
            savePath = directory + "../../../../../answersJson";

            //MessageBox.Show(directory);
            var selectionFilePath = directory + "../../../../../questionsJson/section1.json";
            //MessageBox.Show(selectionFilePath);
            var allFileData = File.ReadAllText(selectionFilePath);
            //MessageBox.Show(allFileData);
            var questions = JsonConvert.DeserializeObject<Ques[]>(allFileData);
            foreach (var item in questions)
            {
                WR1.Children.Add(new MyWrapPanel(item.text, Wind.Width, item.code));
            }
            WR1.Children.Add(new MyWrapPanel("abcd", 100, 100));


            

            foreach (var item in WR1.Children)
            {
                if (item is MyWrapPanel myWrapPanel)
                {
                    foreach (var child in myWrapPanel.Children)
                    {
                        if (child is TextBlock myTB)
                        {
                            myTB.Width = actualWith * 0.78;
                        }
                        if (child is ComboBox myCB)
                        {
                            myCB.Width = actualWith * 0.15;
                        }
                    }
                }
            }
        }

        public string fileName = "section1.json";
        public string savePath;
        public double actualWith = 400;
        public string standart_path = "questionsJson/section1.json";
        private void anotherFileButton_Click(object sender, RoutedEventArgs e)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            //MessageBox.Show(directory);
            var selectionFilePath = directory + "../../../../../questionsJson/section1.json";
            
            OpenFileDialog dialog = new OpenFileDialog();

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    selectionFilePath = dialog.FileName;
                    fileName = dialog.SafeFileName;
                    MessageBox.Show(fileName);
                }
                //MessageBox.Show(selectionFilePath);
                var allFileData = File.ReadAllText(selectionFilePath);
                //MessageBox.Show(allFileData);
                var questions = JsonConvert.DeserializeObject<Ques[]>(allFileData);
                WR1.Children.Clear();
                foreach (var item in questions)
                {
                    MessageBox.Show($"{item.code}");
                    WR1.Children.Add(new MyWrapPanel(item.text, Wind.Width, item.code));
                }
            }
            catch
            {
                MessageBox.Show("Ошибка открытия файла");
            }   
        }

        public class MyWrapPanel : WrapPanel
        {
            TextBlock myTextBlock1 = new TextBlock();
            ComboBox myComboBox1 = new ComboBox();
            public int number;
            public string text;
            public MyWrapPanel(string text, double actualWidht, int number)
            {
                this.number = number;
                this.text = text;

                myTextBlock1.Background = new SolidColorBrush(Color.FromArgb(128, 128, 128, 128)); ;
                myTextBlock1.Text = text;
                myTextBlock1.Padding = new Thickness(10);
                myTextBlock1.TextWrapping = TextWrapping.Wrap;
                myTextBlock1.TextAlignment = TextAlignment.Center;
                myTextBlock1.Width = actualWidht * 0.75;
                
                myComboBox1.Items.Add("Да");
                myComboBox1.Items.Add("Нет");
                myComboBox1.Width = actualWidht * 0.15;

                this.Children.Add(myTextBlock1);
                this.Children.Add(myComboBox1);
            }
        }

        public class Ques
        {
            public string text { get; set; }
            public int code { get; set; }
            public Ques(string text, int number)
            {
                this.text = text;
                this.code = number;
            }
        }
        public class Answer
        {
            public int number { get; set; }
            public string answer { get; set; }
            public Answer(int number, string answer)
            {
                this.number = number;
                this.answer = answer;
            }
        }

        public List<Ques> quesList = new List<Ques>();
        public List<Answer> answerList = new List<Answer>();

        private async void Wind_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            actualWith = Convert.ToDouble(Wind.Width); // Ширина экрана компьютера
            //TextBlock myTextBlock1 = new TextBlock();
            //myTextBlock1.Text = "Дада";
            //ComboBox myComboBox1 = new ComboBox();
            //myComboBox1.Items.Add("Да");
            //myComboBox1.Items.Add("Нет");

            

            foreach (var item in WR1.Children)
            {
                if (item is MyWrapPanel myWrapPanel)
                {
                    foreach (var child in myWrapPanel.Children)
                    {
                        if (child is TextBlock myTB)
                        {
                            myTB.Width = actualWith * 0.78;
                        }
                        if (child is ComboBox myCB)
                        {
                            myCB.Width = actualWith * 0.15;
                        }
                    }
                }
            }


            //MyWrapPanel myWrapP = new MyWrapPanel("очень интересный вопрос", actualWith, 100);
            //MyWrapPanel myWrapP2 = new MyWrapPanel("очень интересный вопрос", actualWith, 101);

            //myWrapP.Children.Add(myTextBlock1);
            //myWrapP.Children.Add(myComboBox1);


            //WR1.Children.Add(myWrapP);
            //WR1.Children.Add(myWrapP2);

        }

        private void saveFileButton_Click(object sender, RoutedEventArgs e)
        {
            answerList.Clear();
            foreach (var item in WR1.Children)
            {
                if (item is MyWrapPanel myWrapPanel)
                {
                    foreach (var child in myWrapPanel.Children)
                    {
                        if (child is ComboBox myCB)
                        {
                            if (myCB.SelectedValue is not null)
                            {
                                answerList.Add(new Answer(myWrapPanel.number, myCB.SelectedValue.ToString()));
                            }
                        }
                    }
                }
            }

            DateTime dateTime = System.DateTime.Now;
            string dt = Convert.ToString(dateTime).Replace('.', '_').Replace(' ', '-').Replace(':', '_');
            MessageBox.Show($"{dt}");
            string actualSavePath = $"{savePath}/{dt}{fileName}";
            var ansverMass = answerList.ToArray();
            File.WriteAllText(actualSavePath, JsonConvert.SerializeObject(ansverMass, Formatting.Indented));




        }
    }
}