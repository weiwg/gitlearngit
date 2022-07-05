/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：李席高
 * 功能：图片工具类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20181019 lixigao  创建   
 * 20181219 luzhicheng  增加ImagePathToBytes方法,解决ImageToBytes线程占用问题
 ***************************************************** */


using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Encoder = System.Drawing.Imaging.Encoder;

namespace LY.Report.Core.Util.Tool
{
    /// <summary>
    /// 图片帮族类
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image">image</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    image.Dispose();//使用完毕必须释放,以防止线程占用
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// Convert imagePath to Byte[]
        /// </summary>
        /// <param name="imagePath">图片在系统的相对路径或绝对路径</param>
        /// <returns></returns>
        public static byte[] ImagePathToBytes(string imagePath)
        {
            try
            {
                using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    return buffer;
                }
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// Convert imagePath to Byte[]
        /// </summary>
        /// <param name="imagePath">图片在系统的绝对路径</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ImagePathToBytes(string imagePath, int width, int height)
        {
            return ResizeImage(ImagePathToBytes(imagePath), width, height);
        }

        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            try
            {
                MemoryStream ms = new MemoryStream(buffer);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
            catch (Exception)
            {
                // ignored
            }
            return null;

        }

        /// <summary>
        /// Convert Byte[] to a picture and Store it in file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CreateImageFromBytes(string fileName, byte[] buffer)
        {
            string file = fileName;
            Image image = BytesToImage(buffer);
            ImageFormat format = image.RawFormat;
            if (format.Equals(ImageFormat.Jpeg))
            {
                file += ".jpeg";
            }
            else if (format.Equals(ImageFormat.Png))
            {
                file += ".png";
            }
            else if (format.Equals(ImageFormat.Bmp))
            {
                file += ".bmp";
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                file += ".gif";
            }
            else if (format.Equals(ImageFormat.Icon))
            {
                file += ".icon";
            }
            FileInfo info = new FileInfo(file);
            Directory.CreateDirectory(info.Directory.FullName);
            File.WriteAllBytes(file, buffer);
            return file;
        }

        /// <summary>
        /// 输出错误图片
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static MemoryStream GraphicsErrorImage(string msg)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(40, Color.White));
            g.FillRectangle(Brushes.White, 0, 0, 65, 31);
            g.DrawString(msg, new Font("黑体", 15f), Brushes.White, 50.0F, 50.0F);
            bmp.Save(ms, ImageFormat.Jpeg);
            g.Dispose();
            bmp.Dispose();
            return ms;
        }
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        #region 生成文件MD5值
        /// <summary>
        /// 生成文件MD5值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string CreatFileMd5(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return BitConverter.ToString(retVal).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 生成文件MD5值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CreatFileMd5(byte[] buffer)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffer);
            return BitConverter.ToString(retVal).Replace("-", "").ToLower();
        }
        #endregion

        #region 生成文件SHA1值
        /// <summary>
        /// 生成文件SHA1值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string CreatFileSha1(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] retVal = sha1.ComputeHash(stream);
            return BitConverter.ToString(retVal).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 生成文件SHA1值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CreatFileSha1(byte[] buffer)
        {
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] retVal = sha1.ComputeHash(buffer);
            return BitConverter.ToString(retVal).Replace("-", "").ToLower();
        }
        #endregion

        #region 生成指定宽高图片
        /// <summary>
        /// 生成指定宽高图片
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] buffer, int width, int height)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return null;
            }

            Image image = Image.FromStream(new MemoryStream(buffer));
            if (width <= 0 && height <= 0)
            {
                return buffer;
            }

            //参数不完整时,使用等比缩放
            if (height <= 0 || width <= 0)
            {
                float imgScalingRatio = height > 0 ? height / ((float)image.Height) : width / ((float)image.Width);//图片缩放比例
                width = (int)(image.Width * imgScalingRatio);
                height = (int)(image.Height * imgScalingRatio);
            }

            if (height == image.Height && width == image.Width)
            {
                //如果宽高一致,则不处理
                return buffer;
            }
            Image newImage = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(newImage);
            graphics.Clear(Color.White);
            graphics.InterpolationMode = InterpolationMode.Default;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            EncoderParameters encoderParams = new EncoderParameters();
            EncoderParameter parameter = new EncoderParameter(Encoder.Quality, 90L);
            encoderParams.Param[0] = parameter;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo encoder = imageEncoders.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));
            MemoryStream newStream = new MemoryStream();
            if (encoder != null)
            {
                newImage.Save(newStream, encoder, encoderParams);
            }


            image.Dispose();
            newImage.Dispose();
            graphics.Dispose();
            return ImageToBytes(new Bitmap(newStream));//重新转换,解决生成的图片流不能直接生成图片的问题
        }
        #endregion

        #region 生成方形图片
        /// <summary>
        /// 生成方形图片
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream ResizeSquareImage(Stream stream)
        {
            Image image = Image.FromStream(stream);
            stream.Seek(0, SeekOrigin.Begin);
            int width = image.Width;
            int height = image.Height;
            if (height == width)
            {
                //如果宽高一致,则不处理
                return stream;
            }
            int newWidth = width > height ? width : height;//选取最大长度
            int xOffset = width < height ? (height - width) / 2 : 0;
            int yOffset = width > height ? (width - height) / 2 : 0;
            Image newImage = new Bitmap(newWidth, newWidth);
            Graphics graphics = Graphics.FromImage(newImage);
            graphics.Clear(Color.White);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawImage(image, new Rectangle(xOffset, yOffset, width, height), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
            EncoderParameters encoderParams = new EncoderParameters();
            EncoderParameter parameter = new EncoderParameter(Encoder.Quality, 100L);
            encoderParams.Param[0] = parameter;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo encoder = null;
            for (int i = 0; i < imageEncoders.Length; i++)
            {
                if (imageEncoders[i].FormatDescription.Equals("JPEG"))
                {
                    encoder = imageEncoders[i];
                    break;
                }
            }
            MemoryStream newStream = new MemoryStream();
            if (encoder != null)
            {
                newImage.Save(newStream, encoder, encoderParams);
            }

            image.Dispose();
            newImage.Dispose();
            graphics.Dispose();
            return new MemoryStream(ImageToBytes(new Bitmap(newStream)));//重新转换,解决生成的图片流不能直接生成图片的问题
        }
        #endregion

        #region 生成图片水印

        #region 文字水印
        
        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="stream">原图Stream</param>
        /// <param name="watermarkText">水印文字</param>
        public static Stream WatermarkText(Stream stream, string watermarkText)
        {
            return WatermarkText(stream, watermarkText, 9);
        }

        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="stream">原图Stream</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="fontname">字体</param>
        /// <returns></returns>
        public static Stream WatermarkTextFullScreen(Stream stream, string watermarkText, int fontsize = 22, string fontname = "微软雅黑")
        {
            Image img = Image.FromStream(stream);
            stream.Seek(0, SeekOrigin.Begin);
            int width = img.Width;
            int height = img.Height;

            //生成新图 新建一个bmp图片
            Image newImage = new Bitmap(img);
            //新建一个画板
            Graphics newG = Graphics.FromImage(newImage);
            //画图
            newG.DrawImage(newImage, 0, 0);
            //设置质量
            newG.InterpolationMode = InterpolationMode.HighQualityBicubic;
            newG.SmoothingMode = SmoothingMode.HighQuality;

            newG.DrawImage(img, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel);
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
            //将原点移动 到图片中点
            newG.TranslateTransform(0, height);
            //以原点为中心 转 -45度
            newG.RotateTransform(-45);

            int txtHeiht = width <200 && height < 200 ? 50 :100;//行高
            fontsize = txtHeiht < 100 && fontsize > 20 ? 16 : fontsize;
            int textWidth = watermarkText.Length * fontsize;//水印长度
            int txtWidthSpace = textWidth < 50 ? 25 : 50;//列间隔
            int heightCount = (height > width ? height : width) / txtHeiht;
            heightCount = heightCount > 2 ? heightCount :3;//最少3行
            int widthCount = width / textWidth > 3 ? Convert.ToInt32(width / textWidth) : 4;//最少4列
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            for (int i = 0; i < heightCount * 2; i++)//行循环
            {
                for (int j = 0; j < (widthCount > heightCount ? widthCount : heightCount) * 2; j++)//列循环
                {
                    newG.DrawString(watermarkText, drawFont, semiTransBrush, new PointF(-100 * i + j * (textWidth + txtWidthSpace), -txtHeiht * heightCount + i * txtHeiht));
                }
            }
            MemoryStream ms = new MemoryStream();
            newImage.Save(ms, ImageFormat.Jpeg);
            newImage.Dispose();
            img.Dispose();

            return ms;
        }

        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="stream">原图Stream</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="watermarkStatus">图片水印位置 1=左上 2=中上 3=右上 4=左中 5=正中 6=右中 7=左下 8=中下 9=右下</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="fontname">字体</param>
        /// <returns></returns>
        public static Stream WatermarkText(Stream stream, string watermarkText, int watermarkStatus, int fontsize = 14, string fontname = "微软雅黑")
        {
            Image img = Image.FromStream(stream);
            stream.Seek(0, SeekOrigin.Begin);
            int xpos = 0;
            int ypos = 0;
            int width = img.Width;
            int height = img.Height;
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);

            //生成新图 新建一个bmp图片
            Image newImage = new Bitmap(width, height);
            //新建一个画板
            Graphics newG = Graphics.FromImage(newImage);
            //设置质量
            newG.InterpolationMode = InterpolationMode.HighQualityBicubic;
            newG.SmoothingMode = SmoothingMode.HighQuality;
            //设置背景色
            newG.Clear(Color.White);
            //画图
            newG.DrawImage(img, new Rectangle(0, 0, newImage.Width, newImage.Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            Graphics g = Graphics.FromImage(newImage);
            SizeF crSize = g.MeasureString(watermarkText, drawFont);
            //计算水印坐标
            CalculateCoordinate(watermarkStatus, width, height, crSize.Width, crSize.Height, out xpos, out ypos);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.Black), xpos, ypos);
            g.Dispose();
            newG.Dispose();
            MemoryStream ms = new MemoryStream();
            newImage.Save(ms, ImageFormat.Jpeg);
            newImage.Dispose();
            img.Dispose();

            return ms;
        }
        #endregion

        #region 图片水印
        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="stream">原图Stream</param>
        /// <param name="watermarkStream">水印Stream</param>
        public static Stream WatermarkPhoto(Stream stream, Stream watermarkStream)
        {
            return WatermarkPhoto(stream, watermarkStream, 9, 5);
        }

        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="stream">原图Stream</param>
        /// <param name="watermarkStream">水印Stream</param>
        /// <param name="maxWidth">指定图片最大宽度</param>
        /// <param name="maxHeight">指定图片最大高度</param>
        /// <param name="watermarkStatus">图片水印位置 1=左上 2=中上 3=右上 4=左中 5=正中 6=右中 7=左下 8=中下 9=右下</param>
        /// <param name="watermarkTransparency">水印的透明度 1--10 10为不透明</param>
        /// <returns></returns>
        public static Stream WatermarkPhoto(Stream stream, Stream watermarkStream, int watermarkStatus, int watermarkTransparency)
        {
            Image img = Image.FromStream(stream);
            Image wmImage = Image.FromStream(watermarkStream);
            stream.Seek(0, SeekOrigin.Begin);
            int xpos = 0;
            int ypos = 0;
            float width = img.Width;
            float height = img.Height;
            float transparency = (watermarkTransparency >= 1 && watermarkTransparency <= 10) ? (watermarkTransparency / 10.0F) : 0.5F;
            MemoryStream ms = new MemoryStream();
            //如果计算后的宽高等于原图宽高则不作处理，直接保存
            if (img.Width == width && img.Height == height)
            {
                Graphics g = Graphics.FromImage(img);
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();
                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };
                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
                float[][] colorMatrixElements = { 
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };
                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                //计算水印坐标
                CalculateCoordinate(watermarkStatus, width, height, wmImage.Width, wmImage.Height, out xpos, out ypos);
                g.DrawImage(wmImage, new Rectangle(xpos, ypos, wmImage.Width, wmImage.Height), 0, 0, wmImage.Width, wmImage.Height, GraphicsUnit.Pixel, imageAttributes);
                g.Dispose();
                wmImage.Dispose();
                img.Save(ms, ImageFormat.Jpeg);
            }
            else
            {
                //生成新图 新建一个bmp图片
                Image newImage = new Bitmap((int)width, (int)height);
                //新建一个画板
                Graphics newG = Graphics.FromImage(newImage);
                //设置质量
                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //设置背景色
                newG.Clear(Color.White);
                //画图
                newG.DrawImage(img, new Rectangle(0, 0, newImage.Width, newImage.Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);

                Graphics g = Graphics.FromImage(newImage);
                //透明属性
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();
                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };
                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
                float[][] colorMatrixElements = { 
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };
                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                //计算水印坐标
                CalculateCoordinate(watermarkStatus, width, height, wmImage.Width, wmImage.Height, out xpos, out ypos);
                g.DrawImage(wmImage, new Rectangle(xpos, ypos, wmImage.Width, wmImage.Height), 0, 0, wmImage.Width, wmImage.Height, GraphicsUnit.Pixel, imageAttributes);
                g.Dispose();
                wmImage.Dispose();
                newImage.Save(ms, ImageFormat.Jpeg);
                newG.Dispose();
                newImage.Dispose();
                img.Dispose();
            }
            return ms;
        }

        #endregion

        #region 计算图片宽高
        /// <summary>
        /// 计算图片宽高
        /// </summary>
        /// <param name="maxWidth">指定图片最大宽度</param>
        /// <param name="maxHeight">指定图片最大高度</param>
        /// <param name="minWidth">指定图片最小宽度</param>
        /// <param name="minHeight">指定图片最小高度</param>
        /// <param name="width">原图宽度</param>
        /// <param name="height">原图高度</param>
        public static void CalculateWidthHeight(float maxWidth, float maxHeight, float minWidth, float minHeight, ref float width, ref float height)
        {
            //最大宽高>=原图宽高>=最小宽高 不做处理
            if (maxWidth >= width && maxHeight >= height && width >= minWidth && height >= minHeight)
            {

            }
            else
            {
                //宽大于高（横图）
                if (width > height)
                {
                    height = width > maxWidth ? height * (maxWidth / width) : (width < minWidth ? height * (minWidth / width) : height);
                    width = width > maxWidth ? maxWidth : (width < minWidth ? minWidth : width);
                }
                //宽等于高（正方图）
                else if (width == height)
                {
                    width = width > maxWidth ? maxWidth : (width < minWidth ? minWidth : width);
                    height = height > maxHeight ? maxHeight : (height < minHeight ? minHeight : height);
                }
                //高大于宽（竖图）
                else
                {
                    width = height > maxHeight ? width * (maxHeight / height) : (height < minHeight ? width * (minHeight / height) : width);
                    height = height > maxHeight ? maxHeight : (height < minHeight ? minHeight : height);
                }
            }
        }
        #endregion

        #region 计算水印坐标
        /// <summary>
        /// 计算水印坐标
        /// </summary>
        /// <param name="watermarkStatus">图片水印位置 1=左上 2=中上 3=右上 4=左中 5=正中 6=右中 7=左下 8=中下 9=右下</param>
        /// <param name="width">原图宽</param>
        /// <param name="height">原图高</param>
        /// <param name="watermarkWidth">水印宽</param>
        /// <param name="watermarkHeight">水印高</param>
        /// <param name="xpos">x轴坐标</param>
        /// <param name="ypos">y轴坐标</param>
        public static void CalculateCoordinate(int watermarkStatus, float width, float height, float watermarkWidth, float watermarkHeight, out int xpos, out int ypos)
        {
            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(width * (float).01);
                    ypos = (int)(height * (float).01);
                    break;
                case 2:
                    xpos = (int)((width * (float).50) - (watermarkWidth / 2));
                    ypos = (int)(height * (float).01);
                    break;
                case 3:
                    xpos = (int)((width * (float).99) - watermarkWidth);
                    ypos = (int)(height * (float).01);
                    break;
                case 4:
                    xpos = (int)(width * (float).01);
                    ypos = (int)((height * (float).50) - (watermarkHeight / 2));
                    break;
                case 5:
                    xpos = (int)((width * (float).50) - (watermarkWidth / 2));
                    ypos = (int)((height * (float).50) - (watermarkHeight / 2));
                    break;
                case 6:
                    xpos = (int)((width * (float).99) - watermarkWidth);
                    ypos = (int)((height * (float).50) - (watermarkHeight / 2));
                    break;
                case 7:
                    xpos = (int)(width * (float).01);
                    ypos = (int)((height * (float).99) - watermarkHeight);
                    break;
                case 8:
                    xpos = (int)((width * (float).50) - (watermarkWidth / 2));
                    ypos = (int)((height * (float).99) - watermarkHeight);
                    break;
                case 9:
                    xpos = (int)((width * (float).99) - watermarkWidth);
                    ypos = (int)((height * (float).99) - watermarkHeight);
                    break;
                default:
                    xpos = 0;
                    ypos = 0;
                    break;
            }
        }
        #endregion

        #endregion

    }

}
