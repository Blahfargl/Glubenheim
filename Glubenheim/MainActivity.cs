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
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, MainLauncher = true)]
	public class MainActivity : Activity, View.IOnTouchListener
	{
		// The ip address of the server 
		string ipAddress = "192.168.1.15"; 

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


			Button showPopupMenu = FindViewById<Button> (Resource.Id.popupButton);
			showPopupMenu.Click += (s, arg) => {
				PopupMenu menu = new PopupMenu (this, showPopupMenu);
				menu.Inflate (Resource.Menu.popup_menu);

				menu.MenuItemClick += (s1, arg1) => { 
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

		// Check touch position on button (Could be used to simulate mouse pad) 
		public bool OnTouch(View v, MotionEvent e)
		{
			switch (e.Action)
			{
				case MotionEventActions.Down:
					Console.WriteLine(e.GetX() + " " + e.GetY ());
					break;
				case MotionEventActions.Move:
					Console.WriteLine(e.GetX() + " " + e.GetY ());
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
				// connected to the same address as specified by the server, port 
				// combination.
				Int32 port = 2814;
				TcpClient client = new TcpClient(server, port);

				// Translate the passed message into ASCII and store it as a Byte array.
				Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);         

				// Get a client stream for reading and writing. 
				NetworkStream stream = client.GetStream();

				// Send the message to the connected TcpServer. 
				stream.Write(data, 0, data.Length);

				Console.WriteLine("Sent: {0}", message);         

				// Receive the TcpServer.response. 

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


