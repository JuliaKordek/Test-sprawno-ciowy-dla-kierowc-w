using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace WindowsFormsApp1
{
    public partial class Form5 : Form
    {
        private List<string> soundPaths = new List<string>();
        private Random random = new Random();
        private DateTime startTime;
        private Timer mainTimer = new Timer();
        private Timer countdownTimer = new Timer();
        private bool testStarted = false;
        private int score = 0;
        public Form5()
        {
            InitializeComponent();
            SetupMainTimer(); 
            SetupCountdownTimer();
        }
        private void SetupMainTimer()
        {
            mainTimer.Interval = random.Next(200, 500); 
            mainTimer.Tick += MainTimer_Tick; 
        }
        private void MainTimer_Tick(object sender, EventArgs e) 
        {
            PlayRandomSound();
        }
        private void SetupCountdownTimer()
        {
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;
        }
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = TimeSpan.FromSeconds(10) - (DateTime.Now - startTime);
            label1.Text = $"Czas do końca testu: {remainingTime.TotalSeconds:0} sekund(y)";
            if (remainingTime <= TimeSpan.Zero)
            {
                mainTimer.Stop();
                countdownTimer.Stop();
                ShowResult();
                Close();
            }
        }
        private void LoadSounds()
        {
            string soundPath = "C:\\Users\\hp\\Desktop\\pg\\sem_5\\POSK\\posk2\\projekt2\\dzwieki";

            if (Directory.Exists(soundPath))
            {
                soundPaths.AddRange(Directory.GetFiles(soundPath, "*.wav"));
            }
            else
            {
                MessageBox.Show("Podana ścieżka folderu z dźwiękami nie istnieje.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mainTimer.Stop();
                countdownTimer.Stop();
                Close();
            }
        }
        private void PlayRandomSound()
        {
            if (soundPaths.Count > 0)
            {
                int randomIndex = random.Next(soundPaths.Count);
                string selectedSoundPath = soundPaths[randomIndex];

                SoundPlayer player = new SoundPlayer(selectedSoundPath);
                player.Play();

                mainTimer.Interval = random.Next(200, 500);
            }
            else
            {
                MessageBox.Show("Brak dźwięków w wybranym folderze.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mainTimer.Stop();
                countdownTimer.Stop();
                Close();
            }
        }
        private void ShowResult()
        {
            MessageBox.Show($"Koniec testu. Twój wynik to: {score} punktów.", "Wynik", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!testStarted)
            {
                LoadSounds();
                startTime = DateTime.Now;
                mainTimer.Start();
                countdownTimer.Start();
                testStarted = true;
                
      
            }
            else
            {
                MessageBox.Show("Test został już rozpoczęty.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!testStarted)
            {
                MessageBox.Show("Naciśnij 'Start', aby rozpocząć test.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                
                TimeSpan reactionTime = DateTime.Now - startTime;
                int points = (int)(10 - reactionTime.TotalSeconds); 
                points = Math.Max(points, 0); 
                score += points;

                PlayRandomSound(); 
       
            }
        }
    }
}
