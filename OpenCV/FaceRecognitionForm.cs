using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace FaceRecognitionApp
{
    public class FaceRecognitionForm : Form
    {
        private VideoCapture capture;
        private CascadeClassifier faceCascade;
        private CascadeClassifier eyeCascade;
        private Mat frame;
        private bool isCapturing = false;
        private PictureBox pictureBox;
        private Button btnStartStop;
        private Button btnCapture;
        private Button btnLoadClassifier;
        private Label lblStatus;
        private Timer frameTimer;

        public FaceRecognitionForm()
        {
            // Form temel ayarları - TAM NAMESPACE kullan
            this.Text = "OpenCV Yüz Tanıma Uygulaması";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.LightGray;
            this.Name = "FaceRecognitionForm";

            InitializeCustomComponents();
            CreateCascadeDirectory();
            LoadDefaultClassifiers();
        }

        private void CreateCascadeDirectory()
        {
            string cascadeDir = Path.Combine(Application.StartupPath, "haarcascades");
            if (!Directory.Exists(cascadeDir))
            {
                Directory.CreateDirectory(cascadeDir);
                MessageBox.Show($"Lütfen Haar cascade dosyalarını bu klasöre kopyalayın: {cascadeDir}",
                              "Dosya Gerekli", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitializeCustomComponents()
        {
            // PictureBox - TAM NAMESPACE kullan
            pictureBox = new PictureBox();
            pictureBox.Location = new System.Drawing.Point(10, 10);
            pictureBox.Size = new System.Drawing.Size(640, 480);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(pictureBox);

            // Başlat/Durdur Butonu
            btnStartStop = new Button();
            btnStartStop.Text = "Kamera Başlat";
            btnStartStop.Location = new System.Drawing.Point(660, 10);
            btnStartStop.Size = new System.Drawing.Size(120, 30);
            btnStartStop.BackColor = System.Drawing.Color.LightGreen;
            btnStartStop.Click += BtnStartStop_Click;
            this.Controls.Add(btnStartStop);

            // Yakala Butonu
            btnCapture = new Button();
            btnCapture.Text = "Görüntü Yakala";
            btnCapture.Location = new System.Drawing.Point(660, 50);
            btnCapture.Size = new System.Drawing.Size(120, 30);
            btnCapture.Enabled = false;
            btnCapture.BackColor = System.Drawing.Color.LightBlue;
            btnCapture.Click += BtnCapture_Click;
            this.Controls.Add(btnCapture);

            // Sınıflandırıcı Yükle Butonu
            btnLoadClassifier = new Button();
            btnLoadClassifier.Text = "Sınıflandırıcı Yükle";
            btnLoadClassifier.Location = new System.Drawing.Point(660, 90);
            btnLoadClassifier.Size = new System.Drawing.Size(120, 30);
            btnLoadClassifier.BackColor = System.Drawing.Color.LightYellow;
            btnLoadClassifier.Click += BtnLoadClassifier_Click;
            this.Controls.Add(btnLoadClassifier);

            // Durum Label
            lblStatus = new Label();
            lblStatus.Text = "Hazır - Sınıflandırıcılar yükleniyor...";
            lblStatus.Location = new System.Drawing.Point(10, 500);
            lblStatus.Size = new System.Drawing.Size(600, 20);
            lblStatus.ForeColor = System.Drawing.Color.Blue;
            lblStatus.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.Controls.Add(lblStatus);

            // Timer
            frameTimer = new Timer();
            frameTimer.Interval = 33;
            frameTimer.Tick += FrameTimer_Tick;
        }

        private void LoadDefaultClassifiers()
        {
            try
            {
                string faceCascadePath = @"haarcascades\haarcascade_frontalface_default.xml";
                string eyeCascadePath = @"haarcascades\haarcascade_eye.xml";

                if (!File.Exists(faceCascadePath))
                {
                    string opencvPath = Environment.GetEnvironmentVariable("OPENCV_DIR");
                    if (!string.IsNullOrEmpty(opencvPath))
                    {
                        faceCascadePath = Path.Combine(opencvPath, @"etc\haarcascades\haarcascade_frontalface_default.xml");
                        eyeCascadePath = Path.Combine(opencvPath, @"etc\haarcascades\haarcascade_eye.xml");
                    }
                }

                if (File.Exists(faceCascadePath))
                {
                    faceCascade = new CascadeClassifier(faceCascadePath);
                    lblStatus.Text = "Yüz sınıflandırıcı yüklendi";
                }
                else
                {
                    lblStatus.Text = "Yüz sınıflandırıcı dosyası bulunamadı!";
                    return;
                }

                if (File.Exists(eyeCascadePath))
                {
                    eyeCascade = new CascadeClassifier(eyeCascadePath);
                    lblStatus.Text += " - Göz sınıflandırıcı yüklendi";
                }

                if (faceCascade.Empty())
                {
                    lblStatus.Text = "Yüz sınıflandırıcı yüklenemedi!";
                    return;
                }

                lblStatus.Text = "Sınıflandırıcılar başarıyla yüklendi - Kamera başlatmaya hazır";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Sınıflandırıcı yükleme hatası: {ex.Message}";
            }
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if (!isCapturing)
            {
                StartCamera();
            }
            else
            {
                StopCamera();
            }
        }

        private void StartCamera()
        {
            try
            {
                capture = new VideoCapture(0);

                if (!capture.IsOpened())
                {
                    MessageBox.Show("Kamera açılamadı!");
                    return;
                }

                frame = new Mat();
                isCapturing = true;
                btnStartStop.Text = "Kamerayı Durdur";
                btnStartStop.BackColor = System.Drawing.Color.LightCoral;
                btnCapture.Enabled = true;
                lblStatus.Text = "Kamera çalışıyor...";

                frameTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kamera başlatma hatası: {ex.Message}");
            }
        }

        private void StopCamera()
        {
            isCapturing = false;
            frameTimer.Stop();

            capture?.Release();
            capture?.Dispose();
            capture = null;

            btnStartStop.Text = "Kamera Başlat";
            btnStartStop.BackColor = System.Drawing.Color.LightGreen;
            btnCapture.Enabled = false;
            lblStatus.Text = "Kamera durduruldu";

            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
        }

        private void FrameTimer_Tick(object sender, EventArgs e)
        {
            if (isCapturing && capture != null)
            {
                try
                {
                    if (capture.Read(frame) && !frame.Empty())
                    {
                        DetectAndDrawFaces(frame);

                        using (System.Drawing.Bitmap bitmap = BitmapConverter.ToBitmap(frame))
                        {
                            if (pictureBox.Image != null)
                            {
                                pictureBox.Image.Dispose();
                            }
                            pictureBox.Image = (System.Drawing.Bitmap)bitmap.Clone();
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Frame okuma hatası: {ex.Message}";
                }
            }
        }

        private void DetectAndDrawFaces(Mat image)
        {
            if (faceCascade == null || faceCascade.Empty()) return;

            try
            {
                using (Mat gray = new Mat())
                {
                    Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

                    // OpenCvSharp Rect kullan
                    OpenCvSharp.Rect[] faces = faceCascade.DetectMultiScale(
                        gray,
                        1.1,
                        3,
                        HaarDetectionTypes.ScaleImage,
                        new OpenCvSharp.Size(30, 30));

                    foreach (OpenCvSharp.Rect face in faces)
                    {
                        Cv2.Rectangle(image, face, new Scalar(0, 255, 0), 2);

                        Cv2.PutText(image, $"Yuz",
                                  new OpenCvSharp.Point(face.X, face.Y - 10),
                                  HersheyFonts.HersheySimplex, 0.5, new Scalar(0, 255, 0), 1);

                        if (eyeCascade != null && !eyeCascade.Empty())
                        {
                            using (Mat faceROI = new Mat(gray, face))
                            {
                                OpenCvSharp.Rect[] eyes = eyeCascade.DetectMultiScale(faceROI, 1.1, 2);

                                foreach (OpenCvSharp.Rect eye in eyes)
                                {
                                    OpenCvSharp.Point center = new OpenCvSharp.Point(
                                        face.X + eye.X + eye.Width / 2,
                                        face.Y + eye.Y + eye.Height / 2);
                                    int radius = (int)((eye.Width + eye.Height) * 0.25);
                                    Cv2.Circle(image, center, radius, new Scalar(255, 0, 0), 2);
                                }
                            }
                        }
                    }

                    Cv2.PutText(image, $"Tespit Edilen Yuz: {faces.Length}",
                              new OpenCvSharp.Point(10, 30),
                              HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 255, 0), 2);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Yüz tespit hatası: {ex.Message}";
            }
        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            if (frame != null && !frame.Empty())
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "JPEG Image|*.jpg|PNG Image|*.png|BMP Image|*.bmp",
                    Title = "Görüntüyü Kaydet",
                    FileName = $"yuz_tanima_{DateTime.Now:yyyyMMdd_HHmmss}.jpg"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        frame.SaveImage(saveDialog.FileName);
                        lblStatus.Text = $"Görüntü kaydedildi: {Path.GetFileName(saveDialog.FileName)}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Görüntü kaydetme hatası: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Kaydedilecek görüntü bulunamadı!");
            }
        }

        private void BtnLoadClassifier_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "XML Files|*.xml|All Files|*.*",
                Title = "Sınıflandırıcı Dosyasını Seçin"
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var newCascade = new CascadeClassifier(openDialog.FileName);
                    if (!newCascade.Empty())
                    {
                        faceCascade = newCascade;
                        lblStatus.Text = $"Sınıflandırıcı yüklendi: {Path.GetFileName(openDialog.FileName)}";
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz sınıflandırıcı dosyası!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sınıflandırıcı yükleme hatası: {ex.Message}");
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCamera();
            frameTimer?.Stop();
            frameTimer?.Dispose();

            faceCascade?.Dispose();
            eyeCascade?.Dispose();
            frame?.Dispose();

            if (pictureBox?.Image != null)
            {
                pictureBox.Image.Dispose();
            }

            base.OnFormClosing(e);
        }
    }
}