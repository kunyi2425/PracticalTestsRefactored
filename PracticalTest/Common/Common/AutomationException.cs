using System;

namespace PracticalTest.Common.Common
{    
    public sealed class AutomationException : Exception
    {
        public AutomationException()
        {
        }

        public AutomationException(string message) : base(message)
        {
        }

        public AutomationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}