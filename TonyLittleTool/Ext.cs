using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCAD;

namespace TonyLittleTool
{
    public static class Ext
    {

        public static double GetMidX(this AcadLine aline, double xx = 0, double yy = 0)
        {
            double[] st = (double[])aline.StartPoint;
            double[] ed = (double[])aline.EndPoint;


            return (st[0]+ed[0])*0.5;
        }
    }
}
