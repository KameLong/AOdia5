using Microsoft.VisualBasic;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Net.Http.Headers;
using static System.Collections.Specialized.BitVector32;

namespace AOdiaData
{
    public class Route
    {
        public static Route CreateNewRoute()
        {
            var res=new Route();
            DiaFile.staticDia.routes.Add(res);
            res.color.Value = Color.Black;
            return res;
        }

        [Key]
        [Required]
        public long RouteId { get; set; }

        [NotMapped]
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>("");
        [Column("name")]
        public string dbName { get {
                return Name.Value;
            } set { Name.Value = value; } }

        [NotMapped]
        public ReactiveProperty<Color> color { get;  }=new ReactiveProperty<Color>();
        [Column("color")]
        public string dbColor { get { return ColorTranslator.ToHtml(color.Value); } set { color.Value = ColorTranslator.FromHtml(value); } }

        public virtual ICollection<Path> Paths { get; set; }= new Collection<Path>();

        public Route()
        {
            Random random = new Random();
            RouteId = random.NextInt64();
        }

        public Path AddPath(Station startStation,Station endStation)
        {
            var newPath = Path.CreateNewPath();
            newPath.route = this;
            newPath.startStation = startStation;
            newPath.endStation = endStation;
            newPath.seq = Paths.Count();
            Paths.Add(newPath);

            return newPath;
        }
        public Path AddPathTop(Station startStation)
        {
            if(Paths.Count() == 0) {
                throw new Exception();
            }
            var newPath = Path.CreateNewPath();
            newPath.route = this;
            newPath.startStation = startStation;
            newPath.endStation = Paths.Where(p => p.seq == 0).First().startStation ;
            newPath.seq = 0;
            foreach (var p in Paths)
            {
                p.seq++;
            }
            Paths.Add(newPath);
            return newPath;


        }
        /*
         * splitPathの間にstationを追加します。
         * もともと1つだったPathは二つになります。
         * 新しく作ったPathを返します。
         */
        public Path AddStation(Path splitPath,Station station)
        {
            var newPath = Path.CreateNewPath();
            newPath.route = this;
            newPath.startStation = station;
            newPath.endStation = splitPath.endStation;
            newPath.seq = splitPath.seq + 1;
            splitPath.endStation= station;

            foreach (var p in Paths.Where(p => p.seq > splitPath.seq)){
                p.seq++;
            }
            Paths.Add(newPath);
            return newPath;
        }

    }
}
