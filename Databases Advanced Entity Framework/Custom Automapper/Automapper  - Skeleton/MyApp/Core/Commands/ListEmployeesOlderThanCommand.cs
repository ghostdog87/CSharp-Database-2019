using AutoMapper;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public ListEmployeesOlderThanCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandArgs)
        {
            // ListEmployeesOlderThan <age>

            int age = int.Parse(commandArgs[0]);
            var employees = context
                .Employees
                .Where(x => DateTime.Now.Year - x.Birthday.Value.Year > age)
                .OrderByDescending(x => x.Salary)
                .ToList();

            if (employees == null)
            {
                throw new ArgumentException("There are no employees with that age!");
            }

            //var employeesDto = this.mapper.CreateMappedObject<List<EmployeeDto>>(employees);

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                string managerName = emp.Manager != null ? emp.Manager.FirstName : "[no manager]";

                sb.AppendLine($"{emp.FirstName} {emp.LastName} - ${emp.Salary:F2} - Manager: {managerName}");
            }

            return sb.ToString().Trim();
        }
    }
}
