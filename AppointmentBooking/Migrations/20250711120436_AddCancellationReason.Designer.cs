﻿// <auto-generated />
using System;
using AppointmentBooking.src.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AppointmentBooking.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250711120436_AddCancellationReason")]
    partial class AddCancellationReason
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppointmentBooking.src.Domain.Entities.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("AppointmentStartUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CancellationReason")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomerPhone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time without time zone");

                    b.Property<bool>("IsRecurring")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsReminderSent")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ParentAppointmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<string>("RecurrenceRule")
                        .HasColumnType("text");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TimeZoneId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("appointement", (string)null);
                });

            modelBuilder.Entity("AppointmentBooking.src.Domain.Entities.BlockedTime", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("BlockedTimes");
                });

            modelBuilder.Entity("AppointmentBooking.src.Domain.Entities.ServiceProvider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Specialty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ServiceProviders");
                });

            modelBuilder.Entity("AppointmentBooking.src.Domain.Entities.WorkingHours", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<TimeOnly?>("BreakEnd")
                        .HasColumnType("time without time zone");

                    b.Property<TimeOnly?>("BreakStart")
                        .HasColumnType("time without time zone");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("WorkingHours");
                });

            modelBuilder.Entity("AppointmentBooking.src.Domain.Entities.Appointment", b =>
                {
                    b.HasOne("AppointmentBooking.src.Domain.Entities.ServiceProvider", "Provider")
                        .WithMany("Appointments")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("AppointmentBooking.src.Domain.Entities.BlockedTime", b =>
                {
                    b.HasOne("AppointmentBooking.src.Domain.Entities.ServiceProvider", "Provider")
                        .WithMany("BlockedTimes")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("AppointmentBooking.src.Domain.Entities.WorkingHours", b =>
                {
                    b.HasOne("AppointmentBooking.src.Domain.Entities.ServiceProvider", "Provider")
                        .WithMany("WorkingHours")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("AppointmentBooking.src.Domain.Entities.ServiceProvider", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("BlockedTimes");

                    b.Navigation("WorkingHours");
                });
#pragma warning restore 612, 618
        }
    }
}
