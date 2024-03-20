﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240320095705_UpdateTransportAdvertisementModel")]
    partial class UpdateTransportAdvertisementModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Domain.ModeratorOverviewStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ModeratorOverviewStatuses");
                });

            modelBuilder.Entity("Domain.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Domain.Transport.TransportAdvertisement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<Guid?>("BodyTypeId")
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<double>("EngineCapacity")
                        .HasColumnType("double precision");

                    b.Property<double>("FuelConsumption")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsElectric")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsNew")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("MakeId")
                        .HasColumnType("uuid");

                    b.Property<string>("ManufactureCountry")
                        .HasColumnType("text");

                    b.Property<DateTime>("ManufactureDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Mileage")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ModelId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModeratorOverviewStatusId")
                        .HasColumnType("uuid");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("text");

                    b.Property<string>("SubTitle")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<Guid?>("TypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("BodyTypeId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("MakeId");

                    b.HasIndex("ModelId");

                    b.HasIndex("ModeratorOverviewStatusId");

                    b.HasIndex("TypeId");

                    b.ToTable("TransportAdvertisements");
                });

            modelBuilder.Entity("Domain.Transport.TransportAdvertisementImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TransportAdvertisementId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("TransportAdvertisementId");

                    b.ToTable("TransportAdvertisementImages");
                });

            modelBuilder.Entity("Domain.Transport.TransportBodyType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransportBodyTypes");
                });

            modelBuilder.Entity("Domain.Transport.TransportMake", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransportMakes");
                });

            modelBuilder.Entity("Domain.Transport.TransportMakeModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TransportMakeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TransportModelId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TransportMakeId");

                    b.HasIndex("TransportModelId");

                    b.ToTable("TransportMakeModels");
                });

            modelBuilder.Entity("Domain.Transport.TransportModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransportModels");
                });

            modelBuilder.Entity("Domain.Transport.TransportType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransportTypes");
                });

            modelBuilder.Entity("Domain.Transport.TransportTypeBodyType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TransportBodyTypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TransportTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TransportBodyTypeId");

                    b.HasIndex("TransportTypeId");

                    b.ToTable("TransportTypeBodyTypes");
                });

            modelBuilder.Entity("Domain.Transport.TransportTypeMake", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TransportMakeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TransportTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TransportMakeId");

                    b.HasIndex("TransportTypeId");

                    b.ToTable("TransportTypeMakes");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("bytea");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.UserPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PermissionId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("Domain.Transport.TransportAdvertisement", b =>
                {
                    b.HasOne("Domain.Transport.TransportBodyType", "BodyType")
                        .WithMany()
                        .HasForeignKey("BodyTypeId");

                    b.HasOne("Domain.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.HasOne("Domain.Transport.TransportMake", "Make")
                        .WithMany()
                        .HasForeignKey("MakeId");

                    b.HasOne("Domain.Transport.TransportModel", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId");

                    b.HasOne("Domain.ModeratorOverviewStatus", "ModeratorOverviewStatus")
                        .WithMany()
                        .HasForeignKey("ModeratorOverviewStatusId");

                    b.HasOne("Domain.Transport.TransportType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");

                    b.Navigation("BodyType");

                    b.Navigation("Creator");

                    b.Navigation("Make");

                    b.Navigation("Model");

                    b.Navigation("ModeratorOverviewStatus");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Domain.Transport.TransportAdvertisementImage", b =>
                {
                    b.HasOne("Domain.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("Domain.Transport.TransportAdvertisement", "TransportAdvertisement")
                        .WithMany("Images")
                        .HasForeignKey("TransportAdvertisementId");

                    b.Navigation("Image");

                    b.Navigation("TransportAdvertisement");
                });

            modelBuilder.Entity("Domain.Transport.TransportMakeModel", b =>
                {
                    b.HasOne("Domain.Transport.TransportMake", "TransportMake")
                        .WithMany("TransportMakeModels")
                        .HasForeignKey("TransportMakeId");

                    b.HasOne("Domain.Transport.TransportModel", "TransportModel")
                        .WithMany()
                        .HasForeignKey("TransportModelId");

                    b.Navigation("TransportMake");

                    b.Navigation("TransportModel");
                });

            modelBuilder.Entity("Domain.Transport.TransportTypeBodyType", b =>
                {
                    b.HasOne("Domain.Transport.TransportBodyType", "TransportBodyType")
                        .WithMany()
                        .HasForeignKey("TransportBodyTypeId");

                    b.HasOne("Domain.Transport.TransportType", "TransportType")
                        .WithMany("TransportTypeBodyTypes")
                        .HasForeignKey("TransportTypeId");

                    b.Navigation("TransportBodyType");

                    b.Navigation("TransportType");
                });

            modelBuilder.Entity("Domain.Transport.TransportTypeMake", b =>
                {
                    b.HasOne("Domain.Transport.TransportMake", "TransportMake")
                        .WithMany()
                        .HasForeignKey("TransportMakeId");

                    b.HasOne("Domain.Transport.TransportType", "TransportType")
                        .WithMany("TransportTypeMakes")
                        .HasForeignKey("TransportTypeId");

                    b.Navigation("TransportMake");

                    b.Navigation("TransportType");
                });

            modelBuilder.Entity("Domain.UserPermission", b =>
                {
                    b.HasOne("Domain.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId");

                    b.HasOne("Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Transport.TransportAdvertisement", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("Domain.Transport.TransportMake", b =>
                {
                    b.Navigation("TransportMakeModels");
                });

            modelBuilder.Entity("Domain.Transport.TransportType", b =>
                {
                    b.Navigation("TransportTypeBodyTypes");

                    b.Navigation("TransportTypeMakes");
                });
#pragma warning restore 612, 618
        }
    }
}
