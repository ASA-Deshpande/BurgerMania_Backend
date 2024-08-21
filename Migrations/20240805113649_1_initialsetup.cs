using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BurgerMania.Migrations
{
    /// <inheritdoc />
    public partial class _1_initialsetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BurgerShopSchemaFF");

            migrationBuilder.CreateTable(
                name: "burger",
                schema: "BurgerShopSchemaFF",
                columns: table => new
                {
                    burgerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_burger", x => x.burgerId);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "BurgerShopSchemaFF",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phnNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "order",
                schema: "BurgerShopSchemaFF",
                columns: table => new
                {
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    orderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    totAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.orderId);
                    table.ForeignKey(
                        name: "FK_order_user_UserID",
                        column: x => x.UserID,
                        principalSchema: "BurgerShopSchemaFF",
                        principalTable: "user",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orderItem",
                schema: "BurgerShopSchemaFF",
                columns: table => new
                {
                    orderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BurgerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderItem", x => x.orderItemId);
                    table.ForeignKey(
                        name: "FK_orderItem_burger_BurgerID",
                        column: x => x.BurgerID,
                        principalSchema: "BurgerShopSchemaFF",
                        principalTable: "burger",
                        principalColumn: "burgerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderItem_order_OrderID",
                        column: x => x.OrderID,
                        principalSchema: "BurgerShopSchemaFF",
                        principalTable: "order",
                        principalColumn: "orderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_UserID",
                schema: "BurgerShopSchemaFF",
                table: "order",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_orderItem_BurgerID",
                schema: "BurgerShopSchemaFF",
                table: "orderItem",
                column: "BurgerID");

            migrationBuilder.CreateIndex(
                name: "IX_orderItem_OrderID",
                schema: "BurgerShopSchemaFF",
                table: "orderItem",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orderItem",
                schema: "BurgerShopSchemaFF");

            migrationBuilder.DropTable(
                name: "burger",
                schema: "BurgerShopSchemaFF");

            migrationBuilder.DropTable(
                name: "order",
                schema: "BurgerShopSchemaFF");

            migrationBuilder.DropTable(
                name: "user",
                schema: "BurgerShopSchemaFF");
        }
    }
}
