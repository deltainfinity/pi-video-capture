using System.IO;
using System.Net.Sockets;

namespace PiVideoClient
{
    //gets a video clip from the piCamera server and stores it to a file
    public class PiVideo 
    {
        //instance variables
        string videoName;
        string storagePath;
        int port;
        string hostname;

        //getters
        public string VideoName { get => videoName; }
        public int Port { get => port; }
        public string HostName { get => hostname; }
        //getter and setter for file storage path instance variable
        public string StoragePath { get => storagePath; set => storagePath = value; }

        //constructor
        public PiVideo(int piCameraPort, string piCameraHostName, string fileStoragePath)
        {
            //sanity checks
            if (piCameraPort <= 1024 || piCameraPort > 65536)
                throw new System.ArgumentOutOfRangeException("piCameraPort", piCameraPort, "Port value must be between 1025 and 65536!");
            if (piCameraHostName.Length == 0)
                throw new System.ArgumentNullException("piCameraHostname", "PiCamera host name cannot be an empty string!");
            if (fileStoragePath.Length == 0)
                throw new System.ArgumentNullException("fileStoragePath", "File storage path cannot be an empty string!");
            //assign values for the hostname and port where piCamera server is located
            port = piCameraPort;
            hostname = piCameraHostName;
            //set the location where the video file is to be stored
            storagePath = fileStoragePath;
        }

        //get a new video clip from the piCamera server and write it to a file
        public void GetNewPiCameraVideo() {
    
            //create a new socket and a network stream to receive the data
            TcpClient piClient = new TcpClient();
            NetworkStream piVideoStream;
            try
            {
                //connect to the piCamera server
                piClient.Connect(hostname, port);
                //get the network stream and write the stream to a file
                piVideoStream = piClient.GetStream();
                byte[] data = new byte[4192];
                using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(storagePath + '\\' + videoName)))
                {
                    while (piVideoStream.CanRead)
                    {
                        piVideoStream.Read(data, 0, 4192);
                        bw.Write(data);
                    }
                }
                
                piVideoStream.Close();
            }
            finally
            { 
                piClient.Close();
            }
        }
    }
}
