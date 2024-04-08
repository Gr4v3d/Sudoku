using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sudoku;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    const int SizeOfGrid = 9;
    int SRSize = (int)Math.Floor(Math.Sqrt(SizeOfGrid));
    public int[,] Numbers = new int[9, 9];
    public TextBox[,] TextBoxes = new TextBox[9, 9];
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        Application.Current.MainWindow.Show();

        base.OnClosing(e);
    }
    void CreateMesh()
    {
        for (int i = 0; i < SizeOfGrid; i++)
        {
            for (int j = 0; j < SizeOfGrid; j++) //Zrób z tego metode
            {
                TextBoxes[i, j] = new TextBox();
                TextBoxes[i, j].IsEnabled = false;
                root.Children.Add(TextBoxes[i, j]);
                Grid.SetRow(TextBoxes[i, j], i);
                Grid.SetColumn(TextBoxes[i, j], j);
            }
        }
    }
    void AddButton()
    {
        var button = new Button();
        button.Content = "Sprawdź!";
        button.Name = "Button1";
        RegisterName(button.Name, button);
        button.Click += new RoutedEventHandler(Game);
        root.Children.Add(button);
        Grid.SetRow(button, 9);
        Grid.SetColumn(button, 3);
        Grid.SetColumnSpan(button, 3);
    }
    public void CreateUI()
    {
        for (int i = 0; i < SizeOfGrid; i++)
        {
            root.ColumnDefinitions.Add(new ColumnDefinition());
            root.RowDefinitions.Add(new RowDefinition());
        }
        root.RowDefinitions.Add(new RowDefinition());
        CreateMesh();
        AddButton();
        
    }

    private void Game(object sender, RoutedEventArgs e)
    {
        ColourReset();
        CheckInputsForErrors();
        if (GameLogic())
        {
            switch (EndOfGame())
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    System.Windows.Application.Current.Shutdown();
                    break;
            }
        }
    }
    MessageBoxResult EndOfGame()
    {
        var message = "Udało ci się, wygrałeś!\n Czy chcesz zagrać ponownie ?";
        var caption = "Koniec Gry";
        var image = MessageBoxImage.None;
        return MessageBox.Show(message, caption,MessageBoxButton.YesNo);
    }
    private bool GameLogic()
    {
        
        var win = true;
        //Jednoczesne przeszukanie po rzędach i kolumnach
        for (int i = 0; i < SizeOfGrid; i++)
        {
            var rowState = CheckRow(i);
            if (rowState == 0)
            {
                win = false;
                ColourRow(i);
            }
            else if (rowState == 1) win = false;

            var colState = CheckColumn(i);
            if (colState == 0)
            {
                win = false;
                ColourColumn(i);
            }
            else if (colState == 1) win = false;
        }
        //Przeszukanie wewnątrz kwadratów 3x3
        for(int i = 0; i < SizeOfGrid; i+= SRSize)
        {
            for(int j = 0;j<SizeOfGrid; j+= SRSize)
            {
                if (!CheckInBox(i, j))
                {
                    ColourBox(i, j); 
                    win = false;
                }
            }
        }
        return win;
    }
    //Sprawdza czy istnieją powtórzenia wewnątrz kwadratów
    //Zmienne Row i Column wyznaczają kwadrat który ma być przeszukany
    //na przykład Row = 1 i Column = 1 oznacza centralny kwadrat Sudoku, a Row = 0 i Column = 0, górny lewy kwadrat.
    bool CheckInBox(int Row, int Column)
    {
        var box = new List<int>();
        for(int i = 0;i < SRSize; i++)
        {
            for(int j = 0; j < SRSize; j++)
            {
                if (Numbers[Row + i,Column + j] != 0)
                {
                    if (box.Contains(Numbers[Row + i, Column + j]))
                    {                        
                        return false;
                    }
                    box.Add(Numbers[Row + i, Column + j]);
                }
            }
        }
        return true;
    }
    int CheckRow(int Row)
    {
        //Zmienna lokalna po to aby zdecydować czy w rzędzie występują powtórzenia, czy występują jakiekolwiek zera (miejsca puste/błedne) czy wszystko jest w porządku
        //2 - Wszystko jest w porządku
        //1 - występują zera
        //0 - powtórzenie
        var state = 2;
        var knownElements = new List<int>();
        for (int j = 0; j < SizeOfGrid; j++)
        {
            //0 jest domyślną wartością dla INT
            //Więc jeśli nie ma 0 oznacza to że gracz zmienił wartość w oknie
            if (Numbers[Row, j] != 0)
            {
                //Jeśli w liście znajduje się już element, podkreślam cały rząd
                //tak samo działa przeszukanie po kolumnach
                if (knownElements.Contains(Numbers[Row, j]))
                {
                    return 0;
                }
                knownElements.Add(Numbers[Row, j]);
            }
            else state = 1;
        }
        return state;
    }
    //Działa tak samo jak CheckRow
    int CheckColumn(int Column)
    {
        var state = 2;
        var knownElements = new List<int>();
        for (int j = 0; j < SizeOfGrid; j++)
        {
            if (Numbers[j, Column] != 0)
            {
                if (knownElements.Contains(Numbers[j, Column]))
                {
                    return 0;
                }
                knownElements.Add(Numbers[j, Column]);
            }
            else state = 1;
        }
        return state;
    }
    void ColourBox(int Row, int Column)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var rowIndex = Row  + i;
                var colIndex = Column  + j;
                TextBoxes[rowIndex, colIndex].Background = Brushes.Yellow;
            }
        }
    }
    void ColourRow(int Row)
    {
        for(int i = 0; i<SizeOfGrid; i++)
        {
            TextBoxes[Row,i].Background = Brushes.Gold;
        }
    }
    void ColourColumn(int Column)
    {
        for (int i = 0; i < SizeOfGrid; i++)
        {
            TextBoxes[i,Column].Background = Brushes.Orange;
        }

    }

    void ColourReset()
    {
        for(int i = 0; i<SizeOfGrid; i++)
        {
            for(int j = 0; j<SizeOfGrid; j++)
            {
                TextBoxes[i, j].Background = Brushes.White;
            }
        }
    }
    private void CheckInputsForErrors()
    {
        for (int i = 0; i < SizeOfGrid; i++)
        {
            for (int j = 0; j < SizeOfGrid; j++)
            {
                if (TextBoxes[i, j].Text != "")
                {
                    try
                    {
                        var temp = Convert.ToInt32(TextBoxes[i, j].Text);
                        if (temp < 1 || temp > 9)
                        {
                            TextBoxes[i, j].Background = Brushes.Blue;
                            Numbers[i, j] = 0; // Jeśli podany numer nie jest z przedziału 1 a 9 przypisujemy wartość domyślną 0
                            continue;
                        }
                        Numbers[i, j] = temp;
                    }
                    catch
                    {
                        TextBoxes[i, j].Background = Brushes.Purple;
                        Numbers[i, j] = 0; // Jeśli w textboxie nie wpisanej liczby całkowitej, przypisz wartość domyślną 0
                    }
                }
            }
        }
    }
   

    public MainWindow(int Hints)
    {
        new HomeScreen();
        InitializeComponent();
        CreateUI();
        SetUpGame(Hints);
    }

    public void SetUpGame(int HowManyHints)
    {
        var rng = new Random();
        for (int i = 0; i < SizeOfGrid; i += SRSize)
        {
                FillBox(i,i);                            
        }
        FillCells(0, SRSize);
        for(int i = 0;i < SizeOfGrid; i++)
        {
            for(int j = 0; j < SizeOfGrid; j++)
            {
                TextBoxes[i,j].Text = Numbers[i,j].ToString();
            }
        }
        RemoveElements((SizeOfGrid*SizeOfGrid)- HowManyHints);
    }
    void RemoveElements(int AmountToRemove)
    {
        while(AmountToRemove > 0)
        {
            var rng = new Random();
            var x = rng.Next(SizeOfGrid);
            var y = rng.Next(SizeOfGrid);
            if (Numbers[x,y] == 0) continue;
            Numbers[x, y] = 0;
            TextBoxes[x, y].Text = "";
            TextBoxes[x, y].IsEnabled = true;
            AmountToRemove--;
        }
    }
    public void FillBox(int RowStart, int ColStart)
    {
        var rng = new Random();
        var elements = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int i = 0; i < SRSize; i++)
        {
            for (int j = 0; j < SRSize; j++)
            {
                while (true)
                {
                    var index = rng.Next(elements.Count);
                    Numbers[RowStart + i, ColStart + j] = elements[index];
                    if (CheckInBox(RowStart, ColStart))
                    {
                        elements.RemoveAt(index);
                        TextBoxes[RowStart + i, ColStart + j].Text = Numbers[RowStart + i, ColStart + j].ToString();
                        break;
                    }
                }
            }
        }
    }
    public bool FillCells(int Row, int Column)
    {
        if ( Column >= SizeOfGrid && Row < SizeOfGrid - 1)
        {
            Row = Row + 1;
            Column = 0;
        }
        if(Row >= SizeOfGrid && Column >= SizeOfGrid)
        {
            return true;
        }
        if(Row < SRSize)
        {
            if(Column < SRSize)
                Column = SRSize;
        }
        else if(Row < SizeOfGrid - SRSize)
        {
            if(Column == (int)(Row/SRSize)*SRSize)
                Column = Column + SRSize;
        }
        else
        {
            if (Column == SizeOfGrid - SRSize)
            {
                Row = Row + 1;
                Column = 0;
                if (Row >= SizeOfGrid)
                    return true;
            }
        }
        for(int num = 1; num <=SizeOfGrid ;num++)
        {
            Numbers[Row, Column] = num;
            if (CheckAll(Row, Column))
            {
                if (FillCells(Row, Column + 1))
                    return true;
                
            }
            Numbers[Row, Column] = 0;
        }
        return false;
    }
    bool CheckAll(int Row, int Column)
    {
        return (CheckRow(Row) != 0 && CheckColumn(Column) != 0 && CheckInBox(Row - Row % SRSize, Column - Column % SRSize));
    }
}
