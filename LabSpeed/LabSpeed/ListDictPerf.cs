using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSpeed
{
    class ListDictPerf
    {

        public static void Run()
        {
            var stopwatch = new Stopwatch();
            List<Grade> grades = Grade.GetData().ToList();
            List<Student> students = Student.GetStudents().ToList();

            stopwatch.Start();
            foreach (Student student in students)
            {
                student.Grade = grades.Single(x => x.StudentId == student.Id).Value;
            }
            stopwatch.Stop();
            Console.WriteLine("Using list {0}", stopwatch.Elapsed);
            stopwatch.Reset();
            students = Student.GetStudents().ToList();
            stopwatch.Start();
            Dictionary<Guid, string> dic = Grade.GetData().ToDictionary(x => x.StudentId, x => x.Value);
            foreach (Student student in students)
            {
                student.Grade = dic[student.Id];
            }
            stopwatch.Stop();
            Console.WriteLine("Using dictionary {0}", stopwatch.Elapsed);
            Console.ReadKey();
        }


        public class GuidHelper
        {
            public static List<Guid> ListOfIds = new List<Guid>();

            static GuidHelper()
            {
                for (int i = 0; i < 10000; i++)
                {
                    ListOfIds.Add(Guid.NewGuid());
                }
            }
        }


        public class Grade
        {
            public Guid StudentId { get; set; }
            public string Value { get; set; }

            public static IEnumerable<Grade> GetData()
            {
                for (int i = 0; i < 10000; i++)
                {
                    yield return new Grade
                    {
                        StudentId = GuidHelper.ListOfIds[i],
                        Value = "Value " + i
                    };
                }
            }
        }

        public class Student
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Grade { get; set; }

            public static IEnumerable<Student> GetStudents()
            {
                for (int i = 0; i < 10000; i++)
                {
                    yield return new Student
                    {
                        Id = GuidHelper.ListOfIds[i],
                        Name = "Name " + i
                    };
                }
            }
        }
    }
}
