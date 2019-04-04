using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Contracts
{
    public interface ICommandReader
    {
        string Read(string[] inputArgs);
    }
}
