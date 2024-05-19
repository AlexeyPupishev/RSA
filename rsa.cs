using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;

class Program
{
    static BigInteger ExtEvc(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
    {
        if (a == 0)
        {
            x = 0;
            y = 1;
            return b;
        }
        BigInteger x1, y1;
        BigInteger d = ExtEvc(b % a, a, out x1, out y1);
        x = y1 - (b / a) * x1;
        y = x1;
        return d;
    }

    static BigInteger Inverse(BigInteger a, BigInteger m)
    {
        BigInteger x, y;
        BigInteger g = ExtEvc(a, m, out x, out y);
        if (g != 1)
        {
            Console.WriteLine("Err: ");
            return -1;
        }
        else
        {
            BigInteger res = (x % m + m) % m;
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

    static BigInteger Power(BigInteger x, BigInteger y, BigInteger p)
    {
        BigInteger res = 1;
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

    static bool Witness(BigInteger a, BigInteger n)
    {
        BigInteger r = 0;
        BigInteger d = n - 1;

        while (d % 2 == 0)
        {
            r++;
            d >>= 1;
        }

        BigInteger x = Power(a, d, n);
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

    static bool MillerRabinTest(BigInteger n, int k)
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
            BigInteger a = 2 + rand.Next() % (int)(n - 4);
            if (Witness(a, n))
            {
                return false;
            }
        }

        return true;
    }

    static BigInteger GeneratePrime(BigInteger min, BigInteger max, int k)
    {
        Random rand = new Random();
        BigInteger prime = 0;
        while (true)
        {
            BigInteger num = min + rand.Next() % (max - min + 1);
            if (MillerRabinTest(num, k))
            {
                prime = num;
                break;
            }
        }
        return prime;
    }
