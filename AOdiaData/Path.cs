using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOdiaData
{
//    [Index(nameof(routeID))]
    public class Path
    {
        [Key]
        [Required]
        public long pathID { get; set; }

//        [NotMapped]
        public long routeId { get; set; }
        //        [NotMapped]
        public  Route route { get; set; }
        public int seq { get; set; }

        public long startStationID { get; set; }
        public long endStationID { get; set; }
        public Path()
        {
            Random random = new Random();
            pathID = random.NextInt64();
        }


    }
}
