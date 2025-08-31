﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Application.Models
{
    public class MainWeatherData
    {
        public double Temp { get; set; } 
        public double FeelsLike { get; set; }  
        public double TempMin { get; set; }  
        public double TempMax { get; set; }  
        public int Pressure { get; set; }  
        public int Humidity { get; set; }  
        public int SeaLevel { get; set; }  
        public int GrndLevel { get; set; }  
    }

}
