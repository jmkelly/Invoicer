namespace Invoicer.Web;

public class Result
{

	public bool IsSuccess { get; }
	public string Error { get; }

	public static Result Success()
	{
		return new Result();
	}

	private Result()
	{
		IsSuccess = true;
		Error = string.Empty;
	}

	private Result(string error)
	{
		IsSuccess = false;
		Error = error;
	}

	public static Result Failure(string error) => new(error);

}

public class Result<T>
{
	public bool IsSuccess { get; }
	public T? Value { get; }
	public string Error { get; }

	private Result(T value)
	{
		IsSuccess = true;
		Value = value;
		Error = string.Empty;
	}

	private Result(string error)
	{
		IsSuccess = false;
		Error = error;
	}

	public static Result<T> Success(T value)
	{
		return new Result<T>(value);
	}

	public static Result<T> Failure(string error) => new(error);
}


