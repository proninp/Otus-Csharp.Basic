﻿using System.Diagnostics;

namespace HomeWork07
{
    internal class Program
    {
        private static string ResultTemplate = "------------------------- n = {0} -------------------------";
        private static string RecoursiveType = "Recoursoive";
        private static string RecoursiveCachedType = "Recoursoive cache";
        private static string IterativeType = "Iterative";

        static void Main(string[] args)
        {
            var nums = new int[] { 5, 10, 20, 25, 30, 35, 40, 45 };
            foreach (var n in nums)
                DoFibTests(n);
        }
        #region Print Resultes
        static void DoFibTests(int n)
        {   
            HomeWork01.Library.HomeWorkHelper.PrintConsole(string.Format(ResultTemplate, n));

            var watch = new Stopwatch();
            
            watch.Start();
            var num = GetFibRecoursive(n);
            watch.Stop();
            PrintFunctionResult(RecoursiveType.PadRight(RecoursiveCachedType.Length), num, watch);

            watch.Restart();
            num = GetFibRecoursiveCached(n);
            watch.Stop();
            PrintFunctionResult(RecoursiveCachedType, num, watch);

            watch.Restart();
            num = GetFibIterative(n);
            watch.Stop();
            PrintFunctionResult(IterativeType.PadRight(RecoursiveCachedType.Length), num, watch);
        }
        static void PrintFunctionResult(string calculationType, long num, Stopwatch watch)
        {
            Console.WriteLine($"{calculationType}:\tNumber: {num};\tTime: {watch.Elapsed};\tTicks: {watch.ElapsedTicks}");
        }
        #endregion
        #region Fibonacci Calcualations
        static long GetFibRecoursive(int n)
        {
            if (n < 2)
                return n;
            return GetFibRecoursive(n - 1) + GetFibRecoursive(n - 2);
        }
        static long GetFibRecoursiveCached(int n)
        {
            var map = new Dictionary<int, long>();
            return GetFibRecoursiveCached(n, map);
        }
        static long GetFibRecoursiveCached(int n, Dictionary<int, long> map)
        {
            if (map.ContainsKey(n))
                return map[n];
            long fib;
            if (n < 2)
                fib = n;
            else
                fib = GetFibRecoursiveCached(n - 1, map) + GetFibRecoursiveCached(n - 2, map);
            map[n] = fib;
            return fib;
        }
        static long GetFibIterative(int n)
        {
            if (n < 2)
                return n;
            var a = new long[] { 0, 1 };
            for (int i = 2; i <= n; i++)
                (a[0], a[1]) = (a[1], a[0] + a[1]);
            return a[1];
        }
        #endregion
    }
}