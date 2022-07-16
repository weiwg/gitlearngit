/* ******************************************************
 * 版权：weig
 * 作者：weig
 * 功能：坐标轴计算
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20190122 weig  创建   
 ***************************************************** */

using System;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// 坐标轴计算
    /// </summary>
    public class CoordinateHelper
    {
        private const double EarthRadius = 6378137.0; //地球的行星半径,单位M(腾讯地图尺寸)
        private const double Oblateness = 1 / 298.257; //地球扁率

        #region 根据距离获取对应的坐标差
        /// <summary>
        /// 根据距离获取对应的坐标差
        /// 纬度绝对值大于81.14321542034999°,存在纬度+-大于90°情况,注意判断,并设最大值为90°
        /// </summary>
        /// <param name="latitude">纬度y</param>
        /// <param name="distance">距离(米)</param>
        /// <returns>返回经纬度差值</returns>
        public static CoordinateModel GetCoordinateDifference(double latitude, int distance)
        {
            /*
             * 同一经线上纬度差1°经线长约为111km
             * 同一纬线圈上,经度差1°,其长约为111*cosαkm.（α为地理纬度）
             * 同一经线上纬度差1°,经线长范围110573.1375683825(m)-111692.61056247617(m)(腾讯地图尺寸计算)
            */

            const double longDifference = 110*1000; //经线长度设短一点,扩大坐标范围,以防遗漏边缘数据
            CoordinateModel coordinate = new CoordinateModel
            {
                Longitude = Math.Abs(distance / (longDifference * Math.Cos(GetRadian(latitude)))),
                Latitude = Math.Abs(distance/longDifference)
            };

            return coordinate;
        }

        /// <summary>
        /// 根据距离获取对应的坐标差
        /// 纬度绝对值大于81.14321542034999°,存在纬度+-大于90°情况,注意判断,并设最大值为90°
        /// </summary>
        /// <param name="location">经纬度</param>
        /// <param name="distance">距离(米)</param>
        /// <returns>返回经纬度差值</returns>
        public static DifferenceCoordinateModel GetCoordinateDifference(CoordinateModel location, int distance)
        {
            /*
             * 同一经线上纬度差1°经线长约为111km
             * 同一纬线圈上,经度差1°,其长约为111*cosαkm.（α为地理纬度）
             * 同一经线上纬度差1°,经线长范围110573.1375683825(m)-111692.61056247617(m)(腾讯地图尺寸计算)
            */

            const double longDifference = 110*1000; //经线长度设短一点,扩大坐标范围,以防遗漏边缘数据
            CoordinateModel coordinate = new CoordinateModel
            {
                Longitude = Math.Abs(distance / (longDifference * Math.Cos(GetRadian(location.Latitude)))),
                Latitude = Math.Abs(distance/longDifference)
            };

            DifferenceCoordinateModel differenceCoordinate = new DifferenceCoordinateModel
            {
                MinLongitude = Convert.ToDouble(location.Longitude) - coordinate.Longitude,
                MaxLongitude = Convert.ToDouble(location.Longitude) + coordinate.Longitude,
                MinLatitude = Convert.ToDouble(location.Latitude) - coordinate.Longitude,
                MaxLatitude = Convert.ToDouble(location.Latitude) + coordinate.Longitude
            };
            differenceCoordinate.MinLongitude = differenceCoordinate.MinLongitude < -180 ? -180 - differenceCoordinate.MinLongitude : differenceCoordinate.MinLongitude;
            differenceCoordinate.MinLatitude = differenceCoordinate.MinLatitude < -90 ? -90 : differenceCoordinate.MinLatitude;
            differenceCoordinate.MaxLongitude = differenceCoordinate.MaxLongitude > 180 ? 180 - differenceCoordinate.MaxLongitude : differenceCoordinate.MaxLongitude;
            differenceCoordinate.MaxLatitude = differenceCoordinate.MaxLatitude > 90 ? 90 : differenceCoordinate.MaxLatitude;
            return differenceCoordinate;
        }
        #endregion

        #region 计算两坐标点距离
        /// <summary>
        /// 计算两坐标点距离
        /// </summary>
        /// <param name="longitude1">第一个点经度</param>
        /// <param name="latitude1">第一个点纬度</param>
        /// <param name="longitude2">第二个点经度</param>
        /// <param name="latitude2">第二个点纬度</param>
        /// <returns>返回两点距离(米)</returns>
        public static double CalculateDistance(double longitude1, double latitude1, double longitude2, double latitude2)
        {
            return CalculateDistance(new CoordinateModel {Longitude = longitude1, Latitude = latitude1}, new CoordinateModel {Longitude = longitude2, Latitude = latitude2});
        }

        /// <summary>
        /// 计算两坐标点距离
        /// </summary>
        /// <param name="coordinate1">第一个点</param>
        /// <param name="coordinate2">第二个点</param>
        /// <returns>返回两点距离(米)</returns>
        public static double CalculateDistance(CoordinateModel coordinate1, CoordinateModel coordinate2)
        {
            double f = GetRadian((coordinate1.Latitude + coordinate2.Latitude) / 2);
            double g = GetRadian((coordinate1.Latitude - coordinate2.Latitude) / 2);
            double l = GetRadian((coordinate1.Longitude - coordinate2.Longitude) / 2);

            double sg = Math.Sin(g);
            sg = sg * sg;
            double sl = Math.Sin(l);
            sl = sl * sl;
            double sf = Math.Sin(f);
            sf = sf * sf;

            //const double earthRadius = 6378137.0; //地球的行星半径,单位M(腾讯地图尺寸)
            //const double oblateness = 1 / 298.257;//地球扁率
            
            double s = sg * (1 - sl) + (1 - sf) * sl;
            double c = (1 - sg) * (1 - sl) + sf * sl;

            double w = Math.Atan(Math.Sqrt(s / c));
            double r = Math.Sqrt(s * c) / w;
            double d = 2 * w * EarthRadius;
            double h1 = (3 * r - 1) / 2 / c;
            double h2 = (3 * r + 1) / 2 / s;

            double distance = d * (1 + Oblateness * (h1 * sf * (1 - sg) - h2 * (1 - sf) * sg));
            return double.IsNaN(distance) ? 0.0 : distance;//处理除数为0的情况
        }
        #endregion

        #region 角度转弧度
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        private static double GetRadian(double angle)
        {
            return angle * Math.PI / 180.0;//角度角×度转弧度 π/180
        }
        #endregion

        #region 长度单位转换
        /// <summary>
        /// 长度单位转换
        /// </summary>
        /// <param name="length">长度(千米)</param>
        /// <returns></returns>
        private string ConvertLength(double length)
        {
            //string[] arrLength = length.ToString(CultureInfo.InvariantCulture).Split('.');
            //if (arrLength[0].Length > 3)
            //{
            //    return (Convert.ToDouble(length) / 1000).ToString("0.00")+"公里(km)";
            //}
            //else
            //{
            //    return Convert.ToDouble(length).ToString("0") + "米";
            //}
            if (length < 1)
            {
                return Convert.ToDouble(length * 1000).ToString("0") + "米";
            }
            else
            {
                return Convert.ToDouble(length).ToString("0.00") + "公里";
            }
        }
        #endregion

    }

    #region 坐标轴实体
    /// <summary>
    /// 坐标轴实体
    /// </summary>
    public class CoordinateModel
    {
        private double _longitude;
        /// <summary>
        /// 经度x(经度差x)
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = (value > 180 || value < -180) ? 0 : value; }
        }
        private double _latitude;
        /// <summary>
        /// 纬度y(纬度差y)
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = (value > 90 || value < -90) ? 0 : value; }
        }
    }

    /// <summary>
    /// 距离坐标差实体
    /// </summary>
    public class DifferenceCoordinateModel
    {
        private double _minLongitude;
        /// <summary>
        /// 经度x(经度差x)
        /// </summary>
        public double MinLongitude
        {
            get { return _minLongitude; }
            set { _minLongitude = (value > 180 || value < -180) ? 0 : value; }
        }
        private double _minLatitude;
        /// <summary>
        /// 纬度y(纬度差y)
        /// </summary>
        public double MinLatitude
        {
            get { return _minLatitude; }
            set { _minLatitude = (value > 90 || value < -90) ? 0 : value; }
        }
        private double _maxLongitude;
        /// <summary>
        /// 经度x(经度差x)
        /// </summary>
        public double MaxLongitude
        {
            get { return _maxLongitude; }
            set { _maxLongitude = (value > 180 || value < -180) ? 0 : value; }
        }
        private double _maxLatitude;
        /// <summary>
        /// 纬度y(纬度差y)
        /// </summary>
        public double MaxLatitude
        {
            get { return _maxLatitude; }
            set { _maxLatitude = (value > 90 || value < -90) ? 0 : value; }
        }
    }
    #endregion
}
