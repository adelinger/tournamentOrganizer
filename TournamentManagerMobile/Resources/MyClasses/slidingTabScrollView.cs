using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.View;
using Android.App;
using Android.Content;
using Android.Util;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TournamentManagerMobile.Resources.MyClasses
{
   public class slidingTabScrollView : HorizontalScrollView
    {
        private const int TITLE_OFFSET_DIPS = 24;
        private const int TAB_VIEW_TEXT_SIZE_SP = 12;
        private const int TAB_VIEW_PADDING_DIPS = 16;

        private int mTitleOffset;

        private int mTabViewLayoutID;
        private int mTabViewTextViewID;

        private ViewPager mViewPager;
        private ViewPager.IOnPageChangeListener mViewPagerPageChangeListener;

        public static slidingTabStrip mTabStrip;

        private int mScrollState;

        public interface TabColorizer
        {
            int getIndicatorColor(int position);
            int getDividerColor  (int position);
        }

        public slidingTabScrollView(Context context) :this(context, null) 
        {

        }
        public slidingTabScrollView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {

        }
        public slidingTabScrollView(Context context, IAttributeSet attrs, int defaultStyle) : base (context, attrs, defaultStyle)
        {
            //disable the scroll bar
            HorizontalScrollBarEnabled = false;

            //make sure the tab strips fill the view
            FillViewport = true;
            this.SetBackgroundColor(Android.Graphics.Color.Rgb(0xE5, 0xE5, 0xE5)); //gray color

            mTitleOffset = (int)(TITLE_OFFSET_DIPS * Resources.DisplayMetrics.Density);

            mTabStrip = new slidingTabStrip(context);
            this.AddView(mTabStrip, LayoutParams.MatchParent, LayoutParams.MatchParent);
        }

        public TabColorizer customTabColorizer
        {
            set { mTabStrip.CustomTabColorizer = value; }
        }

        public int [] selectedIndicatorColor
        {
            set { mTabStrip.SelectedIndicatorColors = value; }
        }

        public int [] dividerColors
        {
            set { mTabStrip.DividerColors = value; }
        }
        public ViewPager.IOnPageChangeListener OnPageListener
        {
            set { mViewPagerPageChangeListener = value; }
        }
        public ViewPager viewPager
        {
            set
            {
                mTabStrip.RemoveAllViews();
                mViewPager = value;
                if (value != null)
                {
                    value.PageSelected += Value_PageSelected;
                    value.PageScrollStateChanged += Value_PageScrollStateChanged;
                    value.PageScrolled += Value_PageScrolled;
                    populateTabStrip();
                }
            }
        }

        private void Value_PageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
        {
            int tabCount = mTabStrip.ChildCount;

            if ((tabCount == 0) || (e.Position < 0) || (e.Position >= tabCount))
            {
                //if any of these conditions apply, return, no need to scroll
                return;
            }

            mTabStrip.OnViewPagerPageChanged(e.Position, e.PositionOffset);

            View selectedTitle = mTabStrip.GetChildAt(e.Position);

            int extraOffset = (selectedTitle != null ? (int)(e.Position * selectedTitle.Width) : 0);

            scrollToTab(e.Position, extraOffset);

            if (mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageScrolled(e.Position, e.PositionOffset, e.PositionOffsetPixels);
            }
            
        }

        private void populateTabStrip ()
        {
            PagerAdapter adapter = mViewPager.Adapter;
            for (int i = 0; i < adapter.Count; i++)
            {
                TextView tabView = createDefaultTabView(Context);
                tabView.Text = ((slidingTabsFragments.SamplePageAdapter)adapter).getHeaderTitle(i);
                tabView.SetTextColor(Android.Graphics.Color.Black);
                tabView.Tag = i;
                tabView.Click += TabView_Click;
                mTabStrip.AddView(tabView);
            }
        }

        private void TabView_Click(object sender, EventArgs e)
        {
            TextView clickTab = (TextView)sender;
            int pageToScrollTo = (int)clickTab.Tag;
            mViewPager.CurrentItem = pageToScrollTo;
        }

        private TextView createDefaultTabView(Android.Content.Context context)
        {
            TextView textview = new TextView(context);
            textview.Gravity = GravityFlags.Center;
            textview.SetTextSize(ComplexUnitType.Sp, TAB_VIEW_TEXT_SIZE_SP);
            textview.Typeface = Android.Graphics.Typeface.DefaultBold;

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Honeycomb)
            {
                TypedValue outValue = new TypedValue();
                context.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, outValue, false);
                textview.SetBackgroundResource(outValue.ResourceId);
            }

            if(Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.IceCreamSandwich)
            {
                textview.SetAllCaps(true);
            }
            int padding = (int)(TAB_VIEW_PADDING_DIPS * Resources.DisplayMetrics.Density);
            textview.SetPadding(padding, padding, padding, padding);

            return textview;
        }

        protected override void OnAttachedToWindow ()
        {
            base.OnAttachedToWindow();

            if (mViewPager != null)
            {
                scrollToTab(mViewPager.CurrentItem, 0);
            }
        }

        private void scrollToTab(int tabIndex, int extraOffset)
        {
            int tabCount = mTabStrip.ChildCount;

            if(tabCount == 0 || tabIndex < 0 || tabIndex >= tabCount)
            {
                //no need to go further, dont scroll
                return;
            }

            View selectedChild = mTabStrip.GetChildAt(tabIndex);
            if(selectedChild != null)
            {
                int scrollAmountX = selectedChild.Left + extraOffset;

                if (tabIndex > 0 ||  extraOffset > 0)
                {
                    scrollAmountX -= mTitleOffset;
                }

                this.ScrollTo(scrollAmountX, 0);
            }
        }

        private void Value_PageScrollStateChanged(object sender, ViewPager.PageScrollStateChangedEventArgs e)
        {
            mScrollState = e.State;

            if(mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageScrollStateChanged(e.State);
            }
        }

        private void Value_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if(mScrollState == ViewPager.ScrollStateIdle)
            {
                mTabStrip.OnViewPagerPageChanged(e.Position, 0f);
                scrollToTab(e.Position, 0);
            }

            if(mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageSelected(e.Position);
            }
        }
    }
}