using DataBaseGeo.Model;
using DataBaseGeo.View;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DataBaseGeo.ViewModel
{
    class ApplicationViewModel : NotifyProperty
    {
        DataBase db = DataBase.getInstance();
        DrawingImage image;
        public ObservableCollection<Customer> Customers { get => db.Customers.Local.ToObservableCollection(); }
        public ObservableCollection<Project> Projects { get => db.Projects.Local.ToObservableCollection(); }
        public ObservableCollection<Area> Areas { get => db.Areas.Local.ToObservableCollection(); }
        private Customer selectedCustomer;
        private Project selectedProject;
        private Area selectedArea;
        public ApplicationViewModel()
        {
            AddCustomerCommand = new(AddCustomer);
            DeleteCustomerCommand = new(DeleteCustomer);
            AddProjectCommand = new(AddProject);
            DeleteProjectCommand = new(DeleteProject);
            AddAreaCommand = new(AddArea);
            DeleteAreaCommand = new(DeleteArea);
            OpenAreaCommand = new(OpenArea);
            ZoomCommand = new(Zoom);
        }
        public  RelayCommand AddCustomerCommand { get; set; }
        public RelayCommand DeleteCustomerCommand { get; set; }
        public RelayCommand AddProjectCommand { get; set; }
        public RelayCommand DeleteProjectCommand { get; set; }
        public RelayCommand AddAreaCommand { get; set; }
        public RelayCommand DeleteAreaCommand { get; set; }
        public RelayCommand OpenAreaCommand { get; set; }
        public RelayCommand ZoomCommand { get; set; }
        public Customer SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        public Project SelectedProject
        {
            get => selectedProject;
            set
            {
                selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
                Redraw();
            }
        }
        public Area SelectedArea
        {
            get => selectedArea;
            set
            {
                selectedArea = value;
                OnPropertyChanged(nameof(SelectedArea));
                Redraw();
            }
        }
        void AddCustomer(object obj)
        {
            CustomerWindow customerWindow = new CustomerWindow(new Customer());
            if (customerWindow.ShowDialog() == true)
            {
                Customer customer = customerWindow.Customer;
                if(customer.Name == null)
                {
                    if (MessageBox.Show("Вы заполнили не все поля. Чтобы добавить пользователя нужно ввести данные в каждое свободное поле", 
                        "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return ;
                }
                else if (customer.Surname == null)
                {
                    if (MessageBox.Show("Вы заполнили не все поля. Чтобы добавить пользователя нужно ввести данные в каждое свободное поле",
                        "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
                }
                else if (customer.Phone == null)
                {
                    if (MessageBox.Show("Вы заполнили не все поля. Чтобы добавить пользователя нужно ввести данные в каждое свободное поле",
                        "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
                }
                db.Customers.Add(customer);
                db.SaveChanges();
            }
        }
        void DeleteCustomer(object obj)
        {
            if(SelectedCustomer == null)
            {
                if (MessageBox.Show("Вы не выбрали заказчика","Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            db.Customers.Remove(SelectedCustomer);
            db.SaveChanges();
        }
        void AddProject(object obj)
        {
            if (SelectedCustomer == null)
            {
                if (MessageBox.Show("Вы не выбрали заказчика", "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            ProjectWindow projectWindow = new ProjectWindow(new Project());
            if (projectWindow.ShowDialog() == true)
            {
                
                Project project = projectWindow.Project;
                if (project.Name == null)
                {
                    if (MessageBox.Show("Вы заполнили не все поля. Чтобы добавить проект нужно ввести данные в каждое свободное поле",
                        "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
                }
                else if (project.Address == null)
                {
                    if (MessageBox.Show("Вы заполнили не все поля. Чтобы сохранить пользователя нужно ввести данные в каждое свободное поле",
                        "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
                }
                project.Customer = SelectedCustomer;
                db.Projects.Add(project);
                db.SaveChanges();
            }
        }
        void DeleteProject(object obj)
        {
            if (SelectedProject == null)
            {
                if (MessageBox.Show("Вы не выбрали проект для удаления", "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            db.Projects.Remove(selectedProject);
            db.SaveChanges();
            
        }
        void AddArea(object obj)
        {
            Area area = new Area();
            if (SelectedProject == null)
            {
                if (MessageBox.Show("Вы не выбрали проект. Для создания площади вернитесь в главное окно и выберите проект", "Ошибка!", 
                    MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            area.Project = selectedProject;
            area.Name = $"Площадь {area.Id}";
            db.Areas.Add(area);
            db.SaveChanges();
            OnPropertyChanged(nameof(SelectedProject));
        }
        void DeleteArea(object obj)
        {
            if (SelectedArea == null)
            {
                if (MessageBox.Show("Вы не выбрали площаль для удаления", "Ошибка!", MessageBoxButton.OK) == MessageBoxResult.OK) return;
            }
            db.Areas.Remove(selectedArea);
            db.SaveChanges();
            OnPropertyChanged(nameof(SelectedProject));
        }
        void OpenArea(object obj)
        {
            new AreaWindow()
            {
                DataContext = new AreaViewModel((Area)obj)
            }.ShowDialog();
            OnPropertyChanged(nameof(SelectedProject.Areas));
            OnPropertyChanged(nameof(obj));
            Redraw();
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
            var vis = new DrawingVisualization();
            foreach (var area in SelectedProject?.Areas ?? new())
            {
                area.Draw(vis, area == SelectedArea ? Brushes.Yellow : (area.IsCorrect() ? Brushes.Green : Brushes.Red));
                foreach (var profile in area.Profiles ?? new())
                    profile.Draw(vis, area == SelectedArea ? Brushes.Yellow : (profile.IsCorrect() ? Brushes.Green : Brushes.Red));
            }
            Image = vis.Render();
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
    }   
}
