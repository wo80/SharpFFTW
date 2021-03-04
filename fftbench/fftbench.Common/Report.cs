
namespace fftbench
{
    using fftbench.Benchmark;
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    public class Report
    {
        private Dictionary<int, TestResult> results;

        public Report(Dictionary<int, TestResult> results)
        {
            this.results = results;
        }

        #region Static methods

        public static string CreateText(List<BenchmarkResult> results, string pivot = null)
        {
            var s = new StringBuilder();

            var nfi = NumberFormatInfo.InvariantInfo;

            s.AppendLine();

            foreach (var value in GetList(results, pivot))
            {
                s.AppendFormat("{0,16}: ", value.Test.Name);
                s.AppendFormat(nfi, "{0,4:0.0}  ", value.Total);
                s.Append("[");
                s.AppendFormat(nfi, "min: {0,7:0.00}, ", value.Minimum * 1000);
                s.AppendFormat(nfi, "max: {0,7:0.00}, ", value.Maximum * 1000);
                s.AppendFormat(nfi, "mean: {0,7:0.00}, ", value.Mean * 1000);
                s.AppendFormat(nfi, "stddev: {0,7:0.00}", value.StdDev * 1000);
                s.Append("]");
                s.AppendLine();
            }

            s.AppendLine();
            s.AppendLine("Timing in microseconds.");

            return s.ToString();
        }

        private static List<BenchmarkResult> GetList(List<BenchmarkResult> results, string pivot)
        {
            var test = pivot == null
                ? results.OrderBy(t => t.Total).First()
                : results.Where(t => t.Test.Name == pivot).First();

            var list = new List<BenchmarkResult>(results.Count);

            double value = test.Total;

            foreach (var item in results.OrderBy(t => t.Total))
            {
                item.Total /= value;

                list.Add(item);
            }

            return list;
        }

        #endregion

        public string CreateText()
        {
            var s = new StringBuilder();

            var nfi = NumberFormatInfo.InvariantInfo;

            foreach (var item in results)
            {
                s.AppendLine();
                s.AppendFormat("Size: {0}", item.Key);
                s.AppendLine();

                foreach (var result in item.Value.Results)
                {
                    s.AppendFormat("{0,16}: ", result.Key);
                    s.AppendFormat(nfi, "{0,4:0.0}  ", result.Value.Total);
                    s.Append("[");
                    s.AppendFormat(nfi, "min: {0,7:0.00}, ", result.Value.Minimum * 1000);
                    s.AppendFormat(nfi, "max: {0,7:0.00}, ", result.Value.Maximum * 1000);
                    s.AppendFormat(nfi, "mean: {0,7:0.00}, ", result.Value.Mean * 1000);
                    s.AppendFormat(nfi, "stddev: {0,7:0.00}", result.Value.StdDev * 1000);
                    s.Append("]\n");
                }
            }

            return s.ToString();
        }

        // Generate HTML table (for CodeProject article). 
        public string CreateHtml(Dictionary<int, TestResult> results)
        {
            var s = new StringBuilder();

            var sorted = new Dictionary<string, Dictionary<int, BenchmarkResult>>();

            // Preprocess:
            foreach (var item in results)
            {
                int size = item.Key;

                foreach (var result in item.Value.Results)
                {
                    string name = result.Key;

                    Dictionary<int, BenchmarkResult> r;

                    if (!sorted.TryGetValue(name, out r))
                    {
                        r = new Dictionary<int, BenchmarkResult>();
                        sorted.Add(name, r);
                    }

                    r.Add(size, result.Value);
                }
            }

            foreach (var item in sorted)
            {
                s.AppendLine("\t\t<tr>");
                s.AppendLine("\t\t\t<td>" + item.Key + "</td>");

                foreach (var result in item.Value)
                {
                    s.AppendLine("\t\t\t<td>" + result.Value.Total + "</td>");
                    s.AppendLine("\t\t\t<td>" + result.Value.Mean.ToString("0.00") + "</td>");
                }

                s.AppendLine("\t\t</tr>");
            }

            return s.ToString();
        }
    }
}
