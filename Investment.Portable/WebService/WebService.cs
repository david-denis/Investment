using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Threading;

namespace Investment.Portable
{
	public class InvestmentTypeArray
	{
		public List<TblInvestmentType> investmenttypes { get; set; }
	}

	public class InvestmentTypeResult
	{
		public bool success { get; set; }
		public InvestmentTypeArray data { get; set; }
	}

	public class StatsArray
	{
		public List<TblStats> stats { get; set; }
	}

	public class StatsResult
	{
		public bool success { get; set; }
		public StatsArray data { get; set; }
	}

	public class StatTypesArray
	{
		public List<TblStatsType> statstypes { get; set; }
	}

	public class StatTypesResult
	{
		public bool success { get; set; }
		public StatTypesArray data { get; set; }
	}

	public class CountryArray
	{
		public List<TblCountry> countries { get; set; }
	}

	public class CountryResult
	{
		public bool success { get; set; }
		public CountryArray data { get; set; }
	}

	public class ReferenceArray
	{
		public List<TblReference> statstypes { get; set; }
	}

	public class ReferenceResult
	{
		public bool success { get; set; }
		public ReferenceArray data { get; set; }
	}

	public class CurrentDateTimeResult
	{
		public bool success { get; set; }
		public String data { get; set; }
	}

	public class RequestState
	{
		// This class stores the State of the request. 
		const int BUFFER_SIZE = 1024;
		public StringBuilder requestData;
		public byte[] BufferRead;
		public HttpWebRequest request;
		public HttpWebResponse response;
		public Stream streamResponse;
		public RequestState()
		{
			BufferRead = new byte[BUFFER_SIZE];
			requestData = new StringBuilder("");
			request = null;
			streamResponse = null;
		}
	}

	public class WebService
	{
		public static ManualResetEvent allDone = new ManualResetEvent(false);
		const int BUFFER_SIZE = 1024;
		const int DefaultTimeout = 30 * 1000; // 30 seconds timeout 

		// Abort the request if the timer fires. 
		private static void TimeoutCallback(object state, bool timedOut) { 
			if (timedOut) {
				HttpWebRequest request = state as HttpWebRequest;
				if (request != null) {
					request.Abort();
				}
			}
		}

		public static void ResetEvents()
		{
			allDone.Reset ();
		}

		const String GetRecentInvestTypesUrl = "http://api.investilator.com/v1/investmenttypes/getrecent?lastUpdate="; //12-3-2013
		const String GetAllInvestTypesUrl = "http://api.investilator.com/v1/investmenttypes/get";

		const String GetRecentStatsUrl = "http://api.investilator.com/v1/stats/getrecent?lastUpdate="; //12-3-2013
		const String GetAllStatsUrl = "http://api.investilator.com/v1/stats/get";

		const String GetRecentStatTypesUrl = "http://api.investilator.com/v1/statstypes/getrecent?lastUpdate="; //12-3-2013
		const String GetAllStatTypesUrl = "http://api.investilator.com/v1/statstypes/get";

		const String GetRecentCountriesUrl = "http://api.investilator.com/v1/countries/getrecent?lastUpdate=";//12-3-2013
		const String GetAllCountriesUrl = "http://api.investilator.com/v1/countries/get";

		const String GetRecentReferenceUrl = "http://api.investilator.com/v1/references/getrecent?lastUpdate=";//12-3-2013
		const String GetAllReferenceUrl = "http://api.investilator.com/v1/references/get";

		const String GetLastUpdateTimeGetUrl = "http://api.investilator.com/v1/common/getcurrentdatetime";

		public static String Apiresult = "";

		public WebService ()
		{
		}

		public List<TblInvestmentType> GetInvestTypesFromServer(String recentUpdateTime)
		{
			ResetEvents ();

			String url = GetAllInvestTypesUrl;
			if (String.IsNullOrEmpty (recentUpdateTime) == false)
				url = GetRecentInvestTypesUrl + recentUpdateTime;

			RequestState myRequestState = new RequestState();  

			HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create (url);
			myRequestState.request = myHttpWebRequest;

			IAsyncResult result=
				(IAsyncResult) myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);

			// this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
			//ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

			// The response came in the allowed time. The work processing will happen in the  
			// callback function.
			if (allDone.WaitOne (DefaultTimeout) == false)
				return null;

			InvestmentTypeResult investTypeResult = Newtonsoft.Json.JsonConvert.DeserializeObject<InvestmentTypeResult> (Apiresult);

			if (investTypeResult == null || investTypeResult.success == false)
				return null;

			return investTypeResult.data.investmenttypes;
		}

		public List<TblStats> GetStatsFromServer(String recentUpdateTime)
		{
			ResetEvents ();

			String url = GetAllStatsUrl;
			if (String.IsNullOrEmpty (recentUpdateTime) == false)
				url = GetRecentStatsUrl + recentUpdateTime;

			RequestState myRequestState = new RequestState();  

			HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create (url);
			myRequestState.request = myHttpWebRequest;

			IAsyncResult result=
				(IAsyncResult) myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);

			// this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
			//ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

			// The response came in the allowed time. The work processing will happen in the  
			// callback function.
			if (allDone.WaitOne (DefaultTimeout) == false)
				return null;

			Apiresult = Apiresult.Replace ("null", "0");
			StatsResult statsResult = Newtonsoft.Json.JsonConvert.DeserializeObject<StatsResult> (Apiresult);

			if (statsResult == null || statsResult.success == false)
				return null;

			return statsResult.data.stats;
		}

		public List<TblStatsType> GetStatTypesFromServer(String recentUpdateTime)
		{
			ResetEvents ();

			String url = GetAllStatTypesUrl;
			if (String.IsNullOrEmpty (recentUpdateTime) == false)
				url = GetRecentStatTypesUrl + recentUpdateTime;

			RequestState myRequestState = new RequestState();  

			HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create (url);
			myRequestState.request = myHttpWebRequest;

			IAsyncResult result=
				(IAsyncResult) myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);

			// this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
			//ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

			// The response came in the allowed time. The work processing will happen in the  
			// callback function.
			if (allDone.WaitOne (DefaultTimeout) == false)
				return null;

			StatTypesResult stattypesResult = Newtonsoft.Json.JsonConvert.DeserializeObject<StatTypesResult> (Apiresult);

			if (stattypesResult == null || stattypesResult.success == false)
				return null;

			return stattypesResult.data.statstypes;
		}

		public List<TblCountry> GetCountriesFromServer(String recentUpdateTime)
		{
			ResetEvents ();

			String url = GetAllCountriesUrl;
			if (String.IsNullOrEmpty (recentUpdateTime) == false)
				url = GetRecentCountriesUrl + recentUpdateTime;

			RequestState myRequestState = new RequestState();  

			HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create (url);
			myRequestState.request = myHttpWebRequest;

			IAsyncResult result=
				(IAsyncResult) myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);

			// this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
			//ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

			// The response came in the allowed time. The work processing will happen in the  
			// callback function.
			if (allDone.WaitOne (DefaultTimeout) == false)
				return null;

			CountryResult countryResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CountryResult> (Apiresult);

			if (countryResult == null || countryResult.success == false)
				return null;

			return countryResult.data.countries;
		}

		public List<TblReference> GetReferenceFromServer(String recentUpdateTime)
		{
			ResetEvents ();

			String url = GetAllReferenceUrl;
			if (String.IsNullOrEmpty (recentUpdateTime) == false)
				url = GetRecentReferenceUrl + recentUpdateTime;

			RequestState myRequestState = new RequestState();  

			HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create (url);
			myRequestState.request = myHttpWebRequest;

			IAsyncResult result=
				(IAsyncResult) myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);

			// this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
			//ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

			// The response came in the allowed time. The work processing will happen in the  
			// callback function.
			if (allDone.WaitOne (DefaultTimeout) == false)
				return null;

			ReferenceResult referenceResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ReferenceResult> (Apiresult);

			if (referenceResult == null || referenceResult.success == false)
				return null;

			return referenceResult.data.statstypes;
		}

		public String GetLastUpdateTimeFromServer()
		{
			ResetEvents ();

			String url = GetLastUpdateTimeGetUrl;

			RequestState myRequestState = new RequestState();  

			HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create (url);
			myRequestState.request = myHttpWebRequest;

			IAsyncResult result=
				(IAsyncResult) myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);

			// this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
			//ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

			// The response came in the allowed time. The work processing will happen in the  
			// callback function.
			if (allDone.WaitOne (DefaultTimeout) == false)
				return null;

			CurrentDateTimeResult dateTimeResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentDateTimeResult> (Apiresult);

			if (dateTimeResult == null || dateTimeResult.success == false)
				return null;

			return dateTimeResult.data;
		}

		private static void RespCallback(IAsyncResult asynchronousResult)
		{  
			try
			{
				// State of request is asynchronous.
				RequestState myRequestState=(RequestState) asynchronousResult.AsyncState;
				HttpWebRequest  myHttpWebRequest=myRequestState.request;
				//myHttpWebRequest.ContinueTimeout = 60;
				myRequestState.response = (HttpWebResponse) myHttpWebRequest.EndGetResponse(asynchronousResult);

				// Read the response into a Stream object.
				Stream responseStream = myRequestState.response.GetResponseStream();
				myRequestState.streamResponse=responseStream;

				string res = null;
				using (StreamReader srd = new StreamReader(responseStream)) {
					res = srd.ReadToEnd ();
				}

				Apiresult = res;
			}
			catch(WebException e)
			{

			}
			allDone.Set();
		}
	}
}

