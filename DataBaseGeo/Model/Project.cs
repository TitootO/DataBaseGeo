using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace DataBaseGeo.Model
{
    public class Project:NotifyProperty
    {
        private int id;
        private string name;
        private string address;
        private Customer customer;
        ObservableCollection<Area> areas;
        public override string ToString()
        {
            return $"{name}, {address ?? ""}";
        }
        public int Id { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string? Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public Customer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        public ObservableCollection<Area> Areas
        {
            get { return areas; }
            set
            {
                areas = value;
                OnPropertyChanged(nameof(Areas));
            }
        }
    }
}
