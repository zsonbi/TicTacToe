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
        private Label[,] labelek;

        public MainWindow()
        {
            InitializeComponent();
            labelek = new Label[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Label label = new Label();
                    label.MouseLeftButtonDown += Select;
                    label.FontWeight = FontWeight.FromOpenTypeWeight(500);
                    label.FontSize = 120;
                    label.Content = "X";
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;
                    Grid.SetColumn(label, j);
                    Grid.SetRow(label, i);
                    label.Name = "K" + i + "S" + j;
                    Field.Children.Add(label);
                    labelek[i, j] = label;
                }
            }
            MakeBorders();
        }

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
                    }
                }
                setupped = true;
                return;
            }//if

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    szegelyek[i, j].BorderThickness = new Thickness(width, height, width, height);
                }
            }
        }

        private void Select(object sender, MouseButtonEventArgs e)
        {
            byte chosenx;
            byte choseny;
            Label be = sender as Label;
            string stemp = be.Name.Replace('K', ' ');
            chosenx = Convert.ToByte(stemp.Split('S')[1]);
            choseny = Convert.ToByte(stemp.Split('S')[0]);
        }
    }
}