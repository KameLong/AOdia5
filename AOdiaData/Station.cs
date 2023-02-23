﻿using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
          => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public long StationId { get;set; }

        [NotMapped]
        public ReactiveProperty <string> Name{ get; } = new ReactiveProperty<string>();
        [Column("name")]
        public string DbName { get { return Name.Value;} set { Name.Value = value; } }

        [NotMapped]
        public ReactiveProperty<float> Lat { get; } = new ReactiveProperty<float>();
        [Column("lat")]
        public float DbLat { get { return Lat.Value; } set { Lat.Value = value; } }

        [NotMapped]
        public ReactiveProperty<float> Lon { get; } = new ReactiveProperty<float>();
        [Column("lon")]
        public float DbLon { get { return Lon.Value; } set { Lon.Value = value; } }


        public Station()
        {
            Random random= new Random();
            StationId = random.NextInt64();
        }
    }
}