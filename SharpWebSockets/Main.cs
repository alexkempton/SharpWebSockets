using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace SharpWebSockets
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("WebSockets server started");
			new Controller();
		}
	}
	
	
	internal class Controller{
		public EventWaitHandle Eventhandler = new ManualResetEvent(false);
		public object Locker = new object();
		public string Message {get;set;}
		
		internal Controller(){
			ThreadPool.SetMinThreads (50, 50);
			ThreadPool.SetMaxThreads(50,50);
			IPAddress addr = IPAddress.Parse("46.137.97.137");
			TcpListener listener = new TcpListener(IPAddress.Any, 8181);
			
            listener.Start();
			
			while (true)
	    	{
				TcpClient client = listener.AcceptTcpClient();
     			Task.Factory.StartNew(() =>
					{
						Server s = new Server(this,client);
					});
  	  		}		
		}	
	}
	
}
