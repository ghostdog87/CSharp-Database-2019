using AutoMapper;
using MyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class SetManagerCommand : ICommand
    {
        private readonly MyAppContext context;

        public SetManagerCommand(MyAppContext context)
        {
            this.context = context;
        }

        public string Execute(string[] commandArgs)
        {
            //•	SetManager <employeeId> <managerId> 

            int employeeId = int.Parse(commandArgs[0]);
            int managerId = int.Parse(commandArgs[1]);

            var employee = context.Employees.Find(employeeId);
            var manager = context.Employees.Find(managerId);

            employee.Manager = manager;
            this.context.SaveChanges();

            return $"Successufully updated manager.";
        }
    }
}
