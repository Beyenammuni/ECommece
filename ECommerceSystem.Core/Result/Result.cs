using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Core.Result
{
        public class Result<T>
        {
            public bool IsSuccess { get; }
            public T? Value { get; set; }
            public string Message { get; } = string.Empty;
            public List<string> Errors { get; } = new List<string>();
            public FailureType FailureType { get; }


            protected Result(bool isSuccess, T? value, string message, List<string>? errors, FailureType failureType)
            {
                IsSuccess = isSuccess;
                Value = value;
                Message = message;
                Errors = errors ?? new List<string>();
                FailureType = failureType;
            }

            public static Result<T> Success(T? value, string message = "")
            {
                return new Result<T>(true, value, message, null, FailureType.none);
            }

            public static Result<T> Failure(string message, List<string>? errors = null, FailureType failureType = FailureType.BadRequest)
            {
                return new Result<T>(false, default, message, errors, failureType);
            }


            public static Result<T> NotFound(string message)
            {
                return new Result<T>(false, default, message, new List<string> { }, FailureType.NotFound);
            }
            public static Result<T> BadRequest(string message)
            {
                return new Result<T>(false, default, message, new List<string> { }, FailureType.BadRequest);
            }
            public static Result<T> UnAuthorized(string message)
            {
                return new Result<T>(false, default, message, new List<string> { }, FailureType.Unauthorized);
            }
        }


        public class Result
        {
            public bool IsSuccess { get; }
            public string Message { get; set; } = string.Empty;
            public List<string> Errors { get; } = new List<string>();
            public FailureType FailureType { get; }

            protected Result(bool isSuccess, string message, List<string>? errors, FailureType failureType)
            {
                IsSuccess = isSuccess;
                Message = message;
                Errors = errors ?? new List<string>();
                FailureType = failureType;
            }

            public static Result Success(string message = "")
            {
                return new Result(true, message, null, FailureType.none);
            }

            public static Result Failure(string message, List<string>? errors = null, FailureType failureType = FailureType.BadRequest)
            {
                return new Result(false, message, errors, failureType);
            }

            public static Result NotFound(string message)
            {
                return new Result(false, message, new List<string> { }, FailureType.NotFound);
            }
            public static Result BadRequest(string message)
            {
                return new Result(false, message, new List<string> { }, FailureType.BadRequest);
            }
            public static Result UnAuthorized(string message)
            {
                return new Result(false, message, new List<string> { }, FailureType.Unauthorized);
            }
        }
    }
