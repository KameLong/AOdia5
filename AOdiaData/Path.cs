using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{
    public class Path
    {
        public long pathID { get; set; }

        public long routeID { get; set; }
        public Route route { get; set; }
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
