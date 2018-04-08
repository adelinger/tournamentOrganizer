using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using System.IO;
using TournamentManagerMobile.Activities;
using Android.Views;
using Com.Startapp.Android.Publish;
using Com.Startapp.Android.Publish.Banner;

namespace TournamentManagerMobile
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Tournament Organizer", MainLauncher = true)]
    public class MainActivity : Activity
    {
        StartAppAd startAppAd;
        Banner banner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
         
            StartAppSDK.Init(this, "202635783", true);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            IBannerListener bannerListener = new AdListener();
            banner = FindViewById<Com.Startapp.Android.Publish.Banner.Banner>(Resource.Id.Banner);
            banner.ShowBanner();
            banner.SetBannerListener(bannerListener);

            Button createNewTournamentButton = FindViewById<Button>(Resource.Id.createTournamentButton);
            createNewTournamentButton.Click += createNewTournamentButton_click;

            Button myTournaments = FindViewById<Button>(Resource.Id.myTournamentsButton);
            Button allWinners    = FindViewById<Button>(Resource.Id.allTimeWinnersButton);
            Button about         = FindViewById<Button>(Resource.Id.aboutButton);

           

        void createNewTournamentButton_click(object sender, EventArgs e)
            {
                Intent startNewTournament = new Intent(this, typeof(createNewTournament));
                this.StartActivity(startNewTournament);
            }

            myTournaments.Click += delegate
            {
                Intent startMyTournament = new Intent(this, typeof(myTournaments));
                this.StartActivity(startMyTournament);
            };

            allWinners.Click += delegate
              {
                  Intent intent = new Intent(this, typeof(winnersAllTimeActivity));
                  this.StartActivity(intent);
              };
            about.Click += delegate
            {
                Intent intent = new Intent(this, typeof(settings));
                StartActivity(intent);
            };
            

        }
    }

   internal class AdListener : Java.Lang.Object, IBannerListener
    {

        public void OnClick (View p0)
        {

        }

        public void OnFailedToReceiveAd(View p0)
        {
            
        }
        public void OnReceiveAd(View p0)
        {
           
        }
    }
}

