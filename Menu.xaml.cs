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

        //-----------------------------------------------------------------
        //Only numbers -,-
        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //-----------------------------------------------------------------
        //Checks for incorrect values and alerts the user for the checkbox
        private void checksizetbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox be = sender as TextBox;
            //If it were left empty
            if (be.Text == "")
            {
                be.Background = Brushes.Red;
                return;
            }
            //Alert the user that the value is too big and almost impossible to win for either side
            if (Convert.ToByte(be.Text) > 5)
            {
                be.Background = Brushes.Yellow;
            }//if
            //This value is just a joke if someone want to play with this
            else if (Convert.ToByte(be.Text) <= 2)
            {
                be.Background = Brushes.Red;
            }//else if
             //if it's correct
            else
            {
                be.Background = Brushes.Green;
            }//else
        }

        //-------------------------------------------------------------------
        //Checks for incorrect values and alerts the user for the xtengelytbox and ytengelytbox
        private void tengelyek_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox be = sender as TextBox;
            //if it were left empty
            if (be.Text == "")
            {
                be.Background = Brushes.Red;
                return;
            }
            //Alerts the user that it will be too big
            if (Convert.ToByte(be.Text) > 32)
            {
                MessageBox.Show("I won't suggest doing 32 or more because it might not load and also who needs this big board");
                be.Background = Brushes.Yellow;
            }//if
            //So it won't be too small
            else if (Convert.ToByte(be.Text) < 3)
            {
                be.Background = Brushes.Red;
            }//else if
            //if it's correct
            else
            {
                be.Background = Brushes.Green;
            }//else
        }

        //----------------------------------------------------------------------------------
        //Alert the user that he/she will kill the computer
        private void MiniMaxAIRadiobtn_Checked(object sender, RoutedEventArgs e)
        {
            if ((xtengelytbox.Text != "" && Convert.ToInt32(xtengelytbox.Text) > 3) || (ytengelytbox.Text != "" && Convert.ToInt32(ytengelytbox.Text) > 3))
            {
                MessageBox.Show("Your pc will die with this big board and this type of ai (just a warning)");
            }//if
        }
    }
}