using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.Diagnostics;
using LCPLibrary;

namespace Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        int UDPRecievePort = 4444;
        int UDPDestPort = 5555;

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
                }
                else Debug.WriteLine("Null Command");

            }
        }


        public void SendBroadCastMessage(int remotePort, Command command)
        {
            using UdpClient udpClient = new UdpClient();
            string str = command.Encrypt();
            var data = Encoding.UTF8.GetBytes(str);

            Debug.WriteLine("bc message sended: " + str);
            udpClient.Send(data, data.Length, IPAddress.Broadcast.ToString(), remotePort);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MyIP.IPv4 != null)
            {
                Command command = new Command((int)CommandList.Ping, new string[] { MyIP.IPv4.ToString(), UDPRecievePort.ToString() });
                SendBroadCastMessage(UDPDestPort, command);
            }

        }
    }
}
