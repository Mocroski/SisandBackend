using System.ComponentModel;
using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace SisandBackend.Shared.Utils;

public class PagedInput : IQueryPaging, IQuerySort
{
    #region Properties

    [QueryOperator(Max = 20)]
    [DefaultValue(10)]
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public string Sort { get; set; } = string.Empty;

    #endregion

    #region Constructors
    public PagedInput() { }
    #endregion
}
