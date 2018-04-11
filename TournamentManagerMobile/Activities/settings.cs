using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "About")]
    public class settings : Activity
    {
      
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);


            var id = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            var adView = FindViewById<AdView>(Resource.Id.adViewS);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            Button playersDB  = FindViewById<Button>(Resource.Id.playersBaseButton);
            Button topScorers = FindViewById<Button>(Resource.Id.topScorers);
            Button clubButton = FindViewById<Button>(Resource.Id.clubButton);

            playersDB.Click += delegate
            {
                Intent intent = new Intent(this, typeof(allPlayersActivity));
                StartActivity(intent);               
            };

            topScorers.Click += delegate
            {
                Intent intent = new Intent(this, typeof(topScorerActivity));
                StartActivity(intent);
            };
            clubButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(clubs));
                StartActivity(intent);
            };
        }
    }
}