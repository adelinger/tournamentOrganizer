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
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "clubs")]
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
            

            foreach (var item in clubs)
            {
                allClubsFromDB.Add(item.name);
            }

            var adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allClubsFromDB);
            clubtxt.Adapter = adapter2;

            addButton.Click += delegate
            {
                con.db.Execute("INSERT INTO clubs (clubName) VALUES ('" + clubtxt.Text + "') ");
                fillAndRefreshList(clubListView);
            };

            string selected = "";
            clubListView.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
            {
                selected        = clubs[e.Position].name;
               
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