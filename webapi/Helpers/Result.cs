using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Host
{
    public enum ErrorType
    {
        AuthSettingsValidationFailed
    }
    public class Error
    {
        public Error(ErrorType type, Exception exception)
        {
            Type = type;
            Exception = exception;
        }

        public ErrorType Type { get; private set; }
        public Exception Exception { get; private set; }
    }
    public class Result<T>
    {
        public bool IsFailed => Errors.Any();
        public bool IsSuccess => !IsFailed;

        public T Value { get; }
        public List<Error> Errors { get; }

        private Result(T value)
        {
            Errors = new List<Error>();
            Value = value;
        }

        private Result(T value, Error error)
            : this(value)
        {
            Errors.Add(error);
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Fail(T value, ErrorType type, Exception exception)
        {
            var error = new Error(type, exception);
            return new Result<T>(value, error);
        }

        public Result<T> Combine(Result<T> result)
        {
            Errors.AddRange(result.Errors);
            return this;
        }
    }
}
