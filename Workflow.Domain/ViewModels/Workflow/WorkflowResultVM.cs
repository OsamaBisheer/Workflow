using Workflow.Domain.ViewModels.Step;

namespace Workflow.Domain.ViewModels.Workflow
{
    public class WorkflowResultVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<StepResultVM> Steps { get; set; }
    }
}