using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class OperationStatus
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int NumRecordsAffected { get; set; }
        public FriendlyName ResultingFriendlyName { get; set; }

        public Exception Exception { get; set; }

        public void HandleException(string message, Exception ex)
        {
            Exception = ex;

            this.Message = message;
        }

        public static OperationStatus GetOperationStatusFromException(string message, Exception ex)
        {
            OperationStatus opStatus = new OperationStatus();
            opStatus.Message = message;
            opStatus.Exception = ex;
            return opStatus;
        }
    }
}
