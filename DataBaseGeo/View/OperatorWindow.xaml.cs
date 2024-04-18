using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows;
using DataBaseGeo.Model;

namespace DataBaseGeo.View
{
    /// <summary>
    /// Логика взаимодействия для OperatorWindow.xaml
    /// </summary>
    public partial class OperatorWindow : Window
    {
        public Operator Operator { get; private set; }
        public OperatorWindow(Operator operators)
        {
            InitializeComponent();
            Operator = operators;
            DataContext = Operator;
        }
        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
