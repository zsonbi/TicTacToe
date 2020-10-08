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
        private byte x = 3;//Alapértelmezetten x tengely méret (3)
        private byte y = 3;//Alapértelmezetten y tengely méret (3)
        private bool setupped;//Már meglett-e csinálva a render
        private bool side;//Melyik oldal jön
        private Border[,] szegelyek;//A szegelyeket letároljuk, hogy amikor méretet frissítünk tudjuk majd frissíteni
        private Label[,] labels;//A labeleket letároljuk, hogy amikor méretet frissítünk tudjuk majd frissíteni
        private byte Checksize = 3;//Alapértelmezetten mennyinek kell kigyülnie a győzelemhez (3)
        private bool over;//Valaki nyert-e már
        private Menu menu = new Menu();//A menu ablak megcsinálása
        private bool inprogress;//A játék folyamatba van-e
        private bool AIcontrolled = false; //AI játszik-e
        private bool aiside;//Az ai melyik oldalt képviseli
        private readonly Random rnd = new Random();//Egy szimpla random
        private bool onlyAIPlays = false;//Csak a 2 AI játszik
        private bool calculating = false;//Ezzel mutatjuk, hogy a gif meddig jelenjen meg
        private ameba ameba=new ameba();

        //---------------------------------------------------------------------------------------------
        //MainWindow inicializálása
        public MainWindow()
        {
            InitializeComponent();

            //A Loading gif betöltése
            ImageBehavior.SetAnimatedSource(HourglassGif, new BitmapImage(new Uri(@"Gifs/hourglass.gif", UriKind.Relative)));

            //A játékmező megcsinálása
            SetupWindow().GetAwaiter();
            //A menu Done gombjához hozzárendeljük a handlert
            menu.Donebtn.Click += Done_Click;
        }

        //--------------------------------------------------------------------------------------------
        //A szegélyek megcsinálása
        private void MakeBorders()
        {
            int width = Convert.ToInt32(Field.ActualWidth / Field.ColumnDefinitions.Count / 20);//A grid egy cellájának a szélességének a 20%-a
            int height = Convert.ToInt32(Field.ActualHeight / Field.RowDefinitions.Count / 20);//A grid egy cellájának a Magasságának a 20%-a
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

            //Ha már meglett csinálva egyszer akkor csak ráfrissít a méretekre
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    szegelyek[i, j].BorderThickness = new Thickness(width, height, width, height);
                }//for
            }//for
        }

        //--------------------------------------------------------------------------------
        //Az elemek méretének igazítása
        private void AdjustSize()
        {
            //Szegélyek frissítése
            MakeBorders();
            //A Labelek méretének frissítése
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
        //Ez csinálja meg az egész játékteret és állítja be a változókat
        private async Task SetupWindow()
        {
            //Resetel mindent, ha a menüből indult ez egyébként pedig ezek az alap értékek
            inprogress = false;
            setupped = false;
            side = true;
            over = false;
            //Reseteli az ablakot
            Field.Children.Clear();
            Field.ColumnDefinitions.Clear();
            Field.RowDefinitions.Clear();

            //A megfelelő számú RowDefinition hozzáadása
            for (int i = 0; i < y; i++)
            {
                Field.RowDefinitions.Add(new RowDefinition());
            }
            //A megfelelő számú ColumnDefinition hozzáadása
            for (int i = 0; i < x; i++)
            {
                Field.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //Labelek írásának mérete
            int size = Convert.ToInt32(Field.ActualHeight / Field.RowDefinitions.Count * 0.85) == 0 ? 150 : Convert.ToInt32(Field.ActualHeight / Field.RowDefinitions.Count * 0.85);
            //Labelek megcsinálása (később lehet képek lesznek)
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

            //Szegélyek megcsinálása
            MakeBorders();
            //A játék adatbázisának megcsinálása
            // game = new PlayField(y, x, (byte)(Checksize - 1));
        }

        //--------------------------------------------------------------------------------------
        //A játék resetelése
        private async Task Reset()
        {
            ameba = new ameba(x, y, Checksize);
            over = false;
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    labels[i, j].Content = "";
                }//for
            }//for
            /*   //Ha AI van
              if (AIcontrolled)
              {
                  await InitializeAI();
              }
              else*/
            side = true;
        }

        //-----------------------------------------------------------------------------
        private void Finish()
        {
            foreach (var item in ameba.winnerCells)
            {
                labels[item[0], item[1]].Foreground = System.Windows.Media.Brushes.Yellow;
            }
        }

        //----------------------------------------------------------------------------------
        //Label tartalmának módosítása
        private void ChangeLabel(bool sidebe, byte choseny, byte chosenx)
        {
            //Melyik oldalnak kell a jelét odarakni
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
        //Ha az ablak mérete változott
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustSize();
        }

        //------------------------------------------------------------------------------------------------
        //A kiválasztás
        private async void Select(object sender, MouseButtonEventArgs e)
        {
            //Hogy a játék már folyamatban van
            inprogress = true;
            //Ha vége van a játéknak ne lehessen bele matatni
            if (ameba.isOver || onlyAIPlays || calculating)
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

            //Egy ideiglenes string változó
            string stemp = be.Name.Replace('K', ' ');
            //Meghatározzuk az indexét a kiválasztott elemnek az adatbázisban
            chosenx = Convert.ToByte(stemp.Split('S')[1]);
            choseny = Convert.ToByte(stemp.Split('S')[0]);

            //A label frissítése
            ChangeLabel(side, choseny, chosenx);

            //A game classban is változtatjuk a cellák értékét (Hogy később majd ne keljen mindig kiolvasni a labelekből)
            ameba.Change(choseny, chosenx);

            if (ameba.isOver)
            {
                Finish();
            }

            //Felcseréljük azt, hogy ki jön
            if (!AIcontrolled)
                side = !side;
        }

        //-----------------------------------------------------------------------
        //Menu és Reset előhozása
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //Megvizsgáljuk, hogy CTRL-t nyomja-e
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl)
            {
                //Megnézzük, hogy nyomja-e azokat a gombokat amik kellenek
                switch (e.Key)
                {
                    case Key.R:
                        if (!over)
                        {
                            //Biztos resetelni akarja-e
                            if (MessageBox.Show("A játék resetelése", "Kérdés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            {
                                return;
                            }//if
                        }
                        Reset();
                        break;

                    case Key.M:
                        menu.Show();

                        break;

                    default:
                        break;
                }
            }
        }

        //--------------------------------------------------------------------------------
        //Menüből a tartalom áthozása
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (inprogress)
            {
                if (MessageBox.Show("A játék folyamatban van biztos akarod?", "Kérdés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
            }

            //Ha illegális érték lenne beírva akkor a default érték lesz
            if (menu.checksizetbox.Text == "" || Convert.ToByte(menu.checksizetbox.Text) <= 2)
            {
                Checksize = 3;
            }//if
            else
            {
                Checksize = Convert.ToByte(menu.checksizetbox.Text);
            }//else
             //Az x és y tengely tesztelése
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

            //Az AI legyen-e
            //A vagy jel után azért kell, hogy ne fussunk bele olyan helyzetbe, hogy csak 1 AI van meg amikor egymás ellen akarjuk uszítani őket
            AIcontrolled = (bool)menu.AIcheck.IsChecked || (bool)menu.OnlyAIPlaysCheckBox.IsChecked;

            onlyAIPlays = (bool)menu.OnlyAIPlaysCheckBox.IsChecked;

            //Elrejtjük a menüt
            menu.Hide();

            //Újra megcsináljuk az ablak felépítését
            SetupWindow().GetAwaiter();

            //A méretek frissítése
            AdjustSize();

            Reset();
        }

        //----------------------------------------------------------------------------------------
        //Leálljon minden amikor kell
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            menu.Close();
        }
    }
}