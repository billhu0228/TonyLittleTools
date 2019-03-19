using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoCAD;


namespace TonyLittleTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.WriteLine("输出成功.");
            Console.ReadKey(true);
        }



        public static void Test()
        {
            AcadApplication AcadApp = (AcadApplication)Marshal.GetActiveObject("AutoCAD.Application.19");
            AcadDocument doc = AcadApp.ActiveDocument;
            AcadModelSpace ms = doc.ModelSpace;
            AcadSelectionSets SelSets = doc.SelectionSets;
            
            
            //SelSets.
            Console.WriteLine("请Tony选择中心线.");
            AcadSelectionSet cl = SelSets.Add(new Random().Next(1000, 9999).ToString());
            cl.Clear();
            cl.SelectOnScreen();
            Console.WriteLine("中心线选择完成.");

            
            Console.WriteLine("请Tony选择预应力筋.");
            AcadSelectionSet preBars = SelSets.Add(new Random().Next(1000, 9999).ToString());
            preBars.Clear();
            preBars.SelectOnScreen();
            Console.WriteLine("预应力钢筋选择完成,共计{0}根.",preBars.Count);

            Console.WriteLine("请Tony选择方向线.");
            AcadSelectionSet dirL = SelSets.Add(new Random().Next(1000, 9999).ToString());
            dirL.Clear();
            dirL.SelectOnScreen();
            List<AcadLine> NewDirLSet = new List<AcadLine>();
            foreach (var item in dirL)
            {
                AcadLine DirLine = (AcadLine)item;
                NewDirLSet.Add(DirLine);
            }
            NewDirLSet.Sort((x, y) => x.GetMidX().CompareTo(y.GetMidX()));

            Console.WriteLine("预应力钢筋选择完成,共计{0}根.", dirL.Count);

            AcadLWPolyline CLine = (AcadLWPolyline)cl.Item(0);


            FileStream fs = new FileStream("D:\\TonyReadMe.csv", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);


            foreach (var bar in preBars)
            {
                sw.Flush();
                sw.Write("\n{0},",bar.ToString());
                //Console.WriteLine(bar.ToString());
                AcadLWPolyline PL = (AcadLWPolyline)bar;

                foreach (var item in NewDirLSet)
                {
                    double[] p1, p2;
                    double dist;
                    try
                    {
                        AcadLine DirLine = item;

                        p1=DirLine.IntersectWith(CLine, AcExtendOption.acExtendThisEntity);
                        p2 = DirLine.IntersectWith(PL, AcExtendOption.acExtendThisEntity);
                        if (p1.Count()==0||p2.Count()==0)
                        {
                            continue;
                        }
                        dist = Math.Pow((p1[0] - p2[0]), 2) + Math.Pow((p1[1] - p2[1]), 2) + Math.Pow((p1[2] - p2[2]), 2);
                        dist = Math.Sqrt(dist);

                        int fx = p1[1] < p2[1] ? 1 : -1;

                        //Console.Write("{0:F0},",dist);

                        sw.Write("{0:F0},",fx* dist*1000.0);


                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }


                // Console.WriteLine("Length={0}", L.Length);

            }


            cl.Delete();
            preBars.Delete();
            dirL.Delete();

            sw.Close();
            fs.Close();
        }


    }
}
