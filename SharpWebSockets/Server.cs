using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
namespace SharpWebSockets
{
		internal class Server{
		Controller controller;
		TcpClient client;
		
		internal Server(Controller controller, TcpClient client){
			this.controller = controller;
			this.client = client;
			
			StartHandshake();
		}
		
		
		
		private void StartHandshake(){
			
			using(NetworkStream stream = client.GetStream())
                    {
						byte[] bytes = new byte[500];
           			 	int numberOfBytesRead = 0;
                       	numberOfBytesRead = numberOfBytesRead + stream.Read(bytes,0,bytes.Length);
						// Web client
						if(FigureOutProtocol(bytes)=="websocket76"){
							Console.WriteLine("Start communicating");
							MakeWebSockets76Handshake(stream, bytes,numberOfBytesRead);	
									while(true){
									bytes = new byte[500];			
									try {
										stream.Read(bytes,0,bytes.Length);
										}
									
									catch (IOException e) 
										{
										break;
										}
										CommunicateToWebClient(stream);
										
										}
					
							Console.WriteLine("Web client closed");
						
							}
				
						// Arduino client
						else {
								stream.Read(bytes,0,bytes.Length);
								Console.WriteLine("Arduino client connected");
								int count = 1;
								while(count>0){
									bytes = new byte[500];			
									try {
										count = stream.Read(bytes,0,bytes.Length);
										}
									
									catch (IOException e) 
										{
										break;
										}
										
									
									CommunicateToArduinoClient(bytes);
									
											
										}
								
								Console.WriteLine("Arduino client closed");
					
							}
				
		              }		
			client.Close();
			
				
		}
		
		
		private string FigureOutProtocol(byte[] bytes){
			string beginning = System.Text.Encoding.UTF8.GetString(bytes);
			Regex websockets76 = new Regex("Sec-WebSocket-Key2");
			if(websockets76.IsMatch(beginning)) {
				return "websocket76";
			}
			
			else return "client";
			
		}
		
		private void MakeWebSockets76Handshake(NetworkStream stream, byte[] bytes,int numberOfBytesRead){
			Handshake76 handshake = new Handshake76();
			byte[] handshakeCode = handshake.HandleClientHandshake(bytes, numberOfBytesRead);
			byte[] handshakeResponse = handshake.GetHandShakeResponse(handshakeCode);
			stream.Write(handshakeResponse,0,handshakeResponse.Length);			
		}
		
		
		private void CommunicateToWebClient(NetworkStream stream){
			controller.Eventhandler.WaitOne();
			string msg;
			lock(controller.Locker){
				msg = controller.Message;
					
			}
			if(!stream.CanWrite)return;
			   	byte startByte = 0x00;
        		byte endByte = 0xFF;
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg);
				byte[] frame = new byte[bytes.Length+2];
				frame[0] = startByte;
				frame[frame.Length-1]=endByte;
				Array.Copy(bytes,0,frame,1,bytes.Length);	
				stream.Write(frame,0,frame.Length);
				controller.Eventhandler.Reset();
			
		}
		
		private void CommunicateToArduinoClient(byte[] bytes){
			string msg = System.Text.Encoding.UTF8.GetString(bytes);
			lock (controller.Locker) controller.Message = msg;
			controller.Eventhandler.Set();
			
			
			
		}
		
		
		
		
		
		
	}
}

