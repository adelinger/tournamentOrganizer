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
using TournamentManagerMobile.Resources;
using TournamentManagerMobile.Resources.MyClasses;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "All fixtures and results")]
    public class allFixturesAndResults : Activity
    {
       
        connection con = new connection();
        List<string> players1 = new List<string>();
        List<string> players2 = new List<string>();
        List<string> allPlayers = new List<string>();
        List<string> fixturesAndResult = new List<string>();
        List<string> results = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.allFixturesAndResults);

            var id = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            var adView = FindViewById<AdView>(Resource.Id.adViewAFAR);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            ListView fixtAndResList = FindViewById<ListView>(Resource.Id.fixturesList);

            string oddEven      = Intent.GetStringExtra("oddEven");
            string tournamentID = Intent.GetStringExtra("tournamentID");
            string numOfRounds  = Intent.GetStringExtra("numOfRounds");

            List<person> getAllPlayers = con.db.Query<person>("SELECT people.id, people.name, people.club, peopleTournament.tournamentID, peopleTournament.personID " +
                "FROM people LEFT JOIN peopleTournament ON people.id = peopleTournament.personID " +
                "WHERE peopleTournament.tournamentID =  " + tournamentID+ " OR people.tournamentID = " + tournamentID + " ");
            foreach (var item in getAllPlayers)
            {
                allPlayers.Add(item.name);
            }

            List<results> getResults = con.db.Query<results>("SELECT * FROM results WHERE tournamentID = '" + tournamentID + "' ");
            foreach (var item in getResults)
            {
                results.Add(item.homeGoals + " : " + item.awayGoals);
            }

            leagueType league = new leagueType();
            if (oddEven == "even")
            {
                league.makeFixturesEven(allPlayers, players1, players2);
            }
            if (oddEven == "odd")
            {
                league.makeFixturesOdd(allPlayers, players1, players2);
            }

            if (numOfRounds == "one")
            {
                if (oddEven == "even")
                {
                    makeListEven(allPlayers, fixturesAndResult, players1, players2,0);
                }

                if (oddEven == "odd")
                {
                    makeListOdd(allPlayers, fixturesAndResult, players1, players2,0);
                }
            }

            if (numOfRounds == "two")
            {
                if (oddEven == "even")
                {
                    makeListEven(allPlayers, fixturesAndResult, players1, players2, 1);
                }
                if (oddEven == "odd")
                {
                    makeListOdd(allPlayers, fixturesAndResult, players1, players2, 1);
                }
            }
           
            ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, fixturesAndResult);
            fixtAndResList.Adapter = adapter;

            fixtAndResList.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
            {
                List<results> getPlayerNames = con.db.Query<results>("SELECT * FROM results WHERE tournamentID = '" + tournamentID + "' ");
                try
                {
                    string player1Name = getPlayerNames[e.Position].homePlayerName;
                    string player2Name = getPlayerNames[e.Position].awayPlayerName;
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    dialog.SetTitle("Choose one");
                    dialog.SetMessage("What do you want to see?");
                    dialog.SetNeutralButton("Cancel", (senderAlert, args) =>
                    {
                        dialog.Dispose();
                    });
                    dialog.SetPositiveButton(player2Name, (senderAlert, args) =>
                    {
                        Intent intent = new Intent(this, typeof(stats));

                        intent.PutExtra("playerName", player2Name);
                        intent.PutExtra("tournamentID", tournamentID.ToString());
                        StartActivity(intent);

                    });
                    dialog.SetNegativeButton(player1Name, (senderAlert, args) =>
                    {
                        Intent intent = new Intent(this, typeof(stats));

                        intent.PutExtra("playerName", player1Name);
                        intent.PutExtra("tournamentID", tournamentID.ToString());
                        StartActivity(intent);
                    });
                    Dialog alertDialog = dialog.Create();
                    alertDialog.Show();
                }
                catch (Exception)
                {
                    
                }
               
            };
            
        }

        public void makeListEven (List<string> allPlayers, List<string>fixturesAndResult, List<string>players1, List<string>players2, int oneOrTwo)
        {
            for (int i = 0; i < ((allPlayers.Count / 2) * (allPlayers.Count - 1) + oneOrTwo); i++)
            {
                if (i < (allPlayers.Count / 2) * (allPlayers.Count - 1))
                {
                    try
                    {
                        fixturesAndResult.Add(players1[i] + " vs " + players2[i] + " " + results[i]);
                    }
                    catch (Exception)
                    {

                        fixturesAndResult.Add(players1[i] + " vs " + players2[i]);
                    }
                }
                else
                {
                    for (int j = 0; j < (allPlayers.Count / 2) * (allPlayers.Count - 1); j++)
                    {
                        try
                        {
                            fixturesAndResult.Add(players2[j] + " vs " + players1[j] + " " + results[i+j]);
                        }
                        catch (Exception)
                        {

                            fixturesAndResult.Add(players2[j] + " vs " + players1[j]);
                        }
                    }
                }
               

            }
        }

        public void makeListOdd(List<string> allPlayers, List<string> fixturesAndResult, List<string> players1, List<string> players2, int oneOrTwo)
        {
            for (int i = 0; i < ((allPlayers.Count) * ((allPlayers.Count - 1) / 2) + oneOrTwo); i++)
            {
                if (i < (allPlayers.Count) * ((allPlayers.Count - 1) / 2))
                {
                    try
                    {
                        fixturesAndResult.Add(players1[i] + " vs " + players2[i] + " " + results[i]);
                    }
                    catch (Exception)
                    {
                        fixturesAndResult.Add(players1[i] + " vs " + players2[i]);
                    }
                }
                    
            else
                {
                    for (int j = 0; j < (allPlayers.Count) * ((allPlayers.Count - 1) / 2); j++)
                    {
                        try
                        {
                            fixturesAndResult.Add(players2[j] + " vs " + players1[j] + " " + results[i + j]);
                        }
                        catch (Exception)
                        {
                            fixturesAndResult.Add(players2[j] + " vs " + players1[j]);
                        }
                    }
                   
                }
               
            }
        }
    }
}