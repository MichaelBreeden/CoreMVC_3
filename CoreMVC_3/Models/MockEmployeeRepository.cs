using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC_3.Models
{
    public class MockEmployeeRepository : IEmployeeRepository // Alt-Enter to make implimentation templates
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){ Id = 1, Name = "Fred", Department = "HR", Email="Fred@chaos.com"},
                new Employee(){ Id = 2, Name = "Bert", Department = "IT", Email="Bert@chaos.com"},
                new Employee(){ Id = 3, Name = "Polly", Department = "IT", Email="Polly@chaos.com"}

            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

    }
}
