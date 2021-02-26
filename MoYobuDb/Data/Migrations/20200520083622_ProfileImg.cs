using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace MoYobuDb.Data.Migrations
{
    public partial class ProfileImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "AspNetUsers",
                nullable: true);
                        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

    }
}
