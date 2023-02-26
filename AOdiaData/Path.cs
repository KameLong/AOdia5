using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOdiaData
{
    [Index(nameof(routeId))]
    public class Path
    {
        [Key]
        [Required]
        public long pathID { get; set; }

//        [NotMapped]
        public long routeId { get; set; }
        //        [NotMapped]

        private Route _route;
        public  Route route { get { return _route; } set { _route = value;routeId = _route.RouteId; } }
        public int seq { get; set; }

        public long startStationID { get; set; }

        public Station startStation { get { return AOdiaData.stations.Where(s => s.StationId == startStationID).First(); }set { startStationID = value.StationId; } }

        public long endStationID { get; set; }
        public Station endStation { get { return AOdiaData.stations.Where(s => s.StationId == endStationID).First(); } set { endStationID = value.StationId; } }
        public Path()
        {
            Random random = new Random();
            pathID = random.NextInt64();
        }


    }
}
