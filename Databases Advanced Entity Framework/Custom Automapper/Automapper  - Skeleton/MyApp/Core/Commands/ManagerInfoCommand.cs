using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class ManagerInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public ManagerInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandArgs)
        {
            //•	ManagerInfo <employeeId>

            int employeeId = int.Parse(commandArgs[0]);

            var manager = context
                .Employees
                .Include(x => x.ManagedEmployees)
                .FirstOrDefault(x => x.Id == employeeId);

            var managerDto = this.mapper.CreateMappedObject<ManagerDto>(manager);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.ManagedEmployees.Count}");

            foreach (var employees in managerDto.ManagedEmployees)
            {
                sb.AppendLine($"    - {employees.FirstName} {employees.LastName} - ${employees.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
