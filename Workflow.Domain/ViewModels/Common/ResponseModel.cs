using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.ViewModels.Common
{
    public class ResponseModel
    {
        public ResponseCodeEnum Code { get; set; }
        public string MessageFL { get; set; }
        public string MessageSL { get; set; }
        public dynamic Result { get; set; }
    }
}