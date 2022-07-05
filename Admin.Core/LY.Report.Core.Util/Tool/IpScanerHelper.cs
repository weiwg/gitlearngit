/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：IP库帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20190124 luzhicheng  创建   
 ***************************************************** */

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EonUp.UnifiedAuth.Util.Tool
{
    /// <summary>  
    /// IP库帮助类
    /// </summary>  
    public class IpScanerHelper
    {
        #region 私有成员
        private string _dataPath;
        private string _ip;
        private string _country;
        private string _local;

        private long _firstStartIp;
        private long _lastStartIp;
        private FileStream _objFileStream;
        private long _startIp;
        private long _endIp;
        private int _countryFlag;
        private long _endIpOff;
        private string _errMsg;
        #endregion

        #region 构造函数
        public IpScanerHelper()
        {
            //  
            // TODO: 在此处添加构造函数逻辑  
            //  
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 数据路径（如果没有设置文件所在路径，则使用默认路径：/App_Data/IpScaner/QQWry.Dat）
        /// </summary>
        public string DataPath
        {
            set { _dataPath = value; }
        }
        public string Ip
        {
            set { _ip = value; }
        }
        public string Country
        {
            get { return _country; }
        }
        public string Local
        {
            get { return _local; }
        }
        public string ErrMsg
        {
            get { return _errMsg; }
        }
        #endregion

        #region 搜索匹配数据
        private int QQwry()
        {
            const string pattern = @"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))";
            Regex objRe = new Regex(pattern);
            Match objMa = objRe.Match(_ip);
            if (!objMa.Success)
            {
                _errMsg = "IP格式错误";
                return 4;
            }

            long ipInt = IpToInt(_ip);
            int nRet = 0;

            if (ipInt >= IpToInt("127.0.0.0") && ipInt <= IpToInt("127.255.255.255"))
            {
                nRet = 1;
                _country = "本机内部环回地址";
                _local = "";
                return nRet;
            }
            if ((ipInt >= IpToInt("0.0.0.0") && ipInt <= IpToInt("0.255.255.255")) || (ipInt >= IpToInt("224.0.0.0") && ipInt <= IpToInt("255.255.255.255")))
            {
                nRet = 1;
                _country = "网络保留地址";
                _local = "";
                return nRet;
            }

            try
            {
                //如果没有设置文件所在路径，则使用默认路径
                if (string.IsNullOrEmpty(_dataPath))
                {
                    _dataPath = HttpContext.Current.Server.MapPath("/App_Data/IpScaner/qqwry.dat");
                }

                if (!File.Exists(_dataPath))
                {
                    nRet = 2;
                    _country = "FileDataError";
                    _local = "";
                    return nRet;
                }

                _objFileStream = new FileStream(_dataPath, FileMode.Open, FileAccess.Read);

                //objfs.Seek(0,SeekOrigin.Begin);

                _objFileStream.Position = 0;
                byte[] buff = new Byte[8];
                _objFileStream.Read(buff, 0, 8);
                _firstStartIp = buff[0] + buff[1] * 256 + buff[2] * 256 * 256 + buff[3] * 256 * 256 * 256;
                _lastStartIp = buff[4] * 1 + buff[5] * 256 + buff[6] * 256 * 256 + buff[7] * 256 * 256 * 256;
                long recordCount = Convert.ToInt64((_lastStartIp - _firstStartIp) / 7.0);

                if (recordCount <= 1)
                {
                    _country = "FileDataError";
                    _objFileStream.Close();
                    return 2;
                }

                long rangE = recordCount;
                long rangB = 0;
                long recNo = 0;

                while (rangB < rangE - 1)
                {
                    recNo = (rangE + rangB) / 2;
                    GetStartIp(recNo);
                    if (ipInt == _startIp)
                    {
                        rangB = recNo;
                        break;
                    }
                    if (ipInt > _startIp)
                        rangB = recNo;
                    else
                        rangE = recNo;
                }

                GetStartIp(rangB);
                GetEndIp();

                if (_startIp <= ipInt && _endIp >= ipInt)
                {
                    GetCountry();
                    _local = _local.Replace("（我们一定要解放台湾！！！）", "").Replace("（台湾）", "");
                }
                else
                {
                    nRet = 3;
                    _country = "未知地区";
                    _local = "";
                }
                _local = _local.Replace("CZ88.NET", "").Replace(" ", "");
                _objFileStream.Close();
                return nRet;
            }
            catch
            {
                return 1;
            }

        }
        #endregion

        #region IP地址转换成Int数据
        private long IpToInt(string ip)
        {
            char[] dot = { '.' };
            string[] ipArr = ip.Split(dot);
            if (ipArr.Length == 3)
                ip = ip + ".0";
            ipArr = ip.Split(dot);

            long p1 = long.Parse(ipArr[0]) * 256 * 256 * 256;
            long p2 = long.Parse(ipArr[1]) * 256 * 256;
            long p3 = long.Parse(ipArr[2]) * 256;
            long p4 = long.Parse(ipArr[3]);
            long ipInt = p1 + p2 + p3 + p4;
            return ipInt;
        }
        #endregion

        #region int转换成IP
        private string IntToIp(long ipInt)
        {
            long seg1 = (ipInt & 0xff000000) >> 24;
            if (seg1 < 0)
                seg1 += 0x100;
            long seg2 = (ipInt & 0x00ff0000) >> 16;
            if (seg2 < 0)
                seg2 += 0x100;
            long seg3 = (ipInt & 0x0000ff00) >> 8;
            if (seg3 < 0)
                seg3 += 0x100;
            long seg4 = (ipInt & 0x000000ff);
            if (seg4 < 0)
                seg4 += 0x100;
            string ip = seg1 + "." + seg2 + "." + seg3 + "." + seg4;

            return ip;
        }
        #endregion

        #region 获取起始IP范围
        private long GetStartIp(long recNo)
        {
            long offSet = _firstStartIp + recNo * 7;
            //objfs.Seek(offSet,SeekOrigin.Begin);  
            _objFileStream.Position = offSet;
            byte[] buff = new Byte[7];
            _objFileStream.Read(buff, 0, 7);

            _endIpOff = Convert.ToInt64(buff[4].ToString()) + Convert.ToInt64(buff[5].ToString()) * 256 + Convert.ToInt64(buff[6].ToString()) * 256 * 256;
            _startIp = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256 + Convert.ToInt64(buff[3].ToString()) * 256 * 256 * 256;
            return _startIp;
        }
        #endregion

        #region 获取结束IP
        private long GetEndIp()
        {
            //objfs.Seek(endIpOff,SeekOrigin.Begin);  
            _objFileStream.Position = _endIpOff;
            byte[] buff = new Byte[5];
            _objFileStream.Read(buff, 0, 5);
            _endIp = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256 + Convert.ToInt64(buff[3].ToString()) * 256 * 256 * 256;
            _countryFlag = buff[4];
            return _endIp;
        }
        #endregion

        #region 获取国家/区域偏移量
        private string GetCountry()
        {
            switch (_countryFlag)
            {
                case 1:
                case 2:
                    _country = GetFlagStr(_endIpOff + 4);
                    _local = (1 == _countryFlag) ? " " : GetFlagStr(_endIpOff + 8);
                    break;
                default:
                    _country = GetFlagStr(_endIpOff + 4);
                    _local = GetFlagStr(_objFileStream.Position);
                    break;
            }
            return " ";
        }
        #endregion

        #region 获取国家/区域字符串
        private string GetFlagStr(long offSet)
        {
            byte[] buff = new Byte[3];
            while (true)
            {
                //objfs.Seek(offSet,SeekOrigin.Begin);  
                _objFileStream.Position = offSet;
                int flag = _objFileStream.ReadByte();
                if (flag == 1 || flag == 2)
                {
                    _objFileStream.Read(buff, 0, 3);
                    if (flag == 2)
                    {
                        _countryFlag = 2;
                        _endIpOff = offSet - 4;
                    }
                    offSet = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256;
                }
                else
                {
                    break;
                }
            }
            if (offSet < 12)
                return " ";
            _objFileStream.Position = offSet;
            return GetStr();
        }
        #endregion

        #region GetStr
        private string GetStr()
        {
            string str = "";
            byte[] buff = new byte[2];
            while (true)
            {
                byte lowC = (Byte)_objFileStream.ReadByte();
                if (lowC == 0)
                    break;
                if (lowC > 127)
                {
                    byte upC = (byte)_objFileStream.ReadByte();
                    buff[0] = lowC;
                    buff[1] = upC;
                    Encoding enc = Encoding.GetEncoding("GB2312");
                    str += enc.GetString(buff);
                }
                else
                {
                    str += (char)lowC;
                }
            }
            return str;
        }
        #endregion

        #region 获取IP地址
        public string IpLocation()
        {
            QQwry();
            return (_country + _local);
        }

        public string IpLocation(string ip)
        {
            _ip = ip;
            QQwry();
            return (_country + _local);
        }

        public string IpLocation(string ip, string dataPath)
        {
            _dataPath = dataPath;
            _ip = ip;
            QQwry();
            return (_country + _local);
        }

        #endregion
    }
}
