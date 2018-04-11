using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using System.IO;
using TournamentManagerMobile.Activities;
using Android.Views;
using Android.Gms.Ads;

namespace TournamentManagerMobile
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Tournament Organizer", MainLauncher = true)]
    public class MainActivity : Activity
    {
   
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
         
           

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var id = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            var adView = FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

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
  
}

