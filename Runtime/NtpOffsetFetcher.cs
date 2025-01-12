// namespace NuGet_IID;

// public class SendUdpIID
// {

// }


// # https://buymeacoffee.com/apintio

// import re
// import struct
// import random
// import socket
// import threading
// import websockets
// import asyncio
// import time
// import ntplib

// ####### IID ###

using System;

namespace Eloi.IID
{

    public static class NtpOffsetFetcher
    {
        private static int defaultGlobalNtpOffsetInMilliseconds = 0;

        public static int FetchNtpOffsetInMilliseconds(string ntpServer)
        {
            try
            {
                var ntpData = new byte[48];
                ntpData[0] = 0x1B;
                var addresses = System.Net.Dns.GetHostEntry(ntpServer).AddressList;
                var ipEndPoint = new System.Net.IPEndPoint(addresses[0], 123);
                using (var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp))
                {
                    socket.Connect(ipEndPoint);
                    socket.Send(ntpData);
                    socket.Receive(ntpData);
                }

                ulong intPart = BitConverter.ToUInt32(ntpData, 40);
                ulong fractPart = BitConverter.ToUInt32(ntpData, 44);
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);
                var milliseconds = (intPart * 1000 + (fractPart * 1000) / 0x100000000L);
                var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);
                var offset = (networkDateTime - DateTime.UtcNow).TotalMilliseconds;
                return (int)offset;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error NTP Fetch: {ntpServer} {e}");
                return 0;
            }
        }

        public static void SetGlobalNtpOffsetInMilliseconds(string ntpServer = null)
        {
            try
            {
                if (ntpServer == null)
                {
                    ntpServer = IIDUtility.DefaultNtpServer;
                }
                var offset = FetchNtpOffsetInMilliseconds(ntpServer);
                defaultGlobalNtpOffsetInMilliseconds = offset;
                Console.WriteLine($"Default Global NTP Offset: {defaultGlobalNtpOffsetInMilliseconds} {ntpServer}");
            }
            catch (Exception)
            {
                defaultGlobalNtpOffsetInMilliseconds = 0;
            }
        }

        public static int GetGlobalNtpOffsetInMilliseconds()
        {
            return defaultGlobalNtpOffsetInMilliseconds;
        }

        private static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) + ((x & 0x0000ff00) << 8) + ((x & 0x00ff0000) >> 8) + ((x & 0xff000000) >> 24));
        }
    } 
}


    

    
    
  

    
        
        
            
            
        
        
        
        
 
        
        
        
        
        
        
        
        
        
        
        

    
        
   
        
   
        
        
    
     


    
    



        

        
                
        
        
        
        



        
    
    
            
    
        
        
                    
                    
                    
                    
                    
       
       
       
    
        
            
            


                








        
        
        
        
        
        
        
    
        
    




















































    
    
            
            
        
    


            
        
        
                


            
            
        
    

        


   

        
 

 

        
    



        
        























    
        


      
    


        
            
            
            