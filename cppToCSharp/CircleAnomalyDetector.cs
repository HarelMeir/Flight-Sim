using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

namespace Flight_Sim.cppToCSharp
{
    class CircleAnomalyDetector : ITimeSeriesAnomalyDetector
    {
        List<CorrelatedFeatures> cf;
        float correlationThreshold = 0.9f;
        public void setCorrelationThreshold(float newThreshold)
        {
            correlationThreshold = newThreshold;
        }
        public void setNormalModel(List<CorrelatedFeatures> correlatedFeatures)
        {
            cf = correlatedFeatures;
        }
        public void learnNormal(IDictionary<string, List<float>> table)
        {
            int columnSize = table.ElementAt(0).Value.Count; //table.Keys.ElementAt(0)
            int rowSize = table.Count;
            for (int i = 0; i < table.Count; i++)
            {
                CorrelatedFeatures cfs = new CorrelatedFeatures();
                cfs.Corr = -1;
                cfs.Feature1 = table.ElementAt(i).Key;
                for (int j = i + 1; j < table.Count; j++)
                {
                    float correlation = AnomalyUtils.Pearson(table.ElementAt(i).Value, table.ElementAt(j).Value);
                    if (correlation > correlationThreshold)
                    {
                        if (correlation > cfs.Corr)
                        {
                            cfs.Feature2 = table.ElementAt(j).Key;
                            cfs.Corr = correlation;
                            cfs.Lin_Reg = AnomalyUtils.LinearReg(table.ElementAt(i).Value,
                                table.ElementAt(j).Value);
                            cfs.Threshold = highestDev(table.ElementAt(i).Value,
                                table.ElementAt(j).Value, cfs.Lin_Reg);

                            List<Point> pointsList = new List<Point>();
                            for (int k = 1; k < columnSize; k++) 
                                pointsList.Add(new Point(table[cfs.Feature1].ElementAt(k), table[cfs.Feature2].ElementAt(k)));
                           
                            cfs.CircleThreshold = MinCircle.FindMinCircle(pointsList);
                            cfs.CircleThreshold.radius *= 1.1;
                        }
                    }
                }
                if (cfs.Corr > -1)
                {
                    cf.Add(cfs);
                }
            }
        }
        //sends each x y pair to dev and returns the highest
        public float highestDev(List<float> x, List<float> y, Line line)
        {
            float maxDev = 0;
            for (int i = 0; i < x.Count; i++)
            {
                Point p = new Point(x[i], y[i]);
                float newDev = AnomalyUtils.Dev(p, line);
                if (newDev > maxDev)
                {
                    maxDev = newDev;
                }
            }
            return maxDev;
        }




        public List<AnomalyReport> detect(IDictionary<string, List<float>> table)
        {
            List<AnomalyReport> anomalyReport = new List<AnomalyReport>();
            foreach (CorrelatedFeatures correlatedFeature in cf)
            {
                for (int i = 0; i < table[correlatedFeature.Feature1].Count; i++)
                {
                    Point p = new Point(table[correlatedFeature.Feature1].ElementAt(i), table[correlatedFeature.Feature1].ElementAt(i));
                    if (!MinCircle.IsPointInside(correlatedFeature.CircleThreshold, p))
                        anomalyReport.Add(new AnomalyReport(correlatedFeature.Feature1 + "-" + correlatedFeature.Feature2, i));
                }
            }
            return anomalyReport;
        }

    }
}
