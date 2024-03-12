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

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[,] numery = new string[9, 9];
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
                    tb.Text = (i * 10 + j + 1).ToString();
                    root.Children.Add(tb);
                    RegisterName(tb.Name, tb);
                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, j);
                }
            }
            root.RowDefinitions.Add(new RowDefinition());
            Button guzik = new Button();
            guzik.Content = "Guzior";
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
            /*for (int i = 0; i < 9; i++)
            {
                for (int j = 0; i < 9; j++)
                {*/
                    object objekt = (TextBox)root.FindName("tb00");

                    TextBox tb = objekt as TextBox;
                    tb.Text = "JEST";
           //     }
           // }

        }

        public MainWindow()
        {
            InitializeComponent();
            TworzGrid();
        }
    }
}
