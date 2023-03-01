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
                [NotMapped]

        public  Route route { get { return DiaFile.staticDia.routes.Where(r => r.RouteId == routeId).First(); } set { routeId = value.RouteId; } }
        public int seq { get; set; }

        public long startStationID { get; set; }
        [NotMapped]
        public Station startStation { get { return DiaFile.staticDia.stations.Where(s=>s.StationId==startStationID).First(); }set { startStationID = value.StationId; } }

        public long endStationID { get; set; }
        [NotMapped]
        public Station endStation { get { return DiaFile.staticDia.stations.Where(s=>s.StationId==endStationID).First(); } set { endStationID = value.StationId; } }
        public Path()
        {
            Random random = new Random();
            pathID = random.NextInt64();
        }


    }
}
