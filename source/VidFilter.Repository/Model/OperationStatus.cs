using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public class OperationStatus
    {
        public OperationStatus()
        {
            ExceptionMessages = new List<string>();
            ExceptionStackTraces = new List<string>();
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int NumRecordsAffected { get; set; }

        public ICollection<string> ExceptionMessages { get; set; }
        public ICollection<string> ExceptionStackTraces { get; set; }

        public void HandleException(string message, Exception ex)
        {
            this.Message = message;

            Exception curException = ex;
            while (ex != null)
            {
                this.ExceptionMessages.Add(ex.Message);
                this.ExceptionStackTraces.Add(ex.StackTrace);
                ex = ex.InnerException;
            }
        }

        public static OperationStatus GetOperationStatusFromException(string message, Exception ex)
        {
            OperationStatus opStatus = new OperationStatus();
            opStatus.Message = message;

            Exception curException = ex;
            while (ex != null)
            {
                opStatus.ExceptionMessages.Add(ex.Message);
                opStatus.ExceptionStackTraces.Add(ex.StackTrace);
                ex = ex.InnerException;
            }
            return opStatus;
        }
    }
}
