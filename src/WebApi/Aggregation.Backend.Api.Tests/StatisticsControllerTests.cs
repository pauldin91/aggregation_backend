using Aggregation.Backend.Domain.Constants;
using Aggregation.Backend.Domain.Dtos.Statistics;
using Aggregation.Backend.Infrastructure.Cache;
using Aggregation.Backend.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

public class StatisticsControllerTests
{
    private StatisticsController _controller = null!;
    private PerformanceStatisticsCache _cache = null!;

    public StatisticsControllerTests()
    {
        _cache = new PerformanceStatisticsCache();


        _controller = new StatisticsController(_cache);
        _cache.Record(50);
        _cache.Record(150);
        _cache.Record(300);
    }

    [Test]
    public async Task GetRequestStatistics_ReturnsOkWithCorrectStats()
    {
        var result = await _controller.GetRequestStatistics();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
        var stats = okResult!.Value as RequestStatisticsDto;
        Assert.That(stats, Is.Not.Null);
        Assert.That(stats!.TotalRequests, Is.EqualTo(3));
        Assert.That(stats.Buckets[Buckets.Fast], Is.EqualTo(1));
        Assert.That(stats.Buckets[Buckets.Average], Is.EqualTo(1));
        Assert.That(stats.Buckets[Buckets.Slow], Is.EqualTo(1));
        Assert.That(stats.AverageResponseTimeMs, Is.EqualTo((50 + 150 + 300) / 3));
    }

    
}