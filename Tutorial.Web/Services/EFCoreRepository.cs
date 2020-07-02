using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Web.Data;
using Tutorial.Web.Model;

namespace Tutorial.Web.Services
{
    public class EFCoreRepository : IRepository<Student>
    {
        private readonly DataContext context;

        public EFCoreRepository(DataContext context)
        {
            this.context = context;
        }
        public Student Add(Student newModel)
        {
            context.Students.Add(newModel);
            context.SaveChanges();
            return newModel;
        }

        public Student GeById(int id)
        {
            return context.Students.Find(id);
        }

        public IEnumerable<Student> GetAll()
        {
            return context.Students.ToList();
        }
    }
}
