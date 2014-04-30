using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace tcpListener
{
	class MainClass : Form
	{
		public static void Main (string[] args)
		{
			Int32 numData = 0;
			int new_x = 0;
			int new_y = 0;
			bool XorY = false; // x = false, y = true
			bool intOrString = false; // string = false, int = true

			TcpListener server = null;   

			try
			{
				// Set the TcpListener on some port.
				Int32 port = 2814; 
				// Your local ipAddres. 
				// Open cmd -> write "ipconfig" -> see IPv4-address. 
				IPAddress localAddr = IPAddress.Parse(ipFinder()); 

				server = new TcpListener(localAddr, port); 

				// Start listening for client requests. 
				server.Start(); 

				// Buffer for reading data 
				Byte[] bytes = new Byte[256]; 
				String data = null; 

				// Enter the listening loop
				while(true)
				{
					Console.WriteLine("Local Ip Address: " + localAddr);
					Console.Write("Waiting for a connection... ");

					// Perform a blocking call to accept requests
					TcpClient client = server.AcceptTcpClient();
					Console.WriteLine("Connected!");

					data = null;

					// Get a stream object for reading and writing
					NetworkStream stream = client.GetStream();

					int i;

					// Loop to receive all the data sent by the client
					while((i = stream.Read(bytes, 0, bytes.Length))!=0) 
					{
						// Translate data bytes to an ASCII string
						data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
						Console.WriteLine("Received: {0}", data);

						// Check if string is a series of digits
						try
						{
							numData = Convert.ToInt32(data);
							Console.WriteLine("Input string is a number");
							intOrString = true;
						}
						catch (FormatException)
						{
							Console.WriteLine("Input string not a number");
							intOrString = false;
						}

						// If it's not a number, else if it is a number
						if (intOrString == false)
						{
							msgReceived(data);
						} 
						else if (intOrString == true)
						{
							// To switch between x and y value 
							if (XorY == false)
							{
								new_x = numData;
								XorY = true;
							}
							else if (XorY == true)
							{
								new_y = numData;
								mousePos(new_x, new_y);
								XorY = false;
							}
						}

						// Process the data sent by the client
						data = data.ToUpper();

						byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

						// Send back a response
						stream.Write(msg, 0, msg.Length);
						Console.WriteLine("Sent: {0}", data);
					}

					// Shutdown and end connection
					client.Close();
				}
			}
			catch(SocketException e)
			{
				Console.WriteLine("SocketException: {0}", e);
			}
			finally
			{
				// Stop listening for new clients
				server.Stop();
			}

			Console.WriteLine("\nHit enter to continue...");
			Console.Read();
		}

		//----------------------------------------------------------------------------------------------- What happens when a message is received
		// Sends keystrokes to the active window based on the received message
		public static void msgReceived (string msg)
		{
			//A switchcase in which we set the keybinds of the button input from the android device.
			switch(msg)
			{
			case "UpButton":
				SendKeys.SendWait ("{UP}");
				break;
			case "DownButton":
				SendKeys.SendWait ("{DOWN}");
				break;
			case "LeftButton":
				SendKeys.SendWait ("{LEFT}");
				break;
			case "RightButton":
				SendKeys.SendWait ("{RIGHT}");
				break;
			case "SelectButton":
				SendKeys.SendWait ("p");
				break;
			case "StartButton":
				SendKeys.SendWait ("q");
				break;
			case "YButton":
				SendKeys.SendWait ("YButton");
				break;
			case "XButton":
				SendKeys.SendWait ("XButton");
				break;
			case "AButton":
				SendKeys.SendWait ("x");
				break;
			case "BButton":
				SendKeys.SendWait ("z");
				break;
			case "Bumper1Button":
				SendKeys.SendWait ("Bumper1Button");
				break;
			case "Bumper2Button":
				SendKeys.SendWait ("Bumper2Button");
				break;
			case "MMidButton":
				DoMouseMiddleClick ();
				break;
			case "MRightButton":
				DoMouseRightClick ();
				break;
			case "MLeftButton":
				DoMouseLeftClick ();
				break;
			default:
				// for input text
				SendKeys.SendWait (msg);
				break;
			}
		}

		//----------------------------------------------------------------------------------------------- Setting a new mouse position 
		public static void mousePos(int new_x, int new_y)
		{
			// Get current x and y mouse position
			int crnt_x = Control.MousePosition.X;
			int crnt_y = Control.MousePosition.Y;
			// Setting a new position by adding input value to current position 
			Cursor.Position = new Point (crnt_x + new_x, crnt_y + new_y);
		}

		//----------------------------------------------------------------------------------------------- Simulate mouse clicks
		// Import function for mouse clicks
		[DllImport("USER32.DLL")]
		public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

		private const int mouse_LeftDown = 0x02;
		private const int mouse_LeftUp = 0x04;
		private const int mouse_RightDown = 0x08;
		private const int mouse_RightUp = 0x10;
		private const int mouse_MiddleDown = 0x20;
		private const int mouse_MiddleUp = 0x40;

		public static void DoMouseLeftClick()
		{
			//Call the imported function at the cursor's current position
			int X = Cursor.Position.X;
			int Y = Cursor.Position.Y;
			mouse_event(mouse_LeftDown | mouse_LeftUp, X, Y, 0, 0);
		}
		public static void DoMouseRightClick()
		{
			//Call the imported function at the cursor's current position
			int X = Cursor.Position.X;
			int Y = Cursor.Position.Y;
			mouse_event(mouse_RightDown | mouse_RightUp, X, Y, 0, 0);
		}
		public static void DoMouseMiddleClick()
		{
			//Call the imported function at the cursor's current position
			int X = Cursor.Position.X;
			int Y = Cursor.Position.Y;
			mouse_event(mouse_MiddleDown | mouse_MiddleUp, X, Y, 0, 0);
		}

		//----------------------------------------------------------------------------------------------- Finds the last ip addres on the current list.
		private static string ipFinder(){
			IPHostEntry host;
			string localIP = "?";
			host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					localIP = ip.ToString();
				}
			}
			return localIP;
		}
	}
}


