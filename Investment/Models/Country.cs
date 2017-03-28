using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment
{
    public class TblCountry
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(512)]
        public String FieldID { get; set; }

        [MaxLength(512)]
        public String Name { get; set; }
    }
}
