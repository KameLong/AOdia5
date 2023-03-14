using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace AOdiaData
{
    public class Station
    {
        public static Station CreateNewStation()
        {
            var s=new Station();
            DiaFile.staticDia.stations.Add(s);
            return s;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
          => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        [Key]
        [Required]
        public long StationId { get;set; }

        [NotMapped]
        public ReactivePropertySlim <string> Name{ get; } = new ReactivePropertySlim<string>();
        [Column("name")]
        public string DbName { get { return Name.Value;} set { Name.Value = value; } }

        [NotMapped]
        public ReactivePropertySlim<float> Lat { get; } = new ReactivePropertySlim<float>();
        [Column("lat")]
        public float DbLat { get { return Lat.Value; } set { Lat.Value = value; } }

        [NotMapped]
        public ReactivePropertySlim<float> Lon { get; } = new ReactivePropertySlim<float>();
        [Column("lon")]
        public float DbLon { get { return Lon.Value; } set { Lon.Value = value; } }


        public Station()
        {
            Random random= new Random();
            StationId = random.NextInt64();
        }
    }
}
