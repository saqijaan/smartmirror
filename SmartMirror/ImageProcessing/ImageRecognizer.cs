using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using System.Drawing;

using SmartMirror.Models;
using System.Diagnostics;

namespace SmartMirror.ImageProcessing
{
    class ImageRecognizer
    {
        private static FaceRecognizer _faceRecognizer;
        private List<Image<Gray,byte>> Images;
        private List<int> Ids;
        private string LastError = null;
        private bool trained = false;
        public bool IsTrained { get { return trained; } }
        public ImageRecognizer()
        {
            Images = new List<Image<Gray, byte>>();
            Ids = new List<int>();
            _faceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8,100);
        }

        private bool LoadTrainingData()
        {
            try
            {
                Images.Clear();
                Ids.Clear();
                foreach (face Face in face.get())
                {
                    Images.Add(Face.Image.ToGray());
                    Ids.Add(Face.Id);
                }
                return true;
            }catch(Exception)
            {
                return false;
            }
        }
        public bool Train()
        {
            try
            {
                LoadTrainingData();
                if (Images != null && Ids != null)
                {
                    _faceRecognizer.Train(Images.ToArray(), Ids.ToArray());
                    trained = true;
                    
                    Debug.Write(string.Format("Trained with {0} Images\n",Images.Count()));
                    return true;
                }
                else
                    return false;
            }catch(Exception ex)
            {
                LastError = ex.ToString();
                return false;
            }
            
        }

        public bool reTrain()
        {
            try
            {
                LoadTrainingData();
                if (Images != null && Ids != null)
                {
                    _faceRecognizer.Train(Images.ToArray(), Ids.ToArray());
                    trained = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                LastError = ex.ToString();
                return false;
            }
        }

        public int RecognizeUser(Image<Gray, byte> userImage)
        {
            var result = _faceRecognizer.Predict(userImage.Resize(100, 100, Inter.Cubic));
            return result.Label;
        }

    }
}
