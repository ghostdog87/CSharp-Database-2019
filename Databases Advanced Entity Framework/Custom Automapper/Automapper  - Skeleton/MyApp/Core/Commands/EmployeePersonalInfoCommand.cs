using AutoMapper;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Commands
{
    class EmployeePersonalInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public EmployeePersonalInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandArgs)
        {
            //EmployeePersonalInfo <employeeId>

            int employeeId = int.Parse(commandArgs[0]);

            var employee = context.Employees.Find(employeeId);

            var employeeDto = this.mapper.CreateMappedObject<EmployeeDto>(employee);

            return $"ID: {employeeId} - {employeeDto.FirstName} {employeeDto.LastName} -  ${employeeDto.Salary:f2}\nBirthday: {employeeDto.Birthday}\nAddress: {employeeDto.Address}";
        }
    }
}
