using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Models;

public  class DataPresente: ValidationAttribute
{
    public override bool IsValid(object? value)
    {

        if (value == null)
        {
            return true;
        }

        DateOnly data = (DateOnly)value;

        if (data < DateOnly.FromDateTime(DateTime.Now))
        {
            ErrorMessage = ("A data da alocação não pode ser no passado");
            return false;
        }

        return true;

    }
}