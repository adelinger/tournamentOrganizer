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
using System.IO;
using TournamentManagerMobile.Resources.MyClasses;
using System.Collections.Generic;
using TournamentManagerMobile.Resources;
using Com.Startapp.Android.Publish;
using Com.Startapp.Android.Publish.Banner;

namespace TournamentManagerMobile.Activities
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "My tournaments")]
    public class myTournaments : Activity
    {
        Banner banner;

        connection con = new connection();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartAppSDK.Init(this, "202635783", true);

            SetContentView(Resource.Layout.myTournaments);

            IBannerListener bannerListener = new AdListener();
            banner = FindViewById<Com.Startapp.Android.Publish.Banner.Banner>(Resource.Id.myTournamentBanner);
            banner.ShowBanner();
            banner.SetBannerListener(bannerListener);

            ListView tournamentsList = FindViewById<ListView>(Resource.Id.listViewTournaments);

            var mytournamentsTable = con.db.Table<tournament>();

            //connection.db.DropTable<winners>();
            //connection.db.DropTable<person>();
            //connection.db.DropTable<tournament>();
            //connection.db.DropTable<personTournament>();
            //connection.db.DropTable<tournamentScorer>();
            //connection.db.DropTable<results>();
            //connection.db.DropTable<points>();

            fillList();
  

            tournamentsList.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
            {
                var selected = tournamentsList.GetItemAtPosition(e.Position);
                List<int> allPeople = new List<int>();
                int id = 0;
                List<tournament> getID = con.db.Query<tournament>("SELECT * FROM tournament WHERE name = '" + selected + "' ");
                foreach (var item in getID)
                {
                    id = item.id;
                }
                List<person> getAll1 = con.db.Query<person>("SELECT * FROM people WHERE tournamentID = '"+id+"' ");
                foreach (var item in getAll1)
                {
                    allPeople.Add(item.id);
                }
                List<personTournament> getAll2 = con.db.Query<personTournament>("SELECT * FROM peopleTournament WHERE tournamentID = '"+id+"' ");
                foreach (var item in getAll2)
                {
                    allPeople.Add(item.personID);
                }

                if(allPeople.Count < 3)
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    dialog.SetTitle("Warning");
                    dialog.SetMessage("This tournament does not have minimum of 3 players.");
                    dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                    {
                        dialog.Dispose();
                    });
                    dialog.SetPositiveButton("Add players", (senderAlert, args) =>
                    {
                        Intent intent = new Intent(this, typeof(addNewPlayers));
                        intent.PutExtra("tournamentName", selected.ToString());
                        StartActivity(intent);
                    });               
                    Dialog alertDialog = dialog.Create();
                    alertDialog.Show();
                    return;
                }
                
                List <tournament> tournamentTypeList = con.db.Query<tournament>("SELECT type FROM tournament where name = '"+selected+"' ");
                List<string> tournamentType = new List<string>();
                foreach (var item in tournamentTypeList)
                {
                    tournamentType.Add(item.type);
                }
                if(tournamentType[0]=="League")
                {
                    Intent intent = new Intent(this, typeof(leagueType));
                    intent.PutExtra("tournamentName", selected.ToString());
                    StartActivity(intent);
                }
                
            };

            tournamentsList.ItemLongClick += delegate (object sender, Android.Widget.AdapterView.ItemLongClickEventArgs e)
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                dialog.SetTitle("Confirm delete");
                dialog.SetMessage("Do you want to delete selected tournament?");
                dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    dialog.Dispose();
                });
                dialog.SetPositiveButton("Delete", (senderAlert, args) =>
                {            
                    var selected = tournamentsList.GetItemAtPosition(e.Position);
                    string name = selected.ToString();
                    var data = mytournamentsTable.Where(x => x.name == name).FirstOrDefault();
                    try
                    {
                        con.db.Delete(data);
                        Toast.MakeText(this, "Successfull", ToastLength.Short).Show();
                        fillList();
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                });
                Dialog alertDialog = dialog.Create();
                alertDialog.Show();
                
            };
                        
        }

        public void fillList ()
        {
            try
            {     
                var mytournamentsTable = con.db.Table<tournament>();
                ListView tournamentsList = FindViewById<ListView>(Resource.Id.listViewTournaments);
                List<string> myTournamentsList = new List<string>();
                foreach (var item in mytournamentsTable)
                {
                    myTournamentsList.Add(item.name);
                }

                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, myTournamentsList);
                tournamentsList.Adapter = adapter;
            }
            catch (Exception)
            {

                Toast.MakeText(this, "There is no existing tournaments. You need to create one first.", ToastLength.Short).Show();
            }
            
        }
    }

}