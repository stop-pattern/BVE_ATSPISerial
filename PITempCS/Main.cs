using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;


namespace PITempCS
{

    /// <summary>メインの機能をここに実装する。</summary>
    public class Main
    {
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        private static State vehicleState = new();

        private static SerialPort myPort = null;
        private static StringBuilder COMnumber;
        private static bool door;

        static internal void Load()
        {
            Openini();
            OpenPort();

            if (myPort != null)
            {
                try
                {
                    myPort.Write(new byte[] { 0 }, 0, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ATSPISerial", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        static internal void Dispose()
        {
            ClosePort();
        }

        static internal void GetVehicleSpec(Spec s)
        {

        }

        static internal void Initialize(int s)
        {

        }
        static unsafe internal void Elapse(State st, int* Pa, int* Sa)
        {
            vehicleState = st;
            if (myPort != null) writePort();
        }

        static internal void SetPower(int p)
        {

        }

        static internal void SetBrake(int b)
        {

        }

        static internal void SetReverser(int r)
        {

        }
        static internal void KeyDown(int k)
        {

        }

        static internal void KeyUp(int k)
        {

        }

        static internal void DoorOpen()
        {
            door = true;
        }
        static internal void DoorClose()
        {
            door = false;
        }
        static internal void HornBlow(int h)
        {

        }
        static internal void SetSignal(int s)
        {

        }
        static internal void SetBeaconData(Beacon b)
        {

        }

        static internal void Openini()
        {

            int capacitySize = 256;

            COMnumber = new StringBuilder(capacitySize);
            string iniFileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/ATSPISerial.ini";
            uint ret = GetPrivateProfileString("Data", "COM", "none", COMnumber, Convert.ToUInt32(COMnumber.Capacity), iniFileName);

            //パスの読み取りに成功しました

        }

        static internal void writePort()
        {
            try
            {
                //! 受信データを読み込む.
                string sendData = GetData(myPort.ReadTo("\n"));

                if (sendData == "") return;

                try
                {
                    myPort.Write(sendData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ATSPISerial", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ClosePort();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return;
        }


        static internal void OpenPort()
        {
            try
            {
                myPort = new SerialPort("COM" + COMnumber.ToString(), 19200, Parity.None, 8, StopBits.One);
                //やっと動くようになった
                myPort.Open();
                myPort.RtsEnable = true;
                myPort.DtrEnable = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ATSPISerial", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ClosePort();
            }
        }

        static internal void ClosePort()
        {
            if (myPort != null)
            {
                try
                {
                    myPort.Close();
                }
                catch { }
                try
                {
                    myPort.Dispose();
                }
                catch { }
                myPort = null;
            }
        }

        /// <summary>
        /// COMポートから読み込んだ指令に対応した情報を返す
        /// </summary>
        /// <param name="readdata">read data from COM port</param>
        /// <returns></returns>
        static internal string GetData(string readdata)
        {
            string ret = "";
            switch (readdata)
            {
                case "Z":
                    ret += "Z" + vehicleState.Z.ToString();
                    break;
                case "V":
                    ret += "V" + vehicleState.V.ToString();
                    break;
                case "T":
                    ret += "T" + vehicleState.T.ToString();
                    break;
                case "BC":
                    ret += "BC" + vehicleState.BC.ToString();
                    break;
                case "MR":
                    ret += "MR" + vehicleState.MR.ToString();
                    break;
                case "ER":
                    ret += "ER" + vehicleState.ER.ToString();
                    break;
                case "BP":
                    ret += "BP" + vehicleState.BP.ToString();
                    break;
                case "SAP":
                    ret += "SAP" + vehicleState.SAP.ToString();
                    break;
                case "I":
                    ret += "I" + vehicleState.I.ToString();
                    break;
                case "D":
                    ret += "D" + Convert.ToInt32(door).ToString();
                    break;
                default:
                    break;
            }
            return ret;
        }

    }
}
