using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Portable
{
    public class TblInvestmentType
    {
		public String ID { get; set; }

        public String Name { get; set; }

        public String Present { get; set; }

        public String Future { get; set; }

        public String TimetoGet { get; set; }

        public String Rate { get; set; }

        public String Periodic { get; set; }
        
        public String Icon { get; set; }

		public bool Deleted { get; set; }
    }
}
