namespace Application.Entity;

public class Result<T>
{
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string ErrorMessage { get; }

        private Result(bool isSuccess, T? value, string errorMessage)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new(true, value, string.Empty);
        public static Result<T> Failure(string error) => new(false, default, error);
    
}