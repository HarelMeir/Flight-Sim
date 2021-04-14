using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

namespace Flight_Sim.cppToCSharp
{
    public class CorrelatedFeatures
    {
        string feature1, feature2;  // names of the correlated features
        public string Feature1 { get { return feature1; } set { feature1 = value; } }
        public string Feature2 { get { return feature2; } set { feature2 = value; } }
        float correlation;
        public float Corr { get { return correlation; } set { correlation = value; } }
        Line lin_reg;
        public Line Lin_Reg { get { return lin_reg; } set { lin_reg = value; } }
        float threshold;
        public float Threshold { get { return threshold; } set { threshold = value; } }
        Circle circleThreshold;
        public Circle CircleThreshold { get { return circleThreshold; } set { circleThreshold = value; } }
    };
}
