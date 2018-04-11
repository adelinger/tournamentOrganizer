 using System;
using System.Linq;
using System.Text;
using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLiteNetExtensions;
using System.IO;
using TournamentManagerMobile.Resources.MyClasses;
using System.Collections.Generic;
using TournamentManagerMobile.Activities;
using Android.Gms.Ads;

namespace TournamentManagerMobile.Resources
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Add new players")]
    public class addNewPlayers : Activity
    {

       
        connection con = new connection();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.addNewPlayers);

            var id = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            var adView = FindViewById<AdView>(Resource.Id.adViewANP);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            var addButton       = FindViewById<Button>(Resource.Id.addButton);
            var addName         = FindViewById<AutoCompleteTextView>(Resource.Id.addName);           
            var addClubName     = FindViewById<AutoCompleteTextView>(Resource.Id.addClub);
            var playersList     = FindViewById<ListView>(Resource.Id.listViewPlayers);
            var startTournament = FindViewById<Button>(Resource.Id.startTournamentButton);

            string tournamentName = Intent.GetStringExtra("tournamentName");             
            List<string> existingClub = new List<string>();
            List<string> allPlayersFromDB = new List<string>();
            List<string> allClubsFromDB = new List<string>();

            autoCompleteName   (allPlayersFromDB, addName);
            autoComleteClubName(allClubsFromDB, addClubName);
            fillList(playersList, tournamentName);

            //add players to database
            addButton.Click += delegate
            {
                if (addClubName.Text =="" || addName.Text == "") { Toast.MakeText(this, "You must add both club and player name", ToastLength.Short).Show(); return; }
                List<int> playerThatExistID = new List<int>();
                List<string> playersAlreadyOnThisTournamentID = new List<string>();
               
                int count = 0;
              
                List<person> onThisTournament = con.db.Query<person>("SELECT people.id, people.name, people.club, peopleTournament.tournamentID, peopleTournament.personID " +
                "FROM people LEFT JOIN peopleTournament ON people.id = peopleTournament.personID " +
                "WHERE peopleTournament.tournamentID =  " + getTournamentID(tournamentName) + " OR people.tournamentID = " + getTournamentID(tournamentName) + " ");

                foreach (var item in onThisTournament)
                {
                    playersAlreadyOnThisTournamentID.Add(item.name);
                }

                foreach (var item in playersAlreadyOnThisTournamentID)
                {
                    if (addName.Text == item) count++;
                }

                if (count > 0) { Toast.MakeText(this, "This player is already added", ToastLength.Short).Show(); return; }

                try
                {
                    club name = new club(addClubName.Text);
                    con.db.Insert(name);                  
                }
                catch (Exception)
                {
                    
                }
                try
                {
                    person person = new person(addName.Text, addClubName.Text, getTournamentID(tournamentName));
                    con.db.Insert(person);
                }
                catch (Exception)
                {                                
                    List<person> Allplayers = con.db.Query<person>("SELECT * FROM people where name = '" + addName.Text + "' AND tournamentID != " + getTournamentID(tournamentName) + "  ");
                    foreach (var item in Allplayers)
                    {
                        playerThatExistID.Add(item.id);
                    }
                    List<personTournament> personTournament = con.db.Query<personTournament>("INSERT INTO peopleTournament (tournamentID, personID, club) " +
                      "values ('" + getTournamentID(tournamentName) + "', '" + playerThatExistID[0] + "', '"+addClubName.Text+"') ");
                }

                fillList(playersList, tournamentName);
                addName.Text = "";
                addClubName.Text = "";
            };          

            playersList.ItemLongClick += delegate (object sender, Android.Widget.AdapterView.ItemLongClickEventArgs e)
            {
                var personTable = con.db.Table<person>();
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                dialog.SetTitle("Confirm delete");
                dialog.SetMessage("Do you want to delete selected player?");
                dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    dialog.Dispose();
                });
                dialog.SetPositiveButton("Delete", (senderAlert, args) =>
                {
                    var selected = playersList.GetItemAtPosition(e.Position);
                    string selectedName = selected.ToString();
                    string subSelectedName = selectedName.Substring(0, selectedName.IndexOf(' '));
                    List<int> personID = new List<int>();
                    List<person> players = con.db.Query<person>("SELECT * FROM people where name = '"+subSelectedName+"'  ");
                    
                    try
                    {
                        foreach (var item in players)
                        {
                            personID.Add(item.id);
                            con.db.Execute("DELETE FROM peopleTournament where personID = '" + personID[0] + "' ");
                            con.db.Delete(item);
                        }
                       
                       
                        Toast.MakeText(this, "Successfull", ToastLength.Short).Show();
                        fillList(playersList, tournamentName);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                });
                Dialog alertDialog = dialog.Create();
                alertDialog.Show();
            };

            startTournament.Click += delegate
            {
                if (getNumOfActivePlayers(tournamentName) < 3) { Toast.MakeText(this, "Leage type requires minimum of 3 players", ToastLength.Short).Show(); return; }

                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                dialog.SetTitle("Confirm start");
                dialog.SetMessage("Are you sure you are ready to start tournament? You can't add the players after the start");
                dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    dialog.Dispose();
                });
                dialog.SetPositiveButton("Yes", (senderAlert, args) =>
                { 
                    List<tournament> tournamentTypeList = con.db.Query<tournament>("SELECT * FROM tournament where id = '" + getTournamentID(tournamentName) + "' ");
                    List<string> tournamentType = new List<string>();
                    foreach (var item in tournamentTypeList)
                    {
                        tournamentType.Add(item.type);
                    }
                    if (tournamentType[0] == "League")
                    {
                        Intent intent = new Intent(this, typeof(leagueType));
                        intent.PutExtra("tournamentName", tournamentName);
                        StartActivity(intent);
                    }

                });
                Dialog alertDialog = dialog.Create();
                alertDialog.Show();


            };

        }

        public int getTournamentID (string tournamentName)
        {           
            List<int> tournamentID = new List<int>();
            List<tournament> tournamentTable = con.db.Query<tournament>("SELECT * FROM tournament WHERE name = '"+tournamentName+"' ");
           

            foreach (var item in tournamentTable)
            {
                tournamentID.Add(item.id);
            }

            int numOf  = tournamentID.Count;
            int ID = tournamentID[0];
            return ID;
        }

        public int getNumOfActivePlayers (string tournamentName)
        {
            List<int> all = new List<int>();

            List<person> getAll1 = con.db.Query<person>("SELECT * FROM people WHERE tournamentID = '" + getTournamentID(tournamentName) + "' ");
            foreach (var item in getAll1)
            {
                all.Add(item.id);
            }
            List<personTournament> getAll2 = con.db.Query<personTournament>("SELECT * FROM peopleTournament WHERE tournamentID = '" + getTournamentID(tournamentName) + "' ");
            foreach (var item in getAll2)
            {
                all.Add(item.personID);
            }
            return all.Count;
        }

        public void fillList (ListView listview, string tournamentName)
        {
            List<string> addedPerson = new List<string>();
            List<int> id = new List<int>();
            List<string> name = new List<string>();
            List<string> club = new List<string>();

            List<person> data = con.db.Query<person>("SELECT * FROM people WHERE tournamentID =  '" + getTournamentID(tournamentName) + "' ");
            List<personTournament> data2 = con.db.Query<personTournament>("SELECT * FROM peopleTournament WHERE tournamentID =  '" + getTournamentID(tournamentName) + "' ");

            foreach (var item in data)
            {
                addedPerson.Add(item.name + " - " + item.club);
            }
            foreach (var item in data2)
            {
                List<person> getName = con.db.Query<person>("SELECT * FROM people WHERE id =  '" + item.personID + "' ");
                foreach (var item2 in getName)
                {
                    addedPerson.Add(item2.name + " - " + item.club);
                }
            }
          

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, addedPerson);
            listview.Adapter = adapter;
        }

        public void autoCompleteName (List<string> allPlayersFromDB, AutoCompleteTextView addName)
        {           
            List<person> data = con.db.Query<person>("SELECT name FROM people");
            foreach (var item in data)
            {
                allPlayersFromDB.Add(item.name);
            }

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allPlayersFromDB);
            addName.Adapter = adapter;
        }

        public void autoComleteClubName (List<string> allClubsFromDB, AutoCompleteTextView addClubName)
        {
            
            List<club> clubs = con.db.Query<club>("SELECT * FROM club");

            foreach (var item in clubs)
            {
                allClubsFromDB.Add(item.name);
            }

            var adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allClubsFromDB);
            addClubName.Adapter = adapter2;
        }
       
    }

}