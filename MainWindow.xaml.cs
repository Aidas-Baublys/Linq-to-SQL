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

            //InsertStudent();
            //InsertLectures();
            //InsertStudentLecture();
            //GetUni("Use");
            //GetLectures("Tomas");
            //GetAllStudentsFromUni("Yale");
            GetUniByGenderPref("female");
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

        public void InsertLectures()
        {
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "History" });
            dataContext.SubmitChanges();

            MainData.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLecture()
        {
            Student tomas = dataContext.Students.First(s => s.Name.Equals("Tomas"));
            Student virga = dataContext.Students.First(s => s.Name.Equals("Virga"));
            Student jonas = dataContext.Students.First(s => s.Name.Equals("Jonas"));
            Student use = dataContext.Students.First(s => s.Name.Equals("Use"));

            Lecture math = dataContext.Lectures.First(l => l.Name.Equals("Math"));
            Lecture history = dataContext.Lectures.First(l => l.Name.Equals("History"));

            List<StudentLecture> studentLectures = new List<StudentLecture>();
            studentLectures.Add(new StudentLecture { Student = tomas, Lecture = math });
            studentLectures.Add(new StudentLecture { Student = tomas, Lecture = history });
            studentLectures.Add(new StudentLecture { Student = virga, Lecture = math });
            studentLectures.Add(new StudentLecture { Student = jonas, Lecture = math });
            studentLectures.Add(new StudentLecture { Student = use, Lecture = history });

            dataContext.StudentLectures.InsertAllOnSubmit(studentLectures);
            dataContext.SubmitChanges();

            MainData.ItemsSource = dataContext.StudentLectures;
        }

        public void GetUni(string studentName)
        {
            Student student = dataContext.Students.First(s => s.Name.Equals(studentName));
            University uni = student.University;
            List<University> u = new List<University>() { uni };
            MainData.ItemsSource = u;
        }

        public void GetLectures(string studentName)
        {
            Student student = dataContext.Students.First(s => s.Name.Equals(studentName));

            var lectures = from sl in student.StudentLectures select sl.Lecture;

            MainData.ItemsSource = lectures;
        }

        public void GetAllStudentsFromUni(string university)
        {
            var students = from uniStud in dataContext.Students 
                           where uniStud.University.Name == university 
                           select uniStud;

            MainData.ItemsSource = students;
        }

        public void GetUniByGenderPref(string gender)
        {
            var transUni = from student in dataContext.Students
                           join university in dataContext.Universities
                           on student.University equals university
                           where student.Gender == gender
                           select university;

            MainData.ItemsSource = transUni;
        }
    }
}
