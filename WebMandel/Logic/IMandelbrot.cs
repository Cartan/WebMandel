using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebMandel.Logic
{
    public interface IMandelbrot
    {
        double XMin { get; set; }
        double YMax { get; set; }
        double Scale { get; set; }
        int Iterations { get; set;}
        byte[] GenerateMandelImage();
    }
}
