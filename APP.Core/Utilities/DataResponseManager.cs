using System.Collections.Generic;

namespace APP.Core.Utilities
{
    public class DataResponse<T>
    {
        public T Data { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }

    public class DataResponseExt<T>
    {
        public List<T> Data { get; set; }
        public int Count { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }

    public struct ResponseCode
    {
        public const string SUCCESS = "00";
        public const string FAILED = "01";
        public const string NOT_FOUND = "02";
        public const string EMPTY = "03";
        public const string ACCESS_DENIED = "04";
        public const string CONFLICT = "05";
        public const string INVALID_DATA = "06";


        public const string INVALID_PASSWORD = "03";
        public const string LOGIN_FAILED = "03";
        public const string USER_DOES_NOT_EXIST = "03";
        public const string USER_ALREADY_EXISTS = "03";


        public const string SYSTEM_ERROR = "99";
    }

    public struct ResponseDescription
    {
        public const string SUCCESS = "SUCCESS";
        public const string FAILED = "FAILED";
        public const string NOT_FOUND = "NOT FOUND";
        public const string ACCESS_DENIED = "ACCESS DENIED!!! YOU DO NOT HAVE THE PRIVILEGE FOR THIS ACTIVITY";
        public const string USER_ALREADY_EXISTS = "USER ALREADY EXISTS";
        public const string EMPTY = "EMPTY";
        public const string INVALID_PASSWORD = "PASSWORD IS NOT CORRECT";
        public const string CHANGE_PASSWORD = "PLEASE CHANGE YOUR PASSWORD";
        public const string USER_DOES_NOT_EXIST = "USER DOES NOT EXIST";
        public const string CONFLICT = "ALREADY EXISTS";
        public const string LOGIN_FAILED = "LOGIN FAILED";
        public const string INVALID_DATA = "SOME DATA SEEMS TO BE INVALID";
        public const string SYSTEM_ERROR = "SYSTEM ERROR";
    }
}
