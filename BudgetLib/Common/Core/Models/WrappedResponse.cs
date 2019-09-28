using System;

namespace Common.Core.Models
{
        public class WrappedResponse
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
    public class WrappedResponse<T>
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
        public T Data { get; set; }

        public static explicit operator WrappedResponse<T>(WrappedResponse v)
        {
            throw new NotImplementedException();
        }
    }
}