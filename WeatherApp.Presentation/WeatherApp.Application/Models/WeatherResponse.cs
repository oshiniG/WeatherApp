using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Enums;

namespace WeatherApp.Application.Models
{
    public class WeatherResponse
    {
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public WeatherCondition Condition { get; set; }
        public string Recommendation { get; set; }
    }
}
