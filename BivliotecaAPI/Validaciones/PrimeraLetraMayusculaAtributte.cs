﻿using System.ComponentModel.DataAnnotations;

namespace BivliotecaAPI.Validaciones
{
    public class PrimeraLetraMayusculaAtributte : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var valueString = value.ToString()!;
            var primeraLetra = valueString[0].ToString();
            if (primeraLetra != primeraLetra.ToUpper()) 
            {

                return new ValidationResult("La primera letra debe ser mayusculas");
            }

            return ValidationResult.Success;
        }
    }
}
