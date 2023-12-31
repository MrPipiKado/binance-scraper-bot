﻿// <auto-generated />
using System;
using BinanceBotNuGetPackage.DB.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BinanceBotNuGetPackage.Migrations
{
    [DbContext(typeof(BinanceBotDbContext))]
    [Migration("20230611194602_ChangedContext2")]
    partial class ChangedContext2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("BinanceBotNuGetPackage.Db.Entities.DateInterval", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DateIntervals");
                });

            modelBuilder.Entity("BinanceBotNuGetPackage.Db.Entities.Deal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("ChangePercentage")
                        .HasColumnType("REAL");

                    b.Property<double>("CurrentPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("DealsVolume")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("FirstInTradingPair")
                        .HasColumnType("INTEGER");

                    b.Property<double>("HighestPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("LowestPrice")
                        .HasColumnType("REAL");

                    b.Property<int>("Period")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SecondInTradingPair")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("FirstInTradingPair");

                    b.HasIndex("Period");

                    b.HasIndex("SecondInTradingPair");

                    b.ToTable("Deals");
                });

            modelBuilder.Entity("BinanceBotNuGetPackage.Db.Entities.PrimaryCryptoInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CryptoName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortCryptoName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("PrimaryCryptoInfo");
                });

            modelBuilder.Entity("BinanceBotNuGetPackage.Db.Entities.Deal", b =>
                {
                    b.HasOne("BinanceBotNuGetPackage.Db.Entities.PrimaryCryptoInfo", "FirstCrypto")
                        .WithMany()
                        .HasForeignKey("FirstInTradingPair")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BinanceBotNuGetPackage.Db.Entities.DateInterval", "DateInterval")
                        .WithMany()
                        .HasForeignKey("Period")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BinanceBotNuGetPackage.Db.Entities.PrimaryCryptoInfo", "SecondCrypto")
                        .WithMany()
                        .HasForeignKey("SecondInTradingPair")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DateInterval");

                    b.Navigation("FirstCrypto");

                    b.Navigation("SecondCrypto");
                });
#pragma warning restore 612, 618
        }
    }
}
