using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LCPLibrary;


namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int UDPRecievePort = 5555;

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(ReceiveUDPMessageAsync);

        }


        async Task ReceiveUDPMessageAsync()
        {
            UdpClient receiver = new UdpClient(UDPRecievePort);
            while (true)
            {
                var result = await receiver.ReceiveAsync();
                var message = Encoding.UTF8.GetString(result.Buffer);
                Command? com = Command.Decrypt(message);
                if (com != null)
                {
                    Debug.WriteLine(com.Encrypt());
                    RunCommand(com.com, com.args);
                }
                else Debug.WriteLine("Null Command");

            }
        }

        void RunCommand(int com, string[]? args = null)
        {
            using UdpClient udpClient = new UdpClient();
            switch (com)
            {
                case (int)CommandList.Ping:
                    Debug.WriteLine("Executing command " + (int)CommandList.Ping);
                    string remoteIP;
                    int remotePort = 0;
                    try
                    {
                        remoteIP = args[0];
                        remotePort = Convert.ToInt32(args[1]);
                    }
                    catch
                    {
                        Debug.WriteLine("Command syntax error");
                        return;
                    }
                    Command command = new Command((int)CommandList.Pong);
                    string str = command.Encrypt();
                    var data = Encoding.UTF8.GetBytes(str);
                    udpClient.Send(data, data.Length, remoteIP, remotePort);
                    break;

            }
        }

    }
}
