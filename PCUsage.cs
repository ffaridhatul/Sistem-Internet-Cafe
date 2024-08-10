using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarNet.Models
{
        public class PCUsage
        {
            public int PCId { get; set; }
            public DateTime StartTime { get; private set; }
            public DateTime? EndTime { get; private set; }
            public bool IsInUse { get; private set; }

            public PCUsage(int pcId)
            {
                PCId = pcId;
                IsInUse = false;
            }

            public void StartUsage()
            {
                StartTime = DateTime.Now;
                IsInUse = true;
            }

            public void StopUsage()
            {
                EndTime = DateTime.Now;
                IsInUse = false;
            }

            public TimeSpan GetDuration()
            {
                return EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;
            }

            public double CalculateBill()
            {
            var duration = GetDuration();
            var minutes = duration.TotalMinutes;

            // Hitung jumlah interval 10 menit, bulatkan ke atas
            var intervals = Math.Ceiling(minutes / 10.0);

            // Hitung total biaya, 1000 rupiah per interval
            return intervals * 1000; // Assuming 1000 IDR per 10 minutes
        }

        public string GetStatus()
        {
            return IsInUse ? $"Mulai: {StartTime.ToShortTimeString()}" : "Tidak digunakan";
        }

        public string GetUsageSummary()
        {
            return $"Biaya = {CalculateBill()} rupiah";
        }
        public string[] ToArray()
        {
            return new string[]
            {
                PCId.ToString(),
                StartTime.ToString("g"),
                EndTime.HasValue ? EndTime.Value.ToString("g") : "N/A",
                GetDuration().ToString(@"hh\:mm\:ss"),
                CalculateBill().ToString()
            };
        }
    }


}
