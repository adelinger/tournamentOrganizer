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
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Players database")]
    public class allPlayersActivity : Activity
    {
        connection con = new connection();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.allPlayersLayout);

            ListView allPlayers = FindViewById<ListView>(Resource.Id.allPlayersLV);
            try
            {
                
                List<string> allPlayersDB = new List<string>();

                List<person> getAll = con.db.Query<person>("SELECT * FROM people");
                foreach (var item in getAll)
                {
                    allPlayersDB.Add(item.name);
                }

                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allPlayersDB);
                allPlayers.Adapter = adapter;
            }
            catch (Exception)
            {
                Toast.MakeText(this, "There is nothing here yet", ToastLength.Short).Show();
            }
          

            allPlayers.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
            {
                List<person> all = con.db.Query<person>("SELECT * FROM people");
                string selected = all[e.Position].name;

                Intent intent = new Intent(this, typeof(stats));
                intent.PutExtra("playerName", selected);

                StartActivity(intent);
            };
        }
    }
}