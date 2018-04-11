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
using TournamentManagerMobile.Resources.MyClasses;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "winnersAllTimeActivity")]
    public class winnersAllTimeActivity : Activity
    {
       
        connection con = new connection();       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
    
            SetContentView(Resource.Layout.winnersAllTime);

            var id = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            var adView = FindViewById<AdView>(Resource.Id.adViewWAT);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);


            this.Title = "All time winners";
          

            try
            {
                ListView playersList = FindViewById<ListView>(Resource.Id.listViewAllTime);

                List<winners> getWinners = con.db.Query<winners>("SELECT * FROM winners");


                winnersAllTimeLV adapter = new winnersAllTimeLV(this, getWinners);
                playersList.Adapter = adapter;

                playersList.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
                {
                    string playerName = getWinners[e.Position].personName;
                    string tournamentName = getWinners[e.Position].tournamentName;
                    int tournamentID = 0;
                    List<tournament> getID = con.db.Query<tournament>("SELECT * FROM tournament where name = '"+tournamentName+"' ");
                    foreach (var item in getID)
                    {
                        tournamentID = item.id;
                    }

                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    dialog.SetTitle("Choose one");
                    dialog.SetMessage("What do you want to see?");
                    dialog.SetNeutralButton("Cancel", (senderAlert, args) =>
                    {
                        dialog.Dispose();
                    });
                    dialog.SetPositiveButton(tournamentName, (senderAlert, args) =>
                    {
                        Intent intent = new Intent(this, typeof(leagueType));
                        intent.PutExtra("tournamentName", tournamentName);
                        StartActivity(intent);

                    });
                    dialog.SetNegativeButton(playerName, (senderAlert, args) =>
                    {
                        Intent intent = new Intent(this, typeof(stats));

                        intent.PutExtra("playerName", playerName);
                        intent.PutExtra("tournamentName", tournamentName);
                        intent.PutExtra("tournamentID", tournamentID.ToString());
                        StartActivity(intent);
                    });
                    Dialog alertDialog = dialog.Create();
                    alertDialog.Show();

                    
                   
                };
            }
            catch (Exception)
            {

                Toast.MakeText(this, "There is nothing here yet", ToastLength.Short).Show();
            }


           
        }

     
    }
}