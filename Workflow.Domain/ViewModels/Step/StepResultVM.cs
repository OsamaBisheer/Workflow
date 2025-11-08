using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.ViewModels.Step
{
    public class StepResultVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssignedToRoleNameFL { get; set; }
        public string AssignedToRoleNameSL { get; set; }
        public ApplicationRoleDescriptionEnum AssignedToRoleEnum { get; set; }
        public string ActionTypeNameFL { get; set; }
        public string ActionTypeNameSL { get; set; }
        public ActionTypeEnum ActionTypeEnum { get; set; }
        public string NextStepName { get; set; }
    }
}