/* ******************************************************
 * 版权：weig
 * 作者：weig
 * 功能：二维码生成工具
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20200619 weig  创建   
 ***************************************************** */

 using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// 二维码生成
    /// </summary>
    public class QrCodeHelper
    {
        #region 生成二维码

        #region 生成二维码
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="contexts">编码内容</param>
        public static Bitmap EncodeQrCode(string contexts)
        {
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.DisableECI = true;
            //设置内容编码
            options.CharacterSet = "UTF-8";
            //设置二维码的宽度和高度
            options.Width = 300;
            options.Height = 300;
            //设置二维码的边距,单位不是固定像素
            options.Margin = 1;

            return EncodeQrCode(contexts, options);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="width">二维码的宽度</param>
        /// <param name="height">二维码的高度</param>
        /// <param name="margin">二维码的边距</param>
        /// <returns></returns>
        public static Bitmap EncodeQrCode(string contexts, int width, int height, int margin)
        {
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.DisableECI = true;
            //设置内容编码
            options.CharacterSet = "UTF-8";
            //设置二维码的宽度和高度
            options.Width = width;
            options.Height = height;
            //设置二维码的边距,单位不是固定像素
            options.Margin = margin;

            return EncodeQrCode(contexts, options);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="options">编码选项</param>
        /// <returns></returns>
        public static Bitmap EncodeQrCode(string contexts, QrCodeEncodingOptions options)
        {
            BarcodeWriter<Bitmap> barcodeWriter = new BarcodeWriter<Bitmap>();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = options;

            return barcodeWriter.Write(contexts);
        }
        #endregion

        #region 生成带Logo的二维码
        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="imageUrl">Logo图片Url</param>
        public static Bitmap EncodeQrCodeByLogo(string contexts, string imageUrl)
        {
            return EncodeQrCodeByLogo(contexts, GetUrlImage(imageUrl), 300, 300, ErrorCorrectionLevel.H);
        }

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="imageUrl">Logo图片Url</param>
        /// <param name="width">二维码的宽度</param>
        /// <param name="height">二维码的高度</param>
        public static Bitmap EncodeQrCodeByLogo(string contexts, string imageUrl, int width, int height)
        {
            return EncodeQrCodeByLogo(contexts, GetUrlImage(imageUrl), width, height, ErrorCorrectionLevel.H);
        }

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="logoBitmap">Logo图片</param>
        public static Bitmap EncodeQrCodeByLogo(string contexts, Bitmap logoBitmap)
        {
            return EncodeQrCodeByLogo(contexts, logoBitmap, 300, 300, ErrorCorrectionLevel.H);
        }

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="logoBitmap">Logo图片</param>
        /// <param name="width">二维码的宽度</param>
        /// <param name="height">二维码的高度</param>
        public static Bitmap EncodeQrCodeByLogo(string contexts, Bitmap logoBitmap, int width, int height)
        {
            return EncodeQrCodeByLogo(contexts, logoBitmap, width, height, ErrorCorrectionLevel.H);
        }

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="logoBitmap">Logo图片</param>
        /// <param name="width">二维码的宽度</param>
        /// <param name="height">二维码的高度</param>
        /// <param name="ecLevel">二维码纠错级别</param>
        public static Bitmap EncodeQrCodeByLogo(string contexts, Bitmap logoBitmap, int width, int height, ErrorCorrectionLevel ecLevel)
        {
            if (logoBitmap == null)
            {
                return EncodeQrCode(contexts, width, height, 1);
            }
            //构造二维码写码器
            MultiFormatWriter multiFormatWriter = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

            hint.Add(EncodeHintType.ERROR_CORRECTION, ecLevel);

            //生成二维码 
            BitMatrix bitMatrix = multiFormatWriter.encode(contexts, BarcodeFormat.QR_CODE, width, height, hint);
            BarcodeWriter<Bitmap> barcodeWriter = new BarcodeWriter<Bitmap>();
            Bitmap qrCodeBitmap = barcodeWriter.Write(bitMatrix);


            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bitMatrix.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleW = Math.Min((int)(rectangle[2] / 3.5), logoBitmap.Width);
            int middleH = Math.Min((int)(rectangle[3] / 3.5), logoBitmap.Height);
            int middleL = (qrCodeBitmap.Width - middleW) / 2;
            int middleT = (qrCodeBitmap.Height - middleH) / 2;

            //将img转换成bmp格式，否则后面无法创建Graphics对象
            Bitmap qrCodeLogoBitmap = new Bitmap(qrCodeBitmap.Width, qrCodeBitmap.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(qrCodeLogoBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(qrCodeBitmap, 0, 0);
            }
            //将二维码插入图片
            Graphics myGraphic = Graphics.FromImage(qrCodeLogoBitmap);
            //白底
            myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
            myGraphic.DrawImage(logoBitmap, middleL, middleT, middleW, middleH);

            return qrCodeLogoBitmap;
        }
        #endregion

        #endregion

        #region 生成条形码
        /// <summary>
        /// 生成条形码
        /// 只支持数字,偶数个数字,最大长度80
        /// </summary>
        /// <param name="contexts">编码内容</param>
        public static Bitmap CreateBarCode(string contexts)
        {
            EncodingOptions options = new EncodingOptions()
            {
                Width = 150,
                Height = 50,
                Margin = 2
            };

            //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
            //如果想生成可识别的可以使用 CODE_128 格式
            return CreateBarCode(contexts, options, BarcodeFormat.CODE_128);
        }

        /// <summary>
        /// 生成条形码
        /// 只支持数字,偶数个数字,最大长度80
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="width">二维码的宽度</param>
        /// <param name="height">二维码的高度</param>
        /// <param name="margin">二维码的边距</param>
        public static Bitmap CreateBarCode(string contexts, int width, int height, int margin)
        {
            EncodingOptions options = new EncodingOptions()
            {
                Width = width,
                Height = height,
                Margin = margin
            };

            //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
            //如果想生成可识别的可以使用 CODE_128 格式
            return CreateBarCode(contexts, options, BarcodeFormat.CODE_128);
        }

        /// <summary>
        /// 生成条形码
        /// 只支持数字,偶数个数字,最大长度80
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="options">编码选项</param>
        public static Bitmap CreateBarCode(string contexts, EncodingOptions options)
        {
            //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
            //如果想生成可识别的可以使用 CODE_128 格式
            return CreateBarCode(contexts, options, BarcodeFormat.CODE_128);
        }

        /// <summary>
        /// 生成条形码
        /// 只支持数字,偶数个数字,最大长度80
        /// </summary>
        /// <param name="contexts">编码内容</param>
        /// <param name="options">编码选项</param>
        /// <param name="barcodeFormat">条形码格式</param>
        public static Bitmap CreateBarCode(string contexts, EncodingOptions options, BarcodeFormat barcodeFormat)
        {
            BarcodeWriter<Bitmap> barcodeWriter = new BarcodeWriter<Bitmap>();
            barcodeWriter.Format = barcodeFormat;
            barcodeWriter.Options = options;
            return barcodeWriter.Write(contexts);
        }
        #endregion

        #region 识别二维码/条形码
        /// <summary>
        /// 识别二维码/条形码
        /// 识别失败，返回空字符串
        /// </summary>
        /// <param name="filename">指定二维码图片位置</param>
        public static string DecodeQrCode(string filename)
        {
            BarcodeReader barcodeReader = new BarcodeReader();
            barcodeReader.Options.CharacterSet = "UTF-8";
            Bitmap map = new Bitmap(filename);
            Result result = barcodeReader.Decode(map);
            return result == null ? "" : result.Text;
        }
        #endregion

        #region 获取url图片
        /// <summary>
        /// 获取url图片
        /// </summary>
        /// <param name="imageUrl">图片Url</param>
        /// <returns></returns>
        public static Bitmap GetUrlImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return null;
            }
            try
            {
                //创建一个request 同时可以配置requst其余属性  
                System.Net.WebRequest imgRequst = System.Net.WebRequest.Create(imageUrl);
                Stream stream = imgRequst.GetResponse().GetResponseStream();
                //以流的方式返回图片  
                return stream == null ? null : new Bitmap(new Bitmap(stream));
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
        #endregion

        #region
        //public static string CreateQrCode(string content, string fileName)
        //{
        //    if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(fileName)) return "";

        //    ByteMatrix matrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, 230, 230);
        //    EncoderParameters eps = new EncoderParameters();
        //    eps.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

        //    int width = matrix.Width;
        //    int height = matrix.Height;
        //    Bitmap bmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            bmap.SetPixel(x, y, matrix.get_Renamed(x, y) != -1 ? ColorTranslator.FromHtml("0xFF000000") : ColorTranslator.FromHtml("0xFFFFFFFF"));
        //        }
        //    }

        //    fileName = HttpContext.Current.Server.MapPath(fileName);
        //    bmap.SetResolution(96f, 96f);
        //    bmap.Save(fileName);

        //    return fileName;
        //}

        //public static Bitmap CreateQrCode(string content)
        //{
        //    if (string.IsNullOrEmpty(content)) return null;

        //    ByteMatrix matrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, 230, 230);
        //    EncoderParameters eps = new EncoderParameters();
        //    eps.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

        //    int width = matrix.Width;
        //    int height = matrix.Height;
        //    Bitmap bmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            bmap.SetPixel(x, y, matrix.get_Renamed(x, y) != -1 ? ColorTranslator.FromHtml("0xFF000000") : ColorTranslator.FromHtml("0xFFFFFFFF"));
        //        }
        //    }

        //    bmap.SetResolution(96f, 96f);

        //    return bmap;
        //}
        #endregion
    }
}
