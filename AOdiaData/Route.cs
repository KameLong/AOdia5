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

        /*
         * Pathを増やす方法
         * AddPath:Pathsに直接pathを追加します。（Paths.countが0の時に使うことを推奨)
         * AddStationTop:起点駅の前にstartStationを入れます。
         * AddStation:指定したPathを二つに分割し、間にstationを追加します。
         * AddStationEnd:終点駅の後にendStationを入れます。
         * 
         */
        public Path AddPath(Station startStation, Station endStation)
        {
            var newPath = Path.CreateNewPath();
            newPath.route = this;
            newPath.startStation = startStation;
            newPath.endStation = endStation;
            newPath.seq = Paths.Count();
            Paths.Add(newPath);

            return newPath;
        }
        public Path AddStationTop(Station startStation)
        {
            if (Paths.Count() == 0)
            {
                throw new Exception();
            }
            var newPath = Path.CreateNewPath();
            newPath.route = this;
            newPath.startStation = startStation;
            newPath.endStation = Paths.Where(p => p.seq == 0).First().startStation;
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
        public Path AddStationEnd(Station endStation)
        {
            if (Paths.Count() == 0)
            {
                throw new Exception();
            }
            var newPath = Path.CreateNewPath();
            newPath.route = this;
            newPath.startStation = Paths.Where(p => p.seq == Paths.Count-1).First().endStation;
            newPath.endStation = endStation;
            newPath.seq = Paths.Count;
            foreach (var p in Paths)
            {
                p.seq++;
            }
            Paths.Add(newPath);
            return newPath;
        }


        /**
         * 駅を削除します
         */

        /**
         * deletePathのstartStationを削除します。
         * ひとつ前のPathのendstationをdeletePathのendStationに変更する必要あり
         * return 成功したか？
         */
        public bool DeleteStation(Path deletePath)
        {
            if (!Paths.Contains(deletePath))
            {
                return false;
            }
            DiaFile.staticDia.paths.Remove(deletePath);
            DiaFile.staticDia.SaveChanges();
            if (deletePath.seq == 0)
            {

                foreach (var p in Paths)
                {
                    p.seq--;
                }
                return true;
            }
            foreach (var p in Paths.Where(p=>p.seq>deletePath.seq))
            {
                p.seq--;
            }
            Paths.First(p=>p.seq==deletePath.seq-1).endStation= deletePath.endStation;




            return true;

        }
        public void DeleteEndStation()
        {
            DiaFile.staticDia.paths.Remove(Paths.OrderBy(x => x.seq).Last());
        }

        /*
         * DeleteStationの逆操作です。
         * 削除したばずのpathを再度追加します。
         */
        public void InsertStation(Path path)
        {
            foreach (var p in Paths.Where(p=> { return p.seq >= path.seq; }))
            {
                p.seq++;
            }
            if (path.seq != 0)
            {
                Paths.First(p => p.seq == path.seq - 1).endStation = path.startStation;

            }
            DiaFile.staticDia.paths.Add(path);
            DiaFile.staticDia.SaveChanges();


        }
    }
}
