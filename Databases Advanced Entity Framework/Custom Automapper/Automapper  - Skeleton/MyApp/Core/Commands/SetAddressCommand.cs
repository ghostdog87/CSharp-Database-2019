using AutoMapper;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Commands
{
    public class SetAddressCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public SetAddressCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandArgs)
        {
            //•	SetAddress <employeeId> <address>

            int employeeId = int.Parse(commandArgs[0]);
            string address = commandArgs[1];

            var employee = context.Employees.Find(employeeId);

            employee.Address = address;
            context.SaveChanges();

            var employeeDto = this.mapper.CreateMappedObject<EmployeeDto>(employee);

            return $"Successufully updated Address of: {employeeDto.FirstName} {employeeDto.LastName} -  {employeeDto.Address}";
        }
    }
}
