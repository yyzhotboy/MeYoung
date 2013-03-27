using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;

namespace Common
{
    /// <summary>
    /// 安全性操作工具集。
    /// </summary>
    public class Security
    {
        private static readonly string strkey = System.Configuration.ConfigurationManager.AppSettings["DescKey"];

        #region 检验黑客SQL注入函数 [='/<>-*]
        //public static bool CheckSqlImmitParams(params object[] args)
        //{
        //    string[] Lawlesses={"=","'","<",">"};
        //    if(Lawlesses==null||Lawlesses.Length<=0)
        //        return true;
        //    // 构造正则表达式,例:Lawlesses是=号和'号,则正则表达式为 .*[=}'].*  
        //    string str_Regex=".*[";   
        //    for(int i=0;i< Lawlesses.Length-1;i++)        
        //        str_Regex+=Lawlesses[i]+"|";    
        //    str_Regex+=Lawlesses[Lawlesses.Length-1]+"].*";    
        //    foreach(object arg in args)    
        //    {        
        //        if(arg is string)//如果是字符串,直接检查        
        //        {            
        //            if(Regex.Matches(arg.ToString(),str_Regex).Count>0)            
        //                return false;        
        //        }        
        //        else if(arg is ICollection)//如果是一个集合,则检查集合内元素是否字符串,是字符串,就进行检查       
        //        {           
        //            foreach(object obj in (ICollection)arg)            
        //            {                
        //                if(obj is string)                
        //                {                    
        //                    if(Regex.Matches(obj.ToString(),str_Regex).Count>0)                    
        //                        return false;                
        //                }            
        //            }        
        //        }    
        //    }
        //    return true;
        //}

        /// <summary>
        /// 注入检测
        /// false :非法 , true 通过.
        /// 
        /// zbw Modify 2007-10-19
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool CheckSqlImmitParams(params object[] args)
        {
            //System.DateTime dt = System.DateTime.Now;

            string[] Lawlesses = { @"\", "=", "'", "<", ">", "%", ":" };
            if (Lawlesses == null || Lawlesses.Length <= 0)
                return true;
            // 构造正则表达式,例:Lawlesses是=号和'号,则正则表达式为 .*[=}'].*  
            string str_Regex = ".*[";
            for (int i = 0; i < Lawlesses.Length - 1; i++)
                str_Regex += Lawlesses[i] + "|";
            str_Regex += Lawlesses[Lawlesses.Length - 1] + "].*";
            foreach (object arg in args)
            {
                if (arg is string)//如果是字符串,直接检查        
                {
                    if (arg.ToString().Length <= 100)
                    {
                        if (System.Text.RegularExpressions.Regex.Matches(arg.ToString(), str_Regex).Count > 0)
                            return false;
                        if (CheckSQLString(arg.ToString()))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        string temp = arg.ToString();
                        foreach (string str in Lawlesses)
                        {
                            if (temp.IndexOf(str) >= 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                else if (arg is System.Collections.ICollection)//如果是一个集合,则检查集合内元素是否字符串,是字符串,就进行检查       
                {
                    foreach (object obj in (System.Collections.ICollection)arg)
                    {
                        if (obj is string)
                        {
                            if (obj.ToString().Length <= 100)
                            {
                                if (System.Text.RegularExpressions.Regex.Matches(obj.ToString(), str_Regex).Count > 0)
                                    return false;
                                if (CheckSQLString(obj.ToString()))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                string temp = obj.ToString();
                                foreach (string str in Lawlesses)
                                {
                                    if (temp.IndexOf(str) >= 0)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //TimeSpan ts = System.DateTime.Now - dt;
            //double tt = ts.TotalMilliseconds;
            return true;
        }
        /// <summary>
        /// 对语句的注入检测
        /// 
        ///  ADDED BY ZBW
        /// </summary>
        /// <param name="checkText"></param>
        /// <returns></returns>
        public static bool CheckSQLString(string checkText)
        {
            string tempString = checkText;
            //if (NeedUrlCodeCheck)
            //tempString = HttpUtility.UrlDecode(tempString);
            //说明：不能存在'+空格的存在，并且，不能有空格+字符+空格的命令存在
            string sqlString = @"\s*\'\s+|\s(and|exec|insert|select|delete|update|count|\*|\%|chr|mid|master|truncate|char|declare)\s";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(sqlString);
            return reg.IsMatch(tempString);
        }
        #endregion


        #region 散列技术存储密码，不可逆的加密
        /// <summary>
        /// 散列技术存储密码
        /// </summary>
        /// <param name="strPwd">密码字符串</param>
        /// <param name="jmformat">加密方式MD5,SHA1</param>
        /// <returns>STRING</returns>
        public static string HashingEncrypt(string strPwd, string jmformat)
        {
            if (strPwd.Trim() != String.Empty)
                return FormsAuthentication.HashPasswordForStoringInConfigFile(strPwd, jmformat);
            else
                return null;
        }
        #endregion

        #region MD5加密算法,加密后小写

        /// <summary>
        /// MD5加密算法,加密后小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetMD5(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        #endregion

        #region 创建Salt值

        //*********************************************************************/
        /// <summary>
        /// 创建一个hash的salt值
        /// </summary>
        /// <returns>Salted string</returns>
        //*********************************************************************/
        public static string CreateSalt()
        {
            byte[] bytSalt = new byte[8];
            RNGCryptoServiceProvider rng;

            rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytSalt);

            return Convert.ToBase64String(bytSalt);
        }

        #endregion

        #region base64位加密
        /// <summary>
        /// base64位加密
        /// </summary>
        /// <param name="strInput">字符串</param>
        /// <returns>STRING</returns>
        public static string ToBase64Encrypt(string strInput)
        {
            if (strInput.Trim() == string.Empty)
                return null;
            //加密
            byte[] msg = Encoding.Default.GetBytes(strInput);
            string strSend = Convert.ToBase64String(msg);
            byte[] SMS = Encoding.Default.GetBytes(strSend);
            return Encoding.Default.GetString(SMS); //返回信息
        }
        #endregion

        #region base64位解密
        /// <summary>
        /// base64位解密
        /// </summary>
        /// <param name="strInput">字符串</param>
        /// <returns>STRING</returns>
        public static string FormBase64Encrypt(string strInput)
        {
            if (strInput.Trim() == string.Empty)
                return null;
            byte[] by = Convert.FromBase64String(strInput);
            return Encoding.Default.GetString(by); //返回信息
        }
        #endregion

        #region DESC加密算法
        /// <summary>
        /// DESC加密算法
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static string DescEncrypt(string strdata)
        {
            //加密
            byte[] data = Encoding.Default.GetBytes(strdata);
            //byte[] key =  Encoding.Default.GetBytes(strkey);

            DESCryptoServiceProvider desc = new DESCryptoServiceProvider();     // des进行加密
            desc.Key = ASCIIEncoding.ASCII.GetBytes(strkey);
            desc.IV = ASCIIEncoding.ASCII.GetBytes(strkey);
            MemoryStream ms = new MemoryStream();                                // 存储加密后的数据
            CryptoStream cs = new CryptoStream(ms, desc.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);                                        // 进行加密
            cs.FlushFinalBlock();
            cs.Close();

            //			StringBuilder ret = new StringBuilder();
            //			foreach(byte b in ms.ToArray())
            //			{
            //				ret.AppendFormat("{0:X2}", b);
            //			}
            //			return ret.ToString();                                               // 取加密后的数据
            return Convert.ToBase64String(ms.ToArray());//
        }
        #endregion

        #region DESC解密算法
        /// <summary>
        /// DESC解密算法
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static string DescDecrypt(string strdata)
        {
            byte[] data = Convert.FromBase64String(strdata);// Encoding.Default.GetBytes(strdata);
            //byte[] key =  Encoding.Default.GetBytes(strkey);

            DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
            desc.Key = ASCIIEncoding.ASCII.GetBytes(strkey);
            desc.IV = ASCIIEncoding.ASCII.GetBytes(strkey);
            MemoryStream ms = new MemoryStream();//存储解密后的数据
            CryptoStream cs = new CryptoStream(ms, desc.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);//解密数据
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.Default.GetString(ms.ToArray());
            //StringBuilder ret = new StringBuilder();
            //			foreach(byte b in ms.ToArray())
            //			{
            //				ret.AppendFormat("{0:X2}", b);
            //			}
            //			return ret.ToString(); 
        }
        #endregion


        #region 生成随机数
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="codeCount">随机数个数</param>
        /// <returns>STRING</returns>
        public static string CreateRandomCode(int codeCount)
        {
            string allChar = "2,3,4,5,6,7,8,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,w,x,y";
            //"A,B,C,D,E,F,G,H,I,J,K,M,N,P,Q,R,S,T,U,W,X,Y";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    //rand = new Random(i*temp*((int)DateTime.Now.Ticks));
                    int s = (int)DateTime.Now.Ticks;
                    rand = new Random(i * temp * (s));
                }
                int t = rand.Next(29);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        #endregion

        #region 生成随机数
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="codeCount">随机数个数</param>
        /// <returns>STRING</returns>
        public static string CreateRandomCode()
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                if (temp != -1)
                {
                    //rand = new Random(i*temp*((int)DateTime.Now.Ticks));
                    int s = (int)DateTime.Now.Ticks;
                    rand = new Random(i * temp * (s));
                }
                int t = rand.Next(10);
                if (temp == t)
                {
                    return CreateRandomCode();
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        #endregion

        #region 生成images图片
        /// <summary>
        /// 生成images图片
        /// </summary>
        /// <param name="checkCode">随机数</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream CreateImage(string checkCode)
        {
            //int iwidth = (int)(checkCode.Length * 10);
            //int iheight = 16;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((double)(checkCode.Length * 12)), 22);
            //System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, iheight);
            Graphics g = Graphics.FromImage(image);
            System.IO.MemoryStream ms;
            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                //				for(int i=0; i<25; i++)
                //				{
                //					int x1 = random.Next(image.Width);
                //					int x2 = random.Next(image.Width);
                //					int y1 = random.Next(image.Height);
                //					int y2 = random.Next(image.Height);
                //
                //					g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                //
                //				}

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                //				for(int i=0; i<100; i++)
                //				{
                //					int x = random.Next(image.Width);
                //					int y = random.Next(image.Height);
                //
                //					image.SetPixel(x, y, Color.FromArgb(random.Next()));
                //				}

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                ms = new System.IO.MemoryStream();
                //image.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
            return ms;
        }
        #endregion

        #region 数据库字符串解密
        //		/// <summary>
        //		/// 数据库字符串解密
        //		/// </summary>
        //		/// <param name="InputConnectionString"></param>
        //		/// <returns></returns>
        //		public static string DecryptDBConnectionString(string InputConnectionString)
        //		{
        //			if(InputConnectionString.Equals(string.Empty))
        //			{
        //				return InputConnectionString;
        //			}
        //			DataProtector dp = new DataProtector(Store.Machine);
        //			byte[] decryptedData = dp.Decrypt( Convert.FromBase64String( InputConnectionString ), null );
        //			return Encoding.ASCII.GetString( decryptedData );
        //		}
        #endregion

        #region 数据库字符串加密
        //		/// <summary>
        //		/// 数据库字符串加密
        //		/// </summary>
        //		/// <param name="encryptedString"></param>
        //		/// <returns></returns>
        //		public static string EncryptDBConnectionString(string encryptedString)
        //		{
        //			DataProtector dp = new DataProtector(Store.Machine);
        //			byte[] dataBytes = Encoding.ASCII.GetBytes( encryptedString );
        //			byte[] encryptedBytes = dp.Encrypt( dataBytes, null );
        //			return Convert.ToBase64String( encryptedBytes );
        //		}
        #endregion

        #region 3DES加密 生成机器密钥 LY添加
        /// <summary>
        /// 1.1版本 decryptionKey = createMachineKey(16)，validationKey =  createMachineKey(48)
        /// 2.0版本 decryptionKey = createMachineKey(32)，validationKey =  createMachineKey(128)
        /// </summary>
        /// <param name="length">秘钥长度</param>
        /// <returns></returns>
        public static string createMachineKey(int length)
        {
            byte[] random = new Byte[length / 2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            StringBuilder machineKey = new StringBuilder(length);
            for (int i = 0; i < random.Length; i++)
            {
                machineKey.Append(string.Format("{0:X2}", random[i]));//16进制
            }
            return machineKey.ToString();
        }
        #endregion


        #region RSA 私钥签名-ForPEM


        /// <summary>
        /// RSA 私钥签名 杨栋添加
        /// </summary>
        /// <param name="str">需要加密的原始字符串</param>
        /// <param name="privatefilepath">私钥.PEM 文件路径</param>
        /// <returns>加密后16进制字符串</returns>
        public static string RSAEncodeForPEM(string str, string privatefilepath)
        {
            StreamReader prkey = null;
            BinaryReader binr = null;
            try
            {
                prkey = new StreamReader(privatefilepath);
                string PrivateKey = prkey.ReadToEnd();
                string newPrivateKey = PrivateKey;

                int start = PrivateKey.IndexOf("--\n");
                int end = PrivateKey.IndexOf("\n--");

                if ((start > -1) && (end > -1))
                {
                    newPrivateKey = PrivateKey.Substring(start + 3, end - start - 3);
                }


                byte[] privkey = Convert.FromBase64String(newPrivateKey);

                byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

                // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
                MemoryStream mem = new MemoryStream(privkey);
                binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
                byte bt = 0;
                ushort twobytes = 0;
                int elems = 0;


                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)	//version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                if (true)
                {
                    showBytes("\nModulus", MODULUS);
                    showBytes("\nExponent", E);
                    showBytes("\nD", D);
                    showBytes("\nP", P);
                    showBytes("\nQ", Q);
                    showBytes("\nDP", DP);
                    showBytes("\nDQ", DQ);
                    showBytes("\nIQ", IQ);
                }

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);


                byte[] PlainTextBArray;
                byte[] CypherTextBArray;

                //				UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //				PlainTextBArray   =   ByteConverter.GetBytes(str); //
                PlainTextBArray = Encoding.Default.GetBytes(str); //			
                SHA1 mysha = SHA1.Create();

                CypherTextBArray = RSA.SignData(PlainTextBArray, mysha);

                char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                //byte转换为16进制字符串 CypherTextBArray
                byte[] bytes = CypherTextBArray;
                char[] chars = new char[bytes.Length * 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    int b = bytes[i];
                    chars[i * 2] = hexDigits[b >> 4];
                    chars[i * 2 + 1] = hexDigits[b & 0xF];
                }
                return new string(chars);
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                binr.Close();
                prkey.Close();

            }
        }




        #endregion


        #region PEM 文件 转换成 XML 文件
        /// <summary>
        /// PEM 文件 转换成 XML 文件 杨栋添加
        /// </summary>
        /// <param name="pemstr">PEM文件原型,包含开始和结尾的注释</param>
        /// <param name="xmlfilepath">要保存为 XML文件路径</param>
        /// <returns></returns>
        public static bool PemToXml(string pemstr, string xmlfilepath)
        {

            BinaryReader binr = null;
            try
            {
                string PrivateKey = pemstr;
                string newPrivateKey = PrivateKey;

                int start = PrivateKey.IndexOf("\n");
                int end = PrivateKey.IndexOf("\n--");

                if ((start == -1) || (end == -1))
                {
                    return false;

                }

                newPrivateKey = PrivateKey.Substring(start + 1, end - start - 1);


                byte[] privkey = Convert.FromBase64String(newPrivateKey);

                byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

                // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
                MemoryStream mem = new MemoryStream(privkey);
                binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
                byte bt = 0;
                ushort twobytes = 0;
                int elems = 0;


                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return false;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)	//version number
                    return false;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return false;

                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);
                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                if (true)
                {
                    showBytes("\nModulus", MODULUS);
                    showBytes("\nExponent", E);
                    showBytes("\nD", D);
                    showBytes("\nP", P);
                    showBytes("\nQ", Q);
                    showBytes("\nDP", DP);
                    showBytes("\nDQ", DQ);
                    showBytes("\nIQ", IQ);
                }

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);

                string prikey = RSA.ToXmlString(true);
                FileStream fsFile = new FileStream(xmlfilepath, FileMode.Create);
                StreamWriter fsWrite = new StreamWriter(fsFile);
                fsWrite.WriteLine(prikey);
                fsWrite.Close();
                fsFile.Close();

                binr.Close();

                return true;

            }
            catch (Exception)
            {
                binr.Close();


                return false;
            }

        }


        #region other
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
                {
                    highbyte = binr.ReadByte();	// data size in next 2 bytes
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;		// we already have the data size
                }

            while (binr.PeekChar() == 0x00)
            {	//remove high order zeros in data
                binr.ReadByte();
                count -= 1;
            }
            return count;
        }

        private static void showBytes(String info, byte[] data)
        {
            Console.WriteLine("{0}  [{1} bytes]", info, data.Length);
            for (int i = 1; i <= data.Length; i++)
            {
                Console.Write("{0:X2}  ", data[i - 1]);
                if (i % 16 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine("\n\n");
        }

        #endregion

        #endregion


        #region RSA 私钥签名


        /// <summary>
        /// RSA 私钥签名 杨栋添加
        /// </summary>
        /// <param name="str">要签名的原始字符串</param>
        /// <param name="PrivateXmlfilepath">私钥xml文件地址</param>
        /// <returns>签名后字符串</returns>
        public static string RSAEncode(string str, string PrivateXmlfilepath)
        {
            if (File.Exists(PrivateXmlfilepath) == false)
            {
                return "";
            }
            StreamReader prkey = null;

            try
            {
                prkey = new StreamReader(PrivateXmlfilepath);
                string PrivateKey = prkey.ReadToEnd();
                prkey.Close();
                string newPrivateKey = PrivateKey;


                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(PrivateKey);

                byte[] PlainTextBArray;
                byte[] CypherTextBArray;

                //				UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //				PlainTextBArray   =   ByteConverter.GetBytes(str); //
                PlainTextBArray = Encoding.Default.GetBytes(str); //			
                SHA1 mysha = SHA1.Create();

                CypherTextBArray = RSA.SignData(PlainTextBArray, mysha);

                char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                //byte转换为16进制字符串 CypherTextBArray
                byte[] bytes = CypherTextBArray;
                char[] chars = new char[bytes.Length * 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    int b = bytes[i];
                    chars[i * 2] = hexDigits[b >> 4];
                    chars[i * 2 + 1] = hexDigits[b & 0xF];
                }

                return new string(chars);
            }
            catch (Exception)
            {
                return "";
            }

        }




        #endregion


        #region RSA 公钥验证算法
        /// <summary>
        /// RSA 公钥验证 杨栋添加
        /// </summary>
        /// <param name="RsaEncodeStr">私钥加密后字符串</param>
        /// <param name="RsaPublicfilepath">RSA公钥PEM文件路径</param>
        /// <param name="srcstr">RSA私钥加密前原始字符串</param>
        /// <returns>验证结果 true or false</returns>
        public static bool RSACheck(string RsaEncodeStr, string RsaPublicfilepath, string srcstr)
        {

            StreamReader pukey = null;
            try
            {
                pukey = new StreamReader(RsaPublicfilepath);
                string PublicKey = pukey.ReadToEnd();

                int start = PublicKey.IndexOf("--\n");
                int end = PublicKey.IndexOf("\n--");
                string newPublicKey = PublicKey.Substring(start + 3, end - start - 3);


                RSAParameters rsaP = new RSAParameters();

                byte[] tmpKeyNoB64 = Convert.FromBase64String(newPublicKey);

                int pemModulus = 128;
                int pemPublicExponent = 3;
                byte[] arrPemModulus = new byte[128];
                byte[] arrPemPublicExponent = new byte[3];

                for (int i = 0; i < pemModulus; i++)
                {
                    arrPemModulus[i] = tmpKeyNoB64[29 + i];
                }
                rsaP.Modulus = arrPemModulus;
                for (int i = 0; i < pemPublicExponent; i++)
                {
                    arrPemPublicExponent[i] = tmpKeyNoB64[159 + i];
                }
                rsaP.Exponent = arrPemPublicExponent;



                //				byte[]   PlainTextBArray;     
                //				UnicodeEncoding ByteConverter = new UnicodeEncoding();
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //			rsa.FromXmlString(xmlPublicKey);  
                rsa.ImportParameters(rsaP);
                //				PlainTextBArray   =ByteConverter.GetBytes(RsaEncodeStr);

                string s = RsaEncodeStr;
                byte[] array = new byte[s.Length / 2];
                for (int i = 0; i < s.Length / 2; i++)
                {
                    string str = s.Substring(2 * i, 2);
                    array[i] = (byte)Convert.ToInt32(str, 16);
                }

                //				byte[] bsrc   =ByteConverter.GetBytes(srcstr);

                byte[] bsrc = Encoding.Default.GetBytes(srcstr);

                SHA1 mysha = SHA1.Create();
                bool Bresult = rsa.VerifyData(bsrc, mysha, array);
                return Bresult;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                pukey.Close();

            }

        }
        #endregion
    }
}
