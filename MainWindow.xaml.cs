using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte x = 3;//x axis size default (3)
        private byte y = 3;//y axis size default (3)
        private bool setupped;//if the board have been already made
        private bool side;//which side comes next
        private Border[,] szegelyek;//the borders so it's easier to update their sizes
        private Label[,] labels;//the labels so it's easier to update their sizes and values
        private byte Checksize = 3;//how many do the player need to win default (3)
        private Menu menu;//the menu
        private bool inprogress;//is the game in progress
        private bool AIcontrolled = false; //is the ai playing
        private bool aiside;//which side does the ai playing
        private readonly Random rnd = new Random();//a simple random
        private bool onlyAIPlays = false;//if only just 2 ai playing
        private bool calculating = false;//should the gif still be showing
        private ameba ameba = new ameba(); //The game itself
        private byte aiType = 0; //The Type of the ai (0-Minimax, 1-MCTS)

        //---------------------------------------------------------------------------------------------
        //Constructor
        public MainWindow()
        {
            InitializeComponent();

            //Loading in the Hourglass.gif
            ImageBehavior.SetAnimatedSource(HourglassGif, new BitmapImage(new Uri(@"Gifs/hourglass.gif", UriKind.Relative)));

            //Creates the playfield
            SetupWindow().GetAwaiter();
        }

        //--------------------------------------------------------------------------------------------
        //Makes the borders between the cells
        private void MakeBorders()
        {
            int width = Convert.ToInt32(Field.ActualWidth / Field.ColumnDefinitions.Count / 20);//20% of the width of a cell
            int height = Convert.ToInt32(Field.ActualHeight / Field.RowDefinitions.Count / 20);//20% of the height of a cell
            //Ha még nem lett megcsinálva
            if (!setupped)
            {
                szegelyek = new Border[y, x];
                for (int i = 0; i < y; i++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        Border keret;
                        keret = new Border();
                        keret.BorderBrush = System.Windows.Media.Brushes.Black;
                        Grid.SetRow(keret, i);
                        Grid.SetColumn(keret, j);
                        Field.Children.Add(keret);
                        keret.BorderThickness = new Thickness(9, 9, 9, 9);
                        szegelyek[i, j] = keret;
                    }//for
                }//for
                setupped = true;
                return;
            }//if

            //If the borders are already made then just updates their sizes
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    szegelyek[i, j].BorderThickness = new Thickness(width, height, width, height);
                }//for
            }//for
        }

        //--------------------------------------------------------------------------------
        //Updates the sizes
        private void AdjustSize()
        {
            //Updates the Borders
            MakeBorders();
            //Updates the size of the Labels
            int size = Convert.ToInt32(Field.ActualHeight / Field.RowDefinitions.Count * 0.85);
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    labels[i, j].FontSize = size;
                }//for
            }//for
        }

        //-----------------------------------------------------------------------------------------
        //This makes the playfield and some of the varriables
        private async Task SetupWindow()
        {
            //Resets everything just incase
            inprogress = false;
            setupped = false;
            side = true;
            //Reseteli the window
            Field.Children.Clear();
            Field.ColumnDefinitions.Clear();
            Field.RowDefinitions.Clear();

            //Adding the RowDefinitions
            for (int i = 0; i < y; i++)
            {
                Field.RowDefinitions.Add(new RowDefinition());
            }
            //Adding the ColumnDefinitions
            for (int i = 0; i < x; i++)
            {
                Field.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //The Size of the Labels
            int size = Convert.ToInt32(Field.ActualHeight / Field.RowDefinitions.Count * 0.85) == 0 ? 150 : Convert.ToInt32(Field.ActualHeight / Field.RowDefinitions.Count * 0.85);
            //Making the Labels on the board
            labels = new Label[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Label label = new Label();
                    label.MouseLeftButtonDown += Select;
                    label.FontWeight = FontWeight.FromOpenTypeWeight(700);
                    label.FontSize = size;
                    label.Content = "";
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;
                    label.Padding = new Thickness(0, 0, 0, 0);
                    Grid.SetColumn(label, j);
                    Grid.SetRow(label, i);
                    label.Name = "K" + i + "S" + j;
                    Field.Children.Add(label);
                    labels[i, j] = label;
                }//for
            }//for

            //Makes the borders
            MakeBorders();
        }

        //--------------------------------------------------------------------------------------
        //Resets the game and the board
        private async Task Reset()
        {
            //Get the side of the ai
            if ((bool)menu.randomradiobutton.IsChecked)
                aiside = Convert.ToBoolean(rnd.Next(0, 1));
            else
                aiside = (bool)menu.Xradiobutton.IsChecked;

            //opposite of the aiside if there is an ai
            side = AIcontrolled ? !aiside : true;
            ameba = new ameba(x, y, Checksize, AIcontrolled, aiside, aiType);
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    labels[i, j].Content = "";
                }//for
            }//for

            //if the ai starts make the first move
            if (aiside)
            {
                HourglassGif.Visibility = Visibility.Visible;
                IAction temp = await ameba.Next();
                await ameba.Change(temp.Move[0], temp.Move[1]);
                //Update the label
                ChangeLabel(!side, temp.Move[0], temp.Move[1]);
                side = false;
                HourglassGif.Visibility = Visibility.Hidden;
            }
        }

        //-----------------------------------------------------------------------------
        //Mark the cells which caused the win
        private void Finish()
        {
            foreach (var item in ameba.winnerCells)
            {
                labels[item[0], item[1]].Foreground = System.Windows.Media.Brushes.Yellow;
            }//forea
        }

        //----------------------------------------------------------------------------------
        //Changes the content of the Label
        private void ChangeLabel(bool sidebe, byte choseny, byte chosenx)
        {
            //Which side placed their mark there
            if (sidebe)
            {
                labels[choseny, chosenx].Content = "X";
                labels[choseny, chosenx].Foreground = System.Windows.Media.Brushes.Red;
            }//if
            else
            {
                labels[choseny, chosenx].Content = "O";
                labels[choseny, chosenx].Foreground = System.Windows.Media.Brushes.Blue; ;
            }//if
        }

        //HANDLEREK
        //-----------------------------------------------------------------------------------------
        //If the window size changed run AdjustSize()
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustSize();
        }

        //------------------------------------------------------------------------------------------------
        //Selection of the cell and changing it
        private async void Select(object sender, MouseButtonEventArgs e)
        {
            //A varriable to inicate that the game is in progress
            inprogress = true;
            //When the game is over or the ai is taking it's time the player can't mess stuff up
            if (ameba.isOver || onlyAIPlays || calculating)
            {
                return;
            }//if
            Label be = sender as Label;
            //So the players can't overwrite an already filled cell
            if (be.Content.ToString() != "")
            {
                return;
            }//if
            byte chosenx;//x cord of the selected cell
            byte choseny;//y cord of the selected cell

            //A temporary varriable
            string stemp = be.Name.Replace('K', ' ');
            //Get the cords of the selected cell
            chosenx = Convert.ToByte(stemp.Split('S')[1]);
            choseny = Convert.ToByte(stemp.Split('S')[0]);

            //Update the Label
            ChangeLabel(side, choseny, chosenx);

            //Change the value of the cell also in the game class also checks if we got a winner
            await ameba.Change(choseny, chosenx);
            //If we got a Winner or Draw
            if (ameba.isOver)
            {
                Finish();
                return;
            }//if

            //swap the sides if it's not controlled by ai
            if (!AIcontrolled)
                side = !side;
            else
            {
                HourglassGif.Visibility = Visibility.Visible;
                //The moves which the ai will make
                IAction temp = await ameba.Next();
                await ameba.Change(temp.Move[0], temp.Move[1]);
                //Update the label
                ChangeLabel(!side, temp.Move[0], temp.Move[1]);
                HourglassGif.Visibility = Visibility.Hidden;
            }//else
            //If we got a Winner or Draw
            if (ameba.isOver)
            {
                Finish();
            }//if
        }

        //-----------------------------------------------------------------------
        //Keyboard Control
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //Checks if the user is holding down the CTRL
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl)
            {
                //Checks if the user presses eiter R or M
                switch (e.Key)
                {
                    case Key.R:
                        if (ameba.isOver)
                        {
                            //To make sure if the player really want to quit
                            if (MessageBox.Show("A játék resetelése", "Kérdés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            {
                                return;
                            }//if
                        }
                        //Resets everything
                        Reset();
                        break;

                    case Key.M:
                        //Creates a new Menu Window
                        menu = new Menu();
                        //Gives the menu's Done button a Handler
                        menu.Donebtn.Click += Done_Click;
                        menu.Show();
                        break;

                    default:
                        break;
                }
            }
        }

        //--------------------------------------------------------------------------------
        //Getting the data from the menu
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            //So we warn the user that he/she is about to terminate the game
            if (inprogress)
            {
                if (MessageBox.Show("The game is still in progress are you sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }//if
            }//if

            //Checks the checksizetbox if it's value is illegal give it a default
            if (menu.checksizetbox.Text == "" || Convert.ToByte(menu.checksizetbox.Text) <= 2)
            {
                Checksize = 3;
            }//if
            else
            {
                Checksize = Convert.ToByte(menu.checksizetbox.Text);
            }//else
             //Checks if the x and y textbox values are correct if they're empty give the default
            if (menu.xtengelytbox.Text == "")
            {
                if (menu.ytengelytbox.Text == "")
                {
                    x = 3;
                    y = 3;
                    menu.Hide();
                    return;
                }//if
                else
                {
                    y = Convert.ToByte(menu.ytengelytbox.Text);
                    x = y;
                    menu.Hide();
                    return;
                }//else
            }//if
            else
            {
                x = Convert.ToByte(menu.xtengelytbox.Text);
            }//else
            if (menu.ytengelytbox.Text == "")
            {
                y = x;
            }//if
            else
            {
                y = Convert.ToByte(menu.ytengelytbox.Text);
            }//else

            //Should the program make ais
            //The or operator is needed so we don't run in to a problem such as we don't have 2 ais
            AIcontrolled = (bool)menu.AIcheck.IsChecked || (bool)menu.OnlyAIPlaysCheckBox.IsChecked;

            onlyAIPlays = (bool)menu.OnlyAIPlaysCheckBox.IsChecked;

            if ((bool)menu.MiniMaxAIRadiobtn.IsChecked)
                aiType = 0;
            else if ((bool)menu.MCTSRadiobtn.IsChecked)
                aiType = 1;
            else
                aiType = 2;
            //Hides the menu
            menu.Close();

            //The program creates a new board for us
            SetupWindow().GetAwaiter();

            //Updates the sizes in our window
            AdjustSize();

            //Resets the board
            Reset();
        }

        //----------------------------------------------------------------------------------------
        //So it stops the menu as well
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (menu != null)
                menu.Close();
        }
    }
}