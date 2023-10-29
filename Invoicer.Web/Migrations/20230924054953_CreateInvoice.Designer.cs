﻿// <auto-generated />
using System;
using Invoicer.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Invoicer.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230924054953_CreateInvoice")]
    partial class CreateInvoice
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Invoicer.Web.Pages.Clients.Models.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccountNo")
                        .HasColumnType("text");

                    b.Property<string>("BSB")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Postcode")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Invoicer.Web.Pages.Invoices.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InvoiceCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("InvoiceStatus")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpddatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Invoicer.Web.Pages.MyAccount.MyAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BSB")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MyAccounts");
                });

            modelBuilder.Entity("Invoicer.Web.Pages.Settings.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("AddTax")
                        .HasColumnType("boolean");

                    b.Property<decimal>("TaxRate")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Invoicer.Web.Pages.WorkItems.WorkItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Hours")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("InvoiceId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Rate")
                        .HasColumnType("numeric");

                    b.Property<int>("RateUnits")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("WorkItems");
                });

            modelBuilder.Entity("Invoicer.Web.Pages.Invoices.Invoice", b =>
                {
                    b.HasOne("Invoicer.Web.Pages.Clients.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Invoicer.Web.Pages.WorkItems.WorkItem", b =>
                {
                    b.HasOne("Invoicer.Web.Pages.Clients.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invoicer.Web.Pages.Invoices.Invoice", "Invoice")
                        .WithMany("WorkItems")
                        .HasForeignKey("InvoiceId");

                    b.Navigation("Client");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("Invoicer.Web.Pages.Invoices.Invoice", b =>
                {
                    b.Navigation("WorkItems");
                });
#pragma warning restore 612, 618
        }
    }
}
