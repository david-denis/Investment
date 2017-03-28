using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Animation;
using Investment.Fragments;
using Investment.Portable;
using System.Collections.Generic;
using Android.Text;
using Android.Content.PM;

namespace Investment
{
    [Activity(Label = "Investilator", Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", WindowSoftInputMode = Android.Views.SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity
    {
        #region PRIVATE MEMBERS
        LinearLayout linearHeader;
        LinearLayout linearHdrOverView;
        LinearLayout linearHdrInvestment;
        LinearLayout linearHdrChart;
        LinearLayout linearHdrStat;
        LinearLayout linearHdrMore;

        FrameLayout frmMain;
        FrameLayout frmMenu;
        LinearLayout linearMenu;

        InvestmentFragment investmentFrag;
        #endregion

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            Util.InitializeDB(this);

			Util.DEVICE_VERSION = Android.OS.Build.VERSION.Release;

			PackageInfo pInfo = PackageManager.GetPackageInfo (this.ApplicationContext.PackageName, 0);
			Util.APP_VERSION = pInfo.VersionName;

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            linearHeader = (LinearLayout)this.FindViewById<LinearLayout>(Resource.Id.linearHeader);
            linearHdrOverView = (LinearLayout)this.FindViewById<LinearLayout>(Resource.Id.linearHdrOverview);
            linearHdrInvestment = (LinearLayout)this.FindViewById<LinearLayout>(Resource.Id.linearHdrInvestment);
            linearHdrChart = (LinearLayout)this.FindViewById<LinearLayout>(Resource.Id.linearHdrCharts);
            linearHdrStat = (LinearLayout)this.FindViewById<LinearLayout>(Resource.Id.linearHdrStats);
            linearHdrMore = (LinearLayout)this.FindViewById<LinearLayout>(Resource.Id.linearHdrMore);

            frmMain = (FrameLayout)this.FindViewById<FrameLayout>(Resource.Id.frameMain);
            frmMenu = (FrameLayout)this.FindViewById<FrameLayout>(Resource.Id.frameMenu);
            linearMenu = (LinearLayout)this.FindViewById<LinearLayout>(Resource.Id.linearMenu);

            linearHdrOverView.Click += linearHdr_Click;
            linearHdrInvestment.Click += linearHdr_Click;
            linearHdrChart.Click += linearHdr_Click;
            linearHdrStat.Click += linearHdr_Click;
            linearHdrMore.Click += linearHdr_Click;

            LinearLayout linearMenuAboutUs = FindViewById<LinearLayout>(Resource.Id.linearMenuAboutUs);
            linearMenuAboutUs.Click += linearMenuAboutUs_Click;

            LinearLayout linearMenuSettings = FindViewById<LinearLayout>(Resource.Id.linearMenuSettings);
            linearMenuSettings.Click += linearMenuSettings_Click;

            LinearLayout linearMenuTypeAdd = FindViewById<LinearLayout>(Resource.Id.linearMenuTypeAdd);
            linearMenuTypeAdd.Click += linearMenuTypeAdd_Click;

			LinearLayout linearDefaultCountry = FindViewById<LinearLayout>(Resource.Id.linearMenuDefaultCountry);
			linearDefaultCountry.Click += linearMenuDefaultCountry_Click;

			LinearLayout linearUpdateData = FindViewById<LinearLayout>(Resource.Id.linearMenuUpdateData);
			linearUpdateData.Click += linearMenuUpdateData_Click;

			LinearLayout linearHelpAndLegal = FindViewById<LinearLayout>(Resource.Id.linearHelpAndLegal);
			linearHelpAndLegal.Click += linearMenuHelpAndLegal_Click;

			LinearLayout linearShowFriend = FindViewById<LinearLayout>(Resource.Id.linearShowFriend);
			linearShowFriend.Click += linearMenuShowFriend_Click;

			LinearLayout linearShowLove = FindViewById<LinearLayout>(Resource.Id.linearShowLove);
			linearShowLove.Click += linearMenuShowLove_Click;

			LinearLayout linearEmailSupport = FindViewById<LinearLayout>(Resource.Id.linearHelpEmail);
			linearEmailSupport.Click += linearEmailSupport_Click;

			LinearLayout linearHelpSettings = FindViewById<LinearLayout>(Resource.Id.linearHelpSettings);
			linearHelpSettings.Click += linearHelpSettings_Click;

			LinearLayout linearHelpFAQ = FindViewById<LinearLayout>(Resource.Id.linearHelpFAQ);
			linearHelpFAQ.Click += linearHelpFAQ_Click;

			LinearLayout linearLegalPrivacy = FindViewById<LinearLayout>(Resource.Id.linearLegalPrivacy);
			linearLegalPrivacy.Click += linearLegalPrivacy_Click;

			LinearLayout linearLegalTerms = FindViewById<LinearLayout>(Resource.Id.linearLegalTerms);
			linearLegalTerms.Click += linearLegalTerms_Click;

            ChangeMainView(linearHdrOverView);
		}

        void linearMenuAboutUs_Click(object sender, EventArgs e)
        {
            //ShowMenu();
			var showData = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_ABOUT_US);
			var AboutUsIntent = new Intent(this, typeof(AboutUsActivity));
			AboutUsIntent.PutExtra ("text", showData);

			StartActivity(AboutUsIntent);
        }

		void linearHelpFAQ_Click(object sender, EventArgs e)
		{
			//ShowMenu();
			var showData = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_FAQ);
			var AboutUsIntent = new Intent(this, typeof(AboutUsActivity));
			AboutUsIntent.PutExtra ("text", showData);

			StartActivity(AboutUsIntent);
		}

		void linearLegalPrivacy_Click(object sender, EventArgs e)
		{
			//ShowMenu();
			var showData = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_PRIVACY);
			var AboutUsIntent = new Intent(this, typeof(AboutUsActivity));
			AboutUsIntent.PutExtra ("text", showData);

			StartActivity(AboutUsIntent);
		}

		void linearLegalTerms_Click(object sender, EventArgs e)
		{
			//ShowMenu();
			var showData = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_TERMS_OF_SERVICE);
			var AboutUsIntent = new Intent(this, typeof(AboutUsActivity));
			AboutUsIntent.PutExtra ("text", showData);

			StartActivity(AboutUsIntent);
		}

        void linearMenuSettings_Click(object sender, EventArgs e)
        {
            //ShowMenu();
            //StartActivity(new Intent(this, typeof(SettingsActivity)));
        }

        void linearMenuTypeAdd_Click(object sender, EventArgs e)
        {
            //ShowMenu();
            StartActivity(new Intent(this, typeof(SettingsActivity)));
        }

		void linearMenuDefaultCountry_Click(object sender, EventArgs e)
		{
			//ShowMenu();
			StartActivity(new Intent(this, typeof(DefaultCountryActivity)));
		}

		void linearMenuUpdateData_Click(object sender, EventArgs e)
		{
			//ShowMenu();
			SyncDataTask updateTask = new SyncDataTask();
			updateTask.context = this;
			updateTask.Execute ();
		}

		void linearMenuHelpAndLegal_Click(object sender, EventArgs e)
		{
			FindViewById<LinearLayout> (Resource.Id.linearMenu).Visibility = ViewStates.Invisible;
			FindViewById<LinearLayout> (Resource.Id.linearHelpLegalMenu).Visibility = ViewStates.Visible;
		}

		void linearHelpSettings_Click(object sender, EventArgs e)
		{
			FindViewById<LinearLayout> (Resource.Id.linearMenu).Visibility = ViewStates.Visible;
			FindViewById<LinearLayout> (Resource.Id.linearHelpLegalMenu).Visibility = ViewStates.Invisible;
		}

		void linearMenuShowFriend_Click(object sender, EventArgs e)
		{
			//ShowMenu();
			var emailSubject = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_FRIENDSKNOW_SUBJECT);
			var emailBody = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_FRIENDSKNOW_BODY);
			//emailBody += "<br><br>" + Util.GetEmailFooterInfo ();

			var email = new Intent (Android.Content.Intent.ActionSend);
			email.SetType ("text/html");
			email.PutExtra (Android.Content.Intent.ExtraSubject, emailSubject);
			email.PutExtra (Android.Content.Intent.ExtraText, Html.FromHtml(emailBody));
			StartActivity (email);
		}

		void linearMenuShowLove_Click(object sender, EventArgs e)
		{
			//ShowMenu();
			var uri = Android.Net.Uri.Parse ("https://play.google.com/store/apps/details?id=com.smartegicsys.investilator");
			var intent = new Intent (Intent.ActionView, uri); 
			StartActivity (intent); 
		}

		void linearEmailSupport_Click(object sender, EventArgs e)
		{
			//ShowMenu();
			var emailSubject = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_SUPPORT_SUBJECT);
			var emailEmail = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_SUPPORT_EMAIL);
			var emailBody = Util.GetDatabaseMgr ().GetReferenceValue (Util.REFERENCE_SUPPORT_BODY);
			emailBody += "<br><br>" + Util.GetEmailFooterInfo ();

			var email = new Intent (Android.Content.Intent.ActionSend);
			email.SetType ("text/html");
			email.PutExtra (Android.Content.Intent.ExtraEmail, new string[]{emailEmail} );
			email.PutExtra (Android.Content.Intent.ExtraSubject, emailSubject);
			email.PutExtra (Android.Content.Intent.ExtraText, Html.FromHtml(emailBody));
			StartActivity (email);
		}

        void linearHdr_Click(object sender, EventArgs e)
        {
            LinearLayout linearSelected = sender as LinearLayout;

            ChangeMainView(linearSelected);
        }

        private void ChangeMainView(LinearLayout linearSelected)
        {
            if (linearSelected == linearHdrMore)
                ShowMenu();
            else
            {
                HideMenu();
                LinearLayout.LayoutParams marginParams = new LinearLayout.LayoutParams(0, FrameLayout.LayoutParams.MatchParent, 126);
                marginParams.SetMargins(0, 0, 0, 0);

                linearHdrOverView.LayoutParameters = marginParams;
                linearHdrInvestment.LayoutParameters = marginParams;
                linearHdrChart.LayoutParameters = marginParams;
                linearHdrStat.LayoutParameters = marginParams;
                linearHdrMore.LayoutParameters = marginParams;

                LinearLayout.LayoutParams marginSelParams = new LinearLayout.LayoutParams(0, FrameLayout.LayoutParams.MatchParent, 126);
                marginSelParams.SetMargins(0, 0, 0, 8);

                linearSelected.LayoutParameters = marginSelParams;

                if (linearSelected == linearHdrOverView)
                {
                    OverviewFragment overviewFrag = new OverviewFragment();
                    overviewFrag.ParentContext = this;
                    ShowFragment(overviewFrag);
                }
                else if (linearSelected == linearHdrInvestment)
                {
                    investmentFrag = new InvestmentFragment();
                    investmentFrag.ParentContext = this;
                    ShowFragment(investmentFrag);
                }
                else if (linearSelected == linearHdrChart)
                {
                    ChartFragment chartFrag = new ChartFragment();
                    chartFrag.ParentContext = this;
                    ShowFragment(chartFrag);
                }
                else if (linearSelected == linearHdrStat)
                {
                    StatFragment statFrag = new StatFragment();
                    statFrag.ParentContext = this;
                    ShowFragment(statFrag);
                }
            }
        }

        private void ShowMenu()
        {
            if (frmMenu.Visibility == ViewStates.Visible)
            {
                frmMenu.Visibility = ViewStates.Invisible;

                ValueAnimator animator = ValueAnimator.OfInt(0, linearMenu.Width);
                animator.SetDuration(500);
                animator.Start();

                animator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
                {
                    int newValue = (int)e.Animation.AnimatedValue;
                    // Apply this new value to the object being animated.
                    int left = frmMenu.Width - newValue;
                    linearMenu.Layout(left, linearMenu.Top, left + linearMenu.Width, linearMenu.Bottom);
                };
            }
            else
            {
                frmMenu.Visibility = ViewStates.Visible;

				FindViewById<LinearLayout> (Resource.Id.linearMenu).Visibility = ViewStates.Visible;
				FindViewById<LinearLayout> (Resource.Id.linearHelpLegalMenu).Visibility = ViewStates.Invisible;
            }
        }

        private void HideMenu()
        {
            frmMenu.Visibility = ViewStates.Invisible;
        }

        private void ShowFragment(Fragment newFragment)
        {
            for (int i = 0; i < FragmentManager.BackStackEntryCount; i++)
            {
                FragmentManager.PopBackStackImmediate();
            }

            // Create a new fragment and a transaction.
            FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();

            // Replace the fragment that is in the View fragment_container (if applicable).
            fragmentTx.Replace(Resource.Id.frameMain, newFragment);

            // Add the transaction to the back stack.
            fragmentTx.AddToBackStack(null);

            // Commit the transaction.
            fragmentTx.Commit();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == Util.ACTIVITY_NEW_INVESTMENT)
            {
                if (investmentFrag != null)
                    investmentFrag.InitializeList();
            }
        }

		public class SyncDataTask : AsyncTask
		{
			bool isSuccess = true;
			ProgressDialog p = null;
			public Context context { get; set; }
			List<Investment.Portable.TblInvestmentType> resultInvestTypeList { get; set; }
			List<Investment.Portable.TblStats> resultStatList { get; set; }
			List<Investment.Portable.TblStatsType> resultStatTypeList { get; set; }
			List<Investment.Portable.TblCountry> resultCountryList { get; set; }
			List<Investment.Portable.TblReference> resultReferenceList { get; set; }

			String serverUpdateTime = "";

			protected override void OnPreExecute()
			{
				p = new ProgressDialog(context);
				p.SetMessage("Loading...");
				p.SetCancelable (false);
				p.Show();
			}

			protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
			{
				isSuccess = false;
				WebService service = new WebService ();
				String lasteUpdateTime = Util.GetDataFromPreference (context, "sync_lastupdate", "");
				List<Investment.Portable.TblInvestmentType> resultInvest = service.GetInvestTypesFromServer(lasteUpdateTime);
				if (resultInvest == null)
					return "";
				resultInvestTypeList = resultInvest;
				List<Investment.Portable.TblStats> resultStat = service.GetStatsFromServer(lasteUpdateTime);
				if (resultStat == null)
					return "";
				resultStatList = resultStat;
				List<Investment.Portable.TblStatsType> resultStatType = service.GetStatTypesFromServer(lasteUpdateTime);
				if (resultStatType == null)
					return "";
				resultStatTypeList = resultStatType;
				List<Investment.Portable.TblCountry> resultCountry = service.GetCountriesFromServer(lasteUpdateTime);
				if (resultCountry == null)
					return "";
				resultCountryList = resultCountry;
				List<Investment.Portable.TblReference> resultReference = service.GetReferenceFromServer(lasteUpdateTime);
				if (resultReference == null)
					return "";
				resultReferenceList = resultReference;

				serverUpdateTime = service.GetLastUpdateTimeFromServer();

				isSuccess = true;
				return "";
			}

			protected override void OnPostExecute(Java.Lang.Object result)
			{
				if (p != null)
				{
					p.Dismiss();
					p = null;
				}

				if (isSuccess == false) {
					Toast.MakeText (context, "Sync Failure", ToastLength.Short).Show ();
					return;
				}

				if (resultInvestTypeList != null) {
					Util.GetDatabaseMgr ().SyncInvestTypes (resultInvestTypeList);
				}

				if (resultStatList != null) {
					Util.GetDatabaseMgr ().SyncStats (resultStatList);
				}

				if (resultStatTypeList != null) {
					Util.GetDatabaseMgr ().SyncStatTypes (resultStatTypeList);
				}

				if (resultCountryList != null) {
					Util.GetDatabaseMgr ().SyncCountries (resultCountryList);
				}

				if (resultReferenceList != null) {
					Util.GetDatabaseMgr ().SyncReference (resultReferenceList);
				}

				Util.SaveDataToPreference(context, "sync_lastupdate", serverUpdateTime);

				Toast.MakeText (context, "Sync Finished", ToastLength.Short).Show ();
			}
		}
	}
}


