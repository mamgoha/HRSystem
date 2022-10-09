using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDepartmentService : IDisposable
    {
        Task<IReadOnlyList<Department>> GetAll();
        Task<Department> GetById(int id);
        Task<Department> Add(Department department);
        Task<Department> Update(Department department);
        Task<bool> Remove(Department department);
    }
}
