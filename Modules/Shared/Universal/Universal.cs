using System;                  
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic; 
using IndeedClone.Modules.Shared.Error;

namespace IndeedClone.Modules.Shared.Universal
{
    public static class Universal
    {
        /*
         * Get Variables Globally
         */
        private static readonly ConcurrentDictionary<string, object> _variables = new ConcurrentDictionary<string, object>();

     // # Global Variable to decide if to write an error
        private static bool _Error = false;

      
     // # Static constructor
        static Universal()
        {
            // Initialize with default values if needed
        }

       
    // # Configure whether to print errors or not
        public static void Errors(bool mute = true)
        {
            if (!(mute == true || mute == false))
            {
                ErrorError.SetError("Unable to Mute the Errors");
                return;
            }

            _Error = mute;
        }

       
     // # Function to set the details and call the functions for verification
        public static void Set(string variable, object value = null)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                if (_Error)
                    ErrorError.SetError("Invalid Variable Name Set.");
                return;
            }

            _Set(variable, value);
        }

     // # Private function to actually Set the variable Data
        private static void _Set(string variable, object value)
        {
            _variables.AddOrUpdate(variable, value, (key, oldValue) => value);
        }

     // # Add this method for bulk operations
        public static void SetMultiple(Dictionary<string, object> values)
        {
            foreach (var kvp in values)
            {
                Set(kvp.Key, kvp.Value);
            }
        }

     // # Function to get the value of a variable
        public static object Get(string variable)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                if (_Error)
                    ErrorError.SetError("Invalid Variable Name Set");
                return null;
            }

            if (!_variables.TryGetValue(variable, out object value))
            {
                if (_Error)
                    ErrorError.SetError("Variable Requested is not Set");
                return null;
            }

            return value;
        }

      

    // # Generic version of Get method for type-safe retrieval
        public static T Get<T>(string variable)
        {
            var value = Get(variable);
            if (value is T typedValue)
            {
                return typedValue;
            }

            if (value != null)
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    if (_Error)
                        ErrorError.SetError($"Variable '{variable}' cannot be converted to type {typeof(T).Name}");
                }
            }

            return default;
        }

     // # Add a method to get with default value
        public static T Get<T>(string variable, T defaultValue)
        {
            if (!_variables.TryGetValue(variable, out object value))
                return defaultValue;

            if (value is T typedValue)
                return typedValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                if (_Error)
                    ErrorError.SetError($"Variable '{variable}' cannot be converted to type {typeof(T).Name}");
                return defaultValue;
            }
        }


     // # Function to get all stored variables
     // # Dictionary of all variables or empty dictionary if not in development
        public static Dictionary<string, object> GetAll()
        {
         // # Check environment - using ASP.NET Core style environment check
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("CI_ENVIRONMENT") ?? "Production";

            if (environment?.ToLower() == "development")
            {
                return _variables.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }

            return new Dictionary<string, object>();
        }

     // # Check if a variable exists
        public static bool Exists(string variable)
        {
            return _variables.ContainsKey(variable);
        }

     // # Remove a variable
        public static bool Remove(string variable)
        {
            return _variables.TryRemove(variable, out _);
        }

     // # Clear all variables
        public static void Clear()
        {
            _variables.Clear();
        }

     // # Get count of stored variables
        public static int Count => _variables.Count;
    }
}
