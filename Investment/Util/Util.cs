using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;
using System.Numeric;
using Android.Preferences;

namespace Investment
{
    public class Util
    {
        public static Color BACKGROUND_COLOR = Color.Rgb(58, 68, 77);

        public static Color[] BAR_COLORS = new Color[12] { Color.Rgb(239, 155, 15),
            Color.Rgb(26, 31, 115),
            Color.Rgb(173, 255, 0),
            Color.Rgb(255, 216, 0),
            Color.Rgb(1, 105, 176),
            Color.Rgb(116, 214, 0),
            Color.Rgb(255, 186, 0),
            Color.Rgb(8, 151, 255),
            Color.Rgb(2, 137, 0),
            Color.Rgb(241, 200, 0),
            Color.Rgb(123, 189, 249),
            Color.Rgb(0, 210, 127)
        };

        public static String[] CALCULATE_TYPES = new String[6]{
            "None", "Investment Amount", "Future Amount", "Duration", "Rate", "Periodic Amount"
        };

        public static String[] COMPOUNDING_TYPES = new String[7]{
            "None", "Annually", "Semi-Annually", "Quarterly", "Monthly", "Weekly", "Daily"
        };

        public static String[] CALENDAR_TYPES = new String[2]{
			"Years", "Months"
        };

		public static String[] SOCIAL_TYPES = new String[4]{
			"Facebook", "Twitter", "Google+", "Email"
		};

		public const int MAX_GRAPH_COUNT = 12;

        public const int ACTIVITY_NEW_INVESTMENT = 0;

		public static String APP_PLATFORM = "Android";
		public static String DEVICE_VERSION = "";
		public static String APP_VERSION = "";

		public const String REFERENCE_ABOUT_US = "About Us";
		public const String REFERENCE_FAQ = "FAQ";
		public const String REFERENCE_PRIVACY = "Privacy";
		public const String REFERENCE_TERMS_OF_SERVICE = "Terms of Service";
		public const String REFERENCE_SUPPORT_SUBJECT = "Support Subject";
		public const String REFERENCE_SUPPORT_EMAIL = "Support Email";
		public const String REFERENCE_SUPPORT_BODY = "Support Body";
		public const String REFERENCE_FRIENDSKNOW_SUBJECT = "Friends Know Subject";
		public const String REFERENCE_FRIENDSKNOW_BODY = "Friends Know Body";

        public static String DatabaseFileName = "";

        public static DBManager DatabaseMgr;
        public static DBManager GetDatabaseMgr()
        {
            if (DatabaseMgr == null)
                DatabaseMgr = new DBManager(DatabaseFileName);

            return DatabaseMgr;
        }

        public static void InitializeDB(Context context)
        {
            String dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            DatabaseFileName = dbPath + @"/invest_1219_0342.db";

            //if (File.Exists(DatabaseFileName) == true)
            //    File.Delete(DatabaseFileName);

            if (File.Exists(DatabaseFileName) == false)
            {
                //File.Create(DatabaseFileName);

                using (var asset = context.Assets.Open("investdb.s3db")) using (var dest = File.Create(DatabaseFileName)) asset.CopyTo(dest);

                DatabaseMgr = new DBManager(DatabaseFileName);
            }
        }

        public static double Rate(double nper, double pmt, double pv, double fv, bool bBeginningOfPeriod)
        {
            double dRetvalue = 0;
            try
            {
                dRetvalue = Financial.Rate(nper, pmt, pv, fv, bBeginningOfPeriod ? PaymentDue.BeginningOfPeriod : PaymentDue.EndOfPeriod);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double FV(double rate, double nper, double pmt, double pv, bool bBeginningOfPeriod)
        {
            double dRetvalue = 0;
            try
            {
                dRetvalue = Financial.Fv(rate, nper, pmt, pv, bBeginningOfPeriod ? PaymentDue.BeginningOfPeriod : PaymentDue.EndOfPeriod);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double PV(double rate, double nper, double pmt, double fv, bool bBeginningOfPeriod)
        {
            double dRetvalue = 0;
            try
            {
                dRetvalue = Financial.Pv(rate, nper, pmt, fv, bBeginningOfPeriod ? PaymentDue.BeginningOfPeriod : PaymentDue.EndOfPeriod);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double Nper(double rate, double pmt, double pv, double fv, bool bBeginningOfPeriod)
        {
            double dRetvalue = 0;
            try
            {
                dRetvalue = Financial.NPer(rate, pmt, pv, fv, bBeginningOfPeriod ? PaymentDue.BeginningOfPeriod : PaymentDue.EndOfPeriod);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double Pmt(double rate, double nper, double pv, double fv, bool bBeginningOfPeriod)
        {
            double dRetvalue = 0;
            try
            {
                dRetvalue = Financial.Pmt(rate, nper, pv, fv, bBeginningOfPeriod ? PaymentDue.BeginningOfPeriod : PaymentDue.EndOfPeriod);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double CalculateRate(double pv, double fv, double time, int compounded)
        {
            double dRetvalue = 0;
            try
            {
                if (compounded == 0)
                    dRetvalue = ((fv / pv) - 1) / time;
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double CalculateFV(double pv, double rate, double time, int compounded)
        {
            double dRetvalue = 0;
            try
            {
                if (compounded == 0)
                    dRetvalue = pv * (1 + rate * time);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double CalculatePV(double fv, double rate, double time, int compounded)
        {
            double dRetvalue = 0;
            try
            {
                if (compounded == 0)
                    dRetvalue = fv / (1 + rate * time);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double CalculateTime(double pv, double fv, double rate, int compounded)
        {
            double dRetvalue = 0;
            try
            {
                if (compounded == 0)
                    dRetvalue = (fv / pv - 1);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

        public static double Effect(double nominalrate, double npery)
        {
            double dRetvalue = 0;
            try
            {
				if (npery == 0)
					dRetvalue = nominalrate;
				else
                	dRetvalue = Financial.Effect(nominalrate, npery);
            }
            catch (Exception ex)
            {

            }

            return dRetvalue;
        }

		public static void SaveDataToPreference(Context context, String key, String value)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString(key, value);
			editor.Apply();
		}

		public static void SaveDataToPreference(Context context, String key, int value)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutInt(key, value);
			editor.Apply();
		}

		public static String GetDataFromPreference(Context context, String key, String defaultValue)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
			String value = prefs.GetString (key, defaultValue);

			return value;
		}

		public static int GetDataFromPreference(Context context, String key, int defaultValue)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
			int value = prefs.GetInt (key, defaultValue);

			return value;
		}

		public static float GetTimeToGet(String timetoGet, int calendarType, String dateStart, String dateEnd)
		{
			float fRet = 0;
			if (String.IsNullOrEmpty (timetoGet) || timetoGet.Equals ("0")) {
				DateTime _dateStart, _dateEnd;
				try{
					DateTime.TryParse(dateStart, out _dateStart);
					DateTime.TryParse(dateEnd, out _dateEnd);
					if (_dateEnd.Day == _dateStart.Day && _dateEnd.Month == _dateStart.Month)
						fRet = _dateEnd.Year - _dateStart.Year;
					else {
						float betweenDays = (float)((_dateEnd - _dateStart).TotalDays);
						fRet = betweenDays / 30.0f / 12.0f;
					}
				}
				catch (Exception ex) {
				}
			} else {
				float.TryParse (timetoGet, out fRet);
				if (calendarType == 1)
					fRet /= 12.0f;
			}

			return fRet;
		}

		public static String GetEmailFooterInfo()
		{
			return APP_PLATFORM + " " + DEVICE_VERSION + ", Version = " + APP_VERSION;
		}
    }

	public static class ObjectTypeHelper
	{
		public static T Cast<T>(this Java.Lang.Object obj) where T : class
		{
			var propertyInfo = obj.GetType().GetProperty("Instance");
			return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
		}
	}
}