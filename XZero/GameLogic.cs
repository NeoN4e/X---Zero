using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;



namespace XZero
{


    /// <summary>Игроки</summary>
    public enum Players { Player1, Player2 }

    /// <summary>Игровые Символы</summary>
    public enum GameSymbol { X, O }

    /// <summary>Тип Игры</summary>
    public enum GameType { PlayerPlayer, PlayerPC, PcPC}

    ///<summary>Хранит Комбинации линий для победы</summary>
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
    }

    ///<summary>Количиство позиций занятых в той или иной линии</summary>
    class PlayerPositionQty
    {
        int p1;
        int p2;

        public PlayerPositionQty(int p1, int p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        /// <summary>Фиксирует факт нажатия кнопки в линии, 3- надатия - WinGameExeption</summary>
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

        /// <summary>Количество нажатых кнопок в линии</summary>
        public int count()
        { return p1 + p2; }
    }

    /// <summary>В случае победы</summary>
    class WinGameExeption : ApplicationException
    {
        public WinGameExeption(string msg) : base(msg) { }
    }
    class GameOverExeption : ApplicationException { }
    
    /// <summary>Ошибка логики</summary>
    class GameLogicExeption : ApplicationException { }

    //Делегат
    delegate void MyDelgButton_Click(Button b);
    
    /// <summary>Реализует всю логику игры</summary>
    public class GemeLogic
    {
        /// <summary>Делегат Определяющий Логику в Зависимости от типа игры</summary>
        MyDelgButton_Click OnButtonClick;

        /// <summary>Кто сейчас Ходит</summary>
        public Players CurentPlayer {get; private set;}

        ///<summary>Сколько и кем кнопок нажато в конкретной линии</summary>
        Dictionary<WinLine, PlayerPositionQty> Etalonline = new Dictionary<WinLine, PlayerPositionQty>();

        ///<summary>Кнопки которые можно нажимать</summary>
        List<Button> ButtonPool = new List<Button>();

        public GemeLogic(UIElementCollection Buttons)
        {
            
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

            CurentPlayer = Players.Player1;//1-м ходит 1-й игрок
        }

        public void PressButton(Button B)
        {
            if(this.OnButtonClick != null) OnButtonClick(B);
        }

        /// <summary> Установить Тип Игры</summary>
        public void SetGameType(GameType GT)
        {
            switch (GT)
            {
                case GameType.PcPC:
                    OnButtonClick = (Button B) =>
                                    {
                                        //Отработаем нажатие пользователя
                                        OnPressButton(B);

                                        //Ход 2-го игрока
                                        while (true)
                                        {
                                            B = GetPcButton();
                                            OnPressButton(B);
                                        }
                                    };
                    break;
                case GameType.PlayerPC:
                    OnButtonClick = (Button B) =>
                                    {
                                        //Отработаем нажатие пользователя
                                        OnPressButton(B);

                                        //Ход 2-го игрока
                                        B = GetPcButton();
                                        OnPressButton(B);
                                    };
                    break;
                case GameType.PlayerPlayer:
                    OnButtonClick = (Button B) => { OnPressButton(B); };
                    break;
            }
        }

        /// <summary>Обработчик события нажатие ЛОгической кнопки</summary>
        public void OnPressButton(Button B)
        {
           
            int tag = Int32.Parse(B.Tag.ToString()); // Получим ИНт номер кнопки

            if (ButtonPool.Remove(B)) //Если кнопку еще не нажимали вернет ТРУЕ
            {

                try
                {
                    //Запишем ходы
                    foreach (var item in Etalonline)
                    {
                        if (item.Key.Contains(tag)) item.Value.incr(this.CurentPlayer); // Если в лини есть цифра - Увеличим счетчик тек пользователя
                    }
                }
                finally //Необходимо для того чтоб в случае победы на последней кнопке изменился контент
                {
                    
                    //сменим игрока
                    if (this.CurentPlayer == Players.Player1)
                    {
                        B.Content = GameSymbol.X;
                        this.CurentPlayer = Players.Player2;
                    }
                    else
                    {
                        B.Content = GameSymbol.O;
                        this.CurentPlayer = Players.Player1;
                    }
                }

                B.IsEnabled = false; // Сделаем недоступной нажатую кнопку
            }

            //Завершим игру если все кнопки нажаты
            if (ButtonPool.Count == 0) throw new GameOverExeption();
        }

        /// <summary>Метод ИИ Выбирает ненажатую кнопку в текущей линии</summary>
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

        /// <summary>Метод ИИ Логика Хода</summary>
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

            //Ходим на угад
            return ButtonPool[MyRandom.R.Next(ButtonPool.Count - 1)];
        }
    }

}
