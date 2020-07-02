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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte x = 3;
        private byte y = 3;
        private bool setupped = false;
        private bool side = true;
        private Border[,] szegelyek;
        private Label[,] labels;
        private PlayField game;
        private byte Checksize = 3;
        private bool over = false;

        //---------------------------------------------------------------------------------------------
        //MainWindow inicializálása
        public MainWindow()
        {
            InitializeComponent();
            labels = new Label[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Label label = new Label();
                    label.MouseLeftButtonDown += Select;
                    label.FontWeight = FontWeight.FromOpenTypeWeight(700);
                    label.FontSize = 120;
                    label.Content = "";
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;
                    Grid.SetColumn(label, j);
                    Grid.SetRow(label, i);
                    label.Name = "K" + i + "S" + j;
                    Field.Children.Add(label);
                    labels[i, j] = label;
                }//for
            }//for
            //Szegélyek megcsinálása
            MakeBorders();
            //A játék adatbázisának megcsinálása
            game = new PlayField(y, x, (byte)(Checksize - 1));
        }

        //--------------------------------------------------------------------------------------------
        //A szegélyek megcsinálása
        private void MakeBorders()
        {
            int width = Convert.ToInt32(Field.ColumnDefinitions.First().ActualWidth * 0.05);//A grid egy cellájának a szélességének a 20%-a
            int height = Convert.ToInt32(Field.RowDefinitions.First().ActualHeight * 0.05);//A grid egy cellájának a Magasságának a 20%-a
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
                        keret.BorderBrush = Brushes.Black;
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

            //Ha már meglett csinálva egyszer akkor csak ráfrissít a méretekre
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    szegelyek[i, j].BorderThickness = new Thickness(width, height, width, height);
                }//for
            }//for
        }

        //-----------------------------------------------------------------------------------------
        //Ha valaki győzött kiemeli sárgával azokat amikkel győzött
        private void Finish()
        {
            //Hogyan győzött srégen vagy egy vonalban
            if (game.Wintype)
            {
                sbyte tempx = (sbyte)game.Start[1];
                sbyte tempy = (sbyte)game.Start[0];
                if (tempx > game.End[1])
                {
                    while (tempx >= game.End[1])
                    {
                        labels[tempy, tempx].Foreground = Brushes.Yellow;
                        tempx--;
                        tempy++;
                    }//while
                }//if
                else
                {
                    while (tempx <= game.End[1])
                    {
                        labels[tempy, tempx].Foreground = Brushes.Yellow;
                        tempx++;
                        tempy++;
                    }//while
                }//else
            }//if
            else
            {
                byte tempx = game.Start[1];
                byte tempy = game.Start[0];
                while (tempx <= game.End[1])
                {
                    labels[game.Start[0], tempx].Foreground = Brushes.Yellow;
                    tempx++;
                }//while
                while (tempy <= game.End[0])
                {
                    labels[tempy, game.Start[1]].Foreground = Brushes.Yellow;
                    tempy++;
                }//while
            }//else
        }

        //-----------------------------------------------------------------------------------------
        //Ha az ablak mérete változott
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MakeBorders();
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    labels[i, j].FontSize = Convert.ToInt32(Field.RowDefinitions.First().ActualHeight * 0.8);
                }
            }
        }

        //------------------------------------------------------------------------------------------------
        //A kiválasztás
        private void Select(object sender, MouseButtonEventArgs e)
        {
            //Ha vége van a játéknak ne lehessen bele matatni
            if (over)
            {
                return;
            }//if
            Label be = sender as Label;
            //Ne lehessen felülírni egy választott értéket
            if (be.Content.ToString() != "")
            {
                return;
            }//if
            byte chosenx;
            byte choseny;

            //Melyik oldal jön
            if (side)
            {
                be.Content = "X";
                be.Foreground = Brushes.Red;
            }//if
            else
            {
                be.Content = "O";
                be.Foreground = Brushes.Blue;
            }//if
            //Egy ideiglenes string változó
            string stemp = be.Name.Replace('K', ' ');
            //Meghatározzuk az indexét a kiválasztott elemnek az adatbázisban
            chosenx = Convert.ToByte(stemp.Split('S')[1]);
            choseny = Convert.ToByte(stemp.Split('S')[0]);
            //A game classban is változtatjuk a cellák értékét (Hogy később majd ne keljen mindig kiolvasni a labelekből)
            game.Change(choseny, chosenx, side);
            //Felcseréljük azt, hogy ki jön
            side = !side;
            //Leteszteljük, hogy valaki nyert-e
            if (game.over)
            {
                Finish();
                MessageBox.Show((game.Winner ? "X" : "O") + " Wins");
                over = true;
            }//if
        }
    }
}