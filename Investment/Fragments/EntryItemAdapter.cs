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
using Android.Content.Res;
using Investment.Fragments;

namespace Investment
{
    public class EntryItemAdapter : BaseAdapter<TblEntry>
    {
        List<TblEntry> items;
		Context context;
		InvestmentFragment parentInvestFragment;
		OverviewFragment parentOverviewFragment;

		public EntryItemAdapter(Context context, Fragment parentFragment, List<TblEntry> items, int fromInvestment = 0)
            : base()
		{
			this.context = context;
			if (fromInvestment == 0) {
				this.parentInvestFragment = (InvestmentFragment)parentFragment;
				this.parentOverviewFragment = null;
			} else {
				this.parentInvestFragment = null;
				this.parentOverviewFragment = (OverviewFragment)parentFragment;
			}

			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

        public override TblEntry this[int position]
		{
			get { return items [position]; }
		}

		public override int Count
		{
			get { return items.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items [position];

			View view = convertView;
			if (view == null) {
				view = ((Activity)context).LayoutInflater.Inflate (Resource.Layout.EntryItem, null);
			}

			var rootLinear = view.FindViewById<LinearLayout> (Resource.Id.linearRoot);
			rootLinear.SetTag (Resource.Uid.TAG_ITEM_ID, item.ID);
            if (position % 2 == 0)
                view.FindViewById<LinearLayout>(Resource.Id.linearRoot).SetBackgroundColor(Android.Graphics.Color.Rgb(241, 243, 242));
            else
                view.FindViewById<LinearLayout>(Resource.Id.linearRoot).SetBackgroundColor(Android.Graphics.Color.Rgb(230, 230, 230));

            int indexColor = position % Util.BAR_COLORS.Length;
            view.FindViewById<LinearLayout>(Resource.Id.linearColor).SetBackgroundColor(Util.BAR_COLORS[indexColor]);

            TblInvestmentType investmentType = Util.GetDatabaseMgr().GetInvestmentTypeFromId(item.InvestmentTypeID);
            view.FindViewById<ImageView>(Resource.Id.imgTypeIcon).SetImageResource(context.Resources.GetIdentifier(investmentType.Icon, "drawable", context.PackageName));

            double dAPY = Util.Effect(item.Rate, item.CompoundingType) * 100;

            view.FindViewById<TextView>(Resource.Id.txtName).Text = item.EntryName;
            view.FindViewById<TextView>(Resource.Id.txtInitialPayment).Text = "$" + Math.Round(item.InitialPayment, 2).ToString();
            view.FindViewById<TextView>(Resource.Id.txtFuturePayment).Text = "$" + Math.Round(item.FuturePayment, 2).ToString();
			view.FindViewById<TextView>(Resource.Id.txtTime).Text = String.Format("{0:0.00}", Util.GetTimeToGet(item.TimeToGet, item.CalendarType, item.StartTimeToGet, item.EndTimeToGet));//Math.Round(item.TimeToGet, 2).ToString();
            view.FindViewById<TextView>(Resource.Id.txtGrowthRate).Text = Math.Round(dAPY, 2) + "%";
            view.FindViewById<CheckBox>(Resource.Id.chkPeriod).Checked = (item.Selected == 1);
            view.FindViewById<TextView>(Resource.Id.txtPeriodicPayment).Text = Math.Round(item.DepositPayment, 2).ToString();
            view.FindViewById<TextView>(Resource.Id.txtCompounded).Text = Util.COMPOUNDING_TYPES[item.CompoundingType];

			var CheckBoxPeriod = view.FindViewById<CheckBox> (Resource.Id.chkPeriod);
			CheckBoxPeriod.SetTag (Resource.Uid.TAG_ITEM_ID, item.ID);
			CheckBoxPeriod.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
				bool isCheckedState = CheckBoxPeriod.Checked;

				if (parentInvestFragment != null)
				{
					View root = sender as View;
					int Id = (int)root.GetTag(Resource.Uid.TAG_ITEM_ID);
					if (parentInvestFragment.ItemCheckChanged(Id, isCheckedState) == false)
						CheckBoxPeriod.Checked = false;
				}
				else if (parentOverviewFragment != null)
				{
					View root = sender as View;
					int Id = (int)root.GetTag(Resource.Uid.TAG_ITEM_ID);
					if (parentOverviewFragment.ItemCheckChanged(Id, isCheckedState) == false)
						CheckBoxPeriod.Checked = false;
				}
			};
			/*
			rootLinear.Clickable = true;
			rootLinear.Click += (object sender, EventArgs e) => {
				if (parentInvestFragment != null)
				{
					View root = sender as View;
					int Id = (int)root.GetTag(Resource.Uid.TAG_ITEM_ID);
					parentInvestFragment.ItemSelected(Id);
				}
				else if (parentOverviewFragment != null)
				{
					View root = sender as View;
					int Id = (int)root.GetTag(Resource.Uid.TAG_ITEM_ID);
					parentOverviewFragment.ItemSelected(Id);
				}
			};*/
    
			return view;
		}
    }
}