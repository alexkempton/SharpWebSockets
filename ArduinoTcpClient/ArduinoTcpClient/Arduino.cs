using System;
using System.IO.Ports;
namespace ArduinoTcpClient
{
	public class Arduino
	{
		private readonly SerialPort _port;
        private string _lastData = "";
        private bool _stopped = false;
		Controller controller;
		public Arduino (Controller controller)
		{
			this.controller = controller;
			_port = new SerialPort("COM3") {BaudRate = 9600};
            _port.DataReceived += _port_DataReceived;
            _port.Open();
		}
		
		
		 private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           string data = _port.ReadTo("y");
			Console.WriteLine(data);
			
			controller.Eventhandler.Set();
			string msg;
			lock(controller.Locker){
				msg = controller.Message;
					
			}
			
		}
}
}
