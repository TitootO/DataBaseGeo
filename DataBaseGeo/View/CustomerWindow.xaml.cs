
using System.Windows;
using DataBaseGeo.Model;

namespace DataBaseGeo.View
{
    public partial class CustomerWindow : Window
    {
        public Customer Customer { get; private set; }
        public CustomerWindow( Customer customer)
        {
            InitializeComponent();
            Customer = customer;
            DataContext = Customer;
        }
        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
