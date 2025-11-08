using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.ViewModels.Process
{
    public class ProcessResultVM
    {
        public int Id { get; set; }
        public string WorkflowName { get; set; }
        public ProcessStatusEnum Status { get; set; }
        public string CurrentInitiatorName { get; set; }
        public string CurrentStepName { get; set; }
    }
}