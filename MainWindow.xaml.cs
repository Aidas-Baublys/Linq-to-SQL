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

            InsertUni("MRU");
        }

        public void InsertUni(string name)
        {
            dataContext.ExecuteCommand("delete from University");

            University uni = new University();
            uni.Name = name;
            dataContext.Universities.InsertOnSubmit(uni);
            dataContext.SubmitChanges();

            MainData.ItemsSource = dataContext.Universities;
        }

        public void InsertStudent()
        {
            University uni = dataContext.Universities.First(u => u.Name.Equals("MRU"));
        }
    }
}
