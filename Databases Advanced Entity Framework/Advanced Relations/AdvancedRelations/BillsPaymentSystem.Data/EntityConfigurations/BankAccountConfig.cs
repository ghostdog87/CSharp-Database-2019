using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.Data.EntityConfigurations
{
    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder
                .HasKey(x => x.BankAccountId);

            builder
                .HasOne(x => x.PaymentMethod)
                .WithOne(x => x.BankAccount);

            builder
                .Property(x => x.BankName)
                .HasMaxLength(50)
                .IsUnicode();

            builder
                .Property(x => x.SWIFTCode)
                .HasMaxLength(20)
                .IsUnicode(false);
        }
    }
}
