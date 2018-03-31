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

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "About")]
    public class settings : Activity
    {
        StartAppAd startAppAd;
        Banner banner;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            StartAppSDK.Init(this, "202635783", true);

            SetContentView(Resource.Layout.settings);

            IBannerListener bannerListener = new AdListener();
            banner = FindViewById<Com.Startapp.Android.Publish.Banner.Banner>(Resource.Id.settingsBanner);
            banner.ShowBanner();
            banner.SetBannerListener(bannerListener);


            Button playersDB = FindViewById<Button>(Resource.Id.playersBaseButton);
            Button topScorers = FindViewById<Button>(Resource.Id.topScorers);

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
        }
    }
}