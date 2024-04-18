using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseGeo.Model
{
    public class Customer:NotifyProperty
    {
        private int id;
        private string name;
        private string surname;
        private string phone;
        ObservableCollection<Project> projects;
        public override string ToString()
        {
            return $"{name} {surname} {phone}";
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
        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }
        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set
            {
                projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }
    }
}
