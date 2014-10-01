using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;



namespace XZero
{
    struct WinLine
    {
        public int X1;
        public int X2;
        public int X3;

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
            return "" + X1 + "," + X2 + "," + X3;
        }
    }

    public enum Players { Player1, Player2 }

    public enum GameSymbol { X, O }

    class PlayerPositionQty
    {
        int p1;
        int p2;

        public PlayerPositionQty(int p1, int p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public void incr(Players p)
        {
            if (p == Players.Player1) this.p1++;
            else this.p2++;

            if (p1 >= 3 || p2 >= 3) throw new WinGameExeption(p.ToString());
        }

        public int this[Players p]
        {
            get
            {
                if (p == Players.Player1) return p1;
                else return p2;
            }
        }

        public int count()
        { return p1 + p2; }
    }

    class WinGameExeption : ApplicationException
    {
        public WinGameExeption(string msg) : base(msg) { }
    }

    class GameOverExeption : ApplicationException { }
    class GameLogicExeption : ApplicationException { }

    public class GemeLogic
    {
        public delegate void OnMoveDelegat(Button b);
        public event OnMoveDelegat onMove;
        public void Move(Button b)
        {
            if (onMove != null) onMove(b);
        }
        
        public Players CurentPlayer {get; private set;}

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
            Etalonline.Add(new WinLine(3, 5, 7), new PlayerPositionQty(0, 0));

            foreach (var item in Buttons)
            {
                if (item is Button)
                {
                    Button B = (item as Button);
                    B.Content = "";     //очистим от предыдущей игры
                    B.IsEnabled = true;
                    ButtonPool.Add(B); 
                }
            }

            CurentPlayer = Players.Player1;
        }

        public void PressButton(Button B)
        {
            int tag = Int32.Parse(B.Tag.ToString());

            if (ButtonPool.Remove(B))
            {

                try
                {
                    //Запишем ходы
                    foreach (var item in Etalonline)
                    {
                        if (item.Key.Contains(tag)) item.Value.incr(this.CurentPlayer);
                    }
                }
                finally //Необходимо для того чтоб в случае победы на последней кнопке изменился контент
                {
                    //сменим игрока
                    if (this.CurentPlayer == Players.Player1)
                    {
                        B.Content = "X";
                        this.CurentPlayer = Players.Player2;
                    }
                    else
                    {
                        B.Content = "O";
                        this.CurentPlayer = Players.Player1;
                    }
                }

                B.IsEnabled = false;
            }

            //Завершим игру если все кнопки нажаты
            if (ButtonPool.Count == 0) throw new GameOverExeption();
        }

        Button GetButtonFromWinLine(WinLine wl)
        {
            foreach (Button b in ButtonPool)
            {
                int tag = Int32.Parse(b.Tag.ToString());
                if (tag.Equals(wl.X1) || tag.Equals(wl.X2) || tag.Equals(wl.X3))
                    return b;
            }

            throw new GameLogicExeption();
        }

        public Button GetPcButton()
        {

            //проверим Можно ли выиграть?
            foreach (var item in Etalonline)
            {
                if (item.Value[this.CurentPlayer] == 2 && item.Value.count() == 2)
                    return GetButtonFromWinLine(item.Key);
            }

            //Проверим может противник может выиграть
            foreach (var item in Etalonline)
            {
                if (item.Value[this.CurentPlayer] == 0 && item.Value.count() == 2)
                    return GetButtonFromWinLine(item.Key);
            }

            return ButtonPool[MyRandom.R.Next(ButtonPool.Count - 1)];
        }
    }

}
