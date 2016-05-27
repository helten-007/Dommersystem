using System;

namespace NordicArenaDomainModels.ObjectValidation
{
    /// <summary>
    /// Wrapping a result and a bool
    /// </summary>
    public class ResultReasonTuple
    {
        public bool IsTrue { get; set; }
        public String Reason { get; set; }

        public ResultReasonTuple() { }
        public ResultReasonTuple(bool val) { IsTrue = val; }
        public ResultReasonTuple(bool val, string reason) 
        { 
            IsTrue = val;
            Reason = reason;
        }
    }
}