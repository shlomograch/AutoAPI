using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationAPI.Models
{
    public class TestRunChartData
    {
        public int AmountPass { get; set; }
        public int AmountFail { get; set; }
        public int AmountInfo { get; set; }
        public int AmountAPI { get; set; }
        public int AmountGUI { get; set; }
        public Dictionary<string, int> TestNameAndCount { get; set; }
        public Dictionary<string, double> EnviromentNameAndCount { get; set; }
        public Dictionary<string, int> UserNameAndCount { get; set; }
        public List<TestRunResult> TestRunResults { get; set; }
        public Dictionary<string, double> DtPrecent { get; set; }
        public Dictionary<string, int> TestNameAndFailCount { get; set; }
    }


    public class ExecutedChartData
    {
        public int Executed { get; set; }
        public int NotExecuted { get; set; }
        public List<TestExecutionList> TestRunResults { get; set; }
    }
}