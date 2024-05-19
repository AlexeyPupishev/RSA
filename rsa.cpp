#include <iostream>
#include <cstdlib>
#include <cmath>
#include <ctime>
#include <algorithm>
#include <fstream>
#include <vector>

using namespace std;

long long ext_evc(long long a, long long b, long long& x, long long& y)
{
    if (a == 0)
    {
        x = 0;
        y = 1;
        return b;
    }
    long long x1, y1;
    long long d = ext_evc(b % a, a, x1, y1);
    x = y1 - (b / a) * x1;
    y = x1;
    return d;
}

long long inverse(long long a, long long m)
{
    long long x, y;
    long long g = ext_evc(a, m, x, y);
    if (g != 1)
    {
        cout << "Err: ";
        return -1;
    }
    else {
        long long res = (x % m + m) % m;
        return res;
    }
}

int evc(int a, int b)
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

long long power(long long x, unsigned long long y, long long p)
{
    long long res = 1;
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

bool witness(long long a, long long n)
{
    long long r = 0;
    long long d = n - 1;

    while (d % 2 == 0)
    {
        r++;
        d >>= 1;
    }

    long long x = power(a, d, n);
    if (x == 1 || x == n - 1) {
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

bool millerRabinTest(long long n, int k)
{
    if (n <= 1 || n == 4)
    {
        return false;
    }
    if (n <= 3)
    {
        return true;
    }

    for (int i = 0; i < k; i++)
    {
        long long a = 2 + rand() % (n - 4);
        if (witness(a, n))
        {
            return false;
        }
    }

    return true;
}

long long generatePrime(long long min, long long max, int k)
{
    srand(time(NULL));
    long long prime = 0;
    while (true)
    {
        long long num = min + rand() % (max - min + 1);
        if (millerRabinTest(num, k))
        {
            prime = num;
            break;
        }
    }
    return prime;
}

int main()
{
    // WPF C++ GUI

    long long min = 10000;
    long long max = 100000;


    cout << "Type min and max value of p and q: " << endl;
    cin >> min >> max;

    int s = log2(100000);

    long long p = generatePrime(min, max, s);
    long long q = generatePrime(min + 1, max - 1, s);
    cout << "p = " << p << "; q = " << q << endl;

    long long n = p * q;

    cout << "n = " << n << endl;

    long long euler_func = (p - 1) * (q - 1);

    cout << "euler_func = " << euler_func << endl;

    long long e = 691;
    while (evc(e, euler_func) != 1 && e % 2 != 0)
    {
        e++;
    }

    cout << "e = " << e << endl;

    // Закрытый ключ d
    long long d = inverse(e, euler_func);
    while (d < 0)
    {
        d += euler_func;
    }

    cout << "d: " << d << endl;

    // Шифрование
    cout << "Type the type of the file you want to encrypt: 0 - .txt, 1 - .bin: ";
    int fl;
    cin >> fl;

    
    if(fl == 0)
    {
        ifstream ifs("if.txt");
        if (!ifs.is_open())
        {
            cout << "Err of opening file!" << endl;
            return -1;
        }

        char ch;
        vector<long long> nums;

        while (ifs.get(ch))
        {
            nums.push_back(static_cast<long long>(ch));
        }
        ifs.close();

        for (long long& num : nums)
        {
            num = power(num, e, n);
        }

        ofstream ofs("of.txt");
        if (!ofs.is_open())
        {
            cout << "Err of opening file!" << endl;
            return -1;
        }

        for (long long num : nums)
        {
            ofs << num << " ";
        }
        ofs.close();

        //long long mes = 83231;
        //cout << "message: " << mes << endl;
        //long long c = power(mes, e, n);
        //cout <<"c: " << c << endl;

        // Дешифрование

        /*  long long m = power(c, d, n);
        cout << "res: " << m << endl;*/

        ifstream ifz("of.txt");
        if (!ifz.is_open())
        {
            cout << "Err of opening file!" << endl;
            return -1;
        }

        vector<long long> nums_e;
        long long numb;
        while (ifz >> numb)
        {
            nums_e.push_back(numb);
        }
        ifz.close();

        for (long long& numb : nums_e)
        {
            numb = power(numb, d, n);
        }

        ofstream ofs_d("decr.txt");
        if (!ofs_d.is_open())
        {
            cout << "Err of opening file!" << endl;
            return -1;
        }

        for (long long numb : nums_e)
        {
            ofs_d << static_cast<char>(numb);
        }
        ofs_d.close();
        cout << "Success!";
    }
    else if(fl == 1)
    {
        ifstream ifs("if.bin", ios::binary);
        if (!ifs.is_open())
        {
            cout << "Err of opening file!" << endl;
            return -1;
        }

        vector<long long> nums;
        char ch;
        while (ifs.get(ch))
        {
            nums.push_back(static_cast<long long>(ch));
        }
        ifs.close();

        for (long long& num : nums)
        {
            num = power(num, e, n);
        }

        ofstream ofs("of.bin");
        if (!ofs.is_open())
        {
            cout << "Err of opening file!" << endl;
            return -1;
        }

        for (long long num : nums)
        {
            ofs << num << " ";
        }
        ofs.close();

        // Дешифрование

        ifstream ifz("of.bin");
        if (!ifz.is_open())
        {
            cout << "Err of opening file!" << endl;
            return -1;
        }

        vector<long long> nums_e;
        long long numb;
        while (ifz >> numb)
        {
            nums_e.push_back(numb);
        }
        ifz.close();

        for (long long& numb : nums_e)
        {
            numb = power(numb, d, n);
        }

        ofstream ofs_d("decr.bin", ios::binary);
        if (!ofs_d.is_open())
        {
            cout << "Err of opening file!" << endl;
            return -1;
        }

        for (long long numb : nums_e)
        {
            char c = static_cast<char>(numb);
            ofs_d.write(&c, sizeof(c));
        }
        ofs_d.close();
        cout << "Success!";
    }
    else
    {
        cout << "Err: incorrect fl value!";
    }


}


    //выбор проекта.
    //начата работа по проекту...
    //по шифру Цезаря не надо, только по своему.

    //осн. часть по другому обозначить
    //актуальность, как выполняли, в мельчайших деталях, почему именно такой шифр, почему актуален 6 страниц без учета картинок только текст
