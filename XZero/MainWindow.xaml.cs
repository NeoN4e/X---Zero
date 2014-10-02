using System;
using System.Collections.Generic;
//using System.Linq;
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

namespace XZero
{
    public partial class MainWindow : Window
    {
        GemeLogic GL; //Объект типа игра
                        
        void StartNewGame()
        {
            //Откроем окно настроек
            GL =  new GemeLogic(Grid.Children);
            ChoseWindow win = new ChoseWindow(GL);
            win.ShowDialog();

            this.lbSymbol.Content = String.Format("Вы {0} играете : {1}", GL.CurentPlayer, (GL.CurentPlayer == Players.Player1) ? "X" : "O");
        }

        public MainWindow()
        {
            InitializeComponent();
            
            StartNewGame();//Начнем игру
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = (sender as Button);

                GL.PressButton(b);
            }
            catch (WinGameExeption wge)
            { 
                MessageBox.Show("Победил " + wge.Message);
                StartNewGame();
            }
            catch (GameOverExeption)
            { 
                MessageBox.Show("Игра закончена");
                StartNewGame();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            
        }

    }

   
}

