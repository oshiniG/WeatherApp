using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Application.Models
{
    public class OpenWeatherResponse
    {
        public Coord Coord { get; set; }  
        public List<Weather> Weather { get; set; }  
        public MainWeatherData Main { get; set; }  
        public Wind Wind { get; set; }  
        public Clouds Clouds { get; set; }  
        public Sys Sys { get; set; }  
        public string Name { get; set; }  
        public int Cod { get; set; } 
    }
    public class Coord
    {
        public double Lon { get; set; } 
        public double Lat { get; set; }  
    }
    public class Clouds
    {
        public int All { get; set; } 
    }
    public class Sys
    {
        public int Type { get; set; }  
        public int Id { get; set; } 
        public string Country { get; set; } 
        public int Sunrise { get; set; }  
        public int Sunset { get; set; }  
    }

}
