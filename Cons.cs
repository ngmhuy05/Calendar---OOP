using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThiCuoiKy
{
    public class Cons
    {
        private int row;
        private int column;
        private int sizeBtnWidth;
        private int sizeBtnHeight;
        private int margin;
        private int notifyTime;
        private int timeOut;
        private Color BackColor;

        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public int SizeBtnWidth { get => sizeBtnWidth; set => sizeBtnWidth = value; }
        public int SizeBtnHeight { get => sizeBtnHeight; set => sizeBtnHeight = value; }
        public int Margin { get => margin; set => margin = value; }
        public int NotifyTime { get => notifyTime; set => notifyTime = value; }
        public int TimeOut { get => timeOut; set => timeOut = value; }
        public Color BackColor1 { get => BackColor; set => BackColor = value; }
    }
        public class DarkMode : Cons
    {
        public DarkMode()
        {
            Row = 6;
            Column = 7;
            SizeBtnWidth = 75;
            SizeBtnHeight = 43;
            Margin = 6;
            NotifyTime = 2;
            TimeOut = 10000;
            BackColor1 = Color.BlanchedAlmond;
        }
    }

    public class LightMode : Cons
    {
        public LightMode()
        {
            Row = 6;
            Column = 7;
            SizeBtnWidth = 75;
            SizeBtnHeight = 43;
            Margin = 6;
            NotifyTime = 1;
            TimeOut = 5000;
            BackColor1 = Color.White;
        }
    }
}
