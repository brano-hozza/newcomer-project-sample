using System;
using System.Collections.Generic;

namespace WebApi.Helpers
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

        public ErrorType Type { get; }
        public Exception Exception { get; }
    }
    public sealed class Result<T>
    {
        public bool IsFailed => Errors.Count > 0;
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
