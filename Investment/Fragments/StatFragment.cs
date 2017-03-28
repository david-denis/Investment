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
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using Android.Gms.Ads;

namespace Investment.Fragments
{
    public class StatFragment : Fragment
    {
        View view;
        StatsTypeItemAdapter adapter = null;
        ListView listViewType = null;

        List<TblStatsType> statsTypesList;
        public Context ParentContext { get; set; }

        #region Fragment Lifecycle
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (view != null)
                return view;

            view = inflater.Inflate(Resource.Layout.Stat, container, false);

            listViewType = view.FindViewById<ListView>(Resource.Id.listTypes);
            listViewType.ItemSelected += listViewType_ItemSelected;
            listViewType.ItemClick += listViewType_ItemClick;

            LinearLayout linearAdmob = view.FindViewById<LinearLayout>(Resource.Id.linearAdmob);
            var ad = new AdView(ParentContext);
            ad.AdSize = AdSize.SmartBanner;
            ad.AdUnitId = "ca-app-pub-5961528798514575/1367089442";
            var requestbuilder = new AdRequest.Builder();
            ad.LoadAd(requestbuilder.Build());
            linearAdmob.AddView(ad);

            return view;
        }

        void listViewType_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            InitializeChart(e.Position);
        }

        void listViewType_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            InitializeChart(e.Position);
        }

        async public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            InitializeList();
        }
        #endregion

        #region PRIVATE METHODS
        private void InitializeList()
        {
            var dbMgr = Util.GetDatabaseMgr();
            statsTypesList = dbMgr.GetStatsTypes();

            adapter = new StatsTypeItemAdapter(ParentContext, statsTypesList);
            listViewType.Adapter = adapter;
        }
        #endregion

        public void InitializeChart(int index)
        {
            if (index < 0)
                return;

            var dbMgr = Util.GetDatabaseMgr();
            List<TblStats> statsList = dbMgr.GetStats(statsTypesList[index].FieldID);

            // Draw Bar Chart
            OxyPlot.PlotModel plotModel = new OxyPlot.PlotModel();
            plotModel.PlotType = OxyPlot.PlotType.XY;
            plotModel.Title = "";
			/*
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom
            };

            for (int i = 0; i < statsList.Count; i++)
            {
                var Points = new List<ColumnItem>
                {
                    new ColumnItem(statsList[i].Value, i),
                };

                Android.Graphics.Color color = Util.BAR_COLORS[i % Util.BAR_COLORS.Length];
                var barArray = new ColumnSeries();
                barArray.ItemsSource = Points;
                barArray.FillColor = OxyColor.FromRgb(color.R, color.G, color.B);
                plotModel.Series.Add(barArray);

                categoryAxis.Labels.Add(statsList[i].Year.ToString());
            }

            plotModel.Axes.Add(categoryAxis);
            var plotview = view.FindViewById<OxyPlot.XamarinAndroid.PlotView>(Resource.Id.plotview);
            plotview.Model = plotModel;*/
			var categoryAxis = new CategoryAxis
			{
				Position = AxisPosition.Bottom
			};
			var Points = new List<DataPoint> ();

			statsList.Sort(delegate(TblStats x, TblStats y)
			{
				if (x.Year == y.Year && x.Month == y.Month) return 0;
				else if (x.Year < y.Year) return -1;
				else if (x.Year > y.Year) return 1;
				else if (x.Month < y.Month ) return -1;
				else if (x.Month > y.Month ) return 1;

				return 0;
			});

			for (int i = 0; i < statsList.Count; i++)
			{
				Points.Add(new DataPoint(i, statsList[i].Value));
				List<TblStats> statsEqual = statsList.FindAll (delegate(TblStats stat)
				{
					return stat.Year == statsList[i].Year;
				});

				int nCount = (statsEqual == null) ? 0 : statsEqual.Count;
				String axisLabel = (nCount > 1) ? (statsList [i].Year.ToString () + "." + statsList [i].Month.ToString ()) : statsList [i].Year.ToString ();
				categoryAxis.Labels.Add(axisLabel);
			}

			Android.Graphics.Color color = Android.Graphics.Color.Black;
			var lineSeries = new LineSeries();
			lineSeries.ItemsSource = Points;
			lineSeries.Color = OxyColor.FromRgb(color.R, color.G, color.B);
			plotModel.Series.Add(lineSeries);

			var plotviewline = view.FindViewById<OxyPlot.XamarinAndroid.PlotView>(Resource.Id.plotview);
			plotModel.Axes.Add(categoryAxis);
			plotviewline.Model = plotModel;
        }
    }
}