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
    [Activity(Label = "AddEntryActivity", Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", WindowSoftInputMode = Android.Views.SoftInput.AdjustResize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AddEntryActivity : Activity
    {
        #region CONSTANTS
        const int DATE_DIALOG_ID = 0;
        #endregion

        DateTime _dateStart, _dateEnd;
        bool isSelectedStart;

        LinearLayout linearMuchInvesting;
        LinearLayout linearMuchFuture;
        LinearLayout linearSelectCalendar;
        LinearLayout linearPeriod;
        LinearLayout linearRate;

        Spinner spinnerType;
        Spinner spinnerCalculate;
        Spinner spinnerCompounding;
        Spinner spinnerCalendar;

		List<TblInvestmentType> investTypeList;

        int calculateOption = 0;
        int compoundOption = 0;

		TblEntry curEntryItem;
		int curEntryItemId = -1;

		bool isPeriodic = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AddEntry);

            _dateStart = _dateEnd = DateTime.Today;

            ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
            imgBack.Click += imgBack_Click;

            ImageView imgCalculate = FindViewById<ImageView>(Resource.Id.imgCalculate);
            imgCalculate.Click += imgCalculate_Click;

            ImageView imgSave = FindViewById<ImageView>(Resource.Id.imgSave);
            imgSave.Click += imgSave_Click;

            ImageButton imgSelectStartDate = FindViewById<ImageButton>(Resource.Id.imgSelectStartDate);
            imgSelectStartDate.Click += imgSelectStartDate_Click;

            ImageButton imgSelectEndDate = FindViewById<ImageButton>(Resource.Id.imgSelectEndDate);
            imgSelectEndDate.Click += imgSelectEndDate_Click;

            linearMuchInvesting = FindViewById<LinearLayout>(Resource.Id.linearMuchInvesting);
            linearMuchFuture = FindViewById<LinearLayout>(Resource.Id.linearMuchFuture);
            linearSelectCalendar = FindViewById<LinearLayout>(Resource.Id.linearSelectCalendar);
            linearPeriod = FindViewById<LinearLayout>(Resource.Id.linearPeriod);
            linearRate = FindViewById<LinearLayout>(Resource.Id.linearRate);

            spinnerType = FindViewById<Spinner>(Resource.Id.spinnerInvestType);
            spinnerCalculate = FindViewById<Spinner>(Resource.Id.spinnerCalculate);
            spinnerCompounding = FindViewById<Spinner>(Resource.Id.spinnerCompounding);
            spinnerCalendar = FindViewById<Spinner>(Resource.Id.spinnerMonthYear);

            // Read Investment Types from table
            investTypeList = Util.GetDatabaseMgr().GetInvestmentTypes();
            String[] investTypesArr = new String[investTypeList.Count];
            for (int i = 0; i < investTypeList.Count; i++)
            {
                investTypesArr[i] = investTypeList[i].Name;
            }

            ArrayAdapter<String> adapter_type_group = new ArrayAdapter<String>(this, Resource.Layout.SpinnerText, investTypesArr);
            adapter_type_group.SetDropDownViewResource(Resource.Layout.SpinnerText);
            spinnerType.Adapter = adapter_type_group;

            spinnerType.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)
            {
				if (e.Position != -1)// && _selGroupId != e.Position)
				{
					FindViewById<TextView>(Resource.Id.lblMuchInvesting).Text = investTypeList[e.Position].Present;
					FindViewById<TextView>(Resource.Id.lblMuchFuture).Text = investTypeList[e.Position].Future;
					FindViewById<TextView>(Resource.Id.lblRate).Text = investTypeList[e.Position].Rate;
					FindViewById<TextView>(Resource.Id.lblGetDate).Text = investTypeList[e.Position].TimetoGet;
					FindViewById<TextView>(Resource.Id.lblDeposit).Text = investTypeList[e.Position].Periodic;

					if (String.IsNullOrEmpty(investTypeList[e.Position].Periodic) == false)
						isPeriodic = true;
					else
						isPeriodic = false;

					ChangeUILayout();
				}
			};

            ArrayAdapter<String> adapter_calculate_group = new ArrayAdapter<String>(this, Resource.Layout.SpinnerText, Util.CALCULATE_TYPES);
            adapter_calculate_group.SetDropDownViewResource(Resource.Layout.SpinnerText);
            spinnerCalculate.Adapter = adapter_calculate_group;

            spinnerCalculate.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                if (e.Position != -1)// && _selGroupId != e.Position)
                {
                    calculateOption = e.Position;
                    ChangeUILayout();
                }
            };

            ArrayAdapter<String> adapter_compounding_group = new ArrayAdapter<String>(this, Resource.Layout.SpinnerText, Util.COMPOUNDING_TYPES);
            adapter_compounding_group.SetDropDownViewResource(Resource.Layout.SpinnerText);
            spinnerCompounding.Adapter = adapter_compounding_group;

            spinnerCompounding.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                if (e.Position != -1)// && _selGroupId != e.Position)
                {
                    compoundOption = e.Position;
                    ChangeUILayout();
                }
            };

            ArrayAdapter<String> adapter_calendar_group = new ArrayAdapter<String>(this, Resource.Layout.SpinnerText, Util.CALENDAR_TYPES);
            adapter_calendar_group.SetDropDownViewResource(Resource.Layout.SpinnerText);
            spinnerCalendar.Adapter = adapter_calendar_group;

            spinnerCalendar.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                if (e.Position != -1)// && _selGroupId != e.Position)
                {
                }
            };

            linearMuchInvesting.Visibility = ViewStates.Invisible;
            linearMuchFuture.Visibility = ViewStates.Invisible;
            linearSelectCalendar.Visibility = ViewStates.Invisible;
            linearPeriod.Visibility = ViewStates.Invisible;

			curEntryItemId = -1;
			if (this.Intent != null) {
				int curId = this.Intent.GetIntExtra ("UID", -1);
				if (curId != -1) {
					TblEntry entryItem = Util.GetDatabaseMgr ().GetEntryWithId (curId);
					if (entryItem != null) {
						curEntryItemId = curId;
						curEntryItem = entryItem;

						try{
							spinnerType.SetSelection(GetIndexFromInvestTypeId(entryItem.InvestmentTypeID));
							spinnerCalculate.SetSelection(entryItem.CalculateType);
							spinnerCalendar.SetSelection(entryItem.CalendarType);
							spinnerCompounding.SetSelection(entryItem.CompoundingType);

							FindViewById<TextView>(Resource.Id.txtInvestName).Text = entryItem.EntryName;
							FindViewById<TextView>(Resource.Id.txtMuchInvesting).Text = "" + entryItem.InitialPayment;
							FindViewById<TextView>(Resource.Id.txtMuchFuture).Text = "" + entryItem.FuturePayment;
							FindViewById<TextView>(Resource.Id.txtRate).Text = "" + entryItem.Rate * 100;
							FindViewById<TextView>(Resource.Id.txtGetDate).Text = entryItem.TimeToGet;
							FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked = (entryItem.DepositFlag == 1);
							FindViewById<TextView>(Resource.Id.txtDeposit).Text = "" + entryItem.DepositPayment;
							FindViewById<TextView>(Resource.Id.txtStartDate).Text = IsValidDateTime(entryItem.StartTimeToGet, true) ? _dateStart.ToString("d") : "";
							FindViewById<TextView>(Resource.Id.txtEndDate).Text = IsValidDateTime(entryItem.EndTimeToGet, false) ? _dateEnd.ToString("d") : "";
						}
						catch (Exception ex) {
						}
					}
				}
			}
        }

		bool IsValidDateTime(String time, bool isStart)
		{
			bool isSuccess = true;
			try{
				if (isStart)
					_dateStart = DateTime.Parse(time);
				else
					_dateEnd = DateTime.Parse(time);
			}
			catch (Exception ex) {
				isSuccess = false;
			}

			return isSuccess;
		}

        void imgBack_Click(object sender, EventArgs e)
        {
            Finish();
        }
        
        void imgCalculate_Click(object sender, EventArgs e)
        {
            int compoundingTypeID = (int)spinnerCompounding.SelectedItemId;
            int calendarTypeID = (int)spinnerCalendar.SelectedItemId;
            float initialPayment = 0;
            float.TryParse(FindViewById<TextView>(Resource.Id.txtMuchInvesting).Text, out initialPayment);
            float futurePayment = 0;
            float.TryParse(FindViewById<TextView>(Resource.Id.txtMuchFuture).Text, out futurePayment);
            float fRate = 0;
            float.TryParse(FindViewById<TextView>(Resource.Id.txtRate).Text, out fRate);
			fRate /= 100.0f;
            int depositPayment = 0;
            int.TryParse(FindViewById<TextView>(Resource.Id.txtDeposit).Text, out depositPayment);
            double timetoget = 0;
            double.TryParse(FindViewById<TextView>(Resource.Id.txtGetDate).Text, out timetoget);
            if (calendarTypeID == 1)
                timetoget /= 12.0;
            
            if (timetoget == 0)
            {
				if (_dateEnd.Day == _dateStart.Day && _dateEnd.Month == _dateStart.Month)
					timetoget = _dateEnd.Year - _dateStart.Year;
				else {
					double betweenDays = (_dateEnd - _dateStart).TotalDays;
					timetoget = betweenDays / 30.0 / 12.0;
				}
            }

            double dResult = 0;
            if (compoundingTypeID > 0)
            {
                switch (calculateOption)
                {
                    case 1:
                        dResult = Util.PV(fRate / compoundingTypeID, timetoget * compoundingTypeID, depositPayment, futurePayment * -1, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked);
                        break;
                    case 2:
                        dResult = Util.FV(fRate / compoundingTypeID, timetoget * compoundingTypeID, depositPayment * -1, initialPayment * -1, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked);
                        break;
                    case 3:
                        dResult = Util.Nper(fRate / compoundingTypeID, depositPayment * -1, initialPayment * -1, futurePayment, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked) / compoundingTypeID;
                        break;
                    case 4:
                        dResult = Util.Rate(timetoget * compoundingTypeID, depositPayment * -1, initialPayment * -1, futurePayment, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked) * compoundingTypeID;
                        break;
                    case 5:
                        dResult = Util.Pmt(fRate / compoundingTypeID, timetoget * compoundingTypeID, initialPayment * -1, futurePayment, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked) * -1;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (calculateOption)
                {
                    case 1:
                        dResult = Util.CalculatePV(futurePayment, fRate, timetoget, compoundingTypeID);
                        break;
                    case 2:
                        dResult = Util.CalculateFV(initialPayment, fRate, timetoget, compoundingTypeID);
                        break;
                    case 3:
                        dResult = Util.CalculateTime(initialPayment, futurePayment, fRate, compoundingTypeID);
                        break;
                    case 4:
                        dResult = Util.CalculateRate(initialPayment, futurePayment, timetoget, compoundingTypeID);
                        break;
                    case 5:
                        Toast.MakeText(this, "Can't calculate result for this case", ToastLength.Short).Show();
                        return;
                    default:
                        break;
                }

                //Toast.MakeText(this, "Result = " + dResult.ToString(), ToastLength.Short).Show();
            }

            Intent calcResultIntent = new Intent(this, typeof(CalculateResultDialog));
            calcResultIntent.PutExtra("ResultFieldValue", dResult);
			float fGrowthRate = (float)Util.Effect (fRate, compoundingTypeID) * 100.0f;
			if (calculateOption == 4)
				fGrowthRate = (float)Util.Effect (dResult, compoundingTypeID) * 100.0f;

			fRate *= 100.0f;
            switch (calculateOption)
            {
                case 1:
                    calcResultIntent.PutExtra("ResultFieldName", "Initial Amount");
					calcResultIntent.PutExtra("ResultFieldValue", "$" + dResult.ToString("0.00"));
					calcResultIntent.PutExtra("GrowthRateValue", fGrowthRate.ToString("0.00") + "%");

                    calcResultIntent.PutExtra("InputField1Name", "Future Amount");
                    calcResultIntent.PutExtra("InputField1Value", "$" + futurePayment.ToString("0.00"));
                    calcResultIntent.PutExtra("InputField2Name", "Rate");
                    calcResultIntent.PutExtra("InputField2Value", fRate.ToString("0.00") + "%");
                    calcResultIntent.PutExtra("InputField3Name", "Duration");
					calcResultIntent.PutExtra("InputField3Value", timetoget.ToString("0.00") + " Years");
                    calcResultIntent.PutExtra("InputField4Name", "Compounded");
                    calcResultIntent.PutExtra("InputField4Value", Util.COMPOUNDING_TYPES[compoundingTypeID]);
                    break;
                case 2:
					calcResultIntent.PutExtra("ResultFieldName", "Future Amount");
					calcResultIntent.PutExtra("ResultFieldValue", "$" + dResult.ToString("0.00"));
					calcResultIntent.PutExtra("GrowthRateValue", fGrowthRate.ToString("0.00") + "%");

					calcResultIntent.PutExtra("InputField1Name", "Initial Amount");
					calcResultIntent.PutExtra("InputField1Value", "$" + initialPayment.ToString("0.00"));
					calcResultIntent.PutExtra("InputField2Name", "Rate");
					calcResultIntent.PutExtra("InputField2Value", fRate.ToString("0.00") + "%");
					calcResultIntent.PutExtra("InputField3Name", "Duration");
					calcResultIntent.PutExtra("InputField3Value", timetoget.ToString("0.00") + " Years");
					calcResultIntent.PutExtra("InputField4Name", "Compounded");
					calcResultIntent.PutExtra("InputField4Value", Util.COMPOUNDING_TYPES[compoundingTypeID]);
                    break;
                case 3:
					calcResultIntent.PutExtra("ResultFieldName", "Duration");
					calcResultIntent.PutExtra("ResultFieldValue", dResult.ToString("0.00"));
					calcResultIntent.PutExtra("GrowthRateValue", fGrowthRate.ToString("0.00") + "%");

					calcResultIntent.PutExtra("InputField1Name", "Initial Amount");
					calcResultIntent.PutExtra("InputField1Value", "$" + initialPayment.ToString("0.00"));
					calcResultIntent.PutExtra("InputField2Name", "Future Amount");
					calcResultIntent.PutExtra("InputField2Value", "$" + futurePayment.ToString("0.00"));
					calcResultIntent.PutExtra("InputField3Name", "Rate");
					calcResultIntent.PutExtra("InputField3Value", fRate.ToString("0.00") + "%");
					calcResultIntent.PutExtra("InputField4Name", "Compounded");
					calcResultIntent.PutExtra("InputField4Value", Util.COMPOUNDING_TYPES[compoundingTypeID]);
                    break;
                case 4:
					calcResultIntent.PutExtra("ResultFieldName", "Rate");
					calcResultIntent.PutExtra("ResultFieldValue", (dResult * 100.0f).ToString("0.00") + "%");
					calcResultIntent.PutExtra("GrowthRateValue", fGrowthRate.ToString("0.00") + "%");

					calcResultIntent.PutExtra("InputField1Name", "Initial Amount");
					calcResultIntent.PutExtra("InputField1Value", "$" + initialPayment.ToString("0.00"));
					calcResultIntent.PutExtra("InputField2Name", "Future Amount");
					calcResultIntent.PutExtra("InputField2Value", "$" + futurePayment.ToString("0.00"));
					calcResultIntent.PutExtra("InputField3Name", "Duration");
					calcResultIntent.PutExtra("InputField3Value", timetoget.ToString("0.00") + " Years");
					calcResultIntent.PutExtra("InputField4Name", "Compounded");
					calcResultIntent.PutExtra("InputField4Value", Util.COMPOUNDING_TYPES[compoundingTypeID]);
                    break;
                case 5:
					calcResultIntent.PutExtra("ResultFieldName", "Periodic Amount");
					calcResultIntent.PutExtra("ResultFieldValue", "$" + dResult.ToString("0.00"));
					calcResultIntent.PutExtra("GrowthRateValue", fGrowthRate.ToString("0.00") + "%");

					calcResultIntent.PutExtra("InputField1Name", "Initial Amount");
					calcResultIntent.PutExtra("InputField1Value", "$" + initialPayment.ToString("0.00"));
					calcResultIntent.PutExtra("InputField2Name", "Future Amount");
					calcResultIntent.PutExtra("InputField2Value", "$" + futurePayment.ToString("0.00"));
					calcResultIntent.PutExtra("InputField3Name", "Duration");
					calcResultIntent.PutExtra("InputField3Value", timetoget.ToString("0.00") + " Years");
					calcResultIntent.PutExtra("InputField4Name", "Compounded");
					calcResultIntent.PutExtra("InputField4Value", Util.COMPOUNDING_TYPES[compoundingTypeID]);
                    break;
            }
            
            StartActivity(calcResultIntent);
        }

        void imgSave_Click(object sender, EventArgs e)
        {
            try
            {
                int compoundingTypeID = (int)spinnerCompounding.SelectedItemId;
                int calendarTypeID = (int)spinnerCalendar.SelectedItemId;
                int investmentTypeID = (int)spinnerType.SelectedItemId;
                String entryName = FindViewById<TextView>(Resource.Id.txtInvestName).Text;
                int calculateTypeID = (int)spinnerCalculate.SelectedItemId;
                double initialPayment = 0;
                double.TryParse(FindViewById<TextView>(Resource.Id.txtMuchInvesting).Text, out initialPayment);
                double futurePayment = 0;
                double.TryParse(FindViewById<TextView>(Resource.Id.txtMuchFuture).Text, out futurePayment);
                double fRate = 0;
                double.TryParse(FindViewById<TextView>(Resource.Id.txtRate).Text, out fRate);
				fRate /= 100.0f;
                double timetoget = 0;
                double.TryParse(FindViewById<TextView>(Resource.Id.txtGetDate).Text, out timetoget);
                int depositFlag = (FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked == true) ? 1 : 0;
                double depositPayment = 0;
                double.TryParse(FindViewById<TextView>(Resource.Id.txtDeposit).Text, out depositPayment);
                if (calendarTypeID == 1)
                    timetoget /= 12.0;

                if (timetoget == 0)
                {
					if (_dateEnd.Day == _dateStart.Day && _dateEnd.Month == _dateStart.Month)
						timetoget = _dateEnd.Year - _dateStart.Year;
					else {
						double betweenDays = (_dateEnd - _dateStart).TotalDays;
						timetoget = betweenDays / 30.0 / 12.0;
					}
                }

                if (String.IsNullOrEmpty(entryName) == true)
                {
                    Toast.MakeText(this, "Please Input Name", ToastLength.Short).Show();
                    return;
                }

                if (calculateTypeID == 0)
                {
                    Toast.MakeText(this, "Please select Caclulate Type correctly", ToastLength.Short).Show();
                    return;
                }

                if (compoundingTypeID > 0)
                {
                    switch (calculateOption)
                    {
                        case 1:
                            initialPayment = Util.PV(fRate / compoundingTypeID, timetoget * compoundingTypeID, depositPayment, futurePayment * -1, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked);
                            break;
                        case 2:
                            futurePayment = Util.FV(fRate / compoundingTypeID, timetoget * compoundingTypeID, depositPayment * -1, initialPayment * -1, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked);
                            break;
                        case 3:
                            timetoget = Util.Nper(fRate / compoundingTypeID, depositPayment * -1, initialPayment * -1, futurePayment, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked) / compoundingTypeID;
                            break;
                        case 4:
                            fRate = Util.Rate(timetoget * compoundingTypeID, depositPayment * -1, initialPayment * -1, futurePayment, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked) * compoundingTypeID;
                            break;
                        case 5:
                            depositPayment = Util.Pmt(fRate / compoundingTypeID, timetoget * compoundingTypeID, initialPayment * -1, futurePayment, FindViewById<CheckBox>(Resource.Id.chkAtPeriod).Checked) * -1;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (calculateOption)
                    {
                        case 1:
                            initialPayment = Util.CalculatePV(futurePayment, fRate, timetoget, compoundingTypeID);
                            break;
                        case 2:
                            futurePayment = Util.CalculateFV(initialPayment, fRate, timetoget, compoundingTypeID);
                            break;
                        case 3:
                            timetoget = Util.CalculateTime(initialPayment, futurePayment, fRate, compoundingTypeID);
                            break;
                        case 4:
                            fRate = Util.CalculateRate(initialPayment, futurePayment, timetoget, compoundingTypeID);
                            break;
                        case 5:
                            Toast.MakeText(this, "This case is not valid", ToastLength.Short).Show();
                            return;
                        default:
                            break;
                    }
                }

				if (curEntryItemId == -1)
				{
					String startDate = FindViewById<TextView>(Resource.Id.txtStartDate).Text;
					String endDate = FindViewById<TextView>(Resource.Id.txtEndDate).Text;
					Util.GetDatabaseMgr().AddEntry(investTypeList[investmentTypeID].FieldID, entryName, calculateTypeID, compoundingTypeID, calendarTypeID, initialPayment, futurePayment, fRate, FindViewById<TextView>(Resource.Id.txtGetDate).Text, (FindViewById<TextView>(Resource.Id.txtStartDate)).Text, (FindViewById<TextView>(Resource.Id.txtEndDate)).Text, depositFlag, depositPayment);
				}
				else{
					curEntryItem.InvestmentTypeID = investTypeList[investmentTypeID].FieldID;
					curEntryItem.EntryName = entryName;
					curEntryItem.CalculateType = calculateTypeID;
					curEntryItem.CompoundingType = compoundingTypeID;
					curEntryItem.InitialPayment = (float)initialPayment;
					curEntryItem.FuturePayment = (float)futurePayment;
					curEntryItem.Rate = (float)fRate;
					curEntryItem.TimeToGet = FindViewById<TextView>(Resource.Id.txtGetDate).Text;
					curEntryItem.StartTimeToGet = (FindViewById<TextView>(Resource.Id.txtStartDate)).Text;
					curEntryItem.EndTimeToGet = (FindViewById<TextView>(Resource.Id.txtEndDate)).Text;
					curEntryItem.DepositFlag = depositFlag;
					curEntryItem.DepositPayment = (float)depositPayment;

					Util.GetDatabaseMgr().UpdateEntryWithId(curEntryItem);
				}

                Finish();
            }
            catch (Exception ex)
            {

            }
        }

        private void ChangeUILayout()
        {
            if (calculateOption == 0)
            {
                linearMuchInvesting.Visibility = ViewStates.Invisible;
                linearMuchFuture.Visibility = ViewStates.Invisible;
                linearSelectCalendar.Visibility = ViewStates.Invisible;
                linearRate.Visibility = ViewStates.Invisible;
                linearPeriod.Visibility = ViewStates.Invisible;

                return;
            }

            linearMuchInvesting.Visibility = ViewStates.Visible;
            linearMuchFuture.Visibility = ViewStates.Visible;
            linearSelectCalendar.Visibility = ViewStates.Visible;
            linearRate.Visibility = ViewStates.Visible;
            linearPeriod.Visibility = ViewStates.Visible;

            switch (calculateOption)
            {
                case 1: // Initial payment
                    linearMuchInvesting.Visibility = ViewStates.Gone;
                    break;
                case 2: // Future payment
                    linearMuchFuture.Visibility = ViewStates.Gone;
                    break;
                case 3: // Duration
                    linearSelectCalendar.Visibility = ViewStates.Gone;
                    break;
                case 4: // Rate
                    linearRate.Visibility = ViewStates.Gone;
                    break;
                case 5: // Periodic Amount
                    linearPeriod.Visibility = ViewStates.Invisible;
                    break;
                default:
                    break;
            }

            if (calculateOption != 5)
            {
                if (compoundOption > 0)
                    linearPeriod.Visibility = ViewStates.Visible;
                else
                    linearPeriod.Visibility = ViewStates.Invisible;
            }

			if (isPeriodic == false)
				linearPeriod.Visibility = ViewStates.Invisible;
        }

        void imgSelectStartDate_Click(object sender, EventArgs e)
        {
            isSelectedStart = true;
            ShowDialog(0);
        }

        void imgSelectEndDate_Click(object sender, EventArgs e)
        {
            isSelectedStart = false;
            ShowDialog(0);
        }

        #region DATE DIALOG
        protected override Dialog OnCreateDialog(int id)
        {
            if (id != DATE_DIALOG_ID)
            {
                return null;
            }

            DateTime _date = _dateStart;
            if (isSelectedStart == false)
                _date = _dateEnd;

            return new DatePickerDialog(this, DatePickerCallback, _date.Year,
                _date.Month - 1, _date.Day);
        }

        private void DatePickerCallback(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            String dateStr = "";
            dateStr = e.Date.ToString("d");

            if (isSelectedStart == true)
            {
                _dateStart = e.Date;
                (FindViewById<TextView>(Resource.Id.txtStartDate)).Text = dateStr;
            }
            else
            {
                _dateEnd = e.Date;
                (FindViewById<TextView>(Resource.Id.txtEndDate)).Text = dateStr;
            }
        }
        #endregion

		private int GetIndexFromInvestTypeId(String id)
		{
			int retid = -1;
			for (int i = 0; i < investTypeList.Count; i++) {
				if (investTypeList [i].FieldID.Equals (id)) {
					retid = i;
					break;
				}
			}

			return retid;
		}
    }
}
