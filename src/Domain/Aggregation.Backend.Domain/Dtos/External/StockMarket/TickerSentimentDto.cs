using Newtonsoft.Json;

namespace Aggregation.Backend.Domain.Dtos.External.StockMarket
{
    public class TickerSentimentDto
    {
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("relevance_score")]
        public string RelevanceScore { get; set; }

        [JsonProperty("ticker_sentiment_score")]
        public string TickerSentimentScore { get; set; }

        [JsonProperty("ticker_sentiment_label")]
        public string TickerSentimentLabel { get; set; }
    }


}