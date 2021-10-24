using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;

namespace JxBackendService.Common.Util
{
    public static class ValidateCodeUtil
    {
        ///  <summary>  
        ///  生成隨機碼  
        ///  </summary>  
        ///  <param  name="length">隨機碼個數www.52mvc.com</param>  
        ///  <returns></returns>  
        public static string CreateRandomCode(int length)
        {
            string randomcode = String.Empty;

            var random = new Random((int)DateTime.Now.Ticks);
            string textArray = "0123456789ABCDEFGHGKLMNPQRSTUVWXYZ";
            string textArrayConfig = ConfigurationManager.AppSettings["TextArray"];
            if (!string.IsNullOrEmpty(textArrayConfig))
            {
                textArray = textArrayConfig;
            }

            for (var i = 0; i < length; i++)
            {
                randomcode += textArray.Substring(random.Next() % textArray.Length, 1);
            }
            return randomcode;
        }

        private static int GetRandomNumber(int maxLength)
        {
            //System.Random Ran1 = new Random(Guid.NewGuid().GetHashCode());
            //int intReturn = Ran1.Next(maxLength);
            //Ran1 = null;

            byte[] randomNumber = new byte[1];
            RandomNumberGenerator.Create().GetBytes(randomNumber);
            int intReturn = randomNumber[0] % maxLength;


            return intReturn;
        }

        ///  <summary> 建立隨機碼圖片 </summary>  
        public static Bitmap CreateImage(string randomcode, int mapWidth, int mapHeight, int pixelCalculator)
        {
            int randAngle = 25; //隨機轉動角度
            int distortion = 60; //扭曲
            //int mapwidth = (int)(randomcode.Length * 90);
            //int mapHeight = 120;
            List<int> colorTable = new List<int>();
            Bitmap resultImg = new Bitmap(mapWidth, mapHeight);//建立圖片背景  
            Graphics graph = Graphics.FromImage(resultImg);
            graph.Clear(Color.AliceBlue);//清除畫面，填充背景  
            graph.DrawRectangle(new Pen(Color.Black, 0), 0, 0, resultImg.Width - 1, resultImg.Height - 1);//畫一個邊框  
                                                                                                          //graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//模式  
            Random rand = new Random();

            //定義顏色  
            Color[] c = { Color.Black,
                Color.Red,
                Color.DarkBlue,
                Color.Green,
                Color.Orange,
                Color.Brown,
                Color.DarkCyan,
                Color.Purple,
                Color.Aquamarine,
                Color.BlueViolet,
                Color.Chocolate,
                Color.Gray,
                Color.Honeydew,
                Color.LightBlue
             };

            //背景噪點生成
            Pen blackPen = new Pen(Color.LightGray, 0);
            for (int i = 0; i < 150; i++)
            {
                Color redColor = Color.FromArgb(randNumByColor(), randNumByColor(), randNumByColor());

                blackPen = new Pen(redColor, 0);

                int x = randNumByPoint(0, resultImg.Width);
                int y = randNumByPoint(0, resultImg.Height);

                resultImg.SetPixel(x, y, redColor);
                graph.DrawRectangle(blackPen, x, y, 1, 1);
            }



            #region 背景噪線生成
            //画曲线
            for (int i = 0; i < 10; i++)
            {
                Color redColor = Color.FromArgb(randNumByColor(), randNumByColor(), randNumByColor());
                Random random = new Random(Guid.NewGuid().GetHashCode());
                GraphicsPath gPath1 = new GraphicsPath();

                Pen p = new Pen(redColor);
                Point[] point = {
                                new Point(random.Next(1, resultImg.Width), random.Next(1, resultImg.Height)),
                                new Point(random.Next((resultImg.Width / 10) * 2, resultImg.Width), random.Next(1, resultImg.Height)),
                                new Point(random.Next((resultImg.Width / 10) * 4, resultImg.Width), random.Next(1, resultImg.Height)),
                                new Point(random.Next(1, resultImg.Width), random.Next(1, resultImg.Height))
                                    };
                gPath1.AddBeziers(point);
                graph.DrawPath(p, gPath1);
                p.Dispose();
            }
            #endregion

            //驗證碼旋轉，防止機器識別    
            char[] chars = randomcode.ToString().ToCharArray(0, randomcode.Length);//拆散字串成單字元陣列  
                                                                                   //文字距中  
            StringFormat fontStringFormat = new StringFormat(StringFormatFlags.NoClip);
            fontStringFormat.Alignment = StringAlignment.Center;
            fontStringFormat.LineAlignment = StringAlignment.Center;

            //定義字型  
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial" };
            int fontSize = 48;
            fontSize = (int)(((mapWidth * mapHeight) / pixelCalculator) * 1.5);
            for (int i = 0; i < chars.Length; i++)
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(font.Length);
                float angle = rand.Next(-randAngle, distortion);//轉動的度數

                LinearGradientBrush fontBrush = new LinearGradientBrush(new Rectangle(0, 0, resultImg.Width, resultImg.Height), Color.FromArgb(randNumByColor(), randNumByColor(), randNumByColor()),
                    Color.FromArgb(randNumByColor(), randNumByColor(), randNumByColor()), 45.0f, true);

                GraphicsPath gPath1 = new GraphicsPath(FillMode.Winding);
                Point[] point = {
                                new Point(rand.Next(1, resultImg.Width), rand.Next(1, resultImg.Height)),
                                new Point(rand.Next((resultImg.Width / 10) * 2, resultImg.Width), rand.Next(1, resultImg.Height)),
                                new Point(rand.Next((resultImg.Width / 10) * 4, resultImg.Width), rand.Next(1, resultImg.Height)),
                                new Point(rand.Next(1, resultImg.Width), rand.Next(1, resultImg.Height))
                                    };
                // 文字起始坐標
                Point dot = new Point(randNumByPoint((int)((mapWidth / 10) + (fontSize / 3)), fontSize + (10 * i)),
                    randNumByPoint((int)((mapHeight / 10) + (fontSize / 3)), fontSize + (10 * i)));

                #region 框邊文字
                // fontStyle: 0、 一般文字。1、粗體文字。2、斜體文字。4、加上底線的文字。8、中間有線條經過的文字。
                gPath1.AddBeziers(point);
                gPath1.AddString(chars[i].ToString(), new FontFamily(font[findex]), (int)System.Drawing.FontStyle.Bold, 100f, new PointF(dot.X, dot.Y), fontStringFormat);
                graph.DrawPath(new Pen(fontBrush), gPath1);
                #endregion

                #region 實心文字
                Font fontstyle = new System.Drawing.Font(font[findex], fontSize, System.Drawing.FontStyle.Bold);//字型樣式(引數2為字型大小)  

                graph.TranslateTransform(dot.X, dot.Y);//移動游標到指定位置  
                graph.RotateTransform(angle); // 文字旋轉角度
                graph.DrawString(chars[i].ToString(), fontstyle, fontBrush, 5, 2, fontStringFormat);

                Color redColor = Color.FromArgb(randNumByColor(), randNumByColor(), randNumByColor());
                Pen p = new Pen(redColor);
                graph.DrawBezier(p, point[0], point[1], point[2], point[3]);
                #endregion

                graph.RotateTransform(-angle);// 旋轉角度(轉回去)
                graph.TranslateTransform(2, -dot.Y);//移動游標到指定位置  
            }

            //graph.DrawString(randomcode,fontstyle,new SolidBrush(Color.Blue),2,2); //標準隨機碼  
            //生成圖片  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            resultImg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.ContentType = "image / gif";
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            //graph.Dispose();
            //map.Dispose();
            return resultImg;
        }

        #region 随机生成贝塞尔曲线
        /// <summary>
        /// 随机生成贝塞尔曲线
        /// </summary>
        public static Bitmap DrawRandomBezier(Int32 lineNum, int mapWidth, int mapHeight)
        {
            Bitmap b = new Bitmap(mapWidth, mapHeight);
            Random random = new Random(Guid.NewGuid().GetHashCode());

            b.MakeTransparent();
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            GraphicsPath gPath1 = new GraphicsPath();
            Int32 lineRandNum = random.Next(lineNum);

            for (int i = 0; i < (lineNum - lineRandNum); i++)
            {
                Color redColor = Color.FromArgb(randNumByColor(), randNumByColor(), randNumByColor());

                Pen p = new Pen(redColor);
                Point[] point = {
                                        new Point(random.Next(1, (b.Width / 10)), random.Next(1, (b.Height))),
                                        new Point(random.Next((b.Width / 10) * 2, (b.Width / 10) * 4), random.Next(1, (b.Height))),
                                        new Point(random.Next((b.Width / 10) * 4, (b.Width / 10) * 6), random.Next(1, (b.Height))),
                                        new Point(random.Next((b.Width / 10) * 8, b.Width), random.Next(1, (b.Height)))
                                    };

                gPath1.AddBeziers(point);
                g.DrawPath(p, gPath1);
                p.Dispose();
            }
            for (int i = 0; i < lineRandNum; i++)
            {
                Color redColor = Color.FromArgb(randNumByColor(), randNumByColor(), randNumByColor());

                Pen p = new Pen(redColor);
                Point[] point = {
                                new Point(random.Next(1, b.Width), random.Next(1, b.Height)),
                                new Point(random.Next((b.Width / 10) * 2, b.Width), random.Next(1, b.Height)),
                                new Point(random.Next((b.Width / 10) * 4, b.Width), random.Next(1, b.Height)),
                                new Point(random.Next(1, b.Width), random.Next(1, b.Height))
                                    };
                gPath1.AddBeziers(point);
                g.DrawPath(p, gPath1);
                p.Dispose();
            }
            return b;
        }
        #endregion

        private static int randNumByColor()
        {
            int colorNum = 0;
            Random random = new Random(Guid.NewGuid().GetHashCode());
            colorNum = random.Next(0, 255);
            return colorNum;
        }

        private static int randNumByPoint(int minNum, int maxNum)
        {
            int pointNum = 0;
            Random random = new Random(Guid.NewGuid().GetHashCode());
            pointNum = random.Next(minNum, maxNum);
            return pointNum;
        }
    }
}
