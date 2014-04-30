using System;
using System.Net.Sockets;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;

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
		string ipAddress = "192.168.1.16"; 

		// Build the apllication
		protected override void OnCreate (Bundle bundle)
		{
			// Removes the huge titlebar in the top of the app  
			Window.RequestFeature (WindowFeatures.NoTitle);

			base.OnCreate (bundle);

			// Set the start layout 
			SetLayout1 ();
		} 

		//----------------------------------------------------------------------------------------------- Setting up the layouts
		public void SetLayout1()
		{
			SetContentView (Resource.Layout.LLO1);

			SetPopUpAndHelp ();

			ImageButton UpButton = FindViewById<ImageButton> (Resource.Id.UpButton);
			ImageButton DownButton = FindViewById<ImageButton> (Resource.Id.DownButton);
			ImageButton LeftButton = FindViewById<ImageButton> (Resource.Id.LeftButton);
			ImageButton RightButton = FindViewById<ImageButton> (Resource.Id.RightButton);

			ImageButton SelectButton = FindViewById<ImageButton> (Resource.Id.SelectButton);
			ImageButton StartButton = FindViewById<ImageButton> (Resource.Id.StartButton);

			ImageButton AButton = FindViewById<ImageButton> (Resource.Id.AButton);
			ImageButton BButton = FindViewById<ImageButton> (Resource.Id.BButton);

			UpButton.Click += delegate {
				Connect (ipAddress, "UpButton");
			};
			DownButton.Click += delegate {
				Connect (ipAddress, "DownButton");
			};
			LeftButton.Click += delegate {
				Connect (ipAddress, "LeftButton");
			};
			RightButton.Click += delegate {
				Connect (ipAddress, "RightButton");
			};

			SelectButton.Click += delegate {
				Connect (ipAddress, "SelectButton");
			};
			StartButton.Click += delegate {
				Connect (ipAddress, "StartButton");
			};

			AButton.Click += delegate {
				Connect (ipAddress, "AButton");
			};
			BButton.Click += delegate {
				Connect (ipAddress, "BButton");
			};
		}

		public void SetLayout2()
		{
			SetContentView (Resource.Layout.LLO2);

			SetPopUpAndHelp ();

			ImageButton B1Button = FindViewById<ImageButton> (Resource.Id.Bumper1Button);
			ImageButton B2Button = FindViewById<ImageButton> (Resource.Id.Bumper2Button);

			ImageButton UpButton = FindViewById<ImageButton> (Resource.Id.UpButton);
			ImageButton DownButton = FindViewById<ImageButton> (Resource.Id.DownButton);
			ImageButton LeftButton = FindViewById<ImageButton> (Resource.Id.LeftButton);
			ImageButton RightButton = FindViewById<ImageButton> (Resource.Id.RightButton);

			ImageButton SelectButton = FindViewById<ImageButton> (Resource.Id.SelectButton);
			ImageButton StartButton = FindViewById<ImageButton> (Resource.Id.StartButton);

			ImageButton AButton = FindViewById<ImageButton> (Resource.Id.AButton);
			ImageButton BButton = FindViewById<ImageButton> (Resource.Id.BButton);
			ImageButton YButton = FindViewById<ImageButton> (Resource.Id.YButton);
			ImageButton XButton = FindViewById<ImageButton> (Resource.Id.XButton);

			B1Button.Click += delegate {
				Connect(ipAddress, "Bumper1Button");
			};
			B2Button.Click += delegate {
				Connect(ipAddress, "Bumper2Button");
			};

			UpButton.Click += delegate {
				Connect (ipAddress, "UpButton");
			};
			DownButton.Click += delegate {
				Connect (ipAddress, "DownButton");
			};
			LeftButton.Click += delegate {
				Connect (ipAddress, "LeftButton");
			};
			RightButton.Click += delegate {
				Connect (ipAddress, "RightButton");
			};

			SelectButton.Click += delegate {
				Connect (ipAddress, "SelectButton");
			};
			StartButton.Click += delegate {
				Connect (ipAddress, "StartButton");
			};

			AButton.Click += delegate {
				Connect (ipAddress, "AButton");
			};
			BButton.Click += delegate {
				Connect (ipAddress, "BButton");
			};
			YButton.Click += delegate {
				Connect (ipAddress, "YButton");
			};
			XButton.Click += delegate {
				Connect (ipAddress, "XButton");
			};
		}

		public void SetLayout3()
		{
			SetContentView (Resource.Layout.LLO3);

			// Hide keyboard when enter screen
			Window.SetSoftInputMode(SoftInput.StateHidden);

			SetPopUpAndHelp ();

			ImageButton TouchPad = FindViewById<ImageButton> (Resource.Id.TouchPad);
			ImageButton MMidButton = FindViewById<ImageButton> (Resource.Id.MMidButton);
			ImageButton MRightButton = FindViewById<ImageButton> (Resource.Id.MRightButton);
			ImageButton MLeftButton = FindViewById<ImageButton> (Resource.Id.MLeftButton);

			ImageButton SendButton = FindViewById<ImageButton> (Resource.Id.SendButton);
			ImageButton ClearButton = FindViewById<ImageButton> (Resource.Id.ClearButton);

			EditText eText = FindViewById<EditText> (Resource.Id.eText);

			TouchPad.SetOnTouchListener(this);

			MMidButton.Click += delegate {
				Connect (ipAddress, "MMidButton");
			};
			MRightButton.Click += delegate {
				Connect (ipAddress, "MRightButton");
			};
			MLeftButton.Click += delegate {
				Connect (ipAddress, "MLeftButton");
			};

			SendButton.Click += delegate {
				// Only send if the text has more than 0 digits
				if(eText.Text.Length != 0)
				{
					Connect(ipAddress, eText.Text);
					eText.Text = "";
				}

				// Remove keyboard automatically when button is clicked
				var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
				inputManager.HideSoftInputFromWindow(eText.WindowToken, HideSoftInputFlags.None);
			};

			// Clear the text in the text field
			ClearButton.Click += delegate {
				eText.Text = "";
			};
		}

		//----------------------------------------------------------------------------------------------- PopUp button and help button
		// PopUp button and help button appear in all layouts
		public void SetPopUpAndHelp()
		{
			// Build and shows a PopupMenu, used for the selection of layouts
			ImageButton showPopupMenu = FindViewById<ImageButton> (Resource.Id.PopUp); 
			showPopupMenu.Click += (s, arg) => {
				PopupMenu menu = new PopupMenu (this, showPopupMenu); // Here the button is made into a popupMenu
				menu.Inflate (Resource.Menu.popup_menu); // Loads our popup_menu.xml

				menu.MenuItemClick += (s1, arg1) => { 
					// This switch case checks which item (layout) has been clicked
					// and sets the layout accordingly
					switch (arg1.Item.ItemId)
					{
						case Resource.Id.item1:
							SetLayout1();
							break;
						case Resource.Id.item2:
							SetLayout2();
							break;
						case Resource.Id.item3:
							SetLayout3();
							break;
					}
				};

				menu.DismissEvent += (s2, arg2) => {
					Console.WriteLine ("menu dismissed"); 
				};

				// This line draws the menu
				menu.Show (); 
			};

			// Opens a dialog (popup screen) when clicking the help button
			ImageButton helpButton = FindViewById<ImageButton> (Resource.Id.HelpButton);
			helpButton.Click += OpenHelpDialog;
		}

		void OpenHelpDialog (object sender, EventArgs e)
		{
			// Creates the help dialog and sets a title, bodytext, and 'OK' button
			var helpDialog = (new AlertDialog.Builder (this)).Create ();
			helpDialog.SetTitle ("Help");
			helpDialog.SetMessage (
				"The application currently have 3 layouts that can all be changed between by pressing the “layout” button. " +
				"The buttons are by default bound to the following, but can (currently only) be changed in the code inside the tcpListener:\n" +
				"Up, down, left and right arrows: Bound to their respective arrow keys " +
				"\n\tStart: q " +
				"\n\tSelect: p " +
				"\n\tA: x" +
				"\n\tB: z" +
				"\n\tX: (Not bound)" +
				"\n\tY: (Not bound)" +
				"\n\tLeft Bumper: (not nound)" +
				"\n\tRight Bumper: (not bound)" +
				"\nLeft, right, and middle mouse buttons: Bound to their respective mouse buttons"
			);
			helpDialog.SetButton ("OK", delegate{ });
			helpDialog.Show ();
		}

		//----------------------------------------------------------------------------------------------- On touch listener
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
					// Get the difference between current position and old position
					new_x = e.GetX () - old_x;
					new_y = e.GetY () - old_y;
					// Convert to int, to remove decimal numbers (apparently can't be send through the tcp listener)
					int_x = Convert.ToInt32 (new_x);
					int_y = Convert.ToInt32 (new_y);
					// Convert to string, so it can be send
					send_x = Convert.ToString (int_x);
					send_y = Convert.ToString (int_y);
					
					// Send x and y position over two messages
					Connect (ipAddress, send_x);
					Connect (ipAddress, send_y);
					
					// Set old position to current position
					old_x = e.GetX ();
					old_y = e.GetY ();
					break;
			}
			return true;
		}

		//----------------------------------------------------------------------------------------------- Connection to tcp server
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


