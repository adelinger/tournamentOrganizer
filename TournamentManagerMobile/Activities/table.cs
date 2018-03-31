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
using Com.Startapp.Android.Publish;
using Com.Startapp.Android.Publish.Banner;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "table")]
    public class table : Activity
    {
        Banner banner;

        connection con = new connection();
        List<string> playersOnActiveTournament = new List<string>();
        public string tournamentName { get; set; }
        public string tournamentID { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartAppSDK.Init(this, "202635783", true);

            SetContentView(Resource.Layout.table);

            IBannerListener bannerListener = new AdListener();
            banner = FindViewById<Com.Startapp.Android.Publish.Banner.Banner>(Resource.Id.tableBanner);
            banner.ShowBanner();
            banner.SetBannerListener(bannerListener);

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