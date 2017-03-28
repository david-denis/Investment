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
	[Activity(Label = "InvestmentTypeActivity", Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", WindowSoftInputMode = Android.Views.SoftInput.AdjustResize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class InvestmentTypeActivity : Activity
    {
        public static String[] strings = {"Bank", "Stocks", "Bonds", "Real Estate"};

        public static int[] arr_images = {Resource.Drawable.icon_bank,
                            Resource.Drawable.icon_stock,
                            Resource.Drawable.icon_bond,
                            Resource.Drawable.icon_realestate};

        public static String[] arr_image_names = {"icon_bank",
                            "icon_stock",
                            "icon_bond",
                            "icon_realestate"};

        int itemId = -1;
        int iconIndex = -1;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.InvestmentTypeAdd);

            var imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
            imgBack.Click += imgBack_Click;

            MyAdapter spinnerAdapter = new MyAdapter(this, Resource.Layout.spinner_iconrow, strings);
            Spinner mySpinner = FindViewById<Spinner>(Resource.Id.spinnerIcon);
            mySpinner.ItemSelected += mySpinner_ItemSelected;
            mySpinner.Adapter = spinnerAdapter;

            var btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Click += btnSave_Click;

            itemId = Intent.GetIntExtra("itemid", -1);
            if (itemId != -1)
            {
				TblInvestmentType investTypeData = Util.GetDatabaseMgr().GetInvestmentTypeFromDBId(itemId);
                FindViewById<TextView>(Resource.Id.txtName).Text = investTypeData.Name;
                FindViewById<TextView>(Resource.Id.txtPresentValue).Text = investTypeData.Present;
                FindViewById<TextView>(Resource.Id.txtFutureValue).Text = investTypeData.Future;
                FindViewById<TextView>(Resource.Id.txtTime).Text = investTypeData.TimetoGet;
                FindViewById<TextView>(Resource.Id.txtRate).Text = investTypeData.Rate;
                FindViewById<TextView>(Resource.Id.txtPeriodicAmount).Text = investTypeData.Periodic;

                iconIndex = GetIndexFromStringArray(arr_image_names, investTypeData.Icon);
                mySpinner.SetSelection(iconIndex);
            }
        }

        int GetIndexFromStringArray(String[] itemArray, String iconName)
        {
            int retIdx = -1;
            for (int i = 0; i < itemArray.Length; i++)
            {
                if (itemArray[i].Equals(iconName))
                {
                    retIdx = i;
                    break;
                }
            }

            return retIdx;
        }

        void mySpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            iconIndex = e.Position;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            String investName = FindViewById<TextView>(Resource.Id.txtName).Text;
            String present = FindViewById<TextView>(Resource.Id.txtPresentValue).Text;
            String future = FindViewById<TextView>(Resource.Id.txtFutureValue).Text;
            String time = FindViewById<TextView>(Resource.Id.txtTime).Text;
            String rate = FindViewById<TextView>(Resource.Id.txtRate).Text;
            String periodic = FindViewById<TextView>(Resource.Id.txtPeriodicAmount).Text;

            if (investName == null || investName.Length <= 0)
            {
                Toast.MakeText(this, "Please input Investment Name", ToastLength.Short).Show();
                return;
            }

            if (iconIndex == -1)
            {
                Toast.MakeText(this, "Please choose icon first", ToastLength.Short).Show();
                return;
            }

            DBManager dbMgr = Util.GetDatabaseMgr();
            if (itemId == -1)
                dbMgr.AddInvestType(investName, present, future, time, rate, periodic, arr_image_names[iconIndex]);
            else
                dbMgr.UpdateInvestType(itemId, investName, present, future, time, rate, periodic, arr_image_names[iconIndex]);

            Finish();
        }

        void imgBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public class MyAdapter : ArrayAdapter<String>
        {
            InvestmentTypeActivity context;
            public MyAdapter(InvestmentTypeActivity context, int textViewResourceId, String[] objects)
                : base(context, textViewResourceId, objects)
            {
                this.context = context;
            }

            public override View GetDropDownView(int position, View convertView, ViewGroup parent)
            {
                return GetCustomView(position, convertView, parent);
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                return GetCustomView(position, convertView, parent);
            }

            public View GetCustomView(int position, View convertView, ViewGroup parent)
            {
                LayoutInflater inflater = context.LayoutInflater;
                View row = inflater.Inflate(Resource.Layout.spinner_iconrow, parent, false);

                TextView label = row.FindViewById<TextView>(Resource.Id.name);
                label.Text = strings[position];

                ImageView icon = row.FindViewById<ImageView>(Resource.Id.image);
                icon.SetImageResource(arr_images[position]);

                return row;
            }
        }
    }
}