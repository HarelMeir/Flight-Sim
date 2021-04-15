using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

namespace Flight_Sim.cppToCSharp
{
    public class SimpleAnomalyDetector : ITimeSeriesAnomalyDetector
    {
		List<CorrelatedFeatures> cf;
        float correlationThreshold = 0.9f;

        public SimpleAnomalyDetector()
        {
            this.cf = new List<CorrelatedFeatures>();
        }

        public List<CorrelatedFeatures> Cf
        {
            get
            {
                return this.cf;
            }
        }
        public void SetCorrelationThreshold(float newThreshold)
        {
            correlationThreshold = newThreshold;
        }
        public void SetNormalModel(List<CorrelatedFeatures> correlatedFeatures)
        {
            cf = correlatedFeatures;
        }

        private float FindThreshold(List<float> l1, List<float> l2, Line lin_reg)
        {
            float max = 0;
            int size = l1.Count;
            for(int i = 0; i < size; i++)
            {
                Point point = new Point(l1[i], l2[i]);
                float distance = AnomalyUtils.Dev(point, lin_reg);
                if(distance > max)
                {
                    max = distance;
                }
            }
            return max;
        }

        private void LearnHelper(IDictionary<string, List<float>> table, float maxCore, string f1, string f2)
        { 
                CorrelatedFeatures cfs = new CorrelatedFeatures();
                cfs.Feature1 = f1;
                cfs.Feature2 = f2;
                cfs.Corr = maxCore;
                cfs.Lin_Reg = AnomalyUtils.LinearReg(table[f1], table[f2]);
                cfs.Threshold = FindThreshold(table[f1], table[f2], cfs.Lin_Reg) * 1.1f;
                this.cf.Add(cfs);
        }
        public void LearnNormal(IDictionary<string, List<float>> table)
        {
            int size = table.Count;

            for(int i = 0; i < size; i++)
            {
                string f1 = table.ElementAt(i).Key;
                float maxCore = 0;
                int maxIndex = 0;
                for(int j = i + 1; j < size; j++)
                {
                    float cor = Math.Abs(AnomalyUtils.Pearson(table.ElementAt(i).Value, table.ElementAt(j).Value));
                    if(cor > maxCore)
                    {
                        maxCore = cor;
                        maxIndex = j;
                    }
                }
                string fMostCore = table.ElementAt(maxIndex).Key;
                LearnHelper(table, maxCore, f1, fMostCore);
            }
        }

        public List<AnomalyReport> detect(IDictionary<string, List<float>> table)
        {
            List<AnomalyReport> anomalyReport = new List<AnomalyReport>();
            foreach (CorrelatedFeatures correlatedFeature in cf)
            {
                for (int i = 0; i < table[correlatedFeature.Feature1].Count; i++)
                {
                    Point p = new Point(table[correlatedFeature.Feature1].ElementAt(i), table[correlatedFeature.Feature1].ElementAt(i));
                    if (AnomalyUtils.Dev(p, correlatedFeature.Lin_Reg) > correlatedFeature.Threshold)
                        anomalyReport.Add(new AnomalyReport(correlatedFeature.Feature1 + "-" + correlatedFeature.Feature2, i));
                }
            }
            return anomalyReport;
        }

    }
}
