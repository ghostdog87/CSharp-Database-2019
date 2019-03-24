using BillsPaymentSystem.App.Core.Commands;
using BillsPaymentSystem.App.Core.Contracts;
using BillsPaymentSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BillsPaymentSystem.App.Core
{
    public class CommandInterpreter
    {
        private readonly string Suffix = "Command";

        public string Read(string[] Args, BillsPaymentSystemContext context)
        {
            string command = Args[0];
            string[] argParams = Args.Skip(1).ToArray();

            Type commandType = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(x => x.Name == command + Suffix);

            if (commandType == null)
            {
                throw new ArgumentNullException("Invalid command!");
            }

            var typeInstance = Activator.CreateInstance(commandType, context);

            string result = ((ICommand)typeInstance).Execute(argParams);

            return result;
        }
    }
}
