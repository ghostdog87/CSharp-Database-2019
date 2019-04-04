using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Commands
{
    public class ExitCommand : ICommand
    {
        public string Execute(string[] commandArgs)
        {
            Environment.Exit(1);
            return "";
        }
    }
}
