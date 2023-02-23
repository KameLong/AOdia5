using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{
    public class Route
    {
        public long RouteId { get; set; }

        [NotMapped]
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
        [Column("name")]
        public string DbName { get { return Name.Value; } set { Name.Value = value; } }

        public ICollection<Path> paths { get; set; } =new Collection<Path>();

        public Route()
        {
            Random random = new Random();
            RouteId = random.NextInt64();
        }

    }
}
