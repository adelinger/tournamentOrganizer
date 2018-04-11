using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Widget;

using TournamentManagerMobile.Resources.MyClasses;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Winners")]    
    public class winnersActivity : Activity       
    {
     
        connection con = new connection();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState); 

            SetContentView(Resource.Layout.winners);

            var id = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            var adView = FindViewById<AdView>(Resource.Id.adViewW);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            ListView playersList = FindViewById<ListView>(Resource.Id.listOfWinners);
           
            string tournamentName  = Intent.GetStringExtra("tournamentName");

            try
            {
                List<winners> winners = con.db.Query<winners>("SELECT * FROM winners WHERE tournamentName = '"+tournamentName+"' ");

                winnersLV adapter = new winnersLV(this, winners);
                playersList.Adapter = adapter;

                playersList.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
                {
                    int tournamentID = 0;
                    string playerName = winners[e.Position].personName;
                    List<tournament> getID = con.db.Query<tournament>("SELECT * FROM tournament WHERE name = '" + winners[e.Position].tournamentName + "' ");
                    foreach (var item in getID)
                    {
                        tournamentID = item.id;
                    }                     

                    Intent intent = new Intent(this, typeof(stats));
                    intent.PutExtra("playerName", playerName);
                    intent.PutExtra("tournamentID", tournamentID.ToString());
                    StartActivity(intent);
                };
            }
            catch (Exception)
            {
                Toast.MakeText(this, "This tournament has no winners yet", ToastLength.Short);
            }
          
            
            
        }
    }
}