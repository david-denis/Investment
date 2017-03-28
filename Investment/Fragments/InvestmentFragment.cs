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
using Android.Gms.Ads;

namespace Investment.Fragments
{
    public class InvestmentFragment : Fragment
    {
        View view;
        EntryItemAdapter adapter = null;
        ListView listViewEntry = null;
        EditText txtSearch;
        //AdView adView;

        public Context ParentContext { get; set; }

        #region Fragment Lifecycle
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (view != null)
                return view;

            view = inflater.Inflate(Resource.Layout.Investment, container, false);
            listViewEntry = view.FindViewById<ListView>(Resource.Id.listData);
			listViewEntry.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
			};
            RegisterForContextMenu(listViewEntry);

            txtSearch = view.FindViewById<EditText>(Resource.Id.txtSearch);
            txtSearch.TextChanged += txtSearch_TextChanged;

            LinearLayout linearAdmob = view.FindViewById<LinearLayout>(Resource.Id.linearAdmob);
            var ad = new AdView(ParentContext);
            ad.AdSize = AdSize.SmartBanner;
            ad.AdUnitId = "ca-app-pub-5961528798514575/2983423449";
            var requestbuilder = new AdRequest.Builder();
            ad.LoadAd(requestbuilder.Build());
            linearAdmob.AddView(ad);

            return view;
        }

        async public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            InitializeList();

            ImageView imgAddButton = view.FindViewById<ImageView>(Resource.Id.imgAddButton);
            imgAddButton.Click += imgAddButton_Click;
        }

        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == Util.ACTIVITY_NEW_INVESTMENT)
            {
                InitializeList();
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            if (v == listViewEntry)
            {
                MenuInflater inflater = ((MainActivity)ParentContext).MenuInflater;
                inflater.Inflate(Resource.Menu.listview_type_menu, menu);
                menu.SetHeaderTitle("Action");
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
			TblEntry itemData;

            switch (item.ItemId)
            {
			case Resource.Id.edit:
				itemData = listViewEntry.GetItemAtPosition (info.Position).Cast<TblEntry> ();
				ItemSelected (itemData.ID);
				return true;
			case Resource.Id.delete:
				itemData = listViewEntry.GetItemAtPosition (info.Position).Cast<TblEntry>();
				Util.GetDatabaseMgr ().DeleteEntryId (itemData.ID);

                InitializeList();
                return true;
            default:
                return base.OnContextItemSelected(item);
            }
        }

        void txtSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            InitializeList();
        }

        void imgAddButton_Click(object sender, EventArgs e)
        {
            StartActivityForResult(new Intent(ParentContext, typeof(AddEntryActivity)), Util.ACTIVITY_NEW_INVESTMENT);
        }
        #endregion

        #region PUBLIC METHODS
        public void InitializeList()
        {
            var dbMgr = Util.GetDatabaseMgr();
            List<TblEntry> entryList = dbMgr.GetEntries(txtSearch.Text);

            adapter = new EntryItemAdapter(ParentContext, this, entryList);
            listViewEntry.Adapter = adapter;
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
				return true;
			} else {
				if (Util.GetDatabaseMgr ().IsEnableEntryShow (id)) {
					TblEntry entryItem = Util.GetDatabaseMgr ().GetEntryWithId (id);
					entryItem.Selected = 1;
					Util.GetDatabaseMgr ().UpdateEntryWithId (entryItem);
					return true;
				} else
					return false;
			}
		}
        #endregion
    }
}