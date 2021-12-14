using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataClassifier
{   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Classifier classifier;
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                List<Category> categories = null;
                var path = System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "data.json");

                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    categories = JsonConvert.DeserializeObject<List<Category>>(json);
                }
                classifier = new Classifier(categories);
            }
            catch(Exception ex) 
            {
                MessageBox.Show("В ходе загрузки программы возникла ошибка инициализации классификатора. Дополнительные сведения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void btDefineType_Click(object sender, RoutedEventArgs e)
        {
            List<string> parameters = new List<string>();
            foreach (var element in mainPanel.Children)
            {
                if (element is StackPanel stackPanel) 
                {
                    foreach (var elem in stackPanel.Children)
                    {
                        if (elem is ComboBox comboBox)
                        {
                            string value = (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                            parameters.Add(value);
                        }
                        if (elem is StackPanel stackPanel1)
                        {
                            foreach (var elem1 in stackPanel1.Children)
                            {
                                if(elem1 is CheckBox checkBox)
                                {
                                    if ((bool)checkBox.IsChecked)
                                    {
                                        string value = checkBox.Content.ToString();
                                        parameters.Add(value);
                                    }
                                }
                            }     
                        }
                    }
                }
            }

            textResult.Text = "Байесовский классификатор: " + GetMax(classifier.BayesianClassify(parameters)) + "\n" +
                                  "Линейный классификатор: " + GetMax(classifier.LinearClassify(parameters));
        
            string GetMax(Dictionary<string, double> res) 
            {
                string name = "";
                double value = 0;
                foreach (var item in res)
                {
                    if (item.Value > value) 
                    {
                        name = item.Key;
                        value = item.Value;
                    }
                }
                return name;
            }        
        }
    }
}
