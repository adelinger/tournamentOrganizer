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
using TournamentManagerMobile.Resources;
using Android.Gms.Ads;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "table")]
    public class table : Activity
    {
     
        connection con = new connection();
        List<string> playersOnActiveTournament = new List<string>();
        public string tournamentName { get; set; }
        public string tournamentID { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.table);

            var id = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            var adView = FindViewById<AdView>(Resource.Id.adViewT);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            ListView tableListView = FindViewById<ListView>(Resource.Id.tableList);
            
            tournamentID   = Intent.GetStringExtra("tournamentID");
            tournamentName = Intent.GetStringExtra("tournamentName");

            this.Title = tournamentName + " " + "standings";

            List<points> getPlayersOnThisTournament = con.db.Query<points>("SELECT * FROM points WHERE tournamentID = '"+tournamentID+"' ORDER BY numOfPoints DESC, goalDiference DESC, awayGoals DESC ");

            tableListView.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
            {
                string playerName = getPlayersOnThisTournament[e.Position].playerName;

                Intent intent = new Intent(this,typeof(stats));
                intent.PutExtra("tournamentID", tournamentID);
                intent.PutExtra("playerName", playerName);
                intent.PutExtra("tournamentName", tournamentName);
                StartActivity(intent);
            };
                      
            try
            {
                MyListViewAdapter adapter = new MyListViewAdapter(this, getPlayersOnThisTournament);                  
                tableListView.Adapter = adapter;
            }
            catch (Exception)
            {
            }
            
        }

        
    }
}