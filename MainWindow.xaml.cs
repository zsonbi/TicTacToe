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
                    label.FontWeight = FontWeight.FromOpenTypeWeight(500);
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
            MakeBorders();
            game = new PlayField(y, x, (byte)(Checksize - 1));
        }

        //--------------------------------------------------------------------------------------------
        //A szegélyek megcsinálása
        private void MakeBorders()
        {
            int width = Convert.ToInt32(Field.ColumnDefinitions.First().ActualWidth * 0.05);//A grid egy cellájának a szélességének a 20%-a
            int height = Convert.ToInt32(Field.RowDefinitions.First().ActualHeight * 0.05);//A grid egy cellájának a Magasságának a 20%-a

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

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    szegelyek[i, j].BorderThickness = new Thickness(width, height, width, height);
                }//for
            }//for
        }

        //------------------------------------------------------------------------------------------------
        //A kiválasztás
        private void Select(object sender, MouseButtonEventArgs e)
        {
            if (over)
            {
                return;
            }

            byte chosenx;
            byte choseny;
            Label be = sender as Label;
            //Ne lehessen felülírni egy választott értéket
            if (be.Content.ToString() != "")
            {
                return;
            }//if
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
            side = !side;
            if (game.over)
            {
                Finish();
                /*    Line vonal = new Line();
                    vonal.X1 = game.Start[1] * Convert.ToInt32(Field.ColumnDefinitions.First().ActualWidth);
                    vonal.Y1 = game.Start[0] * Convert.ToInt32(Field.RowDefinitions.First().ActualHeight);
                    vonal.X2 = (game.End[1] + 1) * Convert.ToInt32(Field.ColumnDefinitions.First().ActualWidth);
                    vonal.Y2 = (game.End[0] + 1) * Convert.ToInt32(Field.RowDefinitions.First().ActualHeight);

                    vonal.Stroke = Brushes.Red;
                    vonal.StrokeThickness = 10;
                    Main.Children.Add(vonal);*/
                MessageBox.Show((game.Winner ? "X" : "O") + " Wins");
                over = true;
            }//if
        }

        private void Finish()
        {
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
                    }
                }
                else
                {
                    while (tempx <= game.End[1])
                    {
                        labels[tempy, tempx].Foreground = Brushes.Yellow;
                        tempx++;
                        tempy++;
                    }
                }
            }
            else
            {
                byte tempx = game.Start[1];
                byte tempy = game.Start[0];
                while (tempx <= game.End[1])
                {
                    labels[game.Start[0], tempx].Foreground = Brushes.Yellow;
                    tempx++;
                }
                while (tempy <= game.End[0])
                {
                    labels[tempy, game.Start[1]].Foreground = Brushes.Yellow;
                    tempy++;
                }
            }
        }
    }
}