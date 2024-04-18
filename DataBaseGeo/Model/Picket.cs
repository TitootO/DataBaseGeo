using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataBaseGeo.Model
{
    public class Picket:NotifyProperty
    {
        private int id;
        private double x;
        private double y, ra, th, k, u;
        private double value;
        Profile profile;
        public Point Projection(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double t = ((x - p1.X) * dx + (y - p1.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));
            return new Point(p1.X + t * dx, p1.Y + t * dy);
        }

        public double DistanceToLine(Point p1, Point p2)
        {
            var p = Projection(p1, p2);
            return Math.Sqrt(Math.Pow(p.X - x, 2) + Math.Pow(p.Y - y, 2));
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
        public double Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        public double Ra
        {
            get { return ra; }
            set
            {
                this.ra = value;
                OnPropertyChanged(nameof(Ra));
            }
        }
        public double Th
        {
            get { return th; }
            set
            {
                this.th = value;
                OnPropertyChanged(nameof(Th));
            }
        }
        public double K
        {
            get { return k; }
            set
            {
                this.k = value;
                OnPropertyChanged(nameof(K));
            }
        }
        public double U
        {
            get { return u; }
            set
            {
                this.u = value;
                OnPropertyChanged(nameof(U));
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
    }
}
