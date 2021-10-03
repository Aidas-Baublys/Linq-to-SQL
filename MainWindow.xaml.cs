using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace Link_to_SQL
{
    public partial class MainWindow : Window
    {
        LinqToSQLDataContext dataContext;

        public MainWindow()
        {
            InitializeComponent();

            string cs = ConfigurationManager.ConnectionStrings["Link_to_SQL.Properties.Settings.EchoDBConnectionString"].ConnectionString;
            dataContext = new LinqToSQLDataContext(cs);
            //dataContext.ExecuteCommand("delete from University");

            //InsertUni("MRU");
            //InsertUni("Yale");

            InsertStudent();
        }

        public void InsertUni(string name)
        {
            University uni = new University();
            uni.Name = name;

            dataContext.Universities.InsertOnSubmit(uni);
            dataContext.SubmitChanges();

            MainData.ItemsSource = dataContext.Universities;
        }

        public void InsertStudent()
        {
            University mru = dataContext.Universities.First(u => u.Name.Equals("MRU"));
            University yale = dataContext.Universities.First(u => u.Name.Equals("Yale"));

            List<Student> students = new List<Student>();
            students.Add(new Student { Name = "Tomas", Gender = "male", Id = 1, UniID = yale.Id });
            students.Add(new Student { Name = "Virga", Gender = "female", Id = 2, University = mru });
            students.Add(new Student { Name = "Jonas", Gender = "fury", Id = 3, University = mru });
            students.Add(new Student { Name = "Use", Gender = "female", Id = 4, University = yale });

            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            MainData.ItemsSource = dataContext.Students;
        }
    }
}
