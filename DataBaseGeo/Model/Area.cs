using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataBaseGeo.Model
{
    public class Area:NotifyProperty
    {
        private int id;
        private string name;
        private Project project;
        ObservableCollection<Profile> profiles;
        ObservableCollection<AreaPoint> areaPoints;
        //public void Draw(DrawingVisualization gd, Brush br)
        //{
        //    if (areaPoints is null) return;
        //    gd.DrawPoly(areaPoints.Select(p => p.P).ToArray(), br, 0.5, true);
        //    foreach (var p in areaPoints)
        //        gd.DrawText($"{p.X},{p.Y}", p.X, p.Y, Brushes.Black, 1.5);

        //}
        //public bool IsCorrect()
        //{
        //    for (int i = 0; i < areaPoints?.Count; i++)
        //        for (int j = i + 1; j < areaPoints.Count; j++)
        //            if (AreCrossing(areaPoints[i], areaPoints[(i + 1) % areaPoints.Count], areaPoints[j], areaPoints[(j + 1) % areaPoints.Count]))
        //                return false;
        //    return true;
        //}
        //public bool AreCrossing(AreaPoint p1, AreaPoint p2, AreaPoint p3, AreaPoint p4)
        //{
        //    double mult(double ax, double ay, double bx, double by) => ax * by - bx * ay;
        //    return ((mult(p4.X - p3.X, p4.Y - p3.Y, p1.X - p3.X, p1.Y - p3.Y) * mult(p4.X - p3.X, p4.Y - p3.Y, p2.X - p3.X, p2.Y - p3.Y)) < 0 &&
        //            (mult(p2.X - p1.X, p2.Y - p1.Y, p3.X - p1.X, p3.Y - p1.Y) * mult(p2.X - p1.X, p2.Y - p1.Y, p4.X - p1.X, p4.Y - p1.Y)) < 0);
        //}
        public void Draw(DrawingVisualization gd, Brush br)
        {
            if (areaPoints is null) return;
            gd.DrawPoly(areaPoints.Select(p => p.P).ToArray(), br, 0.5, true);
            foreach (var p in areaPoints)
                gd.DrawText($"{p.X},{p.Y}", p.X, p.Y, Brushes.Black, 1.5);

        }
        public bool IsCorrect()
        {
            for (int i = 0; i < areaPoints?.Count; i++)
                for (int j = i + 1; j < areaPoints.Count; j++)
                    if (AreCrossing(areaPoints[i], areaPoints[(i + 1) % areaPoints.Count], areaPoints[j], areaPoints[(j + 1) % areaPoints.Count]))
                        return false;
            return true;
        }

        public bool AreCrossing(AreaPoint p1, AreaPoint p2, AreaPoint p3, AreaPoint p4)
        {
            double mult(double ax, double ay, double bx, double by) => ax * by - bx * ay;
            return ((mult(p4.X - p3.X, p4.Y - p3.Y, p1.X - p3.X, p1.Y - p3.Y) * mult(p4.X - p3.X, p4.Y - p3.Y, p2.X - p3.X, p2.Y - p3.Y)) < 0 &&
                    (mult(p2.X - p1.X, p2.Y - p1.Y, p3.X - p1.X, p3.Y - p1.Y) * mult(p2.X - p1.X, p2.Y - p1.Y, p4.X - p1.X, p4.Y - p1.Y)) < 0);
        }
        public override string ToString()
        {
            return name is null ? $"Площадь #{id}" : name;
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

        public Project Project
        {
            get { return project; }
            set
            {
                project = value;
                OnPropertyChanged(nameof(Project));
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
        public ObservableCollection<AreaPoint> AreaPoints
        {
            get { return areaPoints; }
            set
            {
                areaPoints = value;
                OnPropertyChanged(nameof(AreaPoints));
            }
        }
    }
}
