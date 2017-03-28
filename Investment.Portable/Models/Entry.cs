using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Portable
{
    public class TblEntry
    {
		public String ID { get; set; }

		public String InvestmentTypeID { get; set; }

        public String EntryName { get; set; }

        public int CalculateType { get; set; }

        public int CompoundingType { get; set; }

        public float InitialPayment { get; set; }

        public float FuturePayment { get; set; }

        public float Rate { get; set; }

        public float TimeToGet { get; set; }

		public String StartTimeToGet { get; set; }

		public String EndTimeToGet { get; set; }

        public int DepositFlag { get; set; }

        public float DepositPayment { get; set; }

		public bool Deleted { get; set; }

		public float GrowthRate { get; set; }

		public int Selected { get; set; }

		public int Published { get; set; }

		public int Private { get; set; }

		public String DateCreated { get; set; }

		public String DateEdited { get; set; }
    }
}
