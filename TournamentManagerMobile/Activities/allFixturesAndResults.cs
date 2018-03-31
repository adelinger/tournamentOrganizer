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
using TournamentManagerMobile.Resources;
using TournamentManagerMobile.Resources.MyClasses;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "All fixtures and results")]
    public class allFixturesAndResults : Activity
    {
        Banner banner;
        connection con = new connection();
        List<string> players1 = new List<string>();
        List<string> players2 = new List<string>();
        List<string> allPlayers = new List<string>();
        List<string> fixturesAndResult = new List<string>();
        List<string> results = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartAppSDK.Init(this, "202635783", true);

            SetContentView(Resource.Layout.allFixturesAndResults);

            IBannerListener bannerListener = new AdListener();
            banner = FindViewById<Com.Startapp.Android.Publish.Banner.Banner>(Resource.Id.fixturesBanner);
            banner.ShowBanner();
            banner.SetBannerListener(bannerListener);

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