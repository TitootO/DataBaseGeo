using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseGeo.Model
{
    public class Operator:NotifyProperty
    {
        private int id;
        private string name;
        private string surname;
        private string phone;
        ObservableCollection<Profile> profiles;
        public override string ToString()
        {
            return $"{phone} {name} {surname}";
        }
        public int Id { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged(nameof(Surname));
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
        public ObservableCollection<Profile> Profiles
        {
            get { return profiles; }
            set
            {
                profiles = value;
                OnPropertyChanged(nameof(Profiles));
            }
        }
    }
}
