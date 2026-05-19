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

        DateTime data = (DateTime)value;

        if (data < DateTime.Now)
        {
            ErrorMessage = ("A data da alocação não pode ser no passado");
            return false;
        }

        return true;

    }
}