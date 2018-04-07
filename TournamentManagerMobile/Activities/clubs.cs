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
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Clubs")]
    public class clubs : Activity
    {
        connection con = new connection();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.clubsLayout);

            ListView clubListView = FindViewById<ListView>(Resource.Id.clubListView);
            Button addButton = FindViewById<Button>(Resource.Id.addClubButton);
            AutoCompleteTextView clubtxt = FindViewById<AutoCompleteTextView>(Resource.Id.clubTxt);

            fillAndRefreshList(clubListView);

            List<string> allClubsFromDB = new List<string>();

            List<club> clubs = con.db.Query<club>("SELECT * FROM club");
            string selected = "";

            foreach (var item in clubs)
            {
                allClubsFromDB.Add(item.name);
            }

            try
            {
                var adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allClubsFromDB);
                clubtxt.Adapter = adapter2;
            }
            catch (Exception)
            {

                Toast.MakeText(this, "There is nothing here yet", ToastLength.Short).Show();
            }
           

            addButton.Click += delegate
            {
                if (addButton.Text == "ADD")
                {
                    if (clubtxt.Text == "")
                    {
                        Toast.MakeText(this, "Club name can't be empty", ToastLength.Short).Show();
                    }
                    try
                    {
                        
                        con.db.Execute("INSERT INTO club (name) VALUES ('" + clubtxt.Text + "') ");
                        fillAndRefreshList(clubListView);
                        clubtxt.Text = "";
                    }
                    catch (Exception)
                    {

                        Toast.MakeText(this, "Club with this name already exists", ToastLength.Short).Show();
                    }
                    
                }
                if (addButton.Text == "UPDATE")
                {
                    if (clubtxt.Text == "")
                    {
                        Toast.MakeText(this, "Club name can't be empty", ToastLength.Short).Show();
                    }
                    con.db.Execute("UPDATE club set name = '" + clubtxt.Text + "' WHERE name = '"+selected+"' ");
                    fillAndRefreshList(clubListView);
                    addButton.Text = "ADD";
                    clubtxt.Text = "";
                }
               
            };

            string clubName = "";
            clubListView.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
            {
                clubName = clubs[e.Position].name;
                Intent intent = new Intent(this, typeof(clubDetails));
                intent.PutExtra("clubName", clubName);
                StartActivity(intent);              
            };

            clubListView.ItemLongClick += delegate (object sender, Android.Widget.AdapterView.ItemLongClickEventArgs e)
            {
                List<club> clubList = con.db.Query<club>("SELECT * FROM club");

                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                dialog.SetTitle("Update player");
                dialog.SetMessage("Do you want to update selected player?");
                
                dialog.SetPositiveButton("YES", (senderAlert, args) =>
                {
                    selected = clubList[e.Position].name;
                    clubtxt.Text = clubList[e.Position].name;
                    addButton.Text = "UPDATE";

                });
                dialog.SetNegativeButton("No", (senderAlert, args) =>
                {
                    dialog.Dispose();
                });
                Dialog alertDialog = dialog.Create();
                alertDialog.Show();
            };
            
        }

        public void fillAndRefreshList(ListView list)
        {
            List<club> getClubs = con.db.Query<club>("SELECT * FROM club");
            List<string> clubsList = new List<string>();
            foreach (var item in getClubs)
            {
                clubsList.Add(item.name);
            }

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, clubsList);
            list.Adapter = adapter;
        }

        
    }

}