using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

/* =====
 * Run this with emulator 15 (Android 4.0 and up, i think) 
 * =====
*/

namespace Glubenheim
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, MainLauncher = true)]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			// Removes the huge titlebar in the top of the app  
			Window.RequestFeature (WindowFeatures.NoTitle);

			base.OnCreate (bundle);

			// Set the view as the main.axml layout 
			SetContentView (Resource.Layout.Main);

			// ImageButton
			ImageButton imgButton = FindViewById<ImageButton> (Resource.Id.myImageButton);

			imgButton.Click += delegate { 
				DisplayCustomToast("Image clicked!");
			};

			// Two buttons and events 
			Button button1 = FindViewById<Button> (Resource.Id.myButton1);
			Button button2 = FindViewById<Button> (Resource.Id.myButton2);

			button1.Click += delegate {
				button1.Text = string.Format ("Thanks! {0} clicks.", count++); 
			};

			Button showPopupMenu = FindViewById<Button> (Resource.Id.popupButton);
			showPopupMenu.Click += (s, arg) => {
				PopupMenu menu = new PopupMenu (this, showPopupMenu);
				menu.Inflate (Resource.Drawable.popup_menu);

				menu.MenuItemClick += (s1, arg1) => {
					Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);

				};

				menu.Show (); 
			}; 
		} 

		// Creating a toast with a given text 
		public void DisplayCustomToast (string stringText)
		{
			Toast toast = Toast.MakeText(this, stringText, ToastLength.Short);
			toast.SetGravity(GravityFlags.Bottom, 0, 0);
			toast.Show();
		}
	}
}


