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
        //Csak számot lehessen beírni
        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //-----------------------------------------------------------------
        //A hibás érték vizsgálata
        private void checksizetbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox be = sender as TextBox;
            //Ha üresen hagyná
            if (be.Text == "")
            {
                be.Background = Brushes.Red;
                return;
            }
            //Ne legyen túl nagy figyelmeztetjük a felhasználót
            if (Convert.ToByte(be.Text) > 5)
            {
                be.Background = Brushes.Yellow;
            }//if
            //Ne adjunk meg hülyeséget
            else if (Convert.ToByte(be.Text) <= 2)
            {
                be.Background = Brushes.Red;
            }//else if
             //Ha megfelel
            else
            {
                be.Background = Brushes.Green;
            }//else
        }

        //-------------------------------------------------------------------
        //A hibás értékek vizsgálata a tengelyek Textboxánál
        private void tengelyek_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox be = sender as TextBox;
            //Ha üresen hagyná
            if (be.Text == "")
            {
                be.Background = Brushes.Red;
                return;
            }
            //Ne legyen túl nagy figyelmeztetjük a felhasználót
            if (Convert.ToByte(be.Text) > 32)
            {
                MessageBox.Show("32-nél többet nem ajánlok, de ám legyen (lehet nem fog menni)");
                be.Background = Brushes.Yellow;
            }//if
            //Ne legyen túl kicsit
            else if (Convert.ToByte(be.Text) < 3)
            {
                be.Background = Brushes.Red;
            }//else if
            //Ha megfelel
            else
            {
                be.Background = Brushes.Green;
            }//else
        }

        //----------------------------------------------------------------------------------
        //Figyelmeztessük a felhasználót, hogy meg fogja ölni a gépét
        private void MiniMaxAIRadiobtn_Checked(object sender, RoutedEventArgs e)
        {
            if ((xtengelytbox.Text != "" && Convert.ToInt32(xtengelytbox.Text) > 3) || (ytengelytbox.Text != "" && Convert.ToInt32(ytengelytbox.Text) > 3))
            {
                MessageBox.Show("3x3-nál szerintem nem akarod nagyobbra a géped meg fog halni sok számítás kell az 3^(x*y) kombináció kiszámolásához");
            }
        }
    }
}