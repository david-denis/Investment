using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Investment
{
    public class DBManager : SQLiteConnection
    {
        public DBManager(String dbPath) : base(dbPath)
		{
		}

        public List<TblInvestmentType> GetInvestmentTypes()
        {
            IEnumerable<TblInvestmentType> investEnumerator = from s in Table<TblInvestmentType>() select s;

            return investEnumerator.ToList();
        }

        public TblInvestmentType GetInvestmentTypeFromId(String id)
        {
			id = id.ToUpper ();
			IEnumerable<TblInvestmentType> investEnumerator = from s in Table<TblInvestmentType>() where s.FieldID.ToUpper().Equals(id) select s;
            if (investEnumerator == null || investEnumerator.Count() == 0)
                return new TblInvestmentType();

            return investEnumerator.FirstOrDefault();
        }

		public TblInvestmentType GetInvestmentTypeFromDBId(int id)
		{
			IEnumerable<TblInvestmentType> investEnumerator = from s in Table<TblInvestmentType>() where s.ID == id select s;
			if (investEnumerator == null || investEnumerator.Count() == 0)
				return new TblInvestmentType();

			return investEnumerator.FirstOrDefault();
		}

        public void AddInvestType(String name, String present, String future, String timetoget, String rate, String periodic, String icon)
        {
            TblInvestmentType newItem = new TblInvestmentType { Name = name, Present = present, Future = future, TimetoGet = timetoget, Rate = rate, Periodic= periodic, Icon = icon, Deleted = 0 };
            Insert(newItem);
        }

        public void UpdateInvestType(int itemId, String name, String present, String future, String timetoget, String rate, String periodic, String icon)
        {
            TblInvestmentType newItem = new TblInvestmentType { Name = name, Present = present, Future = future, TimetoGet = timetoget, Rate = rate, Periodic = periodic, Icon = icon, Deleted = 0 };
            newItem.ID = itemId;
            Update(newItem);
        }

        public void DeleteInvestmentIndex(int index)
        {
            List<TblInvestmentType> investTypes = GetInvestmentTypes();
            if (index < investTypes.Count)
            {
                Delete<TblInvestmentType>(investTypes[index].ID);
            }
        }

		public void SyncInvestTypes(List<Investment.Portable.TblInvestmentType> result)
		{
			for (int i = 0; i < result.Count; i++) {
				try{
					String id = result[i].ID.ToUpper();
					TblInvestmentType item = (from s in Table<TblInvestmentType>() where s.FieldID.ToUpper().Equals(id) select s).FirstOrDefault();
					if (item == null)
						AddInvestType(result[i].Name, result[i].Present, result[i].Future, result[i].TimetoGet, result[i].Rate, result[i].Periodic, result[i].Icon);
					else
					{
						item.Name = result[i].Name;
						item.Present = result[i].Present;
						item.Future = result[i].Future;
						item.TimetoGet = result[i].TimetoGet;
						item.Rate = result[i].Rate;
						item.Periodic = result[i].Periodic;
						item.Icon = result[i].Icon;

						Update(item);
					}
				}
				catch (Exception e) {
				}
			}
		}

		public void SyncStatTypes(List<Investment.Portable.TblStatsType> result)
		{  
			for (int i = 0; i < result.Count; i++) {
				try{
					String id = result[i].ID.ToUpper();
					TblStatsType item = (from s in Table<TblStatsType>() where s.FieldID.ToUpper().Equals(id) select s).FirstOrDefault();
					if (item == null)
						AddStatsType(result[i].ID, result[i].Name, result[i].Icon);
					else
					{
						item.FieldID = result[i].ID;
						item.Name = result[i].Name;
						item.Icon = result[i].Icon;

						Update(item);
					}
				}
				catch (Exception e) {
				}
			}
		}

        public List<TblStatsType> GetStatsTypes()
        {
            IEnumerable<TblStatsType> statsEnumerator = from s in Table<TblStatsType>() select s;

            return statsEnumerator.ToList();
        }

        public void AddStatsType(String fieldID, String name, String icon)
        {
            TblStatsType newItem = new TblStatsType { FieldID = fieldID, Name = name, Icon = icon, Deleted = 0 };
            Insert(newItem);
        }

        public void DeleteStatsTypeIndex(int index)
        {
            List<TblStatsType> statsTypes = GetStatsTypes();
            if (index < statsTypes.Count)
            {
                Delete<TblStatsType>(statsTypes[index].ID);
            }
        }

		public void SyncStats(List<Investment.Portable.TblStats> result)
		{
			for (int i = 0; i < result.Count; i++) {
				try{
					String id = result[i].ID.ToUpper();
					TblStats item = (from s in Table<TblStats>() where s.FieldID.ToUpper().Equals(id) select s).FirstOrDefault();
					if (item == null)
						AddStats(result[i].ID, result[i].StatsTypeID, result[i].CountryID, result[i].Year, result[i].Month, result[i].Value);
					else
					{
						item.FieldID = result[i].ID;
						item.StatsTypeID = result[i].StatsTypeID;
						item.CountryID = result[i].CountryID;
						item.Year = result[i].Year;
						item.Month = result[i].Month;
						item.Value = result[i].Value;

						Update(item);
					}
				}
				catch (Exception e) {
				}
			}
		}

        public List<TblStats> GetStats()
        {
			IEnumerable<TblStats> statsEnumerator = (from s in Table<TblStats>() select s);

            return statsEnumerator.ToList();
        }

        public List<TblStats> GetStats(String statsTypeID)
        {
            IEnumerable<TblStats> statsEnumerator = from s in Table<TblStats>() where s.StatsTypeID == statsTypeID select s;

            return statsEnumerator.ToList();
        }

        public void AddStats(String fieldID, String statusTypeID, String countryID, int year, int month, float value)
        {
            TblStats newItem = new TblStats { FieldID = fieldID, StatsTypeID = statusTypeID, CountryID = countryID, Year = year, Month = month, Value = value, Deleted = 0 };
            Insert(newItem);
        }

        public void DeleteStatsIndex(int index)
        {
            List<TblStats> statsTypes = GetStats();
            if (index < statsTypes.Count)
            {
                Delete<TblStats>(statsTypes[index].ID);
            }
        }

        public List<TblEntry> GetEntries(String searchKey)
        {
            IEnumerable<TblEntry> entryEnumerator = from s in Table<TblEntry>() where s.EntryName.Contains(searchKey) select s;
			try{
				List<TblEntry> entries = entryEnumerator.ToList();
			}
			catch (Exception ex) {
				Console.Out.WriteLine (ex.ToString ());
			}
            return entryEnumerator.ToList();
        }

		public List<TblEntry> GetEntriesForGraph()
		{
			IEnumerable<TblEntry> entryEnumerator = from s in Table<TblEntry>() where s.Selected == 1 select s;
			try{
				List<TblEntry> entries = entryEnumerator.ToList();
			}
			catch (Exception ex) {
				Console.Out.WriteLine (ex.ToString ());
			}
			return entryEnumerator.ToList();
		}

		public TblEntry GetEntryWithId(int uid)
		{
			try{
				TblEntry entry = (from s in Table<TblEntry>() where s.ID == uid select s).FirstOrDefault();

				return entry;
			}
			catch (Exception ex) {
			}

			return null;
		}

		public bool IsEnableEntryShow(int uid)
		{
			IEnumerable<TblEntry> entryEnumerator = from s in Table<TblEntry>() where (s.ID != uid && s.Selected == 1 ) select s;
			try{
				List<TblEntry> entries = entryEnumerator.ToList();
				if (entries.Count >= Util.MAX_GRAPH_COUNT)
					return false;
			}
			catch (Exception ex){
				return true;
			}

			return true;
		}

		public void UpdateEntryWithId(TblEntry entryItem)
		{
			Update(entryItem);
		}

        public void AddEntry(String investmentTypeID, String entryName, int calculateType, int compoundingType, int calendarType, double initialPayment, double futurePayment, double rate, String timetoGet, String startTimeToGet, String endTimeToGet, int depositFlag, double depositPayment)
        {
			Guid newId = Guid.NewGuid();
			TblEntry newItem = new TblEntry { InvestmentTypeID = investmentTypeID, EntryName = entryName, CalculateType = calculateType, CompoundingType = compoundingType, CalendarType = calendarType, InitialPayment = (float)initialPayment, FuturePayment = (float)futurePayment, Rate = (float)rate, TimeToGet = timetoGet, StartTimeToGet = startTimeToGet, EndTimeToGet = endTimeToGet, DepositFlag = depositFlag, DepositPayment = (float)depositPayment, Deleted = 0 };
            Insert(newItem);
        }

        public void DeleteEntryIndex(int index)
        {
            List<TblEntry> entries = GetEntries("");
            if (index < entries.Count)
            {
                Delete<TblEntry>(entries[index].ID);
            }
        }

		public void DeleteEntryId(int id)
		{
			Delete<TblEntry>(id);
		}

		public List<TblCountry> GetCountries()
		{
			IEnumerable<TblCountry> countryEnumerator = from s in Table<TblCountry>() orderby s.Name select s;
			return countryEnumerator.ToList();
		}

		public int GetCountryID(String countryName)
		{
			int idx = -1;
			try{
				List<TblCountry> countryItems = GetCountries();
				for (int i = 0; i < countryItems.Count; i++)
				{
					if (countryItems[i].Name.Equals(countryName))
					{
						idx = i;
						break;
					}
				}
			}
			catch (Exception ex) {
			}

			return idx;
		}

		public void AddCountry(String fieldID, String name)
		{
			TblCountry newItem = new TblCountry { FieldID = fieldID, Name = name };
			Insert(newItem);
		}

		public void SyncCountries(List<Investment.Portable.TblCountry> result)
		{
			for (int i = 0; i < result.Count; i++) {
				try{
					String id = result[i].ID.ToUpper();
					TblCountry item = (from s in Table<TblCountry>() where s.FieldID.ToUpper().Equals(id) select s).FirstOrDefault();
					if (item == null)
						AddCountry(result[i].ID, result[i].Name);
					else
					{
						item.FieldID = result[i].ID;
						item.Name = result[i].Name;

						Update(item);
					}
				}
				catch (Exception e) {
				}
			}
		}

		public void AddReference(String fieldID, String name, String value, String dateCreated, String dateEdited)
		{
			TblReference newItem = new TblReference { FieldID = fieldID, Name = name, Value = value, DateCreated = dateCreated, DateEdited = dateEdited };
			Insert(newItem);
		}

		public void SyncReference(List<Investment.Portable.TblReference> result)
		{
			for (int i = 0; i < result.Count; i++) {
				try{
					String id = result[i].ID.ToUpper();
					TblReference item = (from s in Table<TblReference>() where s.FieldID.ToUpper().Equals(id) select s).FirstOrDefault();
					if (item == null)
						AddReference(result[i].ID, result[i].Name, result[i].Value, result[i].DateCreated, result[i].DateEdited);
					else
					{
						item.FieldID = result[i].ID;
						item.Name = result[i].Name;
						item.Value = result[i].Value;
						item.DateCreated = result[i].DateCreated;
						item.DateEdited = result[i].DateEdited;

						Update(item);
					}
				}
				catch (Exception e) {
				}
			}
		}

		public String GetReferenceValue(String name)
		{
			String result = "";

			try{
				TblReference item = (from s in Table<TblReference>() where s.Name.Equals(name) select s).FirstOrDefault();
				result = item.Value;
			}
			catch (Exception ex) {
			}

			return result;
		}
    }
}
