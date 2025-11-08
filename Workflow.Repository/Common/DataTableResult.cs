using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Workflow.Domain.ViewModels.Common;

namespace Workflow.Repository.Common
{
    public static class DataTableResult
    {
        public static async Task<DataTableResponseVM<T>> ToDataTableResult<T>(this IQueryable<T> q, int totalRecords) where T : class
        {
            return (await q.AsNoTracking().ToListAsync()).ToDataTableResult(totalRecords);
        }

        public static DataTableResponseVM<T> ToDataTableResult<T>(this List<T> list, int totalRecords)
        {
            return new DataTableResponseVM<T>
            {
                Data = list,
                RecordsFiltered = list.Count,
                TotalRecords = totalRecords
            };
        }
    }
}