using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarNet.Models;
using System.IO;


namespace WarNet.Service
{
    public class PCUsageService
    {
        private string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PCUsageLog.txt");

        private List<PCUsage> pcUsages;

        public PCUsageService(int numberOfPCs)
        {
            pcUsages = new List<PCUsage>();
            for (int i = 1; i <= numberOfPCs; i++)
            {
                pcUsages.Add(new PCUsage(i));
            }
        }

        public PCUsage GetPCUsage(int pcId)
        {
            return pcUsages.Find(pc => pc.PCId == pcId);
        }

        public void StartUsage(int pcId)
        {
            var pcUsage = GetPCUsage(pcId);
            if (pcUsage != null && !pcUsage.IsInUse)
            {
                pcUsage.StartUsage();
            }
        }

        public void StopUsage(int pcId)
        {
            var pcUsage = GetPCUsage(pcId);
            if (pcUsage != null && pcUsage.IsInUse)
            {
                pcUsage.StopUsage();
            }
        }

        private void LogUsage(PCUsage pcUsage)
        {
            var logMessage = pcUsage.GetUsageSummary();
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        public List<PCUsage> GetAllUsages()
        {
            return pcUsages;
        }
    }
}
