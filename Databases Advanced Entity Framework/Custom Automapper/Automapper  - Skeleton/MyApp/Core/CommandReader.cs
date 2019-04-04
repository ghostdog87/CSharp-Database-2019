using MyApp.Core.Commands;
using MyApp.Core.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace MyApp.Core
{
    public class CommandReader : ICommandReader
    {
        private readonly IServiceProvider provider;
        private readonly string Suffix = "Command";

        public CommandReader(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public string Read(string[] inputArgs)
        {
            string commandName = inputArgs[0] + Suffix;
            string[] commandArgs = inputArgs.Skip(1).ToArray();

            Type commandType = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(x => x.Name == commandName);

            if (commandType == null)
            {
                throw new ArgumentException("Invalid command!");
            }

            var typeConstructor = commandType
                .GetConstructors()
                .FirstOrDefault();

            var constructorParams = typeConstructor
                .GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();

            var services = constructorParams
                .Select(this.provider.GetService)
                .ToArray();

            var typeInstance = (ICommand)Activator.CreateInstance(commandType, services);

            string result = typeInstance.Execute(commandArgs);

            return result;
        }
    }
}
