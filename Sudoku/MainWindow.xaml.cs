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
using System.Xml.Linq;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int[,] numery = new int[9, 9];
        public void TworzGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                root.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 9; i++)
            {
                root.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < 9; j++)
                {
                    TextBox tb = new TextBox();
                    string name = $"tb{i}{j}";
                    tb.Name = name;
                    tb.Text = (i*10+(j+1)).ToString();
                    root.Children.Add(tb);
                    RegisterName(tb.Name, tb);
                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, j);
                }
            }
            root.RowDefinitions.Add(new RowDefinition());
            Button guzik = new Button();
            guzik.Content = "Guzior";
            guzik.Name = "guzik";
            RegisterName(guzik.Name,guzik);
            guzik.Click += new RoutedEventHandler(sprawdz);
            root.Children.Add(guzik);
            Grid.SetRow(guzik, 9);
            Grid.SetColumn(guzik, 4);
        }

        private void sprawdz(object sender, RoutedEventArgs e)
        {
            przypisz();
        }
        private void przypisz()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    string name = "tb";
                    name = name + i;
                    name = name + j;
                    object objekt = root.FindName(name);
                    TextBox tb = objekt as TextBox;
                    tb.Background = null;
                    try
                    {
                        int temp = Convert.ToInt32(tb.Text.ToString());
                        if(temp <1 || temp > 10)
                        {
                            tb.Background = Brushes.Yellow;
                        }
                        else
                        {
                            numery[i,j] = temp;
                        }
                    }
                    catch
                    {
                        tb.Background = Brushes.Red;
                        object guzik = (Button)root.FindName("guzik");
                        Button button = guzik as Button;
                        button.Content = "Proszę podać liczbę całkowitą";
                    }
                }
            }

        }

        public MainWindow()
        {
            InitializeComponent();
            TworzGrid();
        }
    }
}
