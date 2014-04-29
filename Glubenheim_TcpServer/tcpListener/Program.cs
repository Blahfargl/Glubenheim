using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace tcpListener
{
	class MainClass : System.Windows.Forms.Form
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

						// Possibly deleteable -----
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

		// What happens when a message is received
		// Sends keystrokes to the active window 
		public static void msgReceived (string msg)
		{
			switch(msg)
			{
			case "button1":
				SendKeys.SendWait ("{UP}");
				break;
			case "button2":
				SendKeys.SendWait ("{DOWN}");
				break;
			case "button3":
				SendKeys.SendWait ("{LEFT}");
				break;
			case "button4":
				SendKeys.SendWait ("{RIGHT}");
				break;
			case "button2":
				SendKeys.SendWait ("");
				break;
			case "button2":
				SendKeys.SendWait ("");
				break;
			case "button2":
				SendKeys.SendWait ("{DOWN}");
				break;
			case "button2":
				SendKeys.SendWait ("{DOWN}");
				break;
			case "button2":
				SendKeys.SendWait ("{DOWN}");
				break;
			case "button2":
				SendKeys.SendWait ("{DOWN}");
				break;
			default:
				// for random input text
				SendKeys.SendWait (msg);
					break;
			}
		}

		// Setting a new mouse position 
		public static void mousePos(int new_x, int new_y)
		{
			int crnt_x = System.Windows.Forms.Control.MousePosition.X;
			int crnt_y = System.Windows.Forms.Control.MousePosition.Y;
			Cursor.Position = new Point (crnt_x + new_x, crnt_y + new_y);
		}

		private static string ipFinder(){ //Finds the last local ip address 
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


