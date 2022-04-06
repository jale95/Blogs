using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsEngine.DAL.Interfaces
{
    public interface IRepository<T>
    {
        public T Add(T newobject);
        public IEnumerable<T> GetAll();
        public T GetById(int id);
        public T Update(T updateObject);
        public T Delete(int id);
    }
}
