using Microsoft.VisualBasic;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace AOdiaData
{
    public class Route
    {
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

    }
}
