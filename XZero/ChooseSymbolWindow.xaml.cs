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
    /// Логика взаимодействия для ChooseSymbolWindow.xaml
    /// </summary>
    public partial class ChooseSymbolWindow : Window
    {
        public string Symbol { get; set; }

        public ChooseSymbolWindow()
        {
            InitializeComponent();
        }

        private void rbX_Checked(object sender, RoutedEventArgs e)
        {
            this.Symbol = (sender as RadioButton).Content.ToString();
            this.Close();
        }
    }
}
