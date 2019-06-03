using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Lagerhaus.Errors
{
    public class Error
    {
        public string Code
        {
            get => Regex.Replace(this.GetType().Name, "Error$", "");
        }

        public string Message { get; set; }

        public object AdditionalData { get; set; } = null;

        public Error(string message)
        {
            this.Message = message;
        }
    }
}