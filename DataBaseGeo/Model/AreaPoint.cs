using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace DataBaseGeo.Model
{
    public class AreaPoint:NotifyProperty
    {
        private int id;
        private double x;
        private double y;
        private Area area;

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
        public Area Area
        {
            get { return area; }
            set
            {
                area = value;
                OnPropertyChanged(nameof(Area));
            }
        }
        [NotMapped]
        public Point P => new Point(x, y);
    }
}
