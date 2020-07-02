﻿using System;
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
using System.Text.RegularExpressions;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void checksizetbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox be = sender as TextBox;
            if (Convert.ToByte(be.Text) > 5)
            {
                MessageBox.Show("5-nél többet nem ajánlok, de ám legyen");
                be.Background = Brushes.Yellow;
            }
            else if (Convert.ToByte(be.Text) <= 2)
            {
                be.Background = Brushes.Red;
            }
            else
            {
                be.Background = Brushes.Green;
            }
        }

        private void tengelyek_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox be = sender as TextBox;
            if (Convert.ToByte(be.Text) > 32)
            {
                MessageBox.Show("32-nél többet nem ajánlok, de ám legyen (lehet nem fog menni)");
                be.Background = Brushes.Yellow;
            }
            else if (Convert.ToByte(be.Text) < 3)
            {
                be.Background = Brushes.Red;
            }
            else
            {
                be.Background = Brushes.Green;
            }
        }
    }
}