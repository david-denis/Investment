using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment
{
    public class TblReference
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(512)]
        public String FieldID { get; set; }

        [MaxLength(512)]
        public String Name { get; set; }

		[MaxLength(512)]
		public String Value { get; set; }

		[MaxLength(512)]
		public String DateCreated { get; set; }

		[MaxLength(512)]
		public String DateEdited { get; set; }
    }
}
