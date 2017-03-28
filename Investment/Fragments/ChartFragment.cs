using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Globalization;
using Android.Gms.Ads;

using Xamarin.Auth;
using Xamarin.Social.Services;
using Xamarin.Social;

namespace Investment.Fragments
{
    public class ChartFragment : Fragment
    {
        View view;
		LinearLayout linearShare { get; set; }
        
		public Context ParentContext { get; set; }
		bool IsShareVisible = false;

		private static FacebookService mFacebook;
		private static TwitterService mTwitter;

		public static FacebookService Facebook
		{
			get
			{
				if (mFacebook == null)
				{
					mFacebook = new FacebookService() {
						ClientId = "323631191174521",
						ClientSecret = "",
						RedirectUrl = new Uri ("http://www.investilator.com/success.html")
					};
				}

				return mFacebook;
			}
		}

		public static TwitterService Twitter
		{
			get
			{
				if (mTwitter == null)
				{
					mTwitter = new TwitterService {
						ConsumerKey = "KyWw82bUUkWFzHKFncXYGIy0i",
						ConsumerSecret = "5LQQ7cOGUz4wHbcwOx3c3bKItw19arbRzhxtrtZzAMFkWQ2Drv",
						CallbackUrl = new Uri ("http://www.investilator.com/success.html")
					};
				}

				return mTwitter;
			}
		}

        #region Fragment Lifecycle
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (view != null)
                return view;

            view = inflater.Inflate(Resource.Layout.Chart, container, false);

            InitializeChart();

            LinearLayout linearAdmob = view.FindViewById<LinearLayout>(Resource.Id.linearAdmob);
            var ad = new AdView(ParentContext);
            ad.AdSize = AdSize.SmartBanner;
            ad.AdUnitId = "ca-app-pub-5961528798514575/5936889841";
            var requestbuilder = new AdRequest.Builder();
            ad.LoadAd(requestbuilder.Build());
            linearAdmob.AddView(ad);

			linearShare = view.FindViewById<LinearLayout> (Resource.Id.linearShare);
			ImageView imgShare = view.FindViewById<ImageView> (Resource.Id.imgShareBtn);
			imgShare.Click += (object sender, EventArgs e) => {
				/*if (IsShareVisible == false)
				{
					linearShare.Visibility = ViewStates.Visible;
					IsShareVisible = true;
				}
				else{
					linearShare.Visibility = ViewStates.Invisible;
					IsShareVisible = false;
				}*/
			};

			FrameLayout frmLayout = view.FindViewById<FrameLayout> (Resource.Id.frameLayout2);
			frmLayout.Click += (object sender, EventArgs e) => {
				if (IsShareVisible == true)
				{
					linearShare.Visibility = ViewStates.Visible;
					IsShareVisible = true;
				}
			};

			LinearLayout linearFacebook = view.FindViewById<LinearLayout> (Resource.Id.linearFaceBook);
			linearFacebook.Click += (object sender, EventArgs e) => {
				Share(Facebook);
			};

			LinearLayout linearTwitter = view.FindViewById<LinearLayout> (Resource.Id.linearTwitter);
			linearTwitter.Click += (object sender, EventArgs e) => {
				Share(Twitter);
			};

			LinearLayout linearGooglePlus = view.FindViewById<LinearLayout> (Resource.Id.linearGooglePlus);
			linearGooglePlus.Click += (object sender, EventArgs e) => {

			};

			LinearLayout linearEmail = view.FindViewById<LinearLayout> (Resource.Id.linearEmail);
			linearEmail.Click += (object sender, EventArgs e) => {
				var email = new Intent (Android.Content.Intent.ActionSend);
				email.SetType ("message/rfc822");
				StartActivity (email);
			};

            return view;
        }

        async public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }
        #endregion

		void Share (Xamarin.Social.Service service)
		{
			var plotview = view.FindViewById<OxyPlot.XamarinAndroid.PlotView>(Resource.Id.plotview);
			var plotviewline = view.FindViewById<OxyPlot.XamarinAndroid.PlotView>(Resource.Id.plotviewline);

			Item item = new Item {
				Text = "Share Text Here",
			};

			Intent intent = service.GetShareUI ((Activity)ParentContext, item, shareResult => {
				System.Console.WriteLine(shareResult);
			});

			StartActivity (intent);
		}

        public void InitializeChart()
        {
            var dbMgr = Util.GetDatabaseMgr();
			List<TblEntry> entryList = dbMgr.GetEntriesForGraph();

            // Draw Bar Chart
            OxyPlot.PlotModel plotModel = new OxyPlot.PlotModel();
            plotModel.PlotType = OxyPlot.PlotType.XY;
            plotModel.Title = "";

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom
            };

            for (int i = 0; i < entryList.Count; i++)
            {
                var Points = new List<ColumnItem>
                {
                    new ColumnItem(Util.Effect(entryList[i].Rate, entryList[i].CompoundingType) * 100, i),
                };

                Android.Graphics.Color color = Util.BAR_COLORS[i % Util.BAR_COLORS.Length];
                var barArray = new ColumnSeries();
                barArray.ItemsSource = Points;
                barArray.FillColor = OxyColor.FromRgb(color.R, color.G, color.B);
                plotModel.Series.Add(barArray);

                categoryAxis.Labels.Add("");
            }

            plotModel.Axes.Add(categoryAxis);
            var plotview = view.FindViewById<OxyPlot.XamarinAndroid.PlotView>(Resource.Id.plotview);
            plotview.Model = plotModel;

            // Draw Line Chart
            OxyPlot.PlotModel plotModelLine = new OxyPlot.PlotModel();
            plotModelLine.PlotType = OxyPlot.PlotType.XY;
            plotModelLine.Title = "";

			int maxYears = 0;
			for (int i = 0; i < entryList.Count; i++) {
				float timetoGetYear = Util.GetTimeToGet (entryList [i].TimeToGet, entryList [i].CalendarType, entryList [i].StartTimeToGet, entryList [i].EndTimeToGet);
				int itemYear = (int)Math.Round(timetoGetYear);
				if (itemYear > maxYears)
					maxYears = itemYear;
			}

            for (int i = 0; i < entryList.Count; i++)
            {
				double yearCurValue, yearPrevValue;
                yearPrevValue = yearCurValue = entryList[i].InitialPayment * (1 + entryList[i].Rate);

				int iYear = 0;
				var Points = new List<DataPoint> ();
				Points.Add (new DataPoint(iYear + 1, yearCurValue / entryList[i].InitialPayment));
				for (iYear = 1; iYear < maxYears; iYear++)
				{
					yearCurValue = yearPrevValue * (1 + entryList [i].Rate) > entryList [i].FuturePayment ? entryList [i].FuturePayment : yearPrevValue * (1 + entryList [i].Rate);
					yearPrevValue = yearCurValue;

					Points.Add (new DataPoint(iYear + 1, yearCurValue / entryList[i].InitialPayment));
				}

                Android.Graphics.Color color = Util.BAR_COLORS[i % Util.BAR_COLORS.Length];
                var lineSeries = new LineSeries();
                lineSeries.ItemsSource = Points;
                lineSeries.Color = OxyColor.FromRgb(color.R, color.G, color.B);
                plotModelLine.Series.Add(lineSeries);
            }

            var plotviewline = view.FindViewById<OxyPlot.XamarinAndroid.PlotView>(Resource.Id.plotviewline);
            plotviewline.Model = plotModelLine;
        }
    }
}