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

namespace XZero
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class ChoseWindow : Window
    {
        GemeLogic GL;

        public ChoseWindow(GemeLogic gl)
        {
            this.GL = gl;
            InitializeComponent();
        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Content.ToString() != "X")
            {
                //Ход 2-го игрока
                GL.PressButton(GL.GetPcButton());
            }
        }

        private void RadioButton_PlayerPC(object sender, RoutedEventArgs e)
        {
            this.GL.SetGameType(GameType.PlayerPC);
        }

        private void RadioButton_PlayerPlayer(object sender, RoutedEventArgs e)
        {
            this.GL.SetGameType(GameType.PlayerPlayer);
        }

        private void RadioButton_PCPC(object sender, RoutedEventArgs e)
        {
            this.GL.SetGameType(GameType.PcPC);
        }

    }
}
