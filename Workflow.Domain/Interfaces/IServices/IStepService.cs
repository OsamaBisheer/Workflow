using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.Interfaces.IServices
{
    public interface IStepService
    {
        Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup(int workflowId);
    }
}