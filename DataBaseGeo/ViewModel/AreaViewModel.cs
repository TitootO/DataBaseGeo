using DataBaseGeo.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DataBaseGeo.View;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataBaseGeo.ViewModel
{
    public class AreaViewModel : NotifyProperty
    {
        DataBase db = DataBase.getInstance();
        DrawingImage image;
        public ObservableCollection<AreaPoint> AreaPoints { get => db.AreaPoints.Local.ToObservableCollection(); }

        private Profile selectedProfile;
        private AreaPoint selectedAreaPoint;
        public Area Area { get; set; }
        public AreaViewModel() : this(null) { }
        public AreaViewModel(Area area)
        {   
            Area = area;
            AddPointCommand = new(AddPoint);
            //AddRandomPointCommand = new(AddRandomPoint);
            DeletePointCommand = new(DeletePoint);
            AddProfileCommand = new(AddProfile);
            DeleteProfileCommand = new(DeleteProfile);
            OpenProfileCommand = new(OpenProfile);
            SavePointCommand = new(SavePoint);
            ZoomCommand = new(Zoom);
            Redraw();
        }
        public RelayCommand AddPointCommand { get; set; }
        public RelayCommand AddRandomPointCommand { get; set; }
        public RelayCommand DeletePointCommand { get; set; }
        public RelayCommand AddProfileCommand { get; set; }
        public RelayCommand DeleteProfileCommand { get; set; }
        public RelayCommand OpenProfileCommand { get; set; }
        public RelayCommand SavePointCommand { get; set; }
        public RelayCommand ZoomCommand { get; set; }
        public AreaPoint SelectedAreaPoint
        {
            get => selectedAreaPoint;
            set
            {
                selectedAreaPoint = value;
                OnPropertyChanged(nameof(SelectedAreaPoint));
                Redraw();
            }
        }
        public Profile SelectedProfile
        {
            get => selectedProfile;
            set
            {
                selectedProfile = value;
                OnPropertyChanged(nameof(SelectedProfile));
                Redraw();
            }
        }

        public DrawingImage Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        void AddPoint(object obj)
        {
            AreaPoint areaPoint = new AreaPoint();
            areaPoint.Area = Area;

            db.AreaPoints.Add(areaPoint);
            db.SaveChanges();     
            OnPropertyChanged(nameof(Area));
            Redraw();
        }
        void DeletePoint(object obj)
        {
            if(selectedAreaPoint == null)
            {
                if (MessageBox.Show("Вы не выбрали точку для удаления", "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            db.AreaPoints.Remove(SelectedAreaPoint);
            db.SaveChanges();
            Redraw();
        }
        void AddProfile(object obj)
        {
            Profile profile = new Profile();
            profile.Area = Area;
            
            db.Profiles.Add(profile);
            db.SaveChanges();
            OnPropertyChanged(nameof(Area));
            Redraw();
        }
        void DeleteProfile(object obj)
        {
           if(SelectedProfile == null)
           {
               if (MessageBox.Show("Вы не выбрали профиль для удаления", "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
           }
            db.Profiles.Remove(selectedProfile);
            db.SaveChanges();
            Redraw();
        }
        void OpenProfile(object obj)
        {
            new ProfileWindow()
            {
                DataContext = new ProfileViewModel((Profile)obj)
            }.ShowDialog();
           
            OnPropertyChanged(nameof(obj));
            Redraw();
        }

        void SavePoint(object obj)
        {
            if (obj is AreaPoint)
            {
                db.Entry((AreaPoint)obj).State = EntityState.Modified;
                db.SaveChanges();
                Redraw();
            }
        }
        void Zoom(object obj)
        {
            var e = (MouseWheelEventArgs)obj;
            var image = (Image)e.Source;

            double delta = e.Delta > 0 ? 0.1 : -0.1;
            double scaleX = image.RenderTransform.Value.M11 + delta;
            double scaleY = image.RenderTransform.Value.M22 + delta;

            if (scaleX < 1 || scaleY < 1) return;

            image.RenderTransform = new ScaleTransform(scaleX, scaleY);
            var vp = e.MouseDevice.GetPosition(image);
            image.RenderTransformOrigin = new Point(vp.X / image.ActualWidth, vp.Y / image.ActualHeight);
        }
        void Redraw()
        {
            var vd = new DrawingVisualization();
            Area.Draw(vd, Area.IsCorrect() ? Brushes.Green : Brushes.Red);
            foreach (var p in Area.AreaPoints ?? new())
                vd.DrawCircle(p.X, p.Y, 0.6, SelectedAreaPoint == p ? Brushes.Yellow : Brushes.Green);
            foreach (var p in Area.Profiles ?? new())
                p.Draw(vd, p == SelectedProfile ? (p.IsCorrect() ? Brushes.Yellow : Brushes.Orange)
                                                : (p.IsCorrect() ? Brushes.Green : Brushes.Red));
            Image = vd.Render();

        }
        public string AreaName
        {
            get => Area.Name;
            set
            {
                Area.Name = value;
                db.Entry(Area).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

    }
}
