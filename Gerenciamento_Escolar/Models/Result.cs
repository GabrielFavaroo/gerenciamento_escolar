namespace Gerenciamento_Escolar.Models;

public class Result<T> 
{
    


    private Result(string error, int statusCode)
    {
        success = false;
        this.error = error;
        this.statusCode = statusCode;
    }

    private Result(T value,int statusCode)
    {
        success = true;
        this.value = value;
        this.statusCode = statusCode;
    }

    private Result(int satusCode)
    {
        success = true;
        this.statusCode = satusCode;
    }

    public bool success { get; }
    public string error {get; }
    public T value {get ;}
    public int statusCode { get; }
    
    public static Result<T> Success(T value, int statusCode)
    {
        return new(value,statusCode);
    }

    public static Result<T> Failure(string error, int statusCode)
    {
        return new(error, statusCode);
    }

    public static Result<T> NoContent(int statusCode)
    {
        return new Result<T>(statusCode);
    }
}