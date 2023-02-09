using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMandel.Logic
{
    public struct Complex
    {
        public double Real;
        public double Imag;

        public Complex(double real, double imag)
        {
            Real = real;
            Imag = imag;
        }

        public static implicit operator Complex(double x)
        {
            return new Complex(x, 0);
        }

        public static implicit operator Complex(int x)
        {
            return new Complex(x, 0);
        }

        public static Complex operator +(Complex z1, Complex z2)
        {
            return new Complex(z1.Real + z2.Real, z1.Imag + z2.Imag);
        }

        public static Complex operator *(Complex z1, Complex z2)
        {
            return new Complex(z1.Real * z2.Real - z1.Imag * z2.Imag, z1.Real * z2.Imag + z1.Imag * z2.Real);
        }

        public double Mod2()
        {
            return Real * Real + Imag * Imag;
        }
    }
}