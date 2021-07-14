using System;

namespace AppCore.Business.Models.Results
{
    public class ExceptionResult : Result
    {
        public ExceptionResult(Exception exception, bool showException = true)
            : base(ResultStatus.Exception,
                  showException ?
                    (exception != null ? 
                        "Exception: " + exception.Message + (exception.InnerException != null ? 
                            " | Inner Exception: " + exception.InnerException.Message + (exception.InnerException.InnerException != null ?
                                " | " + exception.InnerException.InnerException.Message
                            : "")
                        : "")
                    : "")
                  : "")
        {

        }

        public ExceptionResult() : base(ResultStatus.Exception, "")
        {

        }
    }

    public class ExceptionResult<TResultType> : Result<TResultType>
    {
        public ExceptionResult(Exception exception, bool showException = true)
            : base(ResultStatus.Exception,
                  showException ?
                    (exception != null ? 
                        "Exception: " + exception.Message + (exception.InnerException != null ? 
                            " | Inner Exception: " + exception.InnerException.Message + (exception.InnerException.InnerException != null ?
                                " | " + exception.InnerException.InnerException.Message
                            : "")
                        : "")
                    : "")
                  : "",
                default)
        {

        }

        public ExceptionResult() : base(ResultStatus.Exception, "", default)
        {

        }
    }
}
