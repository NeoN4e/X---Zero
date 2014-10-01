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
        GemeLogic GL;

        GameSymbol symbol;
        public MainWindow()
        {
            ChooseSymbolWindow win = new ChooseSymbolWindow();
            //win.ShowDialog();
            //this.symbol = win.Symbol;
            this.symbol = GameSymbol.X;
            InitializeComponent();

            GL =  new GemeLogic(Grid.Children);
        }

        //private void Window_Loaded_1(object sender, RoutedEventArgs e)
        //{
        //    lbSymbol.Content += this.symbol;
        //}


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = (sender as Button);
                b.Content = this.symbol++;

                GL.PressButton(b);

                //Ход 2-го игрока
                b = GL.GetPcButton();
                b.Content = this.symbol--;
                GL.PressButton(b);
            }
            catch (WinGameExeption wge)
            {
                MessageBox.Show("Подедил " + wge.Message);
            }
            catch (GameOverExeption woe)
            { MessageBox.Show("Игра закончена"); }
        }


    }

    struct WinLine
    {
        public int X1 ;
        public int X2 ;
        public int X3 ;

        public WinLine(int x1, int x2, int x3)
        {
            X1 = x1;
            X2 = x2;
            X3 = x3;
        }

        /// <summary>Определяет содержит ли текущая линия заданное значение</summary>
        public bool Contains(int item)
        {
            if (this.X1.Equals(item) || this.X2.Equals(item) || this.X3.Equals(item)) return true;
            return false;
        }

        public override string ToString()
        {
            return ""+X1+","+X2+","+X3;
        }
    }

    enum Players { p1, p2 }

    enum GameSymbol { X, O}

    class PlayerPositionQty
    {
        int p1;
        int p2;

        public PlayerPositionQty(int p1, int p2)
        {
            this.p1=p1;
            this.p2=p2;
        }

        public void incr(Players p)
        {
            if(p == Players.p1) this.p1++;
            else this.p2++;

            if (p1 >= 3 || p2 >= 3) throw new WinGameExeption(p.ToString());
        }
    }

    class WinGameExeption : ApplicationException
    {
        public WinGameExeption(string msg) : base(msg) { }
    }

    class GameOverExeption : ApplicationException
    {}

    class GemeLogic
    {
        Players CurentPlayer;
        Dictionary<WinLine, PlayerPositionQty> Etalonline = new Dictionary<WinLine, PlayerPositionQty>();

        List<Button> ButtonPool = new List<Button>();

        public GemeLogic(UIElementCollection Buttons)
        {
            //PlayerPositionQty qty = new PlayerPositionQty(0,0);

            Etalonline.Add(new WinLine(1, 2, 3), new PlayerPositionQty(0, 0));
            Etalonline.Add(new WinLine(4, 5, 6), new PlayerPositionQty(0, 0));
            Etalonline.Add(new WinLine(7, 8, 9), new PlayerPositionQty(0, 0));

            Etalonline.Add(new WinLine(1, 4, 7), new PlayerPositionQty(0, 0));
            Etalonline.Add(new WinLine(2, 5, 8), new PlayerPositionQty(0, 0));
            Etalonline.Add(new WinLine(3, 6, 9), new PlayerPositionQty(0, 0));

            Etalonline.Add(new WinLine(1, 5, 9), new PlayerPositionQty(0, 0));
            Etalonline.Add(new WinLine(3, 5, 9), new PlayerPositionQty(0, 0));

            foreach (var item in Buttons)
            {
                if (item is Button) ButtonPool.Add((item as Button)); 
            }

            CurentPlayer = Players.p1;
        }

        public void PressButton(Button B)
        {
            int tag = Int32.Parse(B.Tag.ToString());
            
            if (ButtonPool.Remove(B))
            {
                //Запишем ходы
                foreach (var item in Etalonline)
                {
                    if (item.Key.Contains(tag)) item.Value.incr(this.CurentPlayer);
                }

                //сменим игрока
                if (this.CurentPlayer == Players.p1) this.CurentPlayer = Players.p2;
                else this.CurentPlayer = Players.p1;
            }

            //Завершим игру если все кнопки нажаты
            if (ButtonPool.Count == 0) throw new GameOverExeption();
        }

        public Button GetPcButton()
        {
            return this.ButtonPool[this.ButtonPool.Count - 1];
        }
    }
}

