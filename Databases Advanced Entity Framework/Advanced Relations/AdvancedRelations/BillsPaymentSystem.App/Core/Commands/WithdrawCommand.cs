using BillsPaymentSystem.App.Core.Contracts;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class WithdrawCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public WithdrawCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] Args)
        {
            int userId = int.Parse(Args[0]);
            decimal amount = decimal.Parse(Args[1]);

            User user = context.Users.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                throw new ArgumentNullException($"User with id {userId} not found!");
            }

            List<BankAccount> bankAccounts = context
                .PaymentMethods
                .Where(x => x.UserId == userId && x.BankAccountId != null)
                .Select(x => new BankAccount
                {
                    Balance = x.BankAccount.Balance
                })
                .ToList();

            foreach (var bankAccount in bankAccounts)
            {
                if (bankAccount.Balance >= amount)
                {
                    bankAccount.Balance -= amount;
                    context.SaveChanges();
                    return $"User with id {userId} withdrawed {amount} from bank account and left {bankAccount.Balance}!";
                }
            }

            List<CreditCard> creditCards = context
                .PaymentMethods
                .Where(x => x.UserId == userId && x.CreditCardId != null)
                .Select(x => new CreditCard
                {
                    Limit = x.CreditCard.Limit,
                })
                .ToList();

            foreach (var creditCard in creditCards)
            {
                if (creditCard.Limit >= amount)
                {
                    creditCard.Limit -= amount;
                    context.SaveChanges();
                    return $"User with id {userId} withdrawed {amount} from credit card and left {creditCard.Limit}!";
                }
            }

            return "Insufficient funds!";
        }
    }
}
