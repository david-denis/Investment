using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;

namespace Investment
{
    [Activity(Label = "Investilator", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Splash);

            MyHandler handler = new MyHandler(this);
            handler.SendEmptyMessageDelayed(0, 3000);
		}

        public class MyHandler : Handler
        {
            Activity _activity;
            public MyHandler(Activity curActivity)
            {
                _activity = curActivity;
            }

            //Continually sends and recieves messages to cycle through the colors
            public override void HandleMessage(Message msg)
            {
                _activity.StartActivity(new Intent(_activity, typeof(MainActivity)));
                _activity.Finish();
            }
        }
	}
}


