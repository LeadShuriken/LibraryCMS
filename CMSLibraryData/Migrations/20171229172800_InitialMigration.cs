using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CMSLibraryData.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CMSLibraryBranch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    Telephone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMSLibraryBranch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMSLibraryCard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Fees = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMSLibraryCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<int>(nullable: true),
                    CloseTime = table.Column<int>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false),
                    OpenTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchHours_CMSLibraryBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "CMSLibraryBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 30, nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    HomeLibraryBranchId = table.Column<int>(nullable: true),
                    LastName = table.Column<string>(maxLength: 30, nullable: false),
                    LibraryCardId = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscribers_CMSLibraryBranch_HomeLibraryBranchId",
                        column: x => x.HomeLibraryBranchId,
                        principalTable: "CMSLibraryBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscribers_CMSLibraryCard_LibraryCardId",
                        column: x => x.LibraryCardId,
                        principalTable: "CMSLibraryCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CMSLibraryAsset",
                columns: table => new
                {
                    Author = table.Column<string>(nullable: true),
                    ISBN = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<decimal>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    Index = table.Column<string>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    NumberOfCopies = table.Column<int>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Agency = table.Column<string>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    Director = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMSLibraryAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CMSLibraryAsset_CMSLibraryBranch_LocationId",
                        column: x => x.LocationId,
                        principalTable: "CMSLibraryBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CMSLibraryAsset_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Checkout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LibraryAssetId = table.Column<int>(nullable: false),
                    LibraryCardId = table.Column<int>(nullable: true),
                    Since = table.Column<DateTime>(nullable: false),
                    Until = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checkout_CMSLibraryAsset_LibraryAssetId",
                        column: x => x.LibraryAssetId,
                        principalTable: "CMSLibraryAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Checkout_CMSLibraryCard_LibraryCardId",
                        column: x => x.LibraryCardId,
                        principalTable: "CMSLibraryCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckoutHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheckedIn = table.Column<DateTime>(nullable: true),
                    CheckedOut = table.Column<DateTime>(nullable: false),
                    LibraryAssetId = table.Column<int>(nullable: false),
                    LibraryCardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckoutHistory_CMSLibraryAsset_LibraryAssetId",
                        column: x => x.LibraryAssetId,
                        principalTable: "CMSLibraryAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckoutHistory_CMSLibraryCard_LibraryCardId",
                        column: x => x.LibraryCardId,
                        principalTable: "CMSLibraryCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hold",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HoldPlaced = table.Column<DateTime>(nullable: false),
                    LibraryAssetId = table.Column<int>(nullable: true),
                    LibraryCardId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hold", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hold_CMSLibraryAsset_LibraryAssetId",
                        column: x => x.LibraryAssetId,
                        principalTable: "CMSLibraryAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hold_CMSLibraryCard_LibraryCardId",
                        column: x => x.LibraryCardId,
                        principalTable: "CMSLibraryCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchHours_BranchId",
                table: "BranchHours",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkout_LibraryAssetId",
                table: "Checkout",
                column: "LibraryAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkout_LibraryCardId",
                table: "Checkout",
                column: "LibraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutHistory_LibraryAssetId",
                table: "CheckoutHistory",
                column: "LibraryAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutHistory_LibraryCardId",
                table: "CheckoutHistory",
                column: "LibraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_CMSLibraryAsset_LocationId",
                table: "CMSLibraryAsset",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CMSLibraryAsset_StatusId",
                table: "CMSLibraryAsset",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Hold_LibraryAssetId",
                table: "Hold",
                column: "LibraryAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Hold_LibraryCardId",
                table: "Hold",
                column: "LibraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_HomeLibraryBranchId",
                table: "Subscribers",
                column: "HomeLibraryBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_LibraryCardId",
                table: "Subscribers",
                column: "LibraryCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchHours");

            migrationBuilder.DropTable(
                name: "Checkout");

            migrationBuilder.DropTable(
                name: "CheckoutHistory");

            migrationBuilder.DropTable(
                name: "Hold");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "CMSLibraryAsset");

            migrationBuilder.DropTable(
                name: "CMSLibraryCard");

            migrationBuilder.DropTable(
                name: "CMSLibraryBranch");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
