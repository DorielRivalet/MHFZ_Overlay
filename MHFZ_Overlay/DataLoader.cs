using Memory;
using MHFZ_Overlay.addresses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MHFZ_Overlay
{
    public class DataLoader
    {
        #region DataLoaderVariables
        //needed for getting data
        readonly Mem m = new();
        bool isHighGradeEdition;
        int index;
        public AddressModel model { get; }


        #endregion


        public DataLoader()
        {
            int PID = m.GetProcIdFromName("mhf");
            if (PID > 0)
            {
                m.OpenProcess(PID);
                try
                {
                    CreateCodeCave(PID);
                }
                catch (Exception) { }
                if (!isHighGradeEdition)
                    model = new AddressModelNotHGE(m);
                else
                    model = new AddressModelHGE(m);
            }
            else
            {
                System.Windows.MessageBox.Show("Launch game first");
                App.Current.Shutdown();
            }
        }



        private void CreateCodeCave(int PID)
        {
            Process? proc = LoadMHFODLL(PID);
            if (proc == null)
            {
                System.Windows.MessageBox.Show("Launch game first");
                App.Current.Shutdown();
                return;
            }
            long searchAddress = m.AoBScan("89 04 8D 00 C6 43 00 61 E9").Result.FirstOrDefault();
            if (searchAddress.ToString("X8") == "00000000")
            {
                //Create codecave and get its address
                long baseScanAddress = m.AoBScan("0F B7 8a 24 06 00 00 0f b7 ?? ?? ?? c1 c1 e1 0b").Result.FirstOrDefault();
                UIntPtr codecaveAddress = m.CreateCodeCave(baseScanAddress.ToString("X8"), new byte[] { 0x0F, 0xB7, 0x8A, 0x24, 0x06, 0x00, 0x00, 0x0F, 0xB7, 0x52, 0x0C, 0x88, 0x15, 0x21, 0x00, 0x0F, 0x15, 0x8B, 0xC1, 0xC1, 0xE1, 0x0B, 0x0F, 0xBF, 0xC9, 0xC1, 0xE8, 0x05, 0x09, 0xC8, 0x01, 0xD2, 0xB9, 0x8E, 0x76, 0x21, 0x25, 0x29, 0xD1, 0x66, 0x8B, 0x11, 0x66, 0xF7, 0xD2, 0x0F, 0xBF, 0xCA, 0x0F, 0xBF, 0x15, 0xC4, 0x22, 0xEA, 0x17, 0x31, 0xC8, 0x31, 0xD0, 0xB9, 0xC0, 0x5E, 0x73, 0x16, 0x0F, 0xBF, 0xD1, 0x31, 0xD0, 0x60, 0x8B, 0x0D, 0x21, 0x00, 0x0F, 0x15, 0x89, 0x04, 0x8D, 0x00, 0xC6, 0x43, 0x00, 0x61 }, 63, 0x100);

                //Change addresses
                UIntPtr storeValueAddress = codecaveAddress + 125;                  //address where store some value?
                string storeValueAddressString = storeValueAddress.ToString("X8");
                byte[] storeValueAddressByte = Enumerable.Range(0, storeValueAddressString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(storeValueAddressString.Substring(x, 2), 16)).ToArray();
                Array.Reverse(storeValueAddressByte, 0, storeValueAddressByte.Length);
                byte[] by15 = { 136, 21 };
                m.WriteBytes(codecaveAddress + 11, by15);
                m.WriteBytes(codecaveAddress + 13, storeValueAddressByte);  //1
                m.WriteBytes(codecaveAddress + 72, storeValueAddressByte);  //2

                WriteByteFromAddress(codecaveAddress, proc, isHighGradeEdition ? 249263758 : 102223598, 33);
                WriteByteFromAddress(codecaveAddress, proc, isHighGradeEdition ? 27534020 : 27601756, 51);
                WriteByteFromAddress(codecaveAddress, proc, isHighGradeEdition ? 2973376 : 2865056, 60);

            }
            else
            {
                LoadMHFODLL(PID);
            }
        }

        void WriteByteFromAddress(UIntPtr codecaveAddress, Process proc, long offset1, int offset2)
        {
            long address = proc.Modules[index].BaseAddress.ToInt32() + offset1;
            string addressString = address.ToString("X8");
            byte[] addressByte = Enumerable.Range(0, addressString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(addressString.Substring(x, 2), 16)).ToArray();
            Array.Reverse(addressByte, 0, addressByte.Length);
            m.WriteBytes(codecaveAddress + offset2, addressByte);
        }

        Process? LoadMHFODLL(int PID)
        {
            //Search and get mhfo-hd.dll module base address
            Process proccess = Process.GetProcessById(PID);
            if (proccess == null)
                return null;
            var ModuleList = new List<string>();
            foreach (ProcessModule md in proccess.Modules)
            {
                string? moduleName = md.ModuleName;
                if (moduleName != null)
                    ModuleList.Add(moduleName);
            }
            index = ModuleList.IndexOf("mhfo-hd.dll");
            if (index > 0)
            {
                isHighGradeEdition = true;
            }
            else
            {
                index = ModuleList.IndexOf("mhfo.dll");
                isHighGradeEdition = false;
            }
            return proccess;
        }



    }
}
