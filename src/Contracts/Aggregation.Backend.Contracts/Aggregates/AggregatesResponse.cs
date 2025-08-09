using Aggregation.Backend.Contracts.News;

namespace Aggregation.Backend.Contracts.Aggregates
{
    public class AggregatesResponse
    {
        public IList<NewsDto> News { get; set; }
        public IList<MusicDto> Musics { get; set; }
        public IList<WeatherDto> Weathers { get; set; }

        public AggregatesResponse(IList<NewsDto> news, IList<MusicDto> musics, IList<WeatherDto> weathers)
        {
            News = news;
            Musics = musics;
            Weathers = weathers;
        }
    }
}