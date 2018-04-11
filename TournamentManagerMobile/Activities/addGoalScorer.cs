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
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "addGoalScorer")]
    public class addGoalScorer : Activity
    {
        connection con = new connection();

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.addGoalScorer);

            var idd = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, idd);
            var adView = FindViewById<AdView>(Resource.Id.adViewAGS);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);


            AutoCompleteTextView scorerNameText = FindViewById<AutoCompleteTextView>(Resource.Id.scorerNameText);
            Button increment                    = FindViewById<Button>(Resource.Id.plusIncrementGoal);
            Button decrement                    = FindViewById<Button>(Resource.Id.decrementGoal);
            TextView numOfGoalsText             = FindViewById<TextView>(Resource.Id.numOfGoalsText);
            Button addGoals                     = FindViewById<Button>(Resource.Id.applyButton);
            ListView playersList                = FindViewById<ListView>(Resource.Id.playersList);

            this.Title = "Top scorers";

            string tournamentIDString = Intent.GetStringExtra("tournamentID");
            int tournamentID = Convert.ToInt32(tournamentIDString); 
            string scorerString = "";
            string nameOfTheScorer = "";
            string tournamentName = Intent.GetStringExtra("tournamentName");
            int tournamentScorerPlayers = 0;
            bool updateReady = false;

            int i = 1;
            increment.Click += delegate
            {
                decrement.Enabled = true;
                i++;
                numOfGoalsText.Text = i.ToString();
            };
            decrement.Click += delegate
             {
                 if (i > 1)
                 {
                     decrement.Enabled = true;
                     i--;
                     numOfGoalsText.Text = i.ToString();
                 }
                 else if (numOfGoalsText.Text == "1" && updateReady == false)
                 {
                     decrement.Enabled = false;
                 }
                 else if (updateReady == true)
                 {
                     decrement.Enabled = true;
                     i--;
                     numOfGoalsText.Text = i.ToString();
                   
                 }
             };

            fillPlayersList(tournamentID, playersList);
            fillAutoCompleteList(scorerNameText);

            addGoals.Click += delegate
            {
                if (scorerNameText.Text == "") { Toast.MakeText(this, "You need to add name of the scorer first!", ToastLength.Short).Show(); return; }

                int numOfGoals = Convert.ToInt32(numOfGoalsText.Text);
                scorers scorers = new scorers(scorerNameText.Text, numOfGoals);
                try
                {
                    con.db.Insert(scorers);
                }
                catch (Exception)
                {
                    List<scorers> updateScorers = con.db.Query<scorers>("UPDATE scorers SET goals = goals + '" + numOfGoals + "' WHERE name = '" + scorerNameText.Text + "' ");
                }

                List<scorers> id = con.db.Query<scorers>("SELECT * FROM scorers WHERE name = '" + scorerNameText.Text + "' ");
                foreach (var item in id)
                {
                    scorerString = item.id.ToString();
                    nameOfTheScorer = item.name;
                }
                int scorerID = Convert.ToInt32(scorerString);

                List<tournamentScorer> list = con.db.Query<tournamentScorer>("SELECT * FROM tournamentScorer WHERE name = '" + nameOfTheScorer + "' AND tournamentID = '" + tournamentID + "' ");
                foreach (var item in list)
                {
                    tournamentScorerPlayers++;
                }

                if (tournamentScorerPlayers == 0)
                {
                    tournamentScorer tournamentScorer = new tournamentScorer(tournamentID, scorerID, nameOfTheScorer, numOfGoals);
                    con.db.Insert(tournamentScorer);
                }
                else
                {
                    con.db.Execute("UPDATE tournamentScorer set goals = goals + '" + numOfGoals + "' WHERE name = '" + scorerNameText.Text + "' ");
                    tournamentScorerPlayers = 0;
                }

                fillAutoCompleteList(scorerNameText);
                scorerNameText.Text = "";
                updateReady = false;
                fillPlayersList(tournamentID, playersList);
            };

            playersList.ItemLongClick += delegate (object sender, Android.Widget.AdapterView.ItemLongClickEventArgs e)
            {
                List<tournamentScorer> tournamentScorer = con.db.Query<tournamentScorer>("SELECT * FROM tournamentScorer WHERE tournamentID = '" + tournamentID + "' ORDER BY goals DESC ");
                try
                {
                    string selected = tournamentScorer[e.Position].name;

                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    dialog.SetTitle("Confirm delete");
                    dialog.SetMessage("Do you want to delete selected player?");
                    dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                    {
                        dialog.Dispose();
                    });
                    dialog.SetPositiveButton("Delete", (senderAlert, args) =>
                    {
                        con.db.Execute("DELETE FROM tournamentScorer WHERE name = '" + selected + "' ");
                        fillPlayersList(tournamentID, playersList);

                    });
                    Dialog alertDialog = dialog.Create();
                    alertDialog.Show();
                }
                catch (Exception)
                {
                }

            };

            playersList.ItemLongClick += delegate (object sender, Android.Widget.AdapterView.ItemLongClickEventArgs e)
            {
                List<tournamentScorer> tournamentScorer = con.db.Query<tournamentScorer>("SELECT * FROM tournamentScorer WHERE tournamentID = '" + tournamentID + "' ORDER BY goals DESC ");
                try
                {
                    string selected = tournamentScorer[e.Position].name;

                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    dialog.SetTitle("Confirm delete");
                    dialog.SetMessage("Do you want to delete selected player?");
                    dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                    {
                        dialog.Dispose();
                    });
                    dialog.SetPositiveButton("Delete", (senderAlert, args) =>
                    {
                        con.db.Execute("DELETE FROM tournamentScorer WHERE name = '" + selected + "' ");
                        fillPlayersList(tournamentID, playersList);

                    });
                    Dialog alertDialog = dialog.Create();
                    alertDialog.Show();
                }
                catch (Exception)
                {

                    throw;
                }


            };
            playersList.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
            {
                List<tournamentScorer> tournamentScorer = con.db.Query<tournamentScorer>("SELECT * FROM tournamentScorer WHERE tournamentID = '" + tournamentID + "' ORDER BY goals DESC ");

                try
                {
                    scorerNameText.Text = tournamentScorer[e.Position].name;
                    decrement.Enabled = true;
                    updateReady = true;
                }
                catch (Exception)
                {

                    throw;
                }

            };
        }

        public void fillPlayersList (int tournamentID, ListView playersList)
        {
            List<tournamentScorer> tournamentScorer = con.db.Query<tournamentScorer>("SELECT * FROM tournamentScorer WHERE tournamentID = '" + tournamentID + "' ORDER BY goals DESC ");
         
            tournamentScorerLVAdapter adapter = new tournamentScorerLVAdapter(this, tournamentScorer);
            playersList.Adapter = adapter;
        }
        
        public void fillAutoCompleteList(AutoCompleteTextView scorerNameText)
        {
            List<string> allGoalScorers = new List<string>();
            List<scorers> allScorersDBList = con.db.Query<scorers>("SELECT * from scorers");
            foreach (var item in allScorersDBList)
            {
                allGoalScorers.Add(item.name);
            }

            ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allGoalScorers);
            scorerNameText.Adapter = adapter;
        }
    }
}