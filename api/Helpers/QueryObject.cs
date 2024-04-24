using Microsoft.AspNetCore.Mvc;

namespace api.Helpers
{
    /// <summary>
    /// class for classifying the type of query logic to use
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class QueryType : Attribute
    {
        public string Type { get; set; }

        public QueryType(string type)
        {
            Type = type;
        }
    }
    public class QueryObject
    {
        [QueryType("Where")]
        public string? Symbol { get; set; } = null;
        [QueryType("Where")]
        public string? CompanyName {get;set;} = null;
        [QueryType("OrderBy")]
        public string? SortBy { get; set; } = null;
        [QueryType("OrderDirection")]
        public bool IsDescending {get;set;} = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize {get;set;} = 20;
    }
}