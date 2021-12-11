﻿using System;
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

namespace Assembly69.Interface.Controls
{
    /// <summary>
    /// Interaction logic for TagFlagsGroup.xaml
    /// </summary>
    public partial class TagFlagsGroup : UserControl
    {
        public MainWindow _mainwindow;
        public Memory.Mem M;
        public long startAddress;
        public int amountOfBytes;
        public int maxBit;

        public TagFlagsGroup()
        {
            InitializeComponent();
        }

        public void generateBits(long startAddress, int amountOfBytes, int maxBit, Dictionary<int, string> descriptions = null)
        {
            this.startAddress = startAddress; 
            this.amountOfBytes = amountOfBytes; 
            this.maxBit = maxBit;  

            if (maxBit == 0)
                this.maxBit = maxBit = amountOfBytes * 8;

            spBitCollection.Children.Clear();

            int maxAmountOfBytes = Math.Clamp((int)Math.Ceiling((double)maxBit / 8), 0, amountOfBytes);
            int bitsLeft = maxBit - 1; // -1 to start at 
            
            for (int @byte = 0; @byte < maxAmountOfBytes; @byte++)
            {
                if (bitsLeft < 0)
                    continue;

                int amountOfBits = @byte * 8 > maxBit ? ((@byte * 8) - maxBit) : 8;
                long addr = startAddress + @byte;
                byte flags_value = (byte) M.ReadByte((addr).ToString("X"));

                for (int bit = 0; bit < amountOfBits; bit++)
                {
                    int currentBitIndex = (@byte * 8) + bit;
                    if (bitsLeft < 0)
                        continue;

                    CheckBox checkbox = null;

                    int _byte = @byte, _bit = bit;

                    checkbox = new CheckBox();
                    checkbox.IsChecked = flags_value.GetBit(bit);
                    checkbox.Checked   += (s, e) => Checkbox_BitIsChanged(_byte, _bit);
                    checkbox.Unchecked += (s, e) => Checkbox_BitIsChanged(_byte, _bit);
                    checkbox.Content =
                        descriptions != null && descriptions.ContainsKey(currentBitIndex)
                        ? descriptions[(@byte * 8) + bit] : "Flag " + (currentBitIndex);

                    //  <TextBlock Foreground="Black">Only show mapped tags</TextBlock>
                    checkbox.ToolTip = new TextBlock() { 
                        Foreground = Brushes.Black
                        , Text = $"Flag Bit {currentBitIndex}, Addr = {startAddress + (@byte * 8)}:^{bit}"
                    };

                    if (checkbox != null)
                        spBitCollection.Children.Add (checkbox);

                    bitsLeft--;
                }
            }

            int x = 0;
        }

        private void Checkbox_BitIsChanged(int byteNo, int bit)
        {
            long targetAddress = startAddress + byteNo;
            byte output = 0;

            for(int x = 0; x < 8; x++)
            {
                int index = (byteNo * 8) + x;
                if (spBitCollection.Children.Count < index)
                    continue;

                var cbx = (CheckBox) spBitCollection.Children[index];
                output.UpdateBit(x, (bool) cbx.IsChecked);
            }

            _mainwindow.Addpokechange(targetAddress, "Flags", output.ToString());
        }
    }
}