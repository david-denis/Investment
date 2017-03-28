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
using Android.Webkit;

namespace Investment
{
    [Activity(Label = "AboutUsActivity", Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AboutUsActivity : Activity
    {
        //string aboutString = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. <br/> <br/> Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. <br/><br/>Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.<br/><br/>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.<br/><br/>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. <br/><br/>Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.<br/><br/> Excepteur. <br/><br/>sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AboutUs);

			String text = "";
			if (this.Intent != null) {
				text = this.Intent.GetStringExtra ("text");
			}

            //TextView txtMainView = FindViewById<TextView>(Resource.Id.txtAboutMain);
            //txtMainView.Text = Html.FromHtml(aboutString).ToString();
			//txtMainView.Text = Html.FromHtml (text).ToString ();

			WebView webview = FindViewById<WebView> (Resource.Id.webView);
			webview.LoadData (text, "text/html", "utf-8");

            ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
            imgBack.Click += imgBack_Click;

            //txtMainView.MovementMethod = new ScrollingMovementMethod();
        }

        void imgBack_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}