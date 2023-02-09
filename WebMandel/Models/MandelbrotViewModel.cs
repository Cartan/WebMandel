using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using WebMandel.Logic;
using WebMandel.Infrastructure;

namespace WebMandel.Models
{
    public class MandelbrotViewModel
    {
        public double XMin { get; set; }
        public double YMax { get; set; }
        public double Scale { get; set; }
        [Range(1, 10000)]
        public int Iterations { get; set; }

        public MandelbrotViewModel()
        {
        }

        public MandelbrotViewModel(IMandelbrot mandel)
        {
            XMin = mandel.XMin;
            YMax = mandel.YMax;
            Scale = mandel.Scale;
            Iterations = mandel.Iterations;
        }

        public void Update(IMandelbrot target)
        {
            target.XMin = XMin;
            target.YMax = YMax;
            target.Scale = Scale;
            target.Iterations = Iterations;
        }
    }
}