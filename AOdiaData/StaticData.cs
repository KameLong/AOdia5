using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{
    public static class StaticData
    {
        public static DiaFile staticDia
        {
            get
            {
                if (_dia == null)
                {
                    _dia = new DiaFile();
                }
                return _dia;
            }
        }
        private static DiaFile? _dia = null;
    }
}
