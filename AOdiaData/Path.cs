using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOdiaData
{
    [Index(nameof(routeId))]
    public class Path
    {
        public static Path CreateNewPath()
        {
            var p=new Path();
            DiaFile.staticDia.paths.Add(p);
            return p;
        }


        [Key]
        [Required]
        public long pathID { get; set; }

//        [NotMapped]
        public long routeId { get; set; }
        [NotMapped]
        private Route? _route { get; set; }

        public Route? route { get {
                _route ??= DiaFile.staticDia.routes.FirstOrDefault(r => r.RouteId == routeId);
                return _route;
            } set {
                _route = value;
                if (value== null)
                {
                    return;
                }
                routeId = value.RouteId;
                } }
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
