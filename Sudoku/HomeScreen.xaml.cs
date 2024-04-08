using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                    ErrorMessage();
                }
                var Window = new MainWindow(hints);
                Window.Show();
                this.Hide();
                input.Text = "";
                
            }
            catch 
            {
                ErrorMessage();
            }
        }
        void ErrorMessage()
        {
            var message = "Proszę podać wartość całkowitą z przedziału 0 a 81";
            var caption = "Błędna wprowadzona wartość";            
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image = MessageBoxImage.Error;
            MessageBoxResult result;

            result = MessageBox.Show(message, caption, button, image,MessageBoxResult.Yes);
        }
        void HowToPlay()
        {
            var message = "Celem sudoku jest ułożenie numerów z przedziału od 1 do 9 w dostępnych okienkach tak, aby w każdym rzędzie, kolumnie oraz \"Pudełku\" 3x3 każdy numer występował tylko raz\n" +
                "Jeśli we wprowadzonych danych są błędy zostaną one oznaczone kolorami:\n" +
                "1. Niebieski - Oznacza liczbę nienależącą do przedziału\n" +
                "2. Fioletowy - Oznacza że w okienko wpisano litere bądź słowo\n" +
                "3. Żółta linia pozioma - Oznacza powtórzenie w rzędzie\n" +
                "4. Żółta linia pionowa - Oznacza powtórzenie w kolumnie\n" +
                "5. Żółty kwadrat 3x3 - Oznacza powtórzenie w kwadracie";
            var caption = "Jak grać";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result;

            result = MessageBox.Show(message, caption, button);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HowToPlay();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();

            base.OnClosing(e);
        }
    }
}
