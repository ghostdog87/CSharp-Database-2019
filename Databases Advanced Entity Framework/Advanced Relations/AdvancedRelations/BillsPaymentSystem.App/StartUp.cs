using BillsPaymentSystem.App.Core;
using BillsPaymentSystem.Data;
using System;

namespace BillsPaymentSystem.App
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            //using (var context = new BillsPaymentSystemContext())
            //{
            //    DataSeeding seedData = new DataSeeding();
            //    seedData.Seed(context);
            //}
            CommandInterpreter commandInterpreter = new CommandInterpreter();
            Engine engine = new Engine(commandInterpreter);
            engine.Run();
        }
    }
}
