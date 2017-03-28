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
    [Activity(Label = "DefaultCountryActivity", Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class DefaultCountryActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.DefaultCountry);

            ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
            imgBack.Click += imgBack_Click;

			List<TblCountry> countryList = Util.GetDatabaseMgr ().GetCountries ();
			String[] countryArr = new String[countryList.Count];
			for (int i = 0; i < countryList.Count; i++)
			{
				countryArr[i] = countryList[i].Name;
			}

			Spinner spinnerCountry = FindViewById<Spinner>(Resource.Id.spinnerCountry);
			ArrayAdapter<String> adapter_type_group = new ArrayAdapter<String>(this, Resource.Layout.SpinnerText, countryArr);
			adapter_type_group.SetDropDownViewResource(Resource.Layout.SpinnerText);
			spinnerCountry.Adapter = adapter_type_group;

			spinnerCountry.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)
			{
				if (e.Position != -1)// && _selGroupId != e.Position)
				{

				}
			};

			int defaultIdx = Util.GetDataFromPreference (this, "defaultCountry", 245);
			if (defaultIdx != -1)
				spinnerCountry.SetSelection (defaultIdx);
			else {

			}

			Button btnSave = FindViewById<Button> (Resource.Id.btnSave);
			btnSave.Click += (object sender, EventArgs e) => {
				Util.SaveDataToPreference(this, "defaultCountry", (int)spinnerCountry.SelectedItemId);
				Finish();
			};
        }

        void imgBack_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}