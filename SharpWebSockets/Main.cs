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
			new Controller();
		}
	}
	
	
	internal class Controller{
		public EventWaitHandle Eventhandler = new ManualResetEvent(false);
		public object Locker = new object();
		public string Message {get;set;}
		//ManualResetEvent[] manualEvents = new ManualResetEvent[50];
		
		internal Controller(){
			ThreadPool.SetMinThreads (50, 50);
			ThreadPool.SetMaxThreads(50,50);
			TcpListener listener = new TcpListener(IPAddress.Loopback, 8181);
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
