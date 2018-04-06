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
    class winnersLV : BaseAdapter<winners>
    {
        private List<winners> myItems;
        private Context myContext;
        public int ordNumber { get; set; }

        public winnersLV(Context context, List<winners> items)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.winnersLV, null, false);
            }
            TextView ordNum = row.FindViewById<TextView>(Resource.Id.number);
            ordNum.Text     = ordNumber.ToString();
            TextView name   = row.FindViewById<TextView>(Resource.Id.playerName);
            name.Text       = myItems[position].personName;
            TextView club   = row.FindViewById<TextView>(Resource.Id.ClubName);
            club.Text       = myItems[position].clubName;

            return row;
        }
    }
}