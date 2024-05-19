using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;

class Program
{
    static long ExtEvc(long a, long b, out long x, out long y)
    {
        if (a == 0)
        {
            x = 0;
            y = 1;
            return b;
        }
        long x1, y1;
        long d = ExtEvc(b % a, a, out x1, out y1);
        x = y1 - (b / a) * x1;
        y = x1;
        return d;
    }

    static long Inverse(long a, long m)
    {
        long x, y;
        long g = ExtEvc(a, m, out x, out y);
        if (g != 1)
        {
            Console.WriteLine("Err: ");
            return -1;
        }
        else
        {
            long res = (x % m + m) % m;
            return res;
        }
    }

    static int Evc(int a, int b)
    {
        int t;
        while (true)
        {
            t = a % b;
            if (t == 0)
                return b;
            a = b;
            b = t;
        }
    }

    static long Power(long x, long y, long p)
    {
        long res = 1;
        x = x % p;

        while (y > 0)
        {
            if (y % 2 == 1)
            {
                res = (res * x) % p;
            }
            y = y / 2;
            x = (x * x) % p;
        }
        return res;
    }

    static bool Witness(long a, long n)
    {
        long r = 0;
        long d = n - 1;

        while (d % 2 == 0)
        {
            r++;
            d >>= 1;
        }

        long x = Power(a, d, n);
        if (x == 1 || x == n - 1)
        {
            return false;
        }

        for (int i = 0; i < r - 1; i++)
        {
            x = (x * x) % n;
            if (x == n - 1)
            {
                return false;
            }
        }

        return true;
    }

    static bool MillerRabinTest(long n, int k)
    {
        if (n <= 1 || n == 4)
        {
            return false;
        }
        if (n <= 3)
        {
            return true;
        }

        Random rand = new Random();
        for (int i = 0; i < k; i++)
        {
            long a = 2 + rand.Next() % (int)(n - 4);
            if (Witness(a, n))
            {
                return false;
            }
        }

        return true;
    }

    static long GeneratePrime(long min, long max, int k)
    {
        Random rand = new Random();
        long prime = 0;
        while (true)
        {
            long num = min + rand.Next() % (max - min + 1);
            if (MillerRabinTest(num, k))
            {
                prime = num;
                break;
            }
        }
        return prime;
    }

    public long GeneratePublicKey(long min, long max)
    {
        int s = (int)Math.Log2(100000);

        long p = GeneratePrime(min, max, s);
        long q = GeneratePrime(min + 1, max - 1, s);
        Console.WriteLine($"p = {p}; q = {q}");

        long n = p * q;

        Console.WriteLine($"n = {n}");

        long eulerFunc = (p - 1) * (q - 1);

        Console.WriteLine($"euler_func = {eulerFunc}");

        long e = 691;
        while (Evc(e, eulerFunc) != 1 && e % 2 != 0)
        {
            e++;
        }

        Console.WriteLine($"e = {e}");

        return e;
    }

    public long GeneratePrivateKey(long e, long min, long max)
    {
        int s = (int)Math.Log2(100000);

        long p = GeneratePrime(min, max, s);
        long q = GeneratePrime(min + 1, max - 1, s);

        long eulerFunc = (p - 1) * (q - 1);

        // Закрытый ключ d
        long d = Inverse(e, eulerFunc);
        while (d < 0)
        {
            d += eulerFunc;
        }

        Console.WriteLine($"d: {d}");

        return d;
    }

    public void EncryptTxtFile(long e, long n)
    {
        string inputPath = "if.txt";
        string outputPath = "of.txt";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Ошибка открытия файла!");
            return;
        }

        var nums = new List<long>();
        foreach (char ch in File.ReadAllText(inputPath))
        {
            nums.Add((long)ch);
        }

        for (int i = 0; i < nums.Count; i++)
        {
            nums[i] = Power(nums[i], e, n);
        }

        using (var writer = new StreamWriter(outputPath))
        {
            foreach (long num in nums)
            {
                writer.Write(num + " ");
            }
        }
    }

    public void EncryptBinFile(long e, long n)
    {
        string inputPath = "if.bin";
        string outputPath = "of.bin";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Ошибка открытия файла!");
            return;
        }

        var nums = new List<long>();
        foreach (byte b in File.ReadAllBytes(inputPath))
        {
            nums.Add((long)b);
        }

        for (int i = 0; i < nums.Count; i++)
        {
            nums[i] = Power(nums[i], e, n);
        }

        using (var writer = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
        {
            foreach (long num in nums)
            {
                writer.Write(num);
            }
        }
    }

}