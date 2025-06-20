﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BivliotecaAPI.Migrations
{
    /// <inheritdoc />
    public partial class NuevasColumnasAutores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Autores",
                newName: "Nombres");

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Autores",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Identificacion",
                table: "Autores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Autores");

            migrationBuilder.DropColumn(
                name: "Identificacion",
                table: "Autores");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Autores");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Autores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
