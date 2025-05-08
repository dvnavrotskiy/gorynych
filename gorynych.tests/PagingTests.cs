using gorynych.api.Contracts;
using Xunit;

namespace gorynych.tests;

public class PagingTests
{
    [Theory]
    [InlineData(0, 0, 10, 1, 10)]
    [InlineData(-2, -10, 10, 1, 10)]
    [InlineData(1, 5, 10, 1, 5)]
    [InlineData(3, 5, 10, 2, 5)]
    [InlineData(4, 10, 21, 3, 10)]
    public void TestNormalization(int page, int pageSize, int total, int pageResult, int pageSizeResult)
    {
        var paging = new Paging { Page = page, PageSize = pageSize };

        paging.Normalize(total);
        
        Assert.Equal(pageResult, paging.Page);
        Assert.Equal(pageSizeResult, paging.PageSize);
    }
}