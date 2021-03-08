using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Library
{
    public class ProcessResult
    {
        public bool Succeed { get; private set; }
        public List<string> Errors { get; private set; }

        public ProcessResult(bool isSuccess, List<string> errors = null)
        {
            this.Succeed = isSuccess;
            this.Errors = errors ?? new List<string>();
        }

        public static ProcessResult Ok()
        {
            return new ProcessResult(true);
        }

        public static ProcessResult Fail()
        {
            return new ProcessResult(false);
        }

        public static ProcessResult Fail(string error)
        {
            return new ProcessResult(false, new List<string> { error });
        }

        public static ProcessResult Fail(IEnumerable<string> errors)
        {
            return new ProcessResult(false, errors.ToList());
        }
    }


    public class ProcessResult<T>
    {
        public bool Succeed { get; private set; }
        public List<string> Errors { get; private set; }
        public T Value { get; private set; }

        public ProcessResult(bool isSuccess, List<string> errors = null, T data = default(T)){
            this.Succeed = isSuccess;
            this.Errors = errors;
            this.Value = data;
        }

        public static ProcessResult<T> Ok(T result)
        {
            return new ProcessResult<T>(true, null, result);
        }

        public static ProcessResult<T> Fail()
        {
            return new ProcessResult<T>(false);
        }

        public static ProcessResult<T> Fail(string error)
        {
            return new ProcessResult<T>(false, new List<string> { error });
        }

        public static ProcessResult<T> Fail(IEnumerable<String> errors)
        {
            return new ProcessResult<T>(false, errors.ToList());
        }
    }
}
