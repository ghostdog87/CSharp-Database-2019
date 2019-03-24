using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.Data.EntityConfigurations
{
    public class CreditCardConfig : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder
                .HasKey(x => x.CreditCardId);

            builder
                .Property(x => x.Limit)
                .IsRequired();

            builder
                .Property(x => x.MoneyOwed)
                .IsRequired();

            builder
                .HasOne(x => x.PaymentMethod)
                .WithOne(x => x.CreditCard);
        }
    }
}
