using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Portable
{
    public class TblStats
    {
		public String ID { get; set; }

        public String StatsTypeID { get; set; }

        public String CountryID { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public float Value { get; set; }

		public bool Deleted { get; set; }
    }
}
