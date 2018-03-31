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
    class MyListViewAdapter : BaseAdapter<points>
    {
        private List<points> myItems;
        private Context myContext;

        public MyListViewAdapter(Context context, List<points> items)
        {
            myItems = items;
            myContext = context;
        }
        public override int Count => myItems.Count;

        public override long GetItemId(int position)
        {
            return position;
        }
        
        public override points this[int position] => myItems[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.tableListView, null, false);
            }
            TextView ordNum = row.FindViewById<TextView>(Resource.Id.ordNumber);
            int num = position + 1;
            ordNum.Text = num.ToString();
            TextView name = row.FindViewById<TextView>(Resource.Id.name);
            name.Text = myItems[position].playerName;
            TextView gamesPlayed = row.FindViewById<TextView>(Resource.Id.gamesPlayed);
            gamesPlayed.Text = myItems[position].gamesPlayed.ToString();
            TextView won = row.FindViewById<TextView>(Resource.Id.goalsScored);
            won.Text = myItems[position].won.ToString();
            TextView draw = row.FindViewById<TextView>(Resource.Id.goalsReceived);
            draw.Text = myItems[position].draw.ToString();
            TextView lost = row.FindViewById<TextView>(Resource.Id.goalDiference);
            lost.Text = myItems[position].lost.ToString();
            TextView goalDiference = row.FindViewById<TextView>(Resource.Id.goalsHome);
            goalDiference.Text = myItems[position].goalDiference.ToString();
            TextView points = row.FindViewById<TextView>(Resource.Id.points);
            points.Text = myItems[position].numOfPoints.ToString();

            return row;
        }
    }
}