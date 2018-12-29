using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartMirror.ImageProcessing
{
    class ImageProcessor
    {
        private CascadeClassifier cascadeClassifier;
        private VideoCapture capture;
        protected CancellationTokenSource ts;
        private ImageRecognizer _Recognizer;
        public ImageProcessor()
        {
            cascadeClassifier = new CascadeClassifier(Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml");
            capture = new VideoCapture(); // new VideoCapture(@"C:\Users\saqib\Downloads\Video\(24) 100 HAPPY FACES - YouTube.MP4");
            ts = new CancellationTokenSource();
            _Recognizer = new ImageRecognizer();
        }
        public void StartProcessing(PictureBox picturebox,PictureBox Face,Action<int> processUserData=null)
        {
            Debug.Write("Processing Started...\n");
            Task.Factory.StartNew(() => {
                if (!_Recognizer.IsTrained)
                {
                    _Recognizer.Train();
                }
                while (!ts.IsCancellationRequested)
                {
                    using (var imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>().Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic))
                    {
                        if (imageFrame != null)
                        {
                            var grayframe = imageFrame.Convert<Gray, Byte>();
                            var faces = cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
                            //.Write("Face count: "+faces.Count());
                            //picturebox.Invoke((MethodInvoker)delegate
                            //{
                            //    Face.Image = null;
                            //});
                            foreach (var face in faces)
                            {
                                picturebox.Invoke((MethodInvoker)delegate
                                {
                                    Face.Image = grayframe.Copy(face).Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic).ToBitmap();
                                });
                                imageFrame.Draw(face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them
                                var Image = grayframe.Copy(face).Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                                Image._EqualizeHist();
                                var result = _Recognizer.RecognizeUser(Image);
                                processUserData?.Invoke(result);
                            }
                           picturebox.Invoke((MethodInvoker)delegate
                           {
                               picturebox.Image = imageFrame.ToBitmap();
                           });
                        }

                    }
                    Thread.Sleep(30);
                }
            },ts.Token);
        }
        public void Stop()
        {
            
            if (ts != null )
            {
                ts.Cancel();
                ts.Dispose();
            }
        }
    }
}
