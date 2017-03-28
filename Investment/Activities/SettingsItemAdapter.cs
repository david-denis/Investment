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

namespace Investment
{
    public class SettingsItemAdapter : BaseAdapter<TblInvestmentType>
    {
        List<TblInvestmentType> items;
		Activity context;

        public SettingsItemAdapter(Activity context, List<TblInvestmentType> items)
            : base()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

        public override TblInvestmentType this[int position]
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
				view = context.LayoutInflater.Inflate (Resource.Layout.TypeListItem, null);
			}

            view.FindViewById<ImageView>(Resource.Id.imgTypeIcon).SetImageResource(context.Resources.GetIdentifier(item.Icon, "drawable", context.PackageName));
            view.FindViewById<TextView>(Resource.Id.txtTypeName).Text = item.Name;

			return view;
		}
    }
}