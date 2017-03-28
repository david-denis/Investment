using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment
{
    public class TblStats
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(512)]
        public String FieldID { get; set; }

        [MaxLength(512)]
        public String StatsTypeID { get; set; }

        [MaxLength(512)]
        public String CountryID { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public float Value { get; set; }

        public int Deleted { get; set; }
    }
}
