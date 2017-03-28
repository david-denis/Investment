using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment
{
    public class TblStatsType
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(512)]
        public String FieldID { get; set; }

        [MaxLength(128)]
        public String Name { get; set; }

        [MaxLength(64)]
        public String Icon { get; set; }

        public int Deleted { get; set; }
    }
}
