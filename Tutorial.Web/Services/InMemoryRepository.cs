using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Web.Model;

namespace Tutorial.Web.Services
{
    public class InMemoryRepository : IRepository<Student>
    {
        private readonly List<Student> _students;
        public InMemoryRepository()
        {
            _students = new List<Student> {
                new Student{
                    Id=1,
                    FirstName = "Nick",
                    LastName = "Carter",
                    BirthDate = new DateTime(1990, 1, 4)
                },
                new Student{
                    Id=2,
                    FirstName = "Kevin",
                    LastName = "Richardson",
                    BirthDate = new DateTime(1960, 1, 14)
                },
                new Student{
                    Id=3,
                    FirstName = "Howie",
                    LastName = "D",
                    BirthDate = new DateTime(1969, 11, 11)
                }
            };
        }

        public IEnumerable<Student> GetAll()
        {
            return _students;
        }

        public Student GeById(int id)
        {
            return _students.FirstOrDefault(student => student.Id == id);
        }

        public Student Add(Student newModel)
        {
            var maxId = _students.Max(x => x.Id);
            newModel.Id = maxId + 1;
            _students.Add(newModel);
            return newModel;
        }
    }
}
