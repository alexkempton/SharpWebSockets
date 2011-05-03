using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;


namespace ArduinoTcpClient
{
	public class Controller
	{
		public EventWaitHandle Eventhandler = new ManualResetEvent(false);
		public object Locker = new object();
		public string Message {get;set;}
		public Controller ()
		{
			// string ip = IPAddress.Loopback.ToString();
			string ip = "46.137.97.137";
			Connect(ip);
			
			/*
			 *  Task.Factory.StartNew(() =>
					{
						Connect(ip);
					});
			  Task.Factory.StartNew(() =>
					{
						new Arduino(this);
					});
			
			 */
			
		}
			
			public void Connect(String server) 
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


