using System;
using System.Net.Sockets;
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
	// To get the ScreenOrientation as landscape
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
	// Activity is an android class. View.IOnTouchListener checks if the screen has been touched
	public class MainActivity : Activity, View.IOnTouchListener
	{
		// The ip address of the server
		// Needs to be changed depending on which network is used
		string ipAddress = "192.168.1.15";

		// Build the apllication
		protected override void OnCreate (Bundle bundle)
		{
			// Removes the huge titlebar in the top of the app  
			Window.RequestFeature (WindowFeatures.NoTitle);

			base.OnCreate (bundle);

			// Set the view as the main.axml layout 
			SetContentView (Resource.Layout.Main);

			// ImageButton
			ImageButton imgButton = FindViewById<ImageButton> (Resource.Id.myImageButton);

			// In case of a click
			imgButton.Click += delegate { 
				DisplayCustomToast("Image clicked!");
			};

			// Buttons and events 
			Button button1 = FindViewById<Button> (Resource.Id.myButton1);
			Button button2 = FindViewById<Button> (Resource.Id.myButton2);
			Button button3 = FindViewById<Button> (Resource.Id.myButton3);
			Button button4 = FindViewById<Button> (Resource.Id.myButton4);

			//button4.SetOnTouchListener (this);

			button1.Click += delegate {
				Connect (ipAddress, "button1");
			};

			button2.Click += delegate {
				Connect (ipAddress, "button2");
			};

			button3.Click += delegate {
				Connect (ipAddress, "button3");
			};

			button4.Click += delegate {
				Connect (ipAddress, "button4");
			};

			// Build and shows a PopupMenu, used for the selection of layouts
			Button showPopupMenu = FindViewById<Button> (Resource.Id.popupButton); // Finds the popupBUtton in the class Main and call it showPopupMenu
			showPopupMenu.Click += (s, arg) => {
				PopupMenu menu = new PopupMenu (this, showPopupMenu); // Here the button is made into a popupMenu
				menu.Inflate (Resource.Menu.popup_menu); // Loader our popup_Menu.xml

				menu.MenuItemClick += (s1, arg1) => { 
					// This switch case checks which item (layout) has been clicked
					switch (arg1.Item.ItemId)
					{
						case Resource.Id.item1:
							DisplayCustomToast("Item 1");
							break;
						case Resource.Id.item2:
							DisplayCustomToast("Item 2");
							break;
						case Resource.Id.item3:
							DisplayCustomToast("Item 3");
							break;
					}
				};

				menu.DismissEvent += (s2, arg2) => {
					Console.WriteLine ("menu dismissed"); 
				};

				// This line draws the menu
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

		float old_x = 0;
		float old_y = 0;
		float new_x = 0;
		float new_y = 0;
		Int32 int_x = 0;
		Int32 int_y = 0;
		string send_x = null;
		string send_y = null;

		// Check touch position on button 
		public bool OnTouch(View v, MotionEvent e)
		{
			switch (e.Action)
			{
				// Get the x and y position for a touch (always before move)
				case MotionEventActions.Down:
					old_x = e.GetX ();
					old_y = e.GetY ();
					Console.WriteLine ("x = " + old_x + " y = " + old_y);
					break;
				// Get the x and y position difference continously
				case MotionEventActions.Move:
					new_x = e.GetX () - old_x;
					new_y = e.GetY () - old_y;
					int_x = Convert.ToInt32 (new_x);
					int_y = Convert.ToInt32 (new_y);
					send_x = Convert.ToString (int_x);
					send_y = Convert.ToString (int_y);

					// Send x and y position over to messages
					Connect (ipAddress, send_x);
					Connect (ipAddress, send_y);

					old_x = e.GetX ();
					old_y = e.GetY ();
					break;
			}
			return true;
		}

		// Connect with Tcp server on computer,
		// while sending a message (server = ipAddress of computer)
		static void Connect(String server, String message) 
		{
			try 
			{
				// Create a TcpClient. 
				// Note, for this client to work you need to have a TcpServer  
				// connected to the same address as specified by the server, port combination.
				Int32 port = 2814;
				TcpClient client = new TcpClient(server, port);

				// Translate the passed message into ASCII and store it as a Byte array.
				Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);         

				// Get a client stream for reading and writing. 
				NetworkStream stream = client.GetStream();

				// Send the message to the connected TcpServer. 
				stream.Write(data, 0, data.Length);

				Console.WriteLine("Sent: {0}", message);         

				// Receive the TcpServer.response. - could be deleted

				// Buffer to store the response bytes.
				data = new Byte[256];

				// String to store the response ASCII representation.
				String responseData = String.Empty;

				// Read the first batch of the TcpServer response bytes.
				Int32 bytes = stream.Read(data, 0, data.Length);
				responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
				Console.WriteLine("Received: {0}", responseData);         

				// Close everything.
				stream.Close();         
				client.Close();         
			} 
			catch (ArgumentNullException e) 
			{
				Console.WriteLine("ArgumentNullException: {0}", e);
			} 
			catch (SocketException e) 
			{
				Console.WriteLine("SocketException: {0}", e);
			}
		}
	}
}


