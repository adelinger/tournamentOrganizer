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
    class topScorersLVAdapter:BaseAdapter<scorers>
    {
        private List<scorers> Items;
        private Context myContext;


        public topScorersLVAdapter(Context context, List<scorers> items)
        {
            Items = items;
            myContext = context;
        }
        public override int Count => Items.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override scorers this[int position] => Items[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.winnersLV, null, false);
            }
            TextView numb = row.FindViewById<TextView>(Resource.Id.number);
            int num = position + 1;
            numb.Text = num.ToString();
            TextView name = row.FindViewById<TextView>(Resource.Id.playerName);
            name.Text = Items[position].name;
            TextView goals = row.FindViewById<TextView>(Resource.Id.ClubName);
            goals.Text = Items[position].goals.ToString();

            return row;
        }
    }
}