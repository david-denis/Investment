using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment
{
    public class TblInvestmentType
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

		[MaxLength(256)]
		public String FieldID { get; set; }

        [MaxLength(512)]
        public String Name { get; set; }

        [MaxLength(512)]
        public String Present { get; set; }

        [MaxLength(512)]
        public String Future { get; set; }

        [MaxLength(512)]
        public String TimetoGet { get; set; }

        [MaxLength(512)]
        public String Rate { get; set; }

        [MaxLength(512)]
        public String Periodic { get; set; }
        
        [MaxLength(512)]
        public String Icon { get; set; }

        public int Deleted { get; set; }
    }
}
