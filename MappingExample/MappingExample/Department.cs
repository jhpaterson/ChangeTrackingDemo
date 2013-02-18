using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MappingExample
{
    public class Department
    {
        public virtual int DepartmentId{ get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        public Department() { }
    }
}
