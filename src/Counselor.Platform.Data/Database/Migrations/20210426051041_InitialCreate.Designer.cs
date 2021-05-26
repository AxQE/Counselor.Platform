﻿// <auto-generated />
using System;
using Counselor.Platform.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Counselor.Platform.Data.Database.Migrations
{
    [DbContext(typeof(PlatformDbContext))]
    [Migration("20210426051041_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Counselor.Platform.Data.Entities.Dialog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_dialogs");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_dialogs_user_id");

                    b.ToTable("dialogs");
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.ErrorCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_error_codes");

                    b.ToTable("error_codes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = ""
                        });
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on");

                    b.Property<int?>("DialogId")
                        .HasColumnType("integer")
                        .HasColumnName("dialog_id");

                    b.Property<int>("Direction")
                        .HasColumnType("integer")
                        .HasColumnName("direction");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on");

                    b.Property<string>("Payload")
                        .HasColumnType("text")
                        .HasColumnName("payload");

                    b.HasKey("Id")
                        .HasName("pk_messages");

                    b.HasIndex("DialogId")
                        .HasDatabaseName("ix_messages_dialog_id");

                    b.ToTable("messages");
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.Transport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_transports");

                    b.ToTable("transports");
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on");

                    b.Property<DateTime?>("LastActivity")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_activity");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.UserTransport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on");

                    b.Property<int?>("TransportId")
                        .HasColumnType("integer")
                        .HasColumnName("transport_id");

                    b.Property<string>("TransportUserId")
                        .HasColumnType("text")
                        .HasColumnName("transport_user_id");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_transports");

                    b.HasIndex("TransportId")
                        .HasDatabaseName("ix_user_transports_transport_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_transports_user_id");

                    b.ToTable("user_transports");
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.Dialog", b =>
                {
                    b.HasOne("Counselor.Platform.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_dialogs_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.Message", b =>
                {
                    b.HasOne("Counselor.Platform.Data.Entities.Dialog", null)
                        .WithMany("Messages")
                        .HasForeignKey("DialogId")
                        .HasConstraintName("fk_messages_dialogs_dialog_id");
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.UserTransport", b =>
                {
                    b.HasOne("Counselor.Platform.Data.Entities.Transport", "Transport")
                        .WithMany()
                        .HasForeignKey("TransportId")
                        .HasConstraintName("fk_user_transports_transports_transport_id");

                    b.HasOne("Counselor.Platform.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_transports_users_user_id");

                    b.Navigation("Transport");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Counselor.Platform.Data.Entities.Dialog", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}