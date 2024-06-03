using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        private List<string> imagePaths = new List<string>();
        private Random random = new Random();
        private DateTime startTime;
        private Timer mainTimer = new Timer();
        private Timer countdownTimer = new Timer();
        private bool testStarted = false;
        private int score = 0;

        public Form3()
        {
            InitializeComponent();
            SetupMainTimer();
            SetupCountdownTimer();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        private void SetupMainTimer()
        {
            mainTimer.Interval = random.Next(200, 500);
            mainTimer.Tick += MainTimer_Tick;
        }
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            LoadAndDisplayRandomImage();
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
        private void LoadAndDisplayRandomImage()
        {
            if (imagePaths.Count > 0)
            {
                int randomIndex = random.Next(imagePaths.Count);
                string selectedImagePath = imagePaths[randomIndex];

                pictureBox1.ImageLocation = selectedImagePath;

                mainTimer.Interval = random.Next(200, 500);
            }
            else
            {
                MessageBox.Show("Brak obrazów w wybranym folderze.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Stop();
                countdownTimer.Stop();
                Close();
            }
        }
        private void LoadImages()
        {
            string imagePath = Path.Combine(Application.StartupPath, "C:\\Users\\hp\\Desktop\\pg\\sem_5\\POSK\\posk2\\projekt2\\Obrazy");

            if (Directory.Exists(imagePath))
            {
                imagePaths.AddRange(Directory.GetFiles(imagePath, "*.jpg"));
            }
            else
            {
                MessageBox.Show("Podana ścieżka folderu nie istnieje.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mainTimer.Stop();
                countdownTimer.Stop();
                Close();
            }
        }
        private void ShowResult()
        {
            MessageBox.Show($"Koniec testu. Twój wynik to: {score} punktów.", "Wynik", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if(!testStarted)
            {
                LoadImages();
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

        private void buttonReaction_Click_1(object sender, EventArgs e)
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

                LoadAndDisplayRandomImage(); 
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}