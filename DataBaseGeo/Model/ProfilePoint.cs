using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataBaseGeo.Model
{
    public class ProfilePoint : NotifyProperty
    {
        private int id;
        private double x;
        private double y;
        Profile profile;
        public override string ToString()
        {
            return $"{X},{Y}";
        }
        public int Id { get; set; }

        public double X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
        public Profile Profile
        {
            get { return profile; }
            set
            {
                profile = value;
                OnPropertyChanged(nameof(Profile));
            }

        }
        [NotMapped]
        public Point P => new Point(x, y);
    }
}
