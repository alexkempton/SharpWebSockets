using System;
using System.Net;
using System.Net.Sockets;

namespace ArduinoTcpClient
{
	public class Controller
	{
		public Controller ()
		{
			string ip = IPAddress.Loopback.ToString();
			Connect(ip,"hello man");
			
		}
			
			public void Connect(String server, String message) 
				{
				  try 
				  {

				    Int32 port = 8181;
				    TcpClient client = new TcpClient(server, port);
				NetworkStream stream = client.GetStream();
				Byte[] data = System.Text.Encoding.UTF8.GetBytes("hello server");   
				stream.Write(data, 0, data.Length);
				
				while(true){
					Console.Write("Write something: ");
					string msg = Console.ReadLine();
					data = System.Text.Encoding.UTF8.GetBytes(msg);   
					stream.Write(data, 0, data.Length);
					Console.WriteLine("Sent: {0}", msg);   
					
				}
				
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
				
				  Console.WriteLine("\n Press Enter to continue...");
				  Console.Read();
				}


		}
}


