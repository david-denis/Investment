using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment
{
    public class TblEntry
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

		[MaxLength(256)]
		public String FieldID { get; set; }

		[MaxLength(256)]
		public String InvestmentTypeID { get; set; }

        [MaxLength(512)]
        public String EntryName { get; set; }

        public int CalculateType { get; set; }

        public int CompoundingType { get; set; }

		public int CalendarType { get; set; }

        public float InitialPayment { get; set; }

        public float FuturePayment { get; set; }

        public float Rate { get; set; }

		[MaxLength(32)]
		public String TimeToGet { get; set; }

		[MaxLength(512)]
		public String StartTimeToGet { get; set; }

		[MaxLength(512)]
		public String EndTimeToGet { get; set; }

        public int DepositFlag { get; set; }

        public float DepositPayment { get; set; }

        public int Deleted { get; set; }

		public float GrowthRate { get; set; }

		public int Selected { get; set; }

		public int Published { get; set; }

		public int Private { get; set; }

		[MaxLength(512)]
		public String DateCreated { get; set; }

		[MaxLength(512)]
		public String DateEdited { get; set; }
    }
}
