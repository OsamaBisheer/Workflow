namespace Workflow.Domain.ViewModels.Common
{
    public class DataTableResponseVM<T>
    {
        public int TotalRecords { get; set; }
        public int RecordsFiltered { get; set; }
        public IList<T> Data { get; set; }
    }
}