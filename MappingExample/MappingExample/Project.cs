using System.Collections.Generic;

namespace MappingExample
{
    public class Project
    {
        public virtual int ProjectId { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        public Project() { }
    }
}
