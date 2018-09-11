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

namespace TournamentManagerMobile.Activities
{
    [Activity(Label = "knockout4Players")]
    public class knockout4Players : Activity
    {
        private Button incrementOne;
        private Button incrementTwo;
        private Button decrementOne;
        private Button decrementTwo;
        private Button chooseMatch;
        private Button applyResult;
        private Button knockOutDraw;
        private Button goalScorers;

        private TextView playerOne;
        private TextView playerTwo;

        private EditText resultOne;
        private EditText resultTwo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.knockout4Players);

            incrementOne = FindViewById<Button>(Resource.Id.plusIncrementKnockout);
            incrementTwo = FindViewById<Button>(Resource.Id.plusIncrementP2Knockout);
            decrementOne = FindViewById<Button>(Resource.Id.minusIncrementKnockout);
            decrementTwo = FindViewById<Button>(Resource.Id.minusIncrementP2Knockout);
            chooseMatch = FindViewById<Button>(Resource.Id.nextMatchKnockout);
            applyResult = FindViewById<Button>(Resource.Id.applyButtonKnockout);

        }
    }
}