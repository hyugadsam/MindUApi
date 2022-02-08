using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Dtos.CustomValidations
{
    public class StringListValidation : ValidationAttribute
    {
        private readonly string regex;

        public StringListValidation(string regex, string FieldType)
        {
            this.regex = regex;
            this.FieldType = FieldType;
        }

        public string FieldType { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Result = string.Empty;
            if (value == null || value.GetType().ToString() != typeof(List<string>).ToString() ) //Si es null o no es un arreglo de strings no se valida
            {
                return ValidationResult.Success;
            }
            var lista = ((List<string>)value);
            Regex rx = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            for (int i = 0; i < lista.Count; i++)
            {
                if (!rx.IsMatch(lista[i]))
                {
                    Result += $"The value {lista[i]} in is not a valid {FieldType} \n";
                }
            }

            if (Result.Length > 0)
            {
                return new ValidationResult(Result);
            }

            return ValidationResult.Success;
        }
    }
}
