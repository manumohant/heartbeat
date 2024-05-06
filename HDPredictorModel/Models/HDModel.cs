using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDPredictor.Models
{
    public class PredictionOutcome
    {
        public string[] predictions { get; set; }
    }

    public class HDModel
    {
        public string Email { get; set; }
        public string Name { get; set; }    
        public int Age { get; set; }
        public int Gender { get; set; }
        public int Chestpain { get; set; }
        public float RestingBloodpressure { get; set; }
        public float SerumCholestrol { get; set; }
        public float FastingBloodSugar { get; set; }
        public int Exercise { get; set; }
        public int ThalACH { get;  set; }
        public int RestEcg { get;  set; }
        public int OldPeak { get; set; }
        public int Slope { get; set; }
    }
}
