using DataBaseGeo.Model;
using System.Windows;

namespace DataBaseGeo.View
{
    public partial class ProjectWindow : Window
    {
        public Project Project { get; private set; }
        public ProjectWindow( Project project)
        {
            
            InitializeComponent();
            Project = project;
            DataContext = Project;
        }
        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
