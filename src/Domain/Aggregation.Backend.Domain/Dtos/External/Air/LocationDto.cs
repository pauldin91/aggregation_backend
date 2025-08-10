using Aggregation.Backend.Domain.Dtos.Interfaces;
using Newtonsoft.Json;

namespace Aggregation.Backend.Domain.Dtos.External.Air
{
    public class LocationDto : IMappedDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }

        public Dictionary<string, string> ToMap()
        {
            var map = new Dictionary<string, string> {
                { nameof(Type), Type },
                { nameof(Coordinates), string.Format("lon={0},lat={1}", Coordinates[0],Coordinates[1])},
            };

            return map;
        }
    }
}