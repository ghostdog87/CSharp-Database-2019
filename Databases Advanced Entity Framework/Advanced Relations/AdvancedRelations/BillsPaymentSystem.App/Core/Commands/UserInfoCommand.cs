using BillsPaymentSystem.App.Core.Contracts;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class UserInfoCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public UserInfoCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] Args)
        {
            int userId = int.Parse(Args[0]);

            User user = context.Users.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                throw new ArgumentNullException($"User with id {userId} not found!");
            }

            var bankAccounts = context
                .PaymentMethods
                .Where(x => x.UserId == userId && x.BankAccountId == 1)
                .Select(x => new
                {
                    id = x.BankAccountId,
                    balance = x.BankAccount.Balance,
                    bank = x.BankAccount.BankName,
                    swift = x.BankAccount.SWIFTCode
                })
                .ToList();

            var creditCards = context
                .PaymentMethods
                .Where(x => x.UserId == userId && x.CreditCardId == 1)
                .Select(x => new
                {
                    id = x.CreditCardId,
                    limit = x.CreditCard.Limit,
                    owed = x.CreditCard.MoneyOwed,
                    limitLeft = x.CreditCard.LimitLeft,
                    expirationDate = x.CreditCard.ExpirationDate
                })
                .ToList();


            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User: {user.FirstName} {user.LastName}");

            if (bankAccounts.Count > 0)
            {
                sb.AppendLine("Bank Accounts:");
            }

            foreach (var bankAcc in bankAccounts)
            {               
                sb.AppendLine($"-- ID: {bankAcc.id}");
                sb.AppendLine($"--- Balance: {bankAcc.balance:F2}");
                sb.AppendLine($"--- Bank: {bankAcc.bank}");
                sb.AppendLine($"--- SWIFT: {bankAcc.swift}");
            }

            if (creditCards.Count > 0)
            {
                sb.AppendLine("Credit Cards:");
            }

            foreach (var creditCard in creditCards)
            {
                sb.AppendLine($"-- ID: {creditCard.id}");
                sb.AppendLine($"--- Limit: {creditCard.limit:F2}");
                sb.AppendLine($"--- Money Owed: {creditCard.owed:F2}");
                sb.AppendLine($"--- Limit Left: {creditCard.limitLeft:F2}");
                sb.AppendLine($"--- Expiration Date: {creditCard.expirationDate}");
            }

            return sb.ToString().TrimEnd();
        }

    }
}
