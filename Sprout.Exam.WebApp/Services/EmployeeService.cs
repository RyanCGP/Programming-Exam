using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess;
using Sprout.Exam.DataAccess.Models;
using Sprout.Exam.WebApp.Services.Interfaces;
using Sprout.Exam.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly SproutExamDbContext _context;
        public EmployeeService()
        {
            _context = new SproutExamDbContext();
        }

        /// <summary>
        /// Returns a list of all employees that are not yet deleted.
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetEmployees()
        {
            return _context.Employees.Where(x => x.IsDeleted == false).ToList();
        }

        /// <summary>
        /// Returns an employee by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee GetEmployeeById(int id)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);

            return employee;
        }

        /// <summary>
        /// Updates the employee data on the database.
        /// </summary>
        /// <param name="editEmployeeDto"></param>
        /// <returns></returns>
        public Employee EditEmployee(EditEmployeeDto editEmployeeDto)
        {
            var employeeToUpdate = _context.Employees.FirstOrDefault(x => x.Id == editEmployeeDto.Id);
            if(employeeToUpdate != null)
            {
                employeeToUpdate.FullName = editEmployeeDto.FullName;
                employeeToUpdate.Tin = editEmployeeDto.Tin;
                employeeToUpdate.Birthdate = editEmployeeDto.Birthdate;
                employeeToUpdate.EmployeeTypeId = editEmployeeDto.TypeId;

                _context.SaveChanges();

                return employeeToUpdate;
            }
            return null;
        }

        /// <summary>
        /// Inserts a new employee to the database.
        /// </summary>
        /// <param name="createEmployeeDto"></param>
        /// <returns></returns>
        public Employee CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var newEmployee = new Employee()
            {
                FullName = createEmployeeDto.FullName,
                Birthdate = createEmployeeDto.Birthdate,
                Tin = createEmployeeDto.Tin,
                EmployeeTypeId = createEmployeeDto.TypeId,
                IsDeleted = false
            };

            _context.Employees.Add(newEmployee);
            _context.SaveChanges();

            return newEmployee;
        }

        /// <summary>
        /// Soft deletes an employee by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee DeleteEmployee(int id)
        {
            var employeeToDelete = _context.Employees.FirstOrDefault(x => x.Id == id);
            
            employeeToDelete.IsDeleted = true;
            //_context.Remove(employeeToDelete); -- Added during the interview
            _context.SaveChanges();

            return employeeToDelete;
        }

        /// <summary>
        /// Calculates the final salary of an employee.
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public decimal CalculateSalary(EmployeeDto employeeDto)
        {
            decimal totalSalary = 0M;

            const int totalWorkDays = 23;
            const decimal tax = 0.12M;
            const decimal baseSalary = 20000.00M;

            const decimal perDayRate = 500.00M;

            var employee = _context.Employees.FirstOrDefault(x => x.Id == employeeDto.Id);

            switch ((Common.Enums.EmployeeType) employee.EmployeeTypeId)
            {
                case Common.Enums.EmployeeType.Regular:
                    var totalWorkedDays = totalWorkDays - employeeDto.AbsentDays;
                    totalSalary = baseSalary - (baseSalary / totalWorkedDays) - (baseSalary * tax);
                    break;
                case Common.Enums.EmployeeType.Contractual:
                    totalSalary = perDayRate * employeeDto.WorkedDays;
                    break;
                default:
                    break;
            }

            return Math.Round(totalSalary, 2, MidpointRounding.AwayFromZero);
        }
    }
}
