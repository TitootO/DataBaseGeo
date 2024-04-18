using DataBaseGeo.Model;
using DataBaseGeo.View;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace DataBaseGeo.ViewModel
{
    public class ProfileViewModel:NotifyProperty
    {
        DataBase db = DataBase.getInstance();
        DrawingImage image;
        DrawingImage graphImage;
        public Profile Profile { get; set; }
        ProfilePoint selectedPoint;
        Picket selectedPicket;
        public ObservableCollection<Operator> Operators { get => db.Operators.Local.ToObservableCollection(); }
        public ObservableCollection<ProfilePoint> ProfilePoints { get => db.ProfilePoints.Local.ToObservableCollection(); }
        public ProfileViewModel() : this(null) { }
        public ProfileViewModel(Profile profile)
        {
            Profile = profile;
            AddOperatorCommand = new(AddOperator);
            DeleteOperatorCommand = new(DeleteOperator);
            AddPointCommand = new(AddPoint);
            DeletePointCommand = new(DeletePoint);
            SavePointCommand = new(SavePoint);
            AddPicketCommand = new(AddPicket);
            DeletePicketCommand = new(DeletePicket);
            SavePicketCommand = new(SavePicket);
            ZoomCommand = new(Zoom);
            Redraw();
        }
        public RelayCommand AddOperatorCommand { get; set; }
        public RelayCommand DeleteOperatorCommand { get; set; }
        public RelayCommand AddPointCommand { get; set; }
        public RelayCommand DeletePointCommand { get; set; }
        public RelayCommand SavePointCommand { get; set; }
        public RelayCommand AddPicketCommand { get; set; }
        public RelayCommand DeletePicketCommand { get; set; }
        public RelayCommand SavePicketCommand { get; set; }
        public RelayCommand ZoomCommand { get; set; }

        public Operator? SelectedOperator
        {
            get => Profile?.Operator;
            set
            {
                Profile.Operator = value;
                db.Entry(Profile).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        void AddOperator(object obj)
        {
            OperatorWindow operatorWindow = new OperatorWindow(new Operator());
            if (operatorWindow.ShowDialog() == true)
            {
                Operator operators = operatorWindow.Operator;
                if (operators.Name == null)
                {
                    if (MessageBox.Show("Вы заполнили не все поля. Чтобы добавить пользователя нужно ввести данные в каждое свободное поле",
                        "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
                }
                else if (operators.Surname == null)
                {
                    if (MessageBox.Show("Вы заполнили не все поля. Чтобы добавить пользователя нужно ввести данные в каждое свободное поле",
                        "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
                }
                else if (operators.Phone == null)
                {
                    if (MessageBox.Show("Вы заполнили не все поля. Чтобы добавить пользователя нужно ввести данные в каждое свободное поле",
                        "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
                }
                db.Operators.Add(operators);
                db.SaveChanges();
                OnPropertyChanged(nameof(Operators));
            }
        }
        void DeleteOperator(object obj)
        {
            if(SelectedOperator == null)
            {
                if (MessageBox.Show("Вы не выбрали оператора для удаления", "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            db.Operators.Remove(SelectedOperator);
            db.SaveChanges();

        }
        void AddPoint(object obj)
        {
            ProfilePoint profilePoint = new ProfilePoint();
            profilePoint.Profile = Profile;
            db.ProfilePoints.Add(profilePoint);
            db.SaveChanges();
            OnPropertyChanged(nameof(Profile));
            Redraw();
        }
        void DeletePoint(object obj)
        {
            if(SelectedPoint == null)
            {
                if (MessageBox.Show("Вы не выбрали точку для удаления", "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            db.ProfilePoints.Remove(SelectedPoint);
            db.SaveChanges();
            Redraw();
        }
        void SavePoint(object obj)
        {
            if (obj is ProfilePoint)
            {
                db.Entry((ProfilePoint)obj).State = EntityState.Modified;
                db.SaveChanges();
                Redraw();
            }
        }
        void AddPicket(object obj)
        {
            Picket picket = new Picket();
            picket.Profile = Profile;
            db.Pickets.Add(picket);
            db.SaveChanges();
            OnPropertyChanged(nameof(Profile));
            Redraw();
        }
        void DeletePicket(object obj)
        {
            if(SelectedPicket == null)
            {
                if (MessageBox.Show("Вы не выбрали точку для удаления", "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            db.Pickets.Remove(SelectedPicket);
            db.SaveChanges();
            OnPropertyChanged(nameof(Profile));
            Redraw();
        }
        void SavePicket(object obj)
        {
            if (obj is Picket)
            {
                db.Entry((Picket)obj).State = EntityState.Modified;
                db.SaveChanges();
                Redraw();
            }
        }
        public Picket SelectedPicket
        {
            get => selectedPicket;
            set
            {
                selectedPicket = value;
                OnPropertyChanged(nameof(SelectedPicket));
                Redraw();
            }
        }
        public ProfilePoint SelectedPoint
        {
            get => selectedPoint;
            set
            {
                selectedPoint = value;
                OnPropertyChanged(nameof(SelectedPoint));
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
        public DrawingImage GraphImage
        {
            get { return graphImage; }
            set
            {
                graphImage = value;
                OnPropertyChanged(nameof(GraphImage));
            }
        }
        void Redraw()
        {
            var pickets = Profile.OrderPickets();

            var vd = new DrawingVisualization();
            foreach (var p in pickets)
                vd.DrawLine(p.proj.X, p.proj.Y, p.pic.X, p.pic.Y, Brushes.Orange, 0.2);
            Profile.Draw(vd, (Profile.IsCorrect() ? Brushes.Green : Brushes.Red));
            foreach (var p in Profile.ProfilePoints ?? new())
                vd.DrawCircle(p.X, p.Y, 0.5, SelectedPoint == p ? Brushes.Yellow : Brushes.Green);
            foreach (var p in Profile.Pickets ?? new())
                vd.DrawCircle(p.X, p.Y, 0.5, p == SelectedPicket ? Brushes.Yellow : Brushes.Orange);
            Image = vd.Render();


            var graph = new DrawingVisualization();
            graph.DrawPoly(pickets.Select((v, i) => new Point(i * 10, v.pic.Ra)).ToList(), Brushes.Orange, 0.3, false);
            graph.DrawPoly(pickets.Select((v, i) => new Point(i * 10, v.pic.Th)).ToList(), Brushes.Green, 0.3, false);
            graph.DrawPoly(pickets.Select((v, i) => new Point(i * 10, v.pic.K)).ToList(), Brushes.Blue, 0.3, false);
            graph.DrawPoly(pickets.Select((v, i) => new Point(i * 10, v.pic.U)).ToList(), Brushes.Red, 0.3, false);
            if (SelectedPicket != null)
                for (int i = 0; i < pickets.Count; i++)
                    if (pickets[i].pic == SelectedPicket)
                    {
                        graph.DrawCircle(i * 10, pickets[i].pic.Ra, 0.5, Brushes.Yellow);
                        graph.DrawCircle(i * 10, pickets[i].pic.Th, 0.5, Brushes.Yellow);
                        graph.DrawCircle(i * 10, pickets[i].pic.K, 0.5, Brushes.Yellow);
                        graph.DrawCircle(i * 10, pickets[i].pic.U, 0.5, Brushes.Yellow);
                    }
            GraphImage = graph.Render(drawAxies: true);
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
    }
}
