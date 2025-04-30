namespace gorynych.api.Contracts;

public sealed class Paging
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public static class PagingExtensions
{
    public static Paging Normalize(this Paging paging, int totalCount)
    {
        if (paging.PageSize < 1 || paging.PageSize > 100)
            paging.PageSize = 10;
        
        var lastPage = (int) Math.Ceiling(totalCount / (double) paging.PageSize);
        
        if (paging.Page < 1)
            paging.Page = 1; 
        else if (paging.Page > lastPage)
            paging.Page = lastPage;

        return paging;
    }
}