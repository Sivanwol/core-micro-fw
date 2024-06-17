using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_vendors_providers_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_providers_media_logo_id",
                table: "providers");

            migrationBuilder.DropIndex(
                name: "ix_providers_logo_id",
                table: "providers");

            migrationBuilder.AlterColumn<int>(
                name: "logo_id",
                table: "vendors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "vendors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "client_id",
                table: "providers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "media",
                table: "providers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "providers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "provider_category",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_vendors_user_id_name",
                table: "vendors",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_providers_client_id",
                table: "providers",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_providers_media",
                table: "providers",
                column: "media");

            migrationBuilder.CreateIndex(
                name: "ix_providers_user_id_name",
                table: "providers",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_provider_category_user_id_name",
                table: "provider_category",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_provider_category_users_user_id",
                table: "provider_category",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_providers_clients_client_id",
                table: "providers",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_providers_media_media",
                table: "providers",
                column: "media",
                principalTable: "media",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_providers_users_user_id",
                table: "providers",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_vendors_users_user_id",
                table: "vendors",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_provider_category_users_user_id",
                table: "provider_category");

            migrationBuilder.DropForeignKey(
                name: "fk_providers_clients_client_id",
                table: "providers");

            migrationBuilder.DropForeignKey(
                name: "fk_providers_media_media",
                table: "providers");

            migrationBuilder.DropForeignKey(
                name: "fk_providers_users_user_id",
                table: "providers");

            migrationBuilder.DropForeignKey(
                name: "fk_vendors_users_user_id",
                table: "vendors");

            migrationBuilder.DropIndex(
                name: "ix_vendors_user_id_name",
                table: "vendors");

            migrationBuilder.DropIndex(
                name: "ix_providers_client_id",
                table: "providers");

            migrationBuilder.DropIndex(
                name: "ix_providers_media",
                table: "providers");

            migrationBuilder.DropIndex(
                name: "ix_providers_user_id_name",
                table: "providers");

            migrationBuilder.DropIndex(
                name: "ix_provider_category_user_id_name",
                table: "provider_category");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "vendors");

            migrationBuilder.DropColumn(
                name: "client_id",
                table: "providers");

            migrationBuilder.DropColumn(
                name: "media",
                table: "providers");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "providers");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "provider_category");

            migrationBuilder.AlterColumn<int>(
                name: "logo_id",
                table: "vendors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_providers_logo_id",
                table: "providers",
                column: "logo_id");

            migrationBuilder.AddForeignKey(
                name: "fk_providers_media_logo_id",
                table: "providers",
                column: "logo_id",
                principalTable: "media",
                principalColumn: "id");
        }
    }
}
