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
    [Activity(Theme = "@style/NoTitleDialog")]
    public class CalculateResultDialog : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CalculateResultDialog);

            ImageView imgClose = FindViewById<ImageView>(Resource.Id.imgClose);
            imgClose.Click += imgClose_Click;

            if (Intent != null)
            {
                String resultFieldName = Intent.GetStringExtra("ResultFieldName");
                String resultFieldValue = Intent.GetStringExtra("ResultFieldValue");

                String resultGrowthRateValue = Intent.GetStringExtra("GrowthRateValue");
                
                String inputField1Name = Intent.GetStringExtra("InputField1Name");
                String inputField1Value = Intent.GetStringExtra("InputField1Value");
                String inputField2Name = Intent.GetStringExtra("InputField2Name");
                String inputField2Value = Intent.GetStringExtra("InputField2Value");
                String inputField3Name = Intent.GetStringExtra("InputField3Name");
                String inputField3Value = Intent.GetStringExtra("InputField3Value");
                String inputField4Name = Intent.GetStringExtra("InputField4Name");
                String inputField4Value = Intent.GetStringExtra("InputField4Value");

                FindViewById<TextView>(Resource.Id.lblResultLabel).Text = resultFieldName;
                FindViewById<TextView>(Resource.Id.lblResult).Text = resultFieldValue;

                //FindViewById<TextView>(Resource.Id.lblGrowthRateLabel).Text = "";
                FindViewById<TextView>(Resource.Id.lblGrowthRate).Text = resultGrowthRateValue;

                FindViewById<TextView>(Resource.Id.lblInput1Label).Text = inputField1Name;
                FindViewById<TextView>(Resource.Id.lblInput1).Text = inputField1Value;
                FindViewById<TextView>(Resource.Id.lblInput2Label).Text = inputField2Name;
                FindViewById<TextView>(Resource.Id.lblInput2).Text = inputField2Value;
                FindViewById<TextView>(Resource.Id.lblInput3Label).Text = inputField3Name;
                FindViewById<TextView>(Resource.Id.lblInput3).Text = inputField3Value;
                FindViewById<TextView>(Resource.Id.lblInput4Label).Text = inputField4Name;
                FindViewById<TextView>(Resource.Id.lblInput4).Text = inputField4Value;
            }
        }

        void imgClose_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}