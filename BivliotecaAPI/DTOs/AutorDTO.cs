﻿using BivliotecaAPI.Entidades;

namespace BivliotecaAPI.DTOs
{
    public class AutorDTO
    {
        public int Id { get; set; }
        public required string NombreCompleto { get; set; }
        public string? Foto { get; set; }

    }
}
