using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using BillsPaymentSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.App
{
    public class DataSeeding
    {
        public void Seed(BillsPaymentSystemContext context)
        {
            SeedUsers(context);
            SeedBankAccount(context);
            SeedCreditCard(context);
            SeedPaymentMethod(context);
        }

        private void SeedUsers(BillsPaymentSystemContext context)
        {
            string[] firstNames = { "pesho", "ivan", "sasho" };
            string[] lastNames = { "peshov", "ivanv", "sashov" };
            string[] emails = { "peshov@abv.bg", "ivanv@abv.bg", "sashov@abv.bg" };
            string[] password = { "peshov", "ivanv", "sashov" };

            List<User> users = new List<User>();

            for (int i = 0; i < 3; i++)
            {
                User user = new User
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    Email = emails[i],
                    Password = password[i]
                };

                if (!IsValid(user))
                {
                    Console.WriteLine($"Invalid user: {user.FirstName}");
                    continue;
                }
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private void SeedBankAccount(BillsPaymentSystemContext context)
        {
            string[] bankNames = { "pesho", "ivan", "sasho" };
            decimal[] balances = { 55, 56, 500 };           
            string[] swiftCodes = { "peshov", "ivanv", "sashov" };

            List<BankAccount> bankAccounts = new List<BankAccount>();

            for (int i = 0; i < 3; i++)
            {
                BankAccount bankAccount = new BankAccount
                {
                    BankName = bankNames[i],
                    Balance = balances[i],
                    SWIFTCode = swiftCodes[i]
                };

                if (!IsValid(bankAccount))
                {
                    Console.WriteLine($"Invalid bank account: {bankAccount.BankName}");
                    continue;
                }
                bankAccounts.Add(bankAccount);
            }

            context.BankAccounts.AddRange(bankAccounts);
            context.SaveChanges();
        }

        private void SeedCreditCard(BillsPaymentSystemContext context)
        {
            decimal[] limits = { 55, 56, 500 };
            decimal[] moneyowed = { 55, 56, 500 };
            DateTime[] expirationDates = 
            {
                DateTime.Now,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(-5)
            };

            List<CreditCard> creditCards = new List<CreditCard>();

            for (int i = 0; i < 3; i++)
            {
                CreditCard creditCard = new CreditCard
                {
                    Limit = limits[i],
                    MoneyOwed = moneyowed[i],
                    ExpirationDate = expirationDates[i]
                };

                if (!IsValid(creditCard))
                {
                    Console.WriteLine($"Invalid credit card with limit: {creditCard.Limit}");
                    continue;
                }
                creditCards.Add(creditCard);
            }

            context.CreditCards.AddRange(creditCards);
            context.SaveChanges();
        }

        private void SeedPaymentMethod(BillsPaymentSystemContext context)
        {
            int?[] bankAccountIds = { null, 1, null };
            int?[] creditCardIds = { 1, null, 1 };
            PaymentType[] types = 
            {
                PaymentType.BankAccount,
                PaymentType.CreditCard,
                PaymentType.BankAccount
            };
            int[] UserId = { 1, 2, 3 };

            List<PaymentMethod> paymentMethods = new List<PaymentMethod>();

            for (int i = 0; i < 3; i++)
            {
                PaymentMethod paymentMethod = new PaymentMethod
                {
                    BankAccountId = bankAccountIds[i],
                    CreditCardId = creditCardIds[i],
                    Type = types[i],
                    UserId = UserId[i]
                };

                if (!IsValid(paymentMethod))
                {
                    Console.WriteLine($"Invalid payment type: {paymentMethod.Type}");
                    continue;
                }
                paymentMethods.Add(paymentMethod);
            }

            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();
        }

        private static bool IsValid(object entity)
        {
            ValidationContext validationContext = new ValidationContext(entity);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

            return isValid;
        }
    }
}