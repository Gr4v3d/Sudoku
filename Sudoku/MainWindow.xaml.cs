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
using System.Windows.Media.Media3D;
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
        public TextBox[,] okna = new TextBox[9, 9];
        public void TworzGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                root.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 9; i++)
            {
                //Przesuń do loopa wyżej
                root.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < 9; j++) //Zrób z tego metode
                {
                    TextBox tb = new TextBox();
                    string name = $"tb{i}{j}";
                    tb.Name = name;
                    tb.Text = ((i*10+(j+1))%9).ToString();
                    root.Children.Add(tb);
                    RegisterName(tb.Name, tb);
                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, j);
                    okna[i,j] = tb;
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
            Grid.SetColumn(guzik, 3);
            Grid.SetColumnSpan(guzik, 3);
        }

        private void sprawdz(object sender, RoutedEventArgs e)
        {
            bool Kontynuuj = przypisz();
            if (Kontynuuj)
            {
                GameLogic();
            }
        }
        private void GameLogic()
        {
            //Przeszukanie po kwadratach
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    ErrorInABox(i, j);
                }
            }
            //Przeszukanie po Rzędach
            for (int i = 0; i < 9; i++)
            {
                List<int> Search = new List<int>(numery[i,0]);
                bool problem = false;
                for(int j = 1; j< 9; j++)
                {
                    foreach (int item in Search)
                    {
                        if(item == numery[i,j]) problem = true; break;
                    }
                    if (problem)
                    {
                        ErrorInARow(i);
                        break;
                    }
                    Search.Add(numery[i,j]);
                }
            }
            //Przeszukanie po kolumnach
            for (int i = 0; i < 9; i++)
            {
                List<int> Search = new List<int>(numery[0, i]);
                bool problem = false;
                for (int j = 1; j < 9; j++)
                {
                    foreach (int item in Search)
                    {
                        if (item == numery[j, i]) problem = true; break;
                    }
                    if (problem)
                    {
                        ErrorInAColumn(j);
                        break;
                    }
                    Search.Add(numery[j, i]);
                }
            }
        }
        private bool przypisz()
        {
            bool CzyJestOk = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    /*string name = "tb";
                    name = name + i;
                    name = name + j;
                    object objekt = root.FindName(name);
                    TextBox tb = objekt as TextBox;
                    tb.Background = null;*/
                    try
                    {
                        int temp = Convert.ToInt32(okna[i,j].Text);
                        if(temp <1 || temp > 10)
                        {
                            okna[i,j].Background = Brushes.Red;
                            CzyJestOk = false;
                        }
                    }
                    catch
                    {
                        okna[i,j].Background = Brushes.DarkRed;
                        object guzik = (Button)root.FindName("guzik");
                        Button button = guzik as Button;
                        button.Content = "Proszę podać liczbę całkowitą";
                        CzyJestOk = false;
                    }
                }
            }
            return CzyJestOk;

        }
        //Zmienia Kolor rzędu lub kolumny (jeśli bool jest true) na podany kolor
        private void ErrorInARow(int index)
        {
            for (int i = 0; i < 9; i++)
            {
                //okna[index,i].Background = Brushes.Yellow;
            }
        }
        private void ErrorInAColumn(int index)
        {
            for(int i = 0; i < 9; i++)
            {
                //okna[i, index].Background = Brushes.Orange;
            }
        }
        // Wysokość i szerokość musi być od 0 - 2 żeby działało zapamiętaj
        private void ErrorInABox(int height,int width)
        {
            List<string> znaneWartosci = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    int row = i + (height * 3);
                    int column = j + (width * 3);
                    foreach (string item in znaneWartosci)
                    {
                        if(item == okna[row, column].Text) okna[row,column].Background = Brushes.DarkOrange;
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
