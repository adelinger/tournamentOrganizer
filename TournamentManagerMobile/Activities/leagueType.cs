using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using TournamentManagerMobile.Resources;
using TournamentManagerMobile.Resources.MyClasses;

namespace TournamentManagerMobile.Activities
{   
    [Activity(Theme = "@android:style/Theme.Material.Light", Label =  "League")]
    public class leagueType : Activity
    {
        person person = new person();
        connection connection = new connection();
        tournament tournament = new tournament();
     
        List<string> players1 = new List<string>();
        List<string> players2 = new List<string>();
        List<string> allPlayers = new List<string>();
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.leagueType);

            Button plusIncrementP1  = FindViewById<Button>  (Resource.Id.plusIncrement);
            Button plusIncrementP2  = FindViewById<Button>  (Resource.Id.plusIncrementP2);
            Button minusIncrementP1 = FindViewById<Button>  (Resource.Id.minusIncrement);
            Button minusIncrementP2 = FindViewById<Button>  (Resource.Id.minusIncrementP2);
            Button apply            = FindViewById<Button>  (Resource.Id.applyButton);
            Button draw             = FindViewById<Button>  (Resource.Id.drawButton);
            Button addGoalscorers   = FindViewById<Button>  (Resource.Id.addGoalscorersButton);
            Button tableButton      = FindViewById<Button>  (Resource.Id.tableButton);
            Button allFixtRes       = FindViewById<Button>  (Resource.Id.allFixturesAndResults);
            Button winners          = FindViewById<Button>  (Resource.Id.winnersButton);
            Button resetButton      = FindViewById<Button>  (Resource.Id.resetLeagueTournament);
            TextView resultP1       = FindViewById<TextView>(Resource.Id.result1);
            TextView resultP2       = FindViewById<TextView>(Resource.Id.result2);
            TextView player1        = FindViewById<TextView>(Resource.Id.player1TextView);
            TextView player2        = FindViewById<TextView>(Resource.Id.Player2textView);

            int tournamentID      = 0;
            apply.Enabled = false;
            string oddEven = "";
            bool firstTimeClicked = true;
            bool allGood = true;

            string tournamentName = Intent.GetStringExtra("tournamentName");
            this.Title = tournamentName + " " + "league";
            List<tournament> getID = connection.db.Query<tournament>("SELECT * FROM tournament WHERE name = '" + tournamentName + "' ");
            foreach (var item in getID)
            {
                tournamentID = item.id;
                tournament.id = tournamentID;
            }
            
            List<person> getAllPlayers = connection.db.Query<person>("SELECT people.id, people.name, people.club, peopleTournament.tournamentID, peopleTournament.personID " +
                "FROM people LEFT JOIN peopleTournament ON people.id = peopleTournament.personID " +
                "WHERE peopleTournament.tournamentID =  " + tournamentID + " OR people.tournamentID = " + tournamentID + " ");
            foreach (var item in getAllPlayers)
            {
                allPlayers.Add(item.name);
            }

            string numOfRounds = "";
            List<tournament> getNumOfRounds = connection.db.Query<tournament>("SELECT * FROM tournament WHERE id = '" + tournamentID + "' ");
            foreach (var item in getNumOfRounds)
            {
                numOfRounds = item.numOfRounds;
            }
            
            int i1 = 0;
            int i2 = 0;
            int gamesPlayed = 0;

            plusIncrementP1.Click  += delegate
            {
                minusIncrementP1.Enabled = true;
                i1++;
                resultP1.Text = i1.ToString();
            };
            minusIncrementP1.Click += delegate
            {
                if(i1 > 0)
                {
                    i1--;
                    resultP1.Text = i1.ToString();
                }
                else
                {
                    minusIncrementP1.Enabled = false;
                }
                
            };

            plusIncrementP2.Click  += delegate
            {
                minusIncrementP2.Enabled = true;
                i2++;
                resultP2.Text = i2.ToString();
            };
            minusIncrementP2.Click += delegate
            {
                if (i2 > 0)
                    { 
                        i2--;
                        resultP2.Text = i2.ToString();
                    }
                else
                {
                    minusIncrementP2.Enabled = false;
                }
               
            };

            if (allPlayers.Count % 2 == 0) { makeFixturesEven(allPlayers, players1, players2); oddEven = "even"; }
            if (allPlayers.Count % 2 != 0) { makeFixturesOdd(allPlayers, players1, players2);   oddEven = "odd"; }

            List<string> existingRecords = new List<string>();
           
            List<points> checkIfRecordsExists = connection.db.Query<points>  ("SELECT * FROM points WHERE tournamentID  = '"+tournamentID+"' ");            
            foreach (var item in checkIfRecordsExists)
            {
                existingRecords.Add(item.playerName);
            }

            if (existingRecords.Count == 0)
            {
                foreach (var item in allPlayers)
                {
                    points startingPoints = new points(item, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, tournamentID);
                    connection.db.Insert(startingPoints);
                }
            }
           

            apply.Click += delegate
            {
                    connection.db.Execute("INSERT INTO results (homePlayerName, awayPlayerName, homeGoals, awayGoals, tournamentID)" +
                    " values ('" + player1.Text + "', '" + player2.Text + "', '" + resultP1.Text + "', '" + resultP2.Text + "', '" + tournamentID + "')");
                   
                    int result1 = Convert.ToInt32(resultP1.Text);
                    int result2 = Convert.ToInt32(resultP2.Text);
                    int result1minus2 = result1 - result2;
                    int result2minus1 = result2 - result1;

                    if (result1 > result2)
                    {
                        connection.db.Execute("UPDATE points SET numOfPoints = numOfPoints + 3, gamesPlayed = gamesPlayed +1, won = won + 1, goalsScored = goalsScored + '" + result1 + "', homeGoals = homeGoals + '" + result1 + "'," +
                                              " goalsReceived = goalsReceived  + '" + result2 + "', goalDiference = goalDiference + '" + result1minus2 + "' " +
                                              " WHERE playerName = '" + player1.Text + "' AND tournamentID = '" + tournamentID + "' ");
                       

                        connection.db.Execute("UPDATE points SET goalsScored = goalsScored + '" + result2 + "', gamesPlayed = gamesPlayed +1, lost = lost + 1, awayGoals = awayGoals + '" + result2 + "'," +
                                             " goalsReceived = goalsReceived  + '" + result1 + "', goalDiference = goalDiference + '"+result2minus1+"' " +
                                             " WHERE playerName = '" + player2.Text + "' AND tournamentID = '" + tournamentID + "' ");
                    }
                    if (result2 > result1)
                    {
                        connection.db.Execute("UPDATE points SET numOfPoints = numOfPoints + 3, won = won + 1, gamesPlayed = gamesPlayed +1, goalsScored = goalsScored + '" + result2 + "', awayGoals = awayGoals + '"+result2+"'," +
                                             " goalsReceived = goalsReceived  + '" + result1 + "', goalDiference = goalDiference + '" + result2minus1 + "' " +
                                             " WHERE playerName = '" + player2.Text + "' AND tournamentID = '" + tournamentID + "' ");


                        connection.db.Execute("UPDATE points SET goalsScored = goalsScored + '" + result1 + "', gamesPlayed = gamesPlayed +1, lost = lost+1, homeGoals = homeGoals + '" + result1 + "'," +
                                             " goalsReceived = goalsReceived  + '" + result2 + "', goalDiference = goalDiference + '" + result1minus2 + "' " +
                                             " WHERE playerName = '" + player1.Text + "' AND tournamentID = '" + tournamentID + "' ");
                    }
                   if (result1 == result2)
                    {
                    connection.db.Execute("UPDATE points SET numOfPoints = numOfPoints + 1, draw = draw + 1, gamesPlayed = gamesPlayed +1, homeGoals = homeGoals + '" + result1 + "', goalsScored = goalsScored + '" + result1 + "'," +
                                          " goalsReceived = goalsReceived + '" + result2 + "' " +
                                          "WHERE playerName = '" + player1.Text + "' AND tournamentID = '" + tournamentID + "' ");

                    connection.db.Execute("UPDATE points SET numOfPoints = numOfPoints + 1, draw = draw +1, gamesPlayed = gamesPlayed +1, goalsScored = goalsScored + '" + result2 + "', awayGoals = awayGoals + '" + result2 + "'," +
                                          " goalsReceived = goalsReceived  + '" + result1 + "' " +
                                          " WHERE playerName = '" + player2.Text + "' AND tournamentID = '" + tournamentID + "' ");
                   }
                

                player1.Text = "";
                player2.Text = "";

                resultP1.Text = "0";
                resultP2.Text = "0";

                i1 = 0;
                i2 = 0;

                draw.Enabled  = true;
                apply.Enabled = false;

                List<points> theWinnerList = connection.db.Query<points>("SELECT * FROM points where tournamentID = '" + tournamentID + "' ORDER BY numOfPoints DESC, goalDiference DESC, awayGoals DESC ");
                List<string> theWinner = new List<string>();
                List<int> theWinnerID = new List<int>();
                string theWinnerClub = "";

                foreach (var item in theWinnerList)
                {
                    theWinner.Add(item.playerName);
                }
                List<person> findID = connection.db.Query<person>("SELECT * FROM people WHERE name = '" + theWinner[0] + "' ");
                foreach (var item in findID)
                {
                    theWinnerID.Add(item.id);
                }

                List<person> findClubInPerson = connection.db.Query<person>("SELECT *FROM people WHERE name = '"+theWinner[0]+"' AND  tournamentID = '"+tournamentID+"' ");
                foreach (var item in findClubInPerson)
                {
                    theWinnerClub = item.club;
                }

                if (theWinnerClub == "")
                {
                    List<personTournament> findClubJoin = connection.db.Query<personTournament>("SELECT *FROM peopleTournament WHERE personID = '" + theWinnerID[0] + "' AND  tournamentID = '" + tournamentID + "' ");
                    foreach (var item in findClubJoin)
                    {
                        theWinnerClub = item.club;
                    }
                }


                if (checkIftournamentIsOver(oddEven, checkExistingMatches(tournamentID)) && numOfRounds == "one")
                {
                    Toast.MakeText(this, "The tournament is over. Congratulations to the winner '"+theWinner[0]+"' ", ToastLength.Short).Show();
                    connection.db.Execute("INSERT INTO winners (personName, clubName, tournamentName) VALUES ('" + theWinner[0] + "', '" + theWinnerClub + "', '" + tournamentName + "') ");
                    draw.Enabled = false;
                    return;                 
                }
                if (checkIfTournamentIsOverTwoRounds(oddEven, checkExistingMatches(tournamentID)) && numOfRounds == "two")
                {
                    Toast.MakeText(this, "The tournament is over. Congratulations to the winner '" + theWinner[0] + "' ", ToastLength.Short).Show();
                    connection.db.Execute("INSERT INTO winners (personName, clubName, tournamentName) VALUES ('" + theWinner[0] + "', '" + theWinnerClub + "', '" + tournamentName + "') ");
                    draw.Enabled = false;
                    return;
                }
            };

            draw.Click += delegate
            {
                if (checkIftournamentIsOver(oddEven, checkExistingMatches(tournamentID)) && numOfRounds == "one")
                {
                    Toast.MakeText(this, "This tournament is over. You can reset it and start again", ToastLength.Short).Show();
                    return;
                }

                if (checkIfTournamentIsOverTwoRounds(oddEven, checkExistingMatches(tournamentID)) && numOfRounds == "two")
                {
                    Toast.MakeText(this, "This tournament is over. You can reset it and start again", ToastLength.Short).Show();
                    return;
                }

                if (numOfRounds == "one")
                {
                    if (firstTimeClicked) { gamesPlayed = checkExistingMatches(tournamentID).Count; }

                    try
                    {
                        player1.Text = players1[gamesPlayed];
                        player2.Text = players2[gamesPlayed];
                        gamesPlayed += 1;
                        firstTimeClicked = false;

                    }
                    catch (Exception)
                    {
                        Toast.MakeText(this, "The tournament is over", ToastLength.Short).Show();
                        player1.Text = "";
                        player2.Text = "";
                    }
                    draw.Enabled = false;
                    apply.Enabled = true;
                }
                
                if (numOfRounds == "two")
                {
                    int maxNumOfMatchesEven = (allPlayers.Count / 2) * (allPlayers.Count - 1);
                    int maxNumOfMatchesOdd  = allPlayers.Count * ((allPlayers.Count - 1) / 2);

                    if (oddEven == "even")
                    {
                        if (firstTimeClicked && checkExistingMatches(tournamentID).Count < maxNumOfMatchesEven) { gamesPlayed = checkExistingMatches(tournamentID).Count; }
                        if (firstTimeClicked && checkExistingMatches(tournamentID).Count > maxNumOfMatchesEven) { gamesPlayed = checkExistingMatches(tournamentID).Count - maxNumOfMatchesEven; allGood = false; }
                    }
                    if (oddEven == "odd")
                    {
                        if (firstTimeClicked && checkExistingMatches(tournamentID).Count < maxNumOfMatchesOdd) { gamesPlayed = checkExistingMatches(tournamentID).Count; }
                        if (firstTimeClicked && checkExistingMatches(tournamentID).Count >=  maxNumOfMatchesOdd) { gamesPlayed = checkExistingMatches(tournamentID).Count - maxNumOfMatchesOdd; allGood = false; }
                    }
                                      

                    try
                    {
                        if(allGood)
                        {
                            player1.Text = players1[gamesPlayed];
                            player2.Text = players2[gamesPlayed];
                            gamesPlayed += 1;
                            firstTimeClicked = false;
                        }
                        if (allGood == false)
                        {
                            player1.Text = players2[gamesPlayed];
                            player2.Text = players1[gamesPlayed];
                            gamesPlayed += 1;
                            firstTimeClicked = false;
                        }
                        
                    }
                    catch (Exception)
                    {
                        gamesPlayed = 0;
                        player1.Text = players2[gamesPlayed];
                        player2.Text = players1[gamesPlayed];                        
                        allGood = false;
                        gamesPlayed = 1;
                    }
                    draw.Enabled = false;
                    apply.Enabled = true;
                }
              

               
            };

            addGoalscorers.Click += delegate
            {
                Intent intent = new Intent(this, typeof(addGoalScorer));
                intent.PutExtra("tournamentID", tournament.id.ToString());
                intent.PutExtra("tournamentName", tournamentName);
                StartActivity(intent);
            };

            allFixtRes.Click += delegate
            {
                Intent intent = new Intent(this, typeof(allFixturesAndResults));
                intent.PutExtra("oddEven", oddEven);
                intent.PutExtra("tournamentID", tournament.id.ToString());
                intent.PutExtra("numOfRounds", numOfRounds);
                StartActivity(intent);
            };

            tableButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof (table));
                intent.PutExtra("tournamentID", tournament.id.ToString());
                intent.PutExtra("tournamentName", tournamentName);
                StartActivity(intent);
            };

            winners.Click += delegate
           {
               Intent intent = new Intent(this, typeof(winnersActivity));
               intent.PutExtra("winnerType", "thisTournament");
               intent.PutExtra("tournamentName", tournamentName);
               StartActivity(intent);
           };
        
                player1.Click += delegate
                {
                    if (player1.Text == "")
                    { return; }
                    Intent intent = new Intent(this, typeof(stats));
                    intent.PutExtra("tournamentID", tournament.id.ToString());
                    intent.PutExtra("playerName", player1.Text);
                    intent.PutExtra("tournamentName", tournamentName);
                    StartActivity(intent);
                };
                player2.Click += delegate
                {
                    if (player2.Text == "")
                    { return; }
                    Intent intent = new Intent(this, typeof(stats));
                    intent.PutExtra("tournamentID", tournament.id.ToString());
                    intent.PutExtra("playerName", player2.Text);
                    intent.PutExtra("tournamentName", tournamentName);
                    StartActivity(intent);
                };
           

          
            resetButton.Click += delegate
            {
                //AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                //dialog.SetTitle("RESET");
                //dialog.SetMessage("Are you sure you want to reset the tournament?");
                //dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                //{
                //    dialog.Dispose();
                //});
                //dialog.SetPositiveButton("Yes", (senderAlert, args) =>
                //{                  
                //    try
                //    {
                //        connection.db.Execute("UPDATE points SET numOfPoints = 0, gamesPlayed = 0, won = 0, draw = 0, lost = 0, homeGoals = 0," +
                //            " awayGoals = 0, goalsScored = 0, goalsReceived = 0, goalDiference = 0 WHERE tournamentID  = '" + tournamentID + "' ");
                //        connection.db.Execute("DELETE FROM results WHERE tournamentID = '" + tournamentID + "' ");
                //        connection.db.Execute("DELETE from tournamentScorer WHERE tournamentID = '" + tournamentID + "' ");

                //        Toast.MakeText(this, "Successfull", ToastLength.Short).Show();
                //    }
                //    catch (Exception)
                //    {
                //        throw;
                //    }

                //});
                //Dialog alertDialog = dialog.Create();
                //alertDialog.Show();

                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
            

        }

        public void makeFixturesEven(List<string> allPlayers, List<string>players1, List<string>players2)
        {         
            for (int i = 0; i < allPlayers.Count-1; i++)
            {
                for (int j = 0; j < allPlayers.Count/2; j++)
                {
                    players1.Add(allPlayers[j]);
                    players2.Add(allPlayers[allPlayers.Count - j - 1]);
                }
                allPlayers.Insert(1, allPlayers[allPlayers.Count - 1]);
                allPlayers.RemoveAt(allPlayers.Count - 1);
            }
        }
        public void makeFixturesOdd(List<string> allPlayers, List<string> players1, List<string> players2)
        {
            for (int i = 0; i < allPlayers.Count; i++)
            {
                for (int j = 0; j < allPlayers.Count/2; j++)
                {
                    players1.Add(allPlayers[j]);
                    players2.Add(allPlayers[allPlayers.Count - j - 1]);
                }
                allPlayers.Insert(0, allPlayers[allPlayers.Count - 1]);
                allPlayers.RemoveAt(allPlayers.Count - 1);
            }
        }
        public List<int> checkExistingMatches (int tournamentID)
        {
            List<int> existingMatches = new List<int>();
            List<results> checkIfResultsExists = connection.db.Query<results>("SELECT * FROM results WHERE tournamentID = '" + tournamentID + "' ");
            foreach (var item in checkIfResultsExists)
            {
                existingMatches.Add(item.matchID);
            }
            return existingMatches;
        }
        public bool checkIftournamentIsOver(string oddEven, List<int> existingMatches)
        {
            if (oddEven == "even" && existingMatches.Count == (allPlayers.Count / 2) * (allPlayers.Count - 1))
            {              
                return true;
            }
            if (oddEven == "odd" && existingMatches.Count == allPlayers.Count * ((allPlayers.Count - 1) / 2))
            {             
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkIfTournamentIsOverTwoRounds(string oddEven, List<int> existingMatches)
        {
            if (oddEven == "even" && existingMatches.Count == (((allPlayers.Count / 2) * (allPlayers.Count - 1))  *2 ))
            {
                return true;
            }
            if (oddEven == "odd" && existingMatches.Count == ((allPlayers.Count * (allPlayers.Count - 1) / 2))  * 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
   
   
    }
}