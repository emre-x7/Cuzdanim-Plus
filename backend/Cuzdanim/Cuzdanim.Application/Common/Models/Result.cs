namespace Cuzdanim.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public string[] Errors { get; }

    protected Result(bool isSuccess, string message, string[] errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors ?? Array.Empty<string>();
    }

    public static Result Success(string message = "İşlem başarılı")
        => new(true, message);

    public static Result Failure(string message, params string[] errors)
        => new(false, message, errors);
}

public class Result<T> : Result
{
    public T Data { get; }

    private Result(bool isSuccess, string message, T data, string[] errors = null)
        : base(isSuccess, message, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data, string message = "İşlem başarılı")
        => new(true, message, data);

    public static new Result<T> Failure(string message, params string[] errors)
        => new(false, message, default, errors);
}