using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ScreenShotLogic
{
    public class ScreenShot
    {
        private Variables variables;
        private string path;
        private System.Threading.Timer screenShotTimer;
        private ProviderScreenShot providerScreenShot;

        public ScreenShot()
        {
            variables = new Variables();
            path = variables.pathScreenShots; 
            providerScreenShot = new ProviderScreenShot();
        }

        public void Start()
        {
            InitTimer();
        }

        public void Stop()
        {
            try
            {
                screenShotTimer.Dispose();
            }
            catch (Exception)
            {
            }
        }


        private void InitTimer()
        {
            screenShotTimer = new System.Threading.Timer(new TimerCallback(ScreenShotTimer_Tick), null, 0, variables.screenShotTimerInMillisec);
        }
        
        private void ScreenShotTimer_Tick(object state)
        {
            #region Старый вариант

            //SaveScreen(ImageFromScreen(true));

            #endregion

            #region Новый вариант

            

            try
            {
                SaveScreen((Bitmap)ScreenCapture.CaptureScreen(true));
                //string fileName = System.IO.Path.Combine(path, $"{DateTime.Now.ToString().Replace('.', '-').Replace(' ', '-').Replace(':', '-')}.jpg");
                //if (!providerScreenShot.VerifyFolder(path))
                //{
                //    Directory.CreateDirectory(path);
                //}

                //Bitmap bitmap;
                //using (bitmap = (Bitmap)ScreenCapture.CaptureScreen(true))
                //{
                //    ffmp.PushFrame(bitmap);
                //    SaveScreen(ImageFromScreen(true));
                //}

                //PrintScreen ps = new PrintScreen();

                //ps.CaptureScreenToFile(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception)
            {
            }

            #endregion



        }


        private void SaveScreen(Bitmap screen)
        {
            try
            {
                string fileName = System.IO.Path.Combine(path, $"{DateTime.Now.ToString().Replace('.', '-').Replace(' ', '-').Replace(':', '-')}.jpg");
                if (!providerScreenShot.VerifyFolder(path))
                {
                    Directory.CreateDirectory(path);
                }
                screen.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception)
            {
            }
            
        }


        public Bitmap ImageFromScreen(bool withCursor) 
        {
            Bitmap bmp = null;
            try
            {
                
                if (Screen.AllScreens.Length > 1)
                {
                    int[] imgWidth = new int[Screen.AllScreens.Length];
                    int[] imgHeight = new int[Screen.AllScreens.Length];
                    for (int i = 0; i < Screen.AllScreens.Length; i++)
                    {
                        imgWidth[i] = Screen.AllScreens[i].Bounds.Width;
                        imgHeight[i] = Screen.AllScreens[i].Bounds.Height;
                    }
                    imgWidth.Max();
                    bmp = new Bitmap(imgWidth.Max() * 2, imgHeight.Max());
                    using (var gr = Graphics.FromImage(bmp))
                    {
                        int curX = 0;
                        int curY = 0;
                        for (int i = 0; i < Screen.AllScreens.Length; i++)
                        {
                            gr.CopyFromScreen(Screen.AllScreens[i].Bounds.X, Screen.AllScreens[i].Bounds.Y, curX, 0, Screen.AllScreens[i].Bounds.Size);
                            curX += Screen.AllScreens[i].Bounds.Width;
                            curY += Screen.AllScreens[i].Bounds.Height;
                        }

                        if (withCursor)
                        {
                            if (Cursor.Current != null)
                                using (Icon cursor = Icon.FromHandle(Cursor.Current.Handle))
                                    gr.DrawIcon(cursor, new Rectangle(Cursor.Position, cursor.Size));
                        }
                    }

                }
            }
            catch (Exception)
            {
                return bmp;
            }
            
            return bmp;
        }

        

    }
}
