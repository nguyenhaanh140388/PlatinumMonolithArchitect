namespace Platinum.Core.Models
{
    public class OperationResult
    {
        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; private set; }
        public string Message { get; set; }
    }
}
