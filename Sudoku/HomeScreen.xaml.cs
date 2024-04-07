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
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Logika interakcji dla klasy HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {

        public HomeScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var input = (TextBox)FindName("Input");
            try
            {
                var hints = Convert.ToInt32(input.Text);
                if(hints < 0 || hints > 81) 
                {
                    var message = "Proszę podać wartość całkowitą z przedziału 0 a 81";
                    SendMessage(message);
                }
                var Window = new MainWindow(hints);
                Window.Show();
                Close();
            }
            catch 
            {
                var message = "Proszę podać wartość całkowitą z przedziału 0 a 81";
                SendMessage(message);
            }
        }
        void SendMessage(string message)
        {
            var caption = "Błędna wprowadzona wartość";            
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image = MessageBoxImage.Error;
            MessageBoxResult result;

            result = MessageBox.Show(message, caption, button, image,MessageBoxResult.Yes);
        }
    }
}
