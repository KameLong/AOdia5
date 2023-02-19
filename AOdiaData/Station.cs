using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{
    public class Station
    {
        public long StationId { get;set; }
        public string? Name{ get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }

        public Station()
        {
            Random random= new Random();
            StationId = random.NextInt64();
        }
    }
}
