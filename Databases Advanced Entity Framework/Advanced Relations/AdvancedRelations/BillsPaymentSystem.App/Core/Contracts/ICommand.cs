using BillsPaymentSystem.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.App.Core.Contracts
{
    public interface ICommand
    {
        string Execute(string[] Args);
    }
}
