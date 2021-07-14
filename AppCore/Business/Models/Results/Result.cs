using AppCore.Business.Models.Results.Bases;

namespace AppCore.Business.Models.Results
{
    public class Result
    {
        public ResultStatus Status { get; }
        public string Message { get; set; }

        public Result(ResultStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }

    public class Result<TResultType> : Result, IResultData<TResultType>
    {
        public TResultType Data { get; }

        public Result(ResultStatus status, string message, TResultType data) : base(status, message)
        {
            Data = data;
        }
    }
}
