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
using Com.Startapp.Android.Publish;
using Com.Startapp.Android.Publish.Banner;
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

            Banner banner;
            StartAppSDK.Init(this, "202635783", true);
           
            SetContentView(Resource.Layout.topScorer);         
            IBannerListener bannerListener = new AdListener();
            banner = FindViewById<Com.Startapp.Android.Publish.Banner.Banner>(Resource.Id.TopScorersBanner);
            banner.ShowBanner();
            banner.SetBannerListener(bannerListener);
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