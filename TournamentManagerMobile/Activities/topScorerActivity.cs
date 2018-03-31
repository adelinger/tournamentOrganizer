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
using TournamentManagerMobile.Resources.MyClasses;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Top scorers")]
    public class topScorerActivity : Activity
    {
        connection con = new connection();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.topScorer);

            try
            {
                ListView topScorersLV = FindViewById<ListView>(Resource.Id.topScorersLV);

                List<scorers> getScorers = con.db.Query<scorers>("SELECT * FROM scorers ORDER BY goals DESC");

                topScorersLVAdapter adapter = new topScorersLVAdapter(this, getScorers);
                topScorersLV.Adapter = adapter;
            }
            catch (Exception)
            {

                Toast.MakeText(this, "There is nothing here yet", ToastLength.Short).Show();
            }

        }
    }
}