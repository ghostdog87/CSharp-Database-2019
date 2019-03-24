using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.Data.EntityConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.UserId);

            builder
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            builder
                .Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            builder
                .Property(x => x.Email)
                .HasMaxLength(80)
                .IsRequired()
                .IsUnicode(false);

            builder
                .Property(x => x.Password)
                .HasMaxLength(25)
                .IsRequired()
                .IsUnicode(false);
        }
    }
}

