using System;
using System.Linq;
using System.Windows.Forms;
using WarNet.Models;
using WarNet.Service;

namespace WarNet
{
    public partial class MainForm : Form
    {
        private PCUsageService _pcUsageService;
        private Timer timer;

        public MainForm()
        {
            InitializeComponent();
            _pcUsageService = new PCUsageService(5);
            InitializeTimer();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("PCId", "PC ID");
            dataGridView1.Columns.Add("StartTime", "Waktu Mulai");
            dataGridView1.Columns.Add("EndTime", "Waktu Selesai");
            dataGridView1.Columns.Add("Duration", "Durasi");
            dataGridView1.Columns.Add("Cost", "Biaya");
        }

        private void AddUsageToGrid(PCUsage pcUsage)
        {
            dataGridView1.Rows.Add(pcUsage.ToArray());
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; // 1 detik
            timer.Tick += timer1_Tick;
            timer.Start();
        }

        private void StartPC(int pcId)
        {
            var pcUsage = _pcUsageService.GetPCUsage(pcId);

            if (pcUsage.IsInUse)
            {
                MessageBox.Show($"Error: PC {pcId} sudah diaktifkan. Anda tidak dapat mengaktifkannya lagi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _pcUsageService.StartUsage(pcId);
            UpdateStatusLabel(pcId);
            MessageBox.Show($"PC {pcId} sudah diaktifkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StopPC(int pcId)
        {
            var pcUsage = _pcUsageService.GetPCUsage(pcId);

            if (pcUsage.IsInUse)
            {
                _pcUsageService.StopUsage(pcId);
                AddUsageToGrid(pcUsage);
                MessageBox.Show($"PC {pcId} sudah dimatikan. {pcUsage.GetUsageSummary()}", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error: PC {pcId} belum diaktifkan. Anda harus menekan tombol 'Mulai' terlebih dahulu.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateStatusLabel(pcId);
        }

        private void UpdateStatusLabel(int pcId)
        {
            var pcUsage = _pcUsageService.GetPCUsage(pcId);
            var statusLabel = this.Controls.Find($"lblStatusPC{pcId}", true).FirstOrDefault() as Label;

            if (statusLabel != null)
            {
                statusLabel.Text = pcUsage.IsInUse ? $"Mulai: {pcUsage.StartTime.ToShortTimeString()}" : "Tidak digunakan";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var pcUsage in _pcUsageService.GetAllUsages())
            {
                if (pcUsage.IsInUse)
                {
                    var elapsed = DateTime.Now - pcUsage.StartTime;
                    var statusLabel = this.Controls.Find($"lblStatusPC{pcUsage.PCId}", true).FirstOrDefault() as Label;

                    if (statusLabel != null)
                    {
                        statusLabel.Text = $"Berjalan: {elapsed.Minutes} menit {elapsed.Seconds} detik";
                    }
                }
            }
        }

        // Event handler untuk tombol start
        private void startButtons1_Click(object sender, EventArgs e) => StartPC(1);
        private void startButtons2_Click(object sender, EventArgs e) => StartPC(2);
        private void startButtons3_Click(object sender, EventArgs e) => StartPC(3);
        private void startButtons4_Click(object sender, EventArgs e) => StartPC(4);
        private void startButtons5_Click(object sender, EventArgs e) => StartPC(5);

        // Event handler untuk tombol stop
        private void stopButtons1_Click(object sender, EventArgs e) => StopPC(1);
        private void stopButtons2_Click(object sender, EventArgs e) => StopPC(2);
        private void stopButtons3_Click(object sender, EventArgs e) => StopPC(3);
        private void stopButtons4_Click(object sender, EventArgs e) => StopPC(4);
        private void stopButtons5_Click(object sender, EventArgs e) => StopPC(5);

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
