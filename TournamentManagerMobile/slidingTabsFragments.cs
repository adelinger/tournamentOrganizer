using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SQLite;
using TournamentManagerMobile.Resources.MyClasses;
using System.IO;
using TournamentManagerMobile.Resources;
using TournamentManagerMobile.Activities;


namespace TournamentManagerMobile
{
    public class slidingTabsFragments : Fragment
    {     
        private slidingTabScrollView mSlidingTabScrollView;
        private ViewPager mViewPager;
        public string player       { get; set; }
        public int    tournamentID { get; set; }

        public slidingTabsFragments(string Player, int TournamentID)
        {
            player = Player;
            tournamentID = TournamentID;
        }
       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_sample, container, false);
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            mSlidingTabScrollView = View.FindViewById<slidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = View.FindViewById<ViewPager>(Resource.Id.viewpager);
            mViewPager.Adapter = new SamplePageAdapter(player, tournamentID, Activity);

            mSlidingTabScrollView.viewPager = mViewPager;
        }
        public class SamplePageAdapter : PagerAdapter
        {
            connection con = new connection();

            List<string> items = new List<string>();

            public string player    { get; set; }
            public int playerID     { get; set; }
            public int tournamentID { get; set; }
            private Activity customActivity { get; set; }

            public SamplePageAdapter(string Player, int TournamentID, Activity activity) : base()
            {
                player       = Player;
                tournamentID = TournamentID;
                items.Add("Details");
                items.Add("Statistics");
                customActivity = activity;
            }
            public override int Count
            {
                get { return items.Count; }
            }
            public override bool IsViewFromObject(View view, Java.Lang.Object obj)
            {
                return view == obj;
            }
            
            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {                
                string club = "";
                List<int> listOfTournaments = new List<int>();
                List<string> StringListOfTournaments = new List<string>();
                List<string> listTournamentsWon = new List<string>();

                if (position == 0)
                {
                    View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.pager_item, container, false);
                    container.AddView(view);
                    TextView playerTxt = view.FindViewById<TextView>(Resource.Id.currentPlayerName);
                    playerTxt.Text = player;

                    List<person> getID = con.db.Query<person>("SELECT * FROM people WHERE name = '" + player + "' ");
                    foreach (var item in getID)
                    {
                        playerID = item.id;
                    }

                    List<person> getClub1 = con.db.Query<person>("SELECT * FROM people WHERE name = '"+player+"' AND tournamentID = '"+tournamentID+"' ");

                    foreach (var item in getClub1)
                    {
                        club = item.club;
                    }
                    if (club == "")
                    {
                        List<personTournament> getClub2 = con.db.Query<personTournament>("SELECT * FROM peopleTournament WHERE PersonID = '" + playerID + "' AND tournamentID = '" + tournamentID + "' ");
                        foreach (var item in getClub2)
                        {
                            club = item.club;
                        }
                    }
                    TextView clubTxt = view.FindViewById<TextView>(Resource.Id.currentClub);
                    clubTxt.Text = club;

                    clubTxt.Click += delegate 
                    {
                        Intent intent = new Intent(view.Context, typeof(clubDetails));

                        intent.PutExtra("clubName", clubTxt.Text);
                        customActivity.StartActivity(intent);
                    };

                    ListView tournamentsTxt = view.FindViewById<ListView>(Resource.Id.listOfTournaments);
                    List<person> getTourn1 = con.db.Query<person>("SELECT * FROM people WHERE name = '" + player + "' ");
                    foreach (var item in getTourn1)
                    {
                        listOfTournaments.Add(item.tournamentID);
                    }
                    List<personTournament> getTourn2 = con.db.Query<personTournament>("SELECT * FROM peopleTournament WHERE personID = '"+playerID+"' ");
                    foreach (var item in getTourn2)
                    {
                        listOfTournaments.Add(item.tournamentID);
                    }
                    string tournamentType = "";
                    foreach (var item in listOfTournaments)
                    {
                       
                        List<tournament> getTournaments = con.db.Query<tournament>("SELECT * FROM tournament WHERE id = '" + item + "' ");
                        foreach (var item2 in getTournaments)
                        {
                            StringListOfTournaments.Add(item2.name);
                            tournamentType = item2.type;
                        }                   
                    }

                    tournamentsTxt.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
                    {
                        if (tournamentType == "League")
                        {
                            Intent intent = new Intent(view.Context, typeof(leagueType));
                            intent.PutExtra("tournamentName", StringListOfTournaments[0]);
                            customActivity.StartActivity(intent);
                        }

                    };


                    ArrayAdapter<string> adapter = new ArrayAdapter<string>(view.Context, Android.Resource.Layout.SimpleListItem1, StringListOfTournaments);
                    tournamentsTxt.Adapter = adapter;

                    ListView tournamentsWonTxt = view.FindViewById<ListView>(Resource.Id.listOfTournamentsWon);
                    string type = "";
                    string name = "";
                    string clubName = "";
                    List<winners> getWon = con.db.Query<winners>("SELECT * FROM winners where PersonName = '" + player + "' ");
                    foreach (var item in getWon)
                    {
                        listTournamentsWon.Add(item.tournamentName + " - " + item.clubName);
                        name = item.tournamentName;
                        clubName = item.clubName;
                    }
                    List<tournament> getType = con.db.Query<tournament>("SELECT * FROM tournament where name = '" + name + "' ");
                    foreach (var item in getType)
                    {
                        type = item.type;
                    }
                    if (!listTournamentsWon.Any())
                    {
                        listTournamentsWon.Add("This player has not won any tournaments yet");
                    }

                    tournamentsWonTxt.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
                    {
                        if (type == "League")
                        {
                            AlertDialog.Builder dialog = new AlertDialog.Builder(view.Context);
                            dialog.SetTitle("Choose one");
                            dialog.SetMessage("What do you want to see?");
                            dialog.SetNeutralButton("Cancel", (senderAlert, args) =>
                            {
                                dialog.Dispose();
                            });
                            dialog.SetPositiveButton(name, (senderAlert, args) =>
                            {
                                Intent intent = new Intent(view.Context, typeof(leagueType));
                                intent.PutExtra("tournamentName", name);
                                customActivity.StartActivity(intent);

                            });
                            dialog.SetNegativeButton(clubName, (senderAlert, args) =>
                            {
                                Intent intent = new Intent(view.Context, typeof(clubDetails));

                                intent.PutExtra("clubName", clubName);
                                customActivity.StartActivity(intent);
                            });
                            Dialog alertDialog = dialog.Create();
                            alertDialog.Show();
                          
                        }
                    };

                    ArrayAdapter<string> adapter2 = new ArrayAdapter<string>(view.Context, Android.Resource.Layout.SimpleExpandableListItem1, listTournamentsWon);
                    tournamentsWonTxt.Adapter = adapter2;

                        return view;
                }
                if (position == 1)
                {
                    List<string> stats = new List<string>();

                    View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.pager_item2, container, false);
                    container.AddView(view);
                    Spinner spinner           = view.FindViewById<Spinner>(Resource.Id.statSpinner);
                    TextView numOfGames       = view.FindViewById<TextView>(Resource.Id.gamesPlayedStat);
                    TextView numOfPoints      = view.FindViewById<TextView>(Resource.Id.numOfPointsStat);
                    TextView gamesWon         = view.FindViewById<TextView>(Resource.Id.gamesWonStat);
                    TextView gamesDraw        = view.FindViewById<TextView>(Resource.Id.gamesDrawStat);
                    TextView gamesLost        = view.FindViewById<TextView>(Resource.Id.gamesLostStat);
                    TextView goalsScored      = view.FindViewById<TextView>(Resource.Id.goalsScoredStat);
                    TextView goalsAgainst     = view.FindViewById<TextView>(Resource.Id.goalsAgainstStat);
                    TextView goalDiference    = view.FindViewById<TextView>(Resource.Id.goalDiferenceStat);
                    TextView homeGoals        = view.FindViewById<TextView>(Resource.Id.homeGoalsStat);
                    TextView awayGaols        = view.FindViewById<TextView>(Resource.Id.awayGoalsStat);
                    TextView goalsPerMatch    = view.FindViewById<TextView>(Resource.Id.goalsPerMatchStat);
                    TextView receivedPerMatch = view.FindViewById<TextView>(Resource.Id.goalsReceivedPerMatchStat);
                    Button refreshButton      = view.FindViewById<Button>(Resource.Id.refreshButton);

                        try
                        {
                            List<points> getStats = con.db.Query<points>("SELECT * FROM points WHERE playerName = '" + player + "' AND tournamentID = '" + tournamentID + "' ");
                            foreach (var item in getStats)
                            {
                                double scored            = Convert.ToDouble(item.goalsScored);
                                double received          = Convert.ToDouble(item.goalsReceived);
                                double played            = Convert.ToDouble(item.gamesPlayed);
                                double scoredPerMatch    = scored / played;
                                double goalsdRecPerMatch = received / played;
                                numOfGames.Text          = item.gamesPlayed.ToString();
                                numOfPoints.Text         = item.numOfPoints.ToString();
                                gamesWon.Text            = item.won.ToString();
                                gamesLost.Text           = item.lost.ToString();
                                gamesDraw.Text           = item.draw.ToString();
                                goalsScored.Text         = item.goalsScored.ToString();
                                goalsAgainst.Text        = item.goalsReceived.ToString();
                                goalDiference.Text       = item.goalDiference.ToString();
                                homeGoals.Text           = item.homeGoals.ToString();
                                awayGaols.Text           = item.awayGoals.ToString();
                                goalsPerMatch.Text       = scoredPerMatch.ToString();
                                receivedPerMatch.Text    = goalsdRecPerMatch.ToString();
                            }
                        }
                        catch (System.Exception)
                        {
                        }
                    refreshButton.Click += delegate
                    {
                        if(spinner.SelectedItemPosition == 0)
                        {
                            try
                            {
                                List<points> getStats = con.db.Query<points>("SELECT * FROM points WHERE playerName = '" + player + "' AND tournamentID = '" + tournamentID + "' ");
                                foreach (var item in getStats)
                                {
                                    double scored            = Convert.ToDouble(item.goalsScored);
                                    double received          = Convert.ToDouble(item.goalsReceived);
                                    double played            = Convert.ToDouble(item.gamesPlayed);
                                    double scoredPerMatch    = scored / played;
                                    double goalsdRecPerMatch = received / played;
                                    numOfGames.Text          = item.gamesPlayed.ToString();
                                    numOfPoints.Text         = item.numOfPoints.ToString();
                                    gamesWon.Text            = item.won.ToString();
                                    gamesLost.Text           = item.lost.ToString();
                                    gamesDraw.Text           = item.draw.ToString();
                                    goalsScored.Text         = item.goalsScored.ToString();
                                    goalsAgainst.Text        = item.goalsReceived.ToString();
                                    goalDiference.Text       = item.goalDiference.ToString();
                                    homeGoals.Text           = item.homeGoals.ToString();
                                    awayGaols.Text           = item.awayGoals.ToString();
                                    goalsPerMatch.Text       = System.Math.Round(scoredPerMatch,    2, MidpointRounding.AwayFromZero).ToString();
                                    receivedPerMatch.Text    = System.Math.Round(goalsdRecPerMatch, 2, MidpointRounding.AwayFromZero).ToString();
                                }
                            }
                            catch (System.Exception)
                            {
                            }
                        }

                        if (spinner.SelectedItemPosition == 1)
                        {
                            double scored            = 0;
                            double received          = 0;
                            double played            = 0;
                            double scoredPerMatch    = 0;
                            double goalsdRecPerMatch = 0;
                            int points = 0;
                            int won    = 0;
                            int draw   = 0;
                            int lost   = 0;
                            int away   = 0;
                            int home   = 0;
                            try
                            {
                                List<points> getStats = con.db.Query<points>("SELECT * FROM points WHERE playerName = '" + player + "' ");
                                foreach (var item in getStats)
                                {

                                    scored   += Convert.ToDouble(item.goalsScored);
                                    received += Convert.ToDouble(item.goalsReceived);
                                    played   += Convert.ToDouble(item.gamesPlayed);                                                               
                                   
                                    home     += item.homeGoals;
                                    away     += item.awayGoals;
                                    points   += item.numOfPoints;
                                    won      += item.won;
                                    draw     += item.draw;
                                    lost     += item.lost;

                                
                                    numOfGames.Text   = played.ToString();
                                    numOfPoints.Text  = points.ToString();
                                    gamesWon.Text     = won.ToString();
                                    gamesDraw.Text    = draw.ToString();
                                    gamesLost.Text    = lost.ToString();
                                    goalsScored.Text  = scored.ToString();
                                    goalsAgainst.Text = received.ToString();
                                    homeGoals.Text    = home.ToString();
                                    awayGaols.Text    = away.ToString();
                                    
                                }
                                scoredPerMatch    += scored / played;
                                goalsdRecPerMatch += received / played;

                                goalsPerMatch.Text = System.Math.Round(scoredPerMatch, 2, MidpointRounding.AwayFromZero).ToString();
                                receivedPerMatch.Text = System.Math.Round(goalsdRecPerMatch, 2, MidpointRounding.AwayFromZero).ToString();
                                goalDiference.Text = (scored - received).ToString();
                            }
                            catch (System.Exception)
                            {
                                throw;
                            }

                        }
                    };

                  
                    return view;
                }

               else { return null; }
                
            }
            public string getHeaderTitle(int position)
            {
                return items[position];
            }
            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
            {
                container.RemoveView((View)obj);
            }
        }
    }

    
}