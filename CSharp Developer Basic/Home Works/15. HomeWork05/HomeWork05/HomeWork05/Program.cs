using System;
using System.Collections.Generic;
using System.Text;
namespace HomeWork5
{
    public class Program
    {
        static void Main(string[] args)
        {
            var quadcopter = new Quadcopter();

            HomeWork01.Library.HomeWorkHelper.PrintConsole("********** Quadcopter **********");
            quadcopter.Charge();
            Console.WriteLine(string.Join(", ", ((List<string>)quadcopter.GetComponents()).ToArray()));
            Console.WriteLine();

            HomeWork01.Library.HomeWorkHelper.PrintConsole("********** IChargeable **********");
            ((IChargeable)quadcopter).Charge();
            Console.WriteLine(((IChargeable)quadcopter).GetInfo());
            Console.WriteLine();

            HomeWork01.Library.HomeWorkHelper.PrintConsole("********** IRobot **********");
            Console.WriteLine(((IRobot)quadcopter).GetRobotType());
            Console.WriteLine(string.Join(", ", ((List<string>)((IRobot)quadcopter).GetComponents()).ToArray()));
            Console.WriteLine(((IRobot)quadcopter).GetInfo());
            Console.WriteLine();

            HomeWork01.Library.HomeWorkHelper.PrintConsole("********** IFlyingRobot **********");
            Console.WriteLine(((IFlyingRobot)quadcopter).GetRobotType());
            Console.WriteLine(string.Join(", ", ((List<string>)((IFlyingRobot)quadcopter).GetComponents()).ToArray()));
            Console.WriteLine(((IFlyingRobot)quadcopter).GetInfo());
            Console.WriteLine();
        }
    }
}