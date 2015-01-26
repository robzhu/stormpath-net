
namespace Stormpath
{
    public class OperationError
    {
        public int Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string DeveloperMessage { get; set; }
        public string MoreInfo { get; set; }
    }
}
