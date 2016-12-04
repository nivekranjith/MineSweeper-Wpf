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
using System.Windows.Threading;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte numMines;  //Number of mines in the game
        static int gridRow; //Number of Rows
        static int gridCol; //Numberof Columns
        byte[,] checkFlags; //Holds data for elements that are flagged
        int iWin; //Check if you win
        int iCheck; //Check how many tiles you have opened
        int bflags; 
        byte timer = 0;

     
        PlayField p; //Class creating an array for the game

        DispatcherTimer _timer; //TImer
        TimeSpan _time;

        public MainWindow()
        {
           

            InitializeComponent();
            //Starts game on easy
            gridCol = 8;
            gridRow = 8;
            numMines = 10;
            gameRestart();

        }

        private void InitializeGrid()
        {
          
            oGrid.Background = System.Windows.Media.Brushes.Bisque;
            //Set the rows

            for (int j = 0; j < gridRow; j++)
            {
                //Add specific number of rows to grid
                RowDefinition Row0 = new RowDefinition();
                //Add row to grid control     
                oGrid.RowDefinitions.Add(Row0);
            }

            for(int j = 0; j<gridCol; j++)
            {
                //Add specific number of columns to grid
                ColumnDefinition Col0 = new ColumnDefinition();
                //Add column to grid control
                oGrid.ColumnDefinitions.Add(Col0);
            }


            for (int k = 0; k < gridRow; k++)
            {
                for (int l = 0; l < gridCol; l++)
                {
                    //Add a blank button to each element of the grid
                    Button MyControl = new Button();
                    MyControl.Content = "";
                    MyControl.Height = 20;
                    MyControl.Width = 20;

                    Grid.SetRow(MyControl, k);
                    Grid.SetColumn(MyControl, l);
                    oGrid.Children.Add(MyControl);                   
                }
            }

            //Show the grid lines
            oGrid.ShowGridLines = true;
        }

        private void oGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Handle when left mouse is clicked
        {

            if(timer == 0) //Start timer the first time you click
            {
                timer = 1;
                _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    tbTime.Text = _time.ToString("c");
                    _time = _time.Add(TimeSpan.FromSeconds(+1));
                }, Application.Current.Dispatcher);

                _timer.Start();


            }

            var element = (UIElement)e.Source; //reference UIelement
            int row = Grid.GetRow(element); //get Row from UIElement
            int column = Grid.GetColumn(element); //get column from UIelement

            //Check if "button" has already been clicked
            if (element.GetType() == typeof(Button)) //Hasnt been clicked before
            {

            }
            else //Has been clicked
            {
                row = -1;
                column = -1;
            }
            Button MyControl = new Button();

            if (row >= 0 && column >= 0) //Change value of the button if button has not been clicked before
            {
                if (checkFlags[row, column] == 1)
                {
                    checkFlags[row, column] = 0;
                    bflags++;
                    tbMines.Text = bflags.ToString();
                }
                if ( p.getField(row, column) == 0)
                {
                    byte[,] arrZeroes = new byte[gridRow, gridCol];//Used to keep track of all zeroes expanded, if a null element is chosen
                    arrZeroes[row, column] = 1;

                    MyControl.Content =  "";
                    MyControl.IsEnabled = false; //After one left click ,button in the grid element is no longer clickable

                      Grid.SetRow(MyControl, row); //Open an null element
                      Grid.SetColumn(MyControl, column);
                      oGrid.Children.Add(MyControl);
                       iCheck++;
                    
                    expandZeroes(arrZeroes, row, column); //Expand all nulls adjacent to the null element clicked

                    if(iCheck == iWin)
                    {//If all elements are opened excluding the mines then you win
                        MessageBox.Show("WIN");
                        timer = 0;
                        _timer.Stop();
                        BitmapImage btm = new BitmapImage(new Uri("win.png", UriKind.Relative)); //Change face to vicory face
                        Image img = new Image();
                        img.Source = btm;
                        img.Stretch = Stretch.Fill;
                        button.Content = img;
                    }

                }
                else if((p.getField(row, column)== 9)) //9 in the array is declared as the mine
                    {

                    RevealBombs(row,column);//Reveal all mines if you clicked of a mine
                 
                    timer = 0;
                    _timer.Stop();
                    BitmapImage btm = new BitmapImage(new Uri("sad.png", UriKind.Relative)); //Change face to sad face
                    Image img = new Image();
                    img.Source = btm;
                    img.Stretch = Stretch.Fill;
                    button.Content = img;

                  
                    BitmapImage btm1 = new BitmapImage(new Uri("bomb1.png", UriKind.Relative)); //Show which mine you clicked on
                    Image img1 = new Image();
                    img1.Source = btm1;
                    img1.Stretch = Stretch.Fill;
                    MyControl.Content = img1;

                    Grid.SetRow(MyControl, row);
                    Grid.SetColumn(MyControl, column);
                    oGrid.Children.Add(MyControl);
                }
                else //if you didnt click on a mine nor a null tile
                {
                   
                    MyControl.Content = p.getField(row, column);
                    MyControl.Foreground = System.Windows.Media.Brushes.Black;
                    MyControl.FontWeight = FontWeights.Bold;
                    MyControl.IsEnabled = false; //After one left click ,button in the grid element is no longer clickable

                    Grid.SetRow(MyControl, row);
                    Grid.SetColumn(MyControl, column);
                    oGrid.Children.Add(MyControl);
                    iCheck++;

                    if(iCheck == iWin)
                    {
                        MessageBox.Show("WIN");
                        timer = 0;
                        _timer.Stop();
                        BitmapImage btm = new BitmapImage(new Uri("win.png", UriKind.Relative)); //Change face to vicory face
                        Image img = new Image();
                        img.Source = btm;
                        img.Stretch = Stretch.Fill;
                        button.Content = img;
                    }
                    
                    
                }
            }
            
       

        }

        private void oGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) //Handles right click for each element in grid
        {

            var element = (UIElement)e.Source;
            int row = Grid.GetRow(element);
            int column = Grid.GetColumn(element);

            if (element.GetType() == typeof(Button)) //Hasnt been clicked before
            {
            }
            else //Has been clicked
            {
                row = -1;
                column = -1;
            }
            if (row >= 0 && column >= 0)
            {
                Button MyControl = new Button();
                MyControl.Content = "F"; //This represents a flag
                MyControl.Foreground = System.Windows.Media.Brushes.Black;
                MyControl.FontWeight = FontWeights.Bold;
                MyControl.IsEnabled = true; //After one left click ,button in the grid element is no longer clickable

                Grid.SetRow(MyControl, row);
                Grid.SetColumn(MyControl, column);
                oGrid.Children.Add(MyControl);

                checkFlags[row, column] = 1;
                bflags--; //Decrements when you have used a flag
                tbMines.Text = bflags.ToString(); //Shows how many flags you have left on the game
            }
        }

        public void expandZeroes(byte[,] arrzeroes, int r, int c) //recursively checks all adjacent nulls to the null we have clicked
                                                                   //This function was called above
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    { }
                    else
                    if ((r + i) >= 0 && (r + i) < gridRow)
                    {
                        if ((c + j) >= 0 && (c + j) < gridCol)
                        {
                            if (p.getField(r + i, c + j) == 0 && arrzeroes[r + i, c + j] == 0)
                            {
                                arrzeroes[r + i, c + j] = 1;

                                checkFlags[r + i, c + j] = 0; //Expand null tiles
                                Button MyControl = new Button();
                                MyControl.Content = "";
                                MyControl.IsEnabled = false;

                                Grid.SetRow(MyControl, r + i);
                                Grid.SetColumn(MyControl, c + j);
                                oGrid.Children.Add(MyControl);
                                iCheck++;

                                for (int k = -1; k <= 1; k++)
                                    for (int l = -1; l <= 1; l++)
                                    {
                                        if ((r + i + k) >= 0 && (r + i + k) < gridRow)
                                        {
                                            if ((c + j + l) >= 0 && (c + j + l) < gridCol)
                                            {
                                                if (p.getField(r + i + k, c + j + l) != 9 && p.getField(r + i + k, c + j + l) != 0)
                                                {//Checks for tiles adjacent to the null but not a mine
                                                    Button MyControl1 = new Button();
                                                    MyControl1.Content = p.getField(r + i + k, c + j + l);
                                                    MyControl1.IsEnabled = false;

                                                    Grid.SetRow(MyControl1, r + i + k);
                                                    Grid.SetColumn(MyControl1, c + j + l);
                                                    oGrid.Children.Add(MyControl1);
                                                    iCheck++;
                                                }
                                            }
                                        }

                                        expandZeroes(arrzeroes, r + i, c + j);

                                    }
                            }
                        }
                    }
                }
        }

        public void RevealBombs(int r,int c)
        {
          //Reveals mines if one is clicked on
          //Function was called above
            for (int i = 0; i<gridRow; i++)
                for(int j = 0; j<gridCol;j++)
                {
                    if (p.getField(i, j) == 9 &&i!=r && j!=c)
                    {
                        if (checkFlags[i, j] == 1)
                        {

                        }
                        else 
                        {

                            Button MyControl = new Button();
                            BitmapImage btm = new BitmapImage(new Uri("bomb.png", UriKind.Relative)); //Assign the mine image to all mines
                            Image img = new Image();
                            img.Source = btm;
                            img.Stretch = Stretch.Fill;
                            MyControl.Content = img;

                            MyControl.IsEnabled = false;

                            Grid.SetRow(MyControl, i);
                            Grid.SetColumn(MyControl, j);
                            oGrid.Children.Add(MyControl);

                            oGrid.IsEnabled = false;
                        }
                    }
                    else if(p.getField(i,j) != 9 && checkFlags[i,j] == 1)
                    {
                        Button MyControl = new Button();
                        BitmapImage btm = new BitmapImage(new Uri("flag1.png", UriKind.Relative));//Shows all flags incorrectly placed
                        Image img = new Image();
                        img.Source = btm;
                        img.Stretch = Stretch.Fill;
                        MyControl.Content = img;

                        MyControl.IsEnabled = false;

                        Grid.SetRow(MyControl, i);
                        Grid.SetColumn(MyControl, j);
                        oGrid.Children.Add(MyControl);

                        oGrid.IsEnabled = false;
                    }
                    else
                    {

                        oGrid.IsEnabled = false;
                    }

                }
        }

        private void button_Click(object sender, RoutedEventArgs e) //Restart button
        {
            gameRestart();
            BitmapImage btm = new BitmapImage(new Uri("Smiley.png", UriKind.Relative));
            Image img = new Image();
            img.Source = btm;
            img.Stretch = Stretch.Fill;
            button.Content = img;
        }

        private void menuEasy_Click(object sender, RoutedEventArgs e)//Clicking on easy mode in menu
        {
            menuEasy.FontWeight.Equals("Bold");
            gridCol = 8;
            gridRow = 8;
            numMines = 10;
            gameRestart();
        }

        private void menuMed_Click(object sender, RoutedEventArgs e)//clicking on medium mode in menu
        {
            gridCol = 16;
            gridRow = 16;
            numMines = 40;
            gameRestart();
        }

        private void menuHard_Click(object sender, RoutedEventArgs e)//clciking on hard mode in menu
        {
            gridCol = 24;
            gridRow = 24;
            numMines = 99;

            gameRestart();
          
        }

        public void gameRestart() //Restart the game whenever necessary
        {
            if (timer == 1)
            {
                _timer.Stop();
            }
            timer = 0;
            _time = TimeSpan.FromSeconds(0);
            tbTime.Text = _time.ToString("c");
            oGrid.RowDefinitions.Clear();
            oGrid.ColumnDefinitions.Clear();
            InitializeGrid();
            oGrid.IsEnabled = true;

            p = new PlayField(numMines, gridRow, gridCol);
            p.createField();
            checkFlags = new byte[gridRow, gridCol];

            iWin = gridRow * gridCol - numMines;
            iCheck = 0;
            tbMines.Text = numMines.ToString();
            bflags = numMines;


        }

      
        private void SliderCol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
          
        }

        private void btnCustom_Click(object sender, RoutedEventArgs e)//if you clicked on the customised tab in menu
        {
            double sliderRow = SliderRow.Value;
            double sliderCol = SliderCol.Value;
            double maxMines = (sliderRow - 1) * (sliderCol - 1);

            if(SliderMines.Value > maxMines)
            {
                MessageBox.Show("Max number of mines for selected row and column is " + maxMines);//checks if you have exceeded the number of mines
                SliderMines.Value = maxMines;
            }
            else
            {
                gridCol = Convert.ToInt32( sliderCol);
                gridRow = Convert.ToInt32(sliderRow);
                numMines = Convert.ToByte(SliderMines.Value);

                gameRestart();

            }

        }

        private void ClrPcker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {//Change colour option in menu
             oGrid.Background = new SolidColorBrush(Color.FromRgb(ClrPcker.SelectedColor.Value.R, ClrPcker.SelectedColor.Value.G, ClrPcker.SelectedColor.Value.B));
             Border border = new Border { Background = new SolidColorBrush(Color.FromRgb(ClrPcker.SelectedColor.Value.R, ClrPcker.SelectedColor.Value.G, ClrPcker.SelectedColor.Value.B)) };
           }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        { //Normal sized grid
            oGrid.Height = 400;
            oGrid.Width = 350;

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //Zoomed in grid
            oGrid.Height = 800;
            oGrid.Width = 700;
        }
    }

    public partial class PlayField //class creating an array representing the field
    {

        private int mines;
        private int numCols;
        private int numRows;
        private byte[,] arrayField;

        public PlayField(int m , int r,int c){
            mines = m;
            numCols = c;
            numRows = r;
           

            }

        public void createField()
        {
            arrayField = new byte[numRows, numCols];

          addMines();//Call function to randomly add mines 
          addNumbers(); // Call function to generate values relative to the mines' position

        }

        public byte getField(int r , int c)
        {
            return arrayField[r,c];
        }

        public void addMines() //Add Mines to field
        {
            for (int i = 0; i <=mines; i++) //randomly add mines
            {
                Random randomx = new Random();
                int x = randomx.Next(0, numRows - 1); //get Random numbers between grid size
                int y = randomx.Next(0, numCols - 1);

                if (arrayField[x, y] == 0) //if element is unassigned then add mine
                {
                    arrayField[x, y] = 9;
                }
                else
                {
                    while (arrayField[x, y] != 0) // if a mine already exists here ,then continuously get random numbers until an unassigned element is found
                    {
                        x = randomx.Next(0, numRows - 1);
                        y = randomx.Next(0, numCols - 1);
                    }
                    arrayField[x, y] = 9;
                }
            }

        }

        public void addNumbers() //add numbers to the Field based on the position of the mines
        {
            for (int i = 0; i < numRows; i++) //iterate through each element in the array(iterate row-by-row due to how arrays are stored in memory i.e. more efficient
                for (int j = 0; j < numCols; j++)
                {
                    if (arrayField[i, j] != 9) // all elements excluding the mines
                    {
                        ///////////////// Check Top edge/////////////////////////

                        if (i == 0)
                        {
                            if (j == 0) //if element is at the top left corner
                            {
                                arrayField[i, j] = getCorners(0, i, j);
                            }
                            else
                            if (j == numCols - 1) //if element is at the top right corner 
                            {
                                arrayField[i, j] = getCorners( 1, i, j);
                            }
                            else //rest of the elements on top edge
                            {
                                arrayField[i, j] = getEdges( 0, i, j);
                            }

                        }
                        else    ////////////Check bottom edge///////////////////////////
                        if (i == numRows - 1)
                        {
                            if (j == 0) //if element is at bottom left corner
                            {
                                arrayField[i, j] = getCorners( 2, i, j);
                            }
                            else
                            if (j == numCols - 1) //if element is at bottom right corner
                            {
                                arrayField[i, j] = getCorners( 3, i, j);
                            }
                            else //rest of the elements on bottom edge 
                            {
                                arrayField[i, j] = getEdges( 1, i, j);
                            }

                        }
                        else if (j == 0) ////////////////////Check left edge///////////////////////
                        {
                            arrayField[i, j] = getEdges( 2, i, j);
                        }
                        else if (j == numCols - 1) ////Check right edge
                        {
                            arrayField[i, j] = getEdges( 3, i, j);
                        }
                        else //////////////Everything inbetween all 4 edges
                        {
                            arrayField[i, j] = getTheRest( i, j);

                        }

                    }

                }            
        }

        public byte getCorners(byte state,int r, int c)
        {
            byte count = 0;
            if (state == 0) // Top left corner
            {
                if (arrayField[r, c + 1] == 9)
                {
                    count++;
                }
                if (arrayField[r + 1, c] == 9)
                {
                    count++;
                }
                if (arrayField[r + 1, c + 1] == 9)
                {
                    count++;
                }
            }

            if (state == 1) //Top right corner
            {
                if (arrayField[r, c - 1] == 9)
                {
                    count++;
                }
                if (arrayField[r + 1, c] == 9)
                {
                    count++;
                }
                if (arrayField[r + 1, c - 1] == 9)
                {
                    count++;
                }
            }

            if (state == 2) //Bottom left corner
            {

                if (arrayField[r - 1, c] == 9)
                {
                    count++;
                }
                if (arrayField[r, c + 1] == 9)
                {
                    count++;
                }
                if (arrayField[r - 1, c + 1] == 9)
                {
                    count++;
                }
            }

            if (state == 3) //Bottom right corner
            {

                if (arrayField[r - 1, c] == 9)
                {
                    count++;
                }
                if (arrayField[r, c - 1] == 9)
                {
                    count++;
                }
                if (arrayField[r - 1, c - 1] == 9)
                {
                    count++;
                }
            }


            return count;
        }

        public byte getEdges(byte state , int r, int c)
        {
            byte count = 0;
            if (state == 0) // top edge
            {
                if (arrayField[r, c - 1] == 9)
                {
                    count++;
                }
                if (arrayField[r, c + 1] == 9)
                {
                    count++;
                }

                for (int i = -1; i <= 1; i++)
                {
                    if (arrayField[r + 1, c + i] == 9)
                    {
                        count++;
                    }
                }
            }

            if (state == 1)
            {
                if (arrayField[r, c - 1] == 9)
                {
                    count++;
                }
                if (arrayField[r, c + 1] == 9)
                {
                    count++;
                }

                for (int i = -1; i <= 1; i++)
                {
                    if (arrayField[r - 1, c + i] == 9)
                    {
                        count++;
                    }
                }
            }

            if (state == 2)
            {

                if (arrayField[r + 1, c] == 9)
                {
                    count++;
                }

                if (arrayField[r - 1, c] == 9)
                {
                    count++;
                }

                for (int i = -1; i <= 1; i++)
                {
                    if (arrayField[r + i, c + 1] == 9)
                    {
                        count++;
                    }
                }

            }

            if (state == 3)
            {
                if (arrayField[r + 1, c] == 9)
                {
                    count++;
                }

                if (arrayField[r - 1, c] == 9)
                {
                    count++;
                }

                for (int i = -1; i <= 1; i++)
                {
                    if (arrayField[r + i, c - 1] == 9)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public byte getTheRest(int r , int c)
        {
            byte count = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0)
                    {
                        // exclude the relevant point
                    }
                    else
                    {
                        if (arrayField[r + i, c + j] == 9)
                        {
                            count++;
                        }
                    }
                }

            return count;
        }


    }

  

}
