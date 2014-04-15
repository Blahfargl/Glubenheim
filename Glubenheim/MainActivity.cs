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

			button2.Click += delegate {
				Connect ("127.0.0.1", "Button 2 was clicked");
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
				//  Stream stream = client.GetStream();

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

			//Console.WriteLine("\n Press Enter to continue...");
			//Console.Read();
		}
	}
}


