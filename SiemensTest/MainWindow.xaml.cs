using System;
using System.Collections.Generic;
using System.Linq;
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
using S7.Net;

namespace SiemensTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Example: https://angolodiwindows.com/2016/12/gestione-plc-con-s7-net-library-parte-uno/
    /// </summary>
    public partial class MainWindow : Window
    {

        Plc _plc = null;

        public MainWindow()
        {
            InitializeComponent();
        }
        // scrittura
        private void btnRESetBit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_plc != null && _plc.IsConnected)
                {
                    string node = this.tbkWriteNode.Text;
                    string value = this.tbkValoreToSend.Text;

                    _plc.Write(node, value);
                    this.WriteLog($"Write to node: {node}");
                }
            }
            catch (Exception ex)
            {
                this.WriteLog(ex.Message);
            }
        }

        // lettura
        private void btnSetBit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_plc != null && _plc.IsConnected)
                {
                    DataType dataType = (DataType)this.cmboxDataType.SelectedItem;
                    int db = int.Parse(this.tbkNumDB.Text);
                    int startByteAddr = int.Parse(this.tbkStartAddr.Text);
                    VarType varType = (VarType)this.cmboxNodeType.SelectedItem;
                    int varCount = int.Parse(this.tbkVarCount.Text);
                    byte bitAdr = byte.Parse(this.tbkBitCount.Text);

                    var value = _plc.Read(dataType, db, startByteAddr, varType, varCount, bitAdr);
                    this.WriteLog($"Read on {dataType}, db {db} the following value {value}");
                }
            }
            catch (Exception ex)
            {
                this.outputTexBlock.Text = ex.Message;
            }
        }

        // connessione
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_plc != null && _plc.IsConnected)
                {
                    _plc.Close();
                    this.btnConnect.Content = "Connect";
                    this.btnSetBit.IsEnabled = false;
                    this.btnRESetBit.IsEnabled = false;
                    this.WriteLog($"Disconnected from {_plc.IP}");
                }
                else
                {
                    string ip = tbxImpostaVariabile.Text;
                    short rack = short.Parse(this.Rack.Text);
                    short slot = short.Parse(this.Slot.Text);
                    CpuType cpuType;

                    if ((bool)this.D.IsChecked)
                    {
                        cpuType = CpuType.S7200;
                    }
                    else if ((bool)this.T.IsChecked)
                    {
                        cpuType = CpuType.S7300;
                    }
                    else if ((bool)this.Q.IsChecked)
                    {
                        cpuType = CpuType.S7400;
                    }
                    else if ((bool)this.MC.IsChecked)
                    {
                        cpuType = CpuType.S71500;
                    }
                    else
                    {
                        cpuType = CpuType.S71200;
                    }

                    _plc = new Plc(cpuType, ip, rack, slot);
                    _plc.Open();
                    this.btnConnect.Content = "Disconnect";
                    this.btnSetBit.IsEnabled = true;
                    this.btnRESetBit.IsEnabled = true;
                    this.WriteLog($"Connected from {_plc.IP}");
                }
            }
            catch (Exception ex)
            {
                this.outputTexBlock.Text = ex.Message;
            }
        }

        /// <summary>
        /// Write the log
        /// </summary>
        /// <param name="txt"></param>
        private void WriteLog(string txt)
        {
            this.outputTexBlock.Text += string.Concat("\n[", DateTime.Now, "] - ", txt);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.cmboxNodeType.ItemsSource = Enum.GetValues(typeof(S7.Net.VarType)).Cast<S7.Net.VarType>();
                this.cmboxNodeType.SelectedIndex = 0;
                this.cmboxDataType.ItemsSource = Enum.GetValues(typeof(S7.Net.DataType)).Cast<S7.Net.DataType>();
                this.cmboxDataType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
