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
                Debug.WriteLine("recieved new message: " + message);
                Command? com = Command.Decrypt(message);
                RunCommand(com);
            }
        }

        void RunCommand(Command? com)
        {
            if (com == null)
            {
                Debug.WriteLine("Null Command");
                return;
            }

            using UdpClient udpClient = new UdpClient();
            Debug.WriteLine("Executing command " + (int)CommandList.Ping);
            switch (com.com)
            {
                case (int)CommandList.Ping:
                    if(com.args == null || com.args.Length < 2)
                    {
                        Debug.WriteLine("Command syntax error");
                        return;
                    }
                    string remoteIP = com.args[0];
                    int remotePort = Convert.ToInt32(com.args[1]);
                    Command command = new Command((int)CommandList.Pong);
                    string message = command.Encrypt();
                    Debug.WriteLine("Sended message: " + message);
                    var data = Encoding.UTF8.GetBytes(message);
                    udpClient.Send(data, data.Length, remoteIP, remotePort);
                    break;

            }
        }

    }
}
