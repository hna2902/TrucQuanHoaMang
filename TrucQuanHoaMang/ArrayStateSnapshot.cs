using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrucQuanHoaMang
{
    public class ArrayStateSnapshot
    {
        public int[] Data { get; private set; }
        public Color[] Colors { get; private set; }

        public ArrayStateSnapshot(int[] data, Color[] colors)
        {
            Data = (int[])data.Clone(); // Clone để tránh tham chiếu
            Colors = (Color[])colors.Clone(); // Clone để tránh tham chiếu
        }
    }
}
