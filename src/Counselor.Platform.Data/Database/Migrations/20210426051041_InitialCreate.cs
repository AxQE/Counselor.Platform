using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace Counselor.Platform.Data.Database.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "error_codes",
				columns: table => new
				{
					id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					description = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_error_codes", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "transports",
				columns: table => new
				{
					id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					name = table.Column<string>(type: "text", nullable: true),
					created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					modified_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_transports", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "users",
				columns: table => new
				{
					id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					username = table.Column<string>(type: "text", nullable: true),
					last_activity = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
					created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					modified_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_users", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "dialogs",
				columns: table => new
				{
					id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					user_id = table.Column<int>(type: "integer", nullable: true),
					state = table.Column<int>(type: "integer", nullable: false),
					name = table.Column<string>(type: "text", nullable: true),
					created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					modified_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_dialogs", x => x.id);
					table.ForeignKey(
						name: "fk_dialogs_users_user_id",
						column: x => x.user_id,
						principalTable: "users",
						principalColumn: "id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "user_transports",
				columns: table => new
				{
					id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					transport_id = table.Column<int>(type: "integer", nullable: true),
					user_id = table.Column<int>(type: "integer", nullable: true),
					transport_user_id = table.Column<string>(type: "text", nullable: true),
					created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					modified_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_user_transports", x => x.id);
					table.ForeignKey(
						name: "fk_user_transports_transports_transport_id",
						column: x => x.transport_id,
						principalTable: "transports",
						principalColumn: "id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "fk_user_transports_users_user_id",
						column: x => x.user_id,
						principalTable: "users",
						principalColumn: "id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "messages",
				columns: table => new
				{
					id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					payload = table.Column<string>(type: "text", nullable: true),
					direction = table.Column<int>(type: "integer", nullable: false),
					dialog_id = table.Column<int>(type: "integer", nullable: true),
					created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					modified_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_messages", x => x.id);
					table.ForeignKey(
						name: "fk_messages_dialogs_dialog_id",
						column: x => x.dialog_id,
						principalTable: "dialogs",
						principalColumn: "id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.InsertData(
				table: "error_codes",
				columns: new[] { "id", "description" },
				values: new object[] { 1, "" });

			migrationBuilder.CreateIndex(
				name: "ix_dialogs_user_id",
				table: "dialogs",
				column: "user_id");

			migrationBuilder.CreateIndex(
				name: "ix_messages_dialog_id",
				table: "messages",
				column: "dialog_id");

			migrationBuilder.CreateIndex(
				name: "ix_user_transports_transport_id",
				table: "user_transports",
				column: "transport_id");

			migrationBuilder.CreateIndex(
				name: "ix_user_transports_user_id",
				table: "user_transports",
				column: "user_id");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "error_codes");

			migrationBuilder.DropTable(
				name: "messages");

			migrationBuilder.DropTable(
				name: "user_transports");

			migrationBuilder.DropTable(
				name: "dialogs");

			migrationBuilder.DropTable(
				name: "transports");

			migrationBuilder.DropTable(
				name: "users");
		}
	}
}
