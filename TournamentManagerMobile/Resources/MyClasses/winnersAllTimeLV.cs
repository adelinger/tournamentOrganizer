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

namespace TournamentManagerMobile.Resources.MyClasses
{
    class winnersAllTimeLV : BaseAdapter<winners>
    {
        private List<winners> myItems;
        private Context myContext;
        public int ordNumber { get; set; }

        public winnersAllTimeLV(Context context, List<winners> items)
        {
            myItems = items;
            myContext = context;
            ordNumber = 1;
        }
        public override int Count => myItems.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override winners this[int position] => myItems[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.winnersAllTimeLV, null, false);
            }
            TextView name = row.FindViewById<TextView>(Resource.Id.allTimeName);
            name.Text = myItems[position].personName;
            TextView clubName = row.FindViewById<TextView>(Resource.Id.allTimeClub);
            clubName.Text = myItems[position].clubName;
            TextView tournament = row.FindViewById<TextView>(Resource.Id.allTimeTournamentName);
            tournament.Text = myItems[position].tournamentName;

            return row;
        }
    }
}