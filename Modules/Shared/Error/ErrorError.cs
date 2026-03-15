using IndeedClone.Modules.SubModules.Auth.DTO;
using System.Diagnostics;

namespace IndeedClone.Modules.Shared.Error
{

    /* 
    * - Library to hold the error details or the success status
    * - Can be used across all modules.
    */

    public static class ErrorError
    {
        // Config
        public static bool DetailedError { get; set; } = false;

        // Thread safety
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        // Error storage (grouped)
        private static readonly Dictionary<string, List<ErrorData>> _errors
            = new Dictionary<string, List<ErrorData>>();

        // Status
        private static int _errorSet = 0;
        private static int _status = 1;

        // Success
        private static string? _successMessage;
        private static object _successData;
        private static bool _successDataSet = false;
        private static object? _returnData;


        static ErrorError()
        {
            _errors["general"] = new List<ErrorData>();
            _errors["success"] = new List<ErrorData>();
            _errors["debug"] = new List<ErrorData>();
        }

        /* =======================
           PUBLIC METHODS
        ======================== */

        public static void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _errors.Clear();
                _errors["general"] = new List<ErrorData>();
                _errors["success"] = new List<ErrorData>();
                _errors["debug"] = new List<ErrorData>();

                _errorSet = 0;
                _status = 1;
                _successMessage = string.Empty;
                _successData = null;
                _successDataSet = false;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public static void SetError(string message, string group = "general")
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

         // # Use caller method name if group not provided
            if (string.IsNullOrEmpty(group))
            {
                var stack = new System.Diagnostics.StackTrace();
             // # caller method
                var frame = stack.GetFrame(1); 
                group = frame?.GetMethod()?.Name ?? "general";
            }

            AddMessage(message, group, isError: true);
        }

        public static void SetSuccess(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            _successMessage = message;
            AddMessage(message, "success", isError: false);
        }

        public static void SetSuccessData(object data)
        {
            if (data == null) return;

            _successDataSet = true;
            _successData = data;
        }

     // # New Improvement -
        public static void SetReturnData(object data)
        {
            _returnData = data;
        }

        public static T GetReturnData<T>()
        {
            if (_returnData is T typedData)
                return typedData;

            throw new InvalidCastException("ReturnData is not of type " + typeof(T).Name);
        }

        public static string GetSuccess()
        {
            //if (_successData == null) return null;

            //// If it's a string
            //if (_successData is string str) return str;

            //// If it's RegistrationResult
            //if (_successData is AuthResponse rr) return rr.Message;

            //return _successData.ToString();

         // # Highest priority: explicit success data
            if (_successDataSet && _successData != null)
            {
                if (_successData is string str)
                    return str;

                if (_successData is AuthResponse rr)
                    return rr.Message;

                return _successData.ToString();
            }

         // # Normal success message (SetSuccess)
            if (!string.IsNullOrWhiteSpace(_successMessage))
                return _successMessage;

            return null;
        }


        // # -- End---


        /// <summary>
        /// TRUE = no error, FALSE = error exists
        /// </summary>
        public static bool CheckError(string group = "")
        {
            _lock.EnterReadLock();
            try
            {
                if (!string.IsNullOrEmpty(group))
                {
                    return !_errors.ContainsKey(group) || _errors[group].Count == 0;
                }

                return _errorSet == 0;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public static Tuple<int, object> GetReturnData(string group = "")
        {
            if (!CheckError(group))
            {
                var errors = GetErrors(group);
                return new Tuple<int, object>(_status, string.Join("<br/>", errors));
            }

            if (_successDataSet)
                return new Tuple<int, object>(_status, _successData);

            return new Tuple<int, object>(_status, _successMessage);
        }

        public static List<string> GetErrors(string group = "")
        {
            _lock.EnterReadLock();
            try
            {
                var result = new List<string>();

                if (!string.IsNullOrEmpty(group) && _errors.ContainsKey(group))
                {
                    result.AddRange(_errors[group].Select(e => e.Message));
                }
                else if (string.IsNullOrEmpty(group))
                {
                    foreach (var g in _errors.Values)
                        result.AddRange(g.Select(e => e.Message));
                }

                return result;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /* =======================
           INTERNAL
        ======================== */

        private static void AddMessage(string message, string group, bool isError)
        {
            var stack = new StackTrace(true);
            var frame = stack.GetFrame(2);

            var data = new ErrorData
            {
                Message = message,
                Group = group,
                Line = frame?.GetFileLineNumber() ?? 0,
                Method = frame?.GetMethod()?.Name ?? "Unknown",
                Time = DateTime.Now
            };

            _lock.EnterWriteLock();
            try
            {
                if (!_errors.ContainsKey(group))
                    _errors[group] = new List<ErrorData>();

                _errors[group].Add(data);

                if (isError)
                {
                    _errorSet = 1;
                    _status = -1;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }  
}


