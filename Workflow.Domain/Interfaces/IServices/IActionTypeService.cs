using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.Interfaces.IServices
{
    public interface IActionTypeService
    {
        Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup();
    }
}