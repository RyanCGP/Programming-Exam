using System;
using System.Collections.Generic;
using Sprout.Exam.DataAccess.Models;
using System.Linq;
using System.Threading.Tasks;
using Sprout.Exam.Business.DataTransferObjects;

namespace Sprout.Exam.WebApp.Services.Interfaces
{
    interface IEmployeeService
    {
        public List<Employee> GetEmployees();
        public Employee GetEmployeeById(int id);
        public Employee EditEmployee(EditEmployeeDto editEmployeeDto);
        public Employee CreateEmployee(CreateEmployeeDto createEmployeeDto);
        public Employee DeleteEmployee(int id);
        public decimal CalculateSalary(EmployeeDto employeeDto);
    }
}
