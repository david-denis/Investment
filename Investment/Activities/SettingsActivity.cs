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
using Android.Text.Method;
using Android.Text;

namespace Investment
{
    [Activity(Label = "SettingsActivity", Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SettingsActivity : Activity
    {
        SettingsItemAdapter adapter = null;
        ListView listViewType = null;

        List<TblInvestmentType> investList;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Settings);

            var imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
            imgBack.Click += imgBack_Click;

            var imgAdd = FindViewById<ImageView>(Resource.Id.imgAddButton);
            imgAdd.Click += imgAdd_Click;

            listViewType = FindViewById<ListView>(Resource.Id.listViewType);
            listViewType.ItemClick += listViewType_ItemClick;
            RegisterForContextMenu(listViewType);
        }

        void listViewType_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Id >= 0)
            {
                if (investList != null)
                {
                    int itemId = (int)investList[(int)e.Id].ID;
                    Intent investTypeIntent = new Intent(this, typeof(InvestmentTypeActivity));
                    investTypeIntent.PutExtra("itemid", itemId);
                    StartActivity(investTypeIntent);
                }
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            if (v == listViewType)
            {
                MenuInflater inflater = this.MenuInflater;
                inflater.Inflate(Resource.Menu.listview_type_menu, menu);
                menu.SetHeaderTitle("Action");
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            switch (item.ItemId)
            {
                case Resource.Id.delete:
                    Util.GetDatabaseMgr().DeleteInvestmentIndex(info.Position);

                    InitializeList();
                    return true;
                default:
                    return base.OnContextItemSelected(item);
            }            
        }

        private void InitializeList()
        {
            var dbMgr = Util.GetDatabaseMgr();
            investList = dbMgr.GetInvestmentTypes();

            adapter = new SettingsItemAdapter(this, investList);
            listViewType.Adapter = adapter;
        }

        protected override void OnResume()
        {
            base.OnResume();

            InitializeList();
        }

        void imgBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        void imgAdd_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(InvestmentTypeActivity)));
        }
    }
}