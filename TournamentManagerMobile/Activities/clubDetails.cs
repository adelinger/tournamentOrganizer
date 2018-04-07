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
using TournamentManagerMobile.Resources;
using TournamentManagerMobile.Resources.MyClasses;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Club details")]
    public class clubDetails : Activity
    {
        connection con = new connection();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.clubDetails);

            TextView numOfPlayerWithClub = FindViewById<TextView>(Resource.Id.numOfPeoplePlayed);
            TextView wonWithClub         = FindViewById<TextView>(Resource.Id.wonByThisClub);
            TextView club                = FindViewById<TextView>(Resource.Id.clubTextView);
            ListView clubsList           = FindViewById<ListView>(Resource.Id.wonByThisClubList);

           string clubName =  Intent.GetStringExtra("clubName");
           club.Text = clubName;

            List<string> peopleWithRequestedClub = new List<string>();

            List<person> getClubs = con.db.Query<person>("SELECT * FROM people WHERE club = '" + clubName + "' ");
            foreach (var item in getClubs)
            {
                peopleWithRequestedClub.Add(item.name);
            }

            numOfPlayerWithClub.Text = peopleWithRequestedClub.Count.ToString();

            List<string> wonWithClubList = new List<string>();

            List<winners> getTournamentsWon = con.db.Query<winners>("SELECT * FROM winners WHERE clubName = '" + clubName + "' ");
            foreach (var item in getTournamentsWon)
            {
                wonWithClubList.Add(item.tournamentName);
            }
            wonWithClub.Text =  "(" + wonWithClubList.Count.ToString() + ")";

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, wonWithClubList);
            clubsList.Adapter = adapter;

           
            clubsList.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
            {
                List<winners> getWon = con.db.Query<winners>("SELECT * FROM winners WHERE clubName = '" + clubName + "' ");
                string tournamentName = getWon[e.Position].tournamentName;
                string tournamentType = "";
                List<tournament> getType = con.db.Query<tournament>("SELECT * FROM tournament WHERE name = '" + tournamentName + "' ");
                foreach (var item in getType)
                {
                    tournamentType = item.type;
                }

                if (tournamentType == "League")
                {
                    Intent intent = new Intent(this, typeof(leagueType));
                    intent.PutExtra("tournamentName", tournamentName);
                    StartActivity(intent);
                }
            };
           
        }
    }
}