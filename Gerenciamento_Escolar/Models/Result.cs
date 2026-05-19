namespace Gerenciamento_Escolar.Models;

public class Result<T> 
{
    


    private Result(string error)
    {
        sucess = false;
        this.error = error;
    }

    private Result(T value)
    {
        sucess = true;
        this.value = value;
    }

    public bool sucess { get; }
    public string error {get; }
    public T value {get ;}
    
    public static Result<T> Success(T value)
    {
        return new(value);
    }

    public static Result<T> Failure(string error)
    {
        return new(error);
    }
}