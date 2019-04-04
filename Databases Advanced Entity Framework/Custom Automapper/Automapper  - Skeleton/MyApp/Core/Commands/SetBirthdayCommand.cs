using AutoMapper;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MyApp.Core.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public SetBirthdayCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandArgs)
        {
            //•	SetBirthday <employeeId> <date: "dd-MM-yyyy"> 

            int employeeId = int.Parse(commandArgs[0]);
            DateTime birthday = DateTime.ParseExact(commandArgs[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var employee = context.Employees.Find(employeeId);

            employee.Birthday = birthday;
            context.SaveChanges();

            var employeeDto = this.mapper.CreateMappedObject<EmployeeDto>(employee);

            return $"Successufully updated Birthday of: {employeeDto.FirstName} {employeeDto.LastName} born on {employeeDto.Birthday}";
        }
    }
}
