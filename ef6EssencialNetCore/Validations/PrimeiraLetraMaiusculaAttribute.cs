using System.ComponentModel.DataAnnotations;
namespace ef6EssencialNetCore.Validations;

    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid (
            object value, ValidationContext validationContext
        )

        {
            if (value == null || string.IsNullOrEmpty(value.ToString())){
                return ValidationResult.Success;
            }

            var primeiraLetraMaiuscula = value.ToString()[0].ToString();
            if (primeiraLetraMaiuscula != primeiraLetraMaiuscula.ToUpper()) 
            {
                return new ValidationResult("A Primeira Letra Deve Ser Maiuscula");
            }

            return ValidationResult.Success;
        }
    }
