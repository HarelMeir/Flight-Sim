using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

namespace Flight_Sim.cppToCSharp
{
    public class AnomalyReport
    {
        string description;
        long timeStep;
        public AnomalyReport(string description, long timeStep)
        {
            this.description = description;
            this.timeStep = timeStep;
        }
        public string desc
        {
            get
            {
                return description;
            }
        }
        public long ts
        {
            get
            {
                return timeStep;
            }
        }
    }
    public interface ITimeSeriesAnomalyDetector
    {
        List<CorrelatedFeatures> Cf {get;}
        void SetNormalModel(List<CorrelatedFeatures> correlatedFeatures);
        void LearnNormal(IDictionary<string, List<float>> table);
        List<AnomalyReport> detect(IDictionary<string, List<float>> table);
    }
}
