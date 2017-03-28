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
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

namespace Investment.Fragments
{
    public class OverviewFragment : Fragment
    {
        View view;
        EntryItemAdapter adapter = null;
        ListView listViewEntry = null;

        public Context ParentContext { get; set; }

        #region Fragment Lifecycle
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (view != null)
                return view;

            view = inflater.Inflate(Resource.Layout.Overview, container, false);
            listViewEntry = view.FindViewById<ListView>(Resource.Id.listData);

            return view;
        }

        async public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            InitializeList();
        }
        #endregion

        #region PUBLIC METHODS
        public void InitializeList()
        {
            var dbMgr = Util.GetDatabaseMgr();
            List<TblEntry> entryList = dbMgr.GetEntries("");

            adapter = new EntryItemAdapter(ParentContext, this, entryList, 1);
            listViewEntry.Adapter = adapter;

			DrawChart ();
        }

		public void DrawChart()
		{
			var dbMgr = Util.GetDatabaseMgr();
			List<TblEntry> entryList = dbMgr.GetEntriesForGraph();

			// Draw Chart
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
		}

		public void ItemSelected(int id)
		{
			Intent intentInvestment = new Intent(ParentContext, typeof(AddEntryActivity));
			intentInvestment.PutExtra("UID", id);
			StartActivityForResult(intentInvestment, Util.ACTIVITY_NEW_INVESTMENT);
		}

		public bool ItemCheckChanged(int id, bool isChecked)
		{
			if (isChecked == false) {
				TblEntry entryItem = Util.GetDatabaseMgr ().GetEntryWithId (id);
				entryItem.Selected = 0;
				Util.GetDatabaseMgr ().UpdateEntryWithId (entryItem);
				DrawChart ();
				return true;
			} else {
				if (Util.GetDatabaseMgr ().IsEnableEntryShow (id)) {
					TblEntry entryItem = Util.GetDatabaseMgr ().GetEntryWithId (id);
					entryItem.Selected = 1;
					Util.GetDatabaseMgr ().UpdateEntryWithId (entryItem);
					DrawChart ();
					return true;
				} else
					return false;
			}
		}
        #endregion
    }
}