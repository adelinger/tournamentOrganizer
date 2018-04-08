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
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Player Info")]
    public class stats : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.stats);
            string playerName = Intent.GetStringExtra("playerName");
            string tIDString  = Intent.GetStringExtra("tournamentID");
            int tournamentID  = Convert.ToInt32(tIDString);

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            slidingTabsFragments fragment = new slidingTabsFragments(playerName, tournamentID);

            transaction.Replace(Resource.Id.sample_content_fragment, fragment);
            transaction.Commit();

        }
        
        
    }
}