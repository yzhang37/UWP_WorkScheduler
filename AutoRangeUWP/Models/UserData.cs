using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace AutoRange.Models
{
    class WeekMode
    {
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public void reset()
        {
            Sunday = false;
            Monday = false;
            Tuesday = false;
            Wednesday = false;
            Thursday = false;
            Friday = false;
            Saturday = false;
        }
    }
    class UserData
    {
        public double value { get; } = Sin(0.5 * 3.1415926);
        public string UserName { get; set; }
        public WeekMode AvailableWeekday { get; set; }
        private void init()
        {
            AvailableWeekday = new WeekMode();
            AvailableWeekday.reset();
        }
        public UserData() { init(); }
        public UserData(string Name)
        {
            UserName = Name;
            init();
        }
    }
    class DisplayDataClass
    {
        public int index { get; set; }
        public string header { get; set; }
        public string subheader { get; set; }
        public DisplayDataClass(int i, string h, string s)
        {
            index = i;
            header = h; subheader = s;
        }
    }
}
