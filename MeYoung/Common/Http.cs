using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Management;
using System.Collections;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Common
{
    public class Http
    {
        #region 获取主机名称
        /// <summary>
        /// 获取本地机器名称
        /// </summary>
        /// <returns>机器名称</returns>
        public static string GetHostName()
        {
            string hostName = "";
            try
            {
                hostName = Dns.GetHostName();
                return hostName;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取本地主机IP
        /// <summary>
        /// 获取本地机器IP
        /// </summary>
        /// <returns>IP地址</returns>
        public static string GetHostIp()
        {
            string localIp = "";
            try
            {
                System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
                for (int i = 0; i < addressList.Length; i++)
                {
                    localIp += addressList[i].ToString();
                }
                return localIp;
            }
            catch
            {
                return null;
            }

        }
        #endregion

        #region 穿过代理服务器获得Ip地址,如果有多个IP，则第一个是用户的真实IP，其余全是代理的IP，用逗号隔开
        /// <summary>
        /// 穿过代理服务器获得Ip地址,如果有多个IP，则第一个是用户的真实IP，其余全是代理的IP，用逗号隔开
        /// </summary>
        /// <returns></returns>
        public static string getRealIp()
        {
            string UserIP;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)  //得到穿过代理服务器的ip地址
            {

                UserIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                UserIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return UserIP;
        }
        #endregion

        #region 获取主机MAC地址
        /// <summary>
        /// 获取本机MAC地址
        /// </summary>
        /// <returns>string null</returns>
        public static string GetHostMac()
        {
            string mac = "";
            try
            {
                ManagementClass mc;
                mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if (mo["IPEnabled"].ToString() == "True")
                        mac = mo["MacAddress"].ToString();
                }
                return mac;
            }
            catch
            {
                return null;
            }

        }
        #endregion

        #region 获取本机IP
        public static string GetLocalIp()
        {
            //Page.Request.UserHostName
            return null;
        }
        #endregion

        #region SMPT邮件
        public class SmtpMail
        {
            public SmtpMail()
            {
                Attachments = new System.Collections.ArrayList();
            }
            #region 私有属性
            private string enter = "\r\n";
            /// <summary>
            /// 设定语言代码，默认设定为GB2312，如不需要可设置为""
            /// </summary>
            private string _charset = "GB2312";
            /// <summary>
            /// 发件人地址
            /// </summary>
            private string _from = "";
            /// <summary>
            /// 发件人姓名
            /// </summary>
            private string _fromName = "";
            /// <summary>
            /// 回复邮件地址
            /// </summary>
            ///public string ReplyTo="";
            /// <summary>
            /// 收件人姓名
            /// </summary>	
            private string _recipientName = "";
            /// <summary>
            /// 收件人列表
            /// </summary>
            private Hashtable Recipient = new Hashtable();
            /// <summary>
            /// 邮件服务器域名
            /// </summary>	
            private string mailserver = "";
            /// <summary>
            /// 邮件服务器端口号
            /// </summary>	
            private int mailserverport = 25;
            /// <summary>
            /// SMTP认证时使用的用户名
            /// </summary>
            private string username = "";
            /// <summary>
            /// SMTP认证时使用的密码
            /// </summary>
            private string password = "";
            /// <summary>
            /// 是否需要SMTP验证
            /// </summary>		
            private bool eSmtp = false;
            /// <summary>
            /// 是否Html邮件
            /// </summary>		
            private bool _html = false;
            /// <summary>
            /// 邮件附件列表
            /// </summary>
            private IList Attachments;
            /// <summary>
            /// 邮件发送优先级，可设置为"High","Normal","Low"或"1","3","5"
            /// </summary>
            private string priority = "Normal";
            /// <summary>
            /// 邮件主题
            /// </summary>		
            private string _subject;
            /// <summary>
            /// 邮件正文
            /// </summary>		
            private string _body;
            /// <summary>
            /// 密送收件人列表
            /// </summary>
            ///private Hashtable RecipientBCC=new Hashtable();
            /// <summary>
            /// 收件人数量
            /// </summary>
            private int RecipientNum = 0;
            /// <summary>
            /// 最多收件人数量
            /// </summary>
            private int recipientmaxnum = 5;
            /// <summary>
            /// 密件收件人数量
            /// </summary>
            ///private int RecipientBCCNum=0;
            /// <summary>
            /// 错误消息反馈
            /// </summary>
            private string errmsg;
            /// <summary>
            /// TcpClient对象，用于连接服务器
            /// </summary>	
            private TcpClient tc;
            /// <summary>
            /// NetworkStream对象
            /// </summary>	
            private NetworkStream ns;
            /// <summary>
            /// 服务器交互记录
            /// </summary>
            private string logs = "";
            /// <summary>
            /// SMTP错误代码哈希表
            /// </summary>
            private Hashtable ErrCodeHT = new Hashtable();
            /// <summary>
            /// SMTP正确代码哈希表
            /// </summary>
            private Hashtable RightCodeHT = new Hashtable();
            #endregion
            #region Properties
            /// <summary>
            /// 邮件主题
            /// </summary>
            public string Subject
            {
                get
                {
                    return this._subject;
                }
                set
                {
                    this._subject = value;
                }
            }
            /// <summary>
            /// 邮件正文
            /// </summary>
            public string Body
            {
                get
                {
                    return this._body;
                }
                set
                {
                    this._body = value;
                }
            }
            /// <summary>
            /// 发件人地址
            /// </summary>
            public string From
            {
                get
                {
                    return _from;
                }
                set
                {
                    this._from = value;
                }
            }
            /// <summary>
            /// 设定语言代码，默认设定为GB2312，如不需要可设置为""
            /// </summary>
            public string Charset
            {
                get
                {
                    return this._charset;
                }
                set
                {
                    this._charset = value;
                }
            }
            /// <summary>
            /// 发件人姓名
            /// </summary>
            public string FromName
            {
                get
                {
                    return this._fromName;
                }
                set
                {
                    this._fromName = value;
                }
            }
            /// <summary>
            /// 收件人姓名
            /// </summary>
            public string RecipientName
            {
                get
                {
                    return this._recipientName;
                }
                set
                {
                    this._recipientName = value;
                }
            }
            /// <summary>
            /// smtp服务器是否需要验证
            /// </summary>
            public bool ESmtp
            {
                get
                {
                    return this.eSmtp;
                }
                set
                {
                    this.eSmtp = value;
                }
            }
            /// <summary>
            /// 邮件服务器域名和验证信息
            /// 形如："user:pass@www.server.com:25"，也可省略次要信息。如"user:pass@www.server.com"或"www.server.com"
            /// </summary>	
            public string MailDomain
            {
                set
                {
                    string maidomain = value.Trim();
                    int tempint;
                    if (maidomain != "")
                    {
                        tempint = maidomain.IndexOf("!");
                        if (tempint != -1)
                        {
                            string str = maidomain.Substring(0, tempint);
                            MailServerUserName = str.Substring(0, str.IndexOf(":"));
                            MailServerPassWord = str.Substring(str.IndexOf(":") + 1, str.Length - str.IndexOf(":") - 1);
                            maidomain = maidomain.Substring(tempint + 1, maidomain.Length - tempint - 1);
                        }
                        tempint = maidomain.IndexOf(":");
                        if (tempint != -1)
                        {
                            mailserver = maidomain.Substring(0, tempint);
                            mailserverport = System.Convert.ToInt32(maidomain.Substring(tempint + 1, maidomain.Length - tempint - 1));
                        }
                        else
                        {
                            mailserver = maidomain;
                        }
                    }
                }
            }

            /// <summary>
            /// 邮件服务器端口号
            /// </summary>	
            public int MailDomainPort
            {
                set
                {
                    mailserverport = value;
                }
            }
            /// <summary>
            /// SMTP认证时使用的用户名
            /// </summary>
            public string MailServerUserName
            {
                set
                {
                    if (value.Trim() != "")
                    {
                        username = value.Trim();
                        ESmtp = true;
                    }
                    else
                    {
                        username = "";
                        ESmtp = false;
                    }
                }
            }

            /// <summary>
            /// SMTP认证时使用的密码
            /// </summary>
            public string MailServerPassWord
            {
                set
                {
                    password = value;
                }
            }
            /// <summary>
            /// 邮件发送优先级，可设置为"High","Normal","Low"或"1","3","5"
            /// </summary>
            public string Priority
            {
                set
                {
                    switch (value.ToLower())
                    {
                        case "high":
                            priority = "High";
                            break;
                        case "1":
                            priority = "High";
                            break;
                        case "normal":
                            priority = "Normal";
                            break;
                        case "3":
                            priority = "Normal";
                            break;
                        case "low":
                            priority = "Low";
                            break;
                        case "5":
                            priority = "Low";
                            break;
                        default:
                            priority = "Normal";
                            break;
                    }
                }
            }
            /// <summary>
            ///  是否Html邮件
            /// </summary>
            public bool Html
            {
                get
                {
                    return this._html;
                }
                set
                {
                    this._html = value;
                }
            }
            /// <summary>
            /// 错误消息反馈
            /// </summary>		
            public string ErrorMessage
            {
                get
                {
                    return errmsg;
                }
            }
            /// <summary>
            /// 服务器交互记录，如发现本组件不能使用的SMTP服务器，请将出错时的Logs发给我（lion-a@sohu.com），我将尽快查明原因。
            /// </summary>
            public string Logs
            {
                get
                {
                    return logs;
                }
            }
            /// <summary>
            /// 最多收件人数量
            /// </summary>
            public int RecipientMaxNum
            {
                set
                {
                    recipientmaxnum = value;
                }
            }
            #endregion
            //这里的方法是只能在类中使用的私有方法，不对外。
            #region Private Helper Functions
            /// <summary>
            /// 添加一个收件人
            /// </summary>	
            /// <param name="Recipients">收件人地址</param>
            private bool AddRecipient(string Recipients)
            {
                if (RecipientNum < recipientmaxnum)
                {
                    Recipient.Add(RecipientNum, Recipients);
                    RecipientNum++;
                    return true;
                }
                else
                {
                    Dispose();
                    throw (new ArgumentOutOfRangeException("Recipients", "收件人过多不可多于 " + recipientmaxnum + " 个"));
                }
            }
            void Dispose()
            {
                if (ns != null) ns.Close();
                if (tc != null) tc.Close();
            }
            /// <summary>
            /// SMTP回应代码哈希表
            /// </summary>
            private void SMTPCodeAdd()
            {
                try
                {
                    ErrCodeHT.Add("500", "邮箱地址错误");
                    ErrCodeHT.Add("501", "参数格式错误");
                    ErrCodeHT.Add("502", "命令不可实现");
                    ErrCodeHT.Add("503", "服务器需要SMTP验证");
                    ErrCodeHT.Add("504", "命令参数不可实现");
                    ErrCodeHT.Add("421", "服务未就绪，关闭传输信道");
                    ErrCodeHT.Add("450", "要求的邮件操作未完成，邮箱不可用（例如，邮箱忙）");
                    ErrCodeHT.Add("550", "要求的邮件操作未完成，邮箱不可用（例如，邮箱未找到，或不可访问）");
                    ErrCodeHT.Add("451", "放弃要求的操作；处理过程中出错");
                    ErrCodeHT.Add("551", "用户非本地，请尝试<forward-path>");
                    ErrCodeHT.Add("452", "系统存储不足，要求的操作未执行");
                    ErrCodeHT.Add("552", "过量的存储分配，要求的操作未执行");
                    ErrCodeHT.Add("553", "邮箱名不可用，要求的操作未执行（例如邮箱格式错误）");
                    ErrCodeHT.Add("432", "需要一个密码转换");
                    ErrCodeHT.Add("534", "认证机制过于简单");
                    ErrCodeHT.Add("538", "当前请求的认证机制需要加密");
                    ErrCodeHT.Add("454", "临时认证失败");
                    ErrCodeHT.Add("530", "需要认证");
                    RightCodeHT.Add("220", "服务就绪");
                    RightCodeHT.Add("250", "要求的邮件操作完成");
                    RightCodeHT.Add("251", "用户非本地，将转发向<forward-path>");
                    RightCodeHT.Add("354", "开始邮件输入，以<enter>.<enter>结束");
                    RightCodeHT.Add("221", "服务关闭传输信道");
                    RightCodeHT.Add("334", "服务器响应验证Base64字符串");
                    RightCodeHT.Add("235", "验证成功");
                }
                catch
                {
                }
            }
            /// <summary>
            /// 将字符串编码为Base64字符串
            /// </summary>
            /// <param name="str">要编码的字符串</param>
            private string Base64Encode(string str)
            {
                byte[] barray;
                barray = Encoding.Default.GetBytes(str);
                return Convert.ToBase64String(barray);
            }
            /// <summary>
            /// 将Base64字符串解码为普通字符串
            /// </summary>
            /// <param name="str">要解码的字符串</param>
            private string Base64Decode(string str)
            {
                byte[] barray;
                barray = Convert.FromBase64String(str);
                return Encoding.Default.GetString(barray);
            }
            /// <summary>
            /// 得到上传附件的文件流
            /// </summary>
            /// <param name="FilePath">附件的绝对路径</param>
            private string GetStream(string FilePath)
            {
                //建立文件流对象
                System.IO.FileStream FileStr = new System.IO.FileStream(FilePath, System.IO.FileMode.Open);
                byte[] by = new byte[System.Convert.ToInt32(FileStr.Length)];
                FileStr.Read(by, 0, by.Length);
                FileStr.Close();
                return (System.Convert.ToBase64String(by));
            }
            /// <summary>
            /// 发送SMTP命令
            /// </summary>	
            private bool SendCommand(string str)
            {
                byte[] WriteBuffer;
                if (str == null || str.Trim() == String.Empty)
                {
                    return true;
                }
                logs += str;
                WriteBuffer = Encoding.Default.GetBytes(str);
                try
                {
                    ns.Write(WriteBuffer, 0, WriteBuffer.Length);
                }
                catch
                {
                    errmsg = "网络连接错误";
                    return false;
                }
                return true;
            }
            /// <summary>
            /// 接收SMTP服务器回应
            /// </summary>
            private string RecvResponse()
            {
                int StreamSize;
                string ReturnValue = String.Empty;
                byte[] ReadBuffer = new byte[1024];
                try
                {
                    StreamSize = ns.Read(ReadBuffer, 0, ReadBuffer.Length);
                }
                catch
                {
                    errmsg = "网络连接错误";
                    return "false";
                }
                if (StreamSize == 0)
                {
                    return ReturnValue;
                }
                else
                {
                    ReturnValue = Encoding.Default.GetString(ReadBuffer).Substring(0, StreamSize);
                    logs += ReturnValue + this.enter;
                    return ReturnValue;
                }
            }
            /// <summary>
            /// 与服务器交互，发送一条命令并接收回应。
            /// </summary>
            /// <param name="str">一个要发送的命令</param>
            /// <param name="errstr">如果错误，要反馈的信息</param>
            private bool Dialog(string str, string errstr)
            {
                if (str == null || str.Trim() == "")
                {
                    return true;
                }
                if (SendCommand(str))
                {
                    string RR = RecvResponse();
                    if (RR == "false")
                    {
                        return false;
                    }
                    string RRCode = (RR == "") ? RR : RR.Substring(0, 3);
                    if (RightCodeHT[RRCode] != null)
                    {
                        return true;
                    }
                    else
                    {
                        if (ErrCodeHT[RRCode] != null)
                        {
                            errmsg += (RRCode + ErrCodeHT[RRCode].ToString());
                            errmsg += enter;
                        }
                        else
                        {
                            errmsg += RR;
                        }
                        errmsg += errstr;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            /// <summary></summary>
            /// 与服务器交互，发送一组命令并接收回应。
            /// </summary>
            private bool Dialog(string[] str, string errstr)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!Dialog(str[i], ""))
                    {
                        errmsg += enter;
                        errmsg += errstr;
                        return false;
                    }
                }
                return true;
            }
            /// <summary>
            /// SendEmail
            /// </summary>
            /// <returns></returns>
            private bool SendEmail()
            {
                //连接网络
                try
                {
                    tc = new TcpClient(mailserver, mailserverport);
                }
                catch (Exception e)
                {
                    errmsg = e.ToString();
                    return false;
                }
                ns = tc.GetStream();//TcpClient对象，用于连接服务器
                SMTPCodeAdd();

                //验证网络连接是否正确
                if (RightCodeHT[RecvResponse().Substring(0, 3)] == null)
                {
                    errmsg = "网络连接失败";
                    return false;
                }
                string[] SendBuffer;
                string SendBufferstr;
                //进行SMTP验证
                if (ESmtp)
                {
                    SendBuffer = new String[4];
                    SendBuffer[0] = "EHLO " + mailserver + enter;
                    SendBuffer[1] = "AUTH LOGIN" + enter;
                    SendBuffer[2] = Base64Encode(username) + enter;
                    SendBuffer[3] = Base64Encode(password) + enter;
                    if (!Dialog(SendBuffer, "SMTP服务器验证失败，请核对用户名和密码。"))

                        return false;
                }
                else
                {
                    SendBufferstr = "HELO " + mailserver + enter;
                    if (!Dialog(SendBufferstr, ""))
                        return false;
                }
                SendBufferstr = "MAIL FROM:<" + From + ">" + enter;
                if (!Dialog(SendBufferstr, "发件人地址错误，或不能为空"))
                    return false;
                //
                SendBuffer = new string[recipientmaxnum];
                for (int i = 0; i < Recipient.Count; i++)
                {
                    SendBuffer[i] = "RCPT TO:<" + Recipient[i].ToString() + ">" + enter;
                }
                if (!Dialog(SendBuffer, "收件人地址有误"))
                    return false;

                SendBufferstr = "DATA" + enter;
                if (!Dialog(SendBufferstr, ""))
                    return false;
                SendBufferstr = "From:" + FromName + "<" + From + ">" + enter;
                SendBufferstr += "To:=?" + Charset.ToUpper() + "?B?" + Base64Encode(RecipientName) + "?=" + "<" + Recipient[0] + ">" + enter;
                SendBufferstr += ((Subject == String.Empty || Subject == null) ? "Subject:" : ((Charset == "") ? ("Subject:" + Subject) : ("Subject:" + "=?" + Charset.ToUpper() + "?B?" + Base64Encode(Subject) + "?="))) + enter;
                SendBufferstr += "X-Priority:" + priority + enter;
                SendBufferstr += "X-MSMail-Priority:" + priority + enter;
                SendBufferstr += "Importance:" + priority + enter;
                SendBufferstr += "X-Mailer: Felix.MailList.SmtpMail Pubclass [cn]" + enter;
                SendBufferstr += "MIME-Version: 1.0" + enter;
                if (Attachments.Count != 0)
                {
                    SendBufferstr += "Content-Type: multipart/mixed;" + enter;
                    SendBufferstr += "	boundary=\"=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====\"" + enter + enter;
                }
                if (Html)
                {
                    if (Attachments.Count == 0)
                    {
                        SendBufferstr += "Content-Type: multipart/alternative;" + enter;//内容格式和分隔符
                        SendBufferstr += "	boundary=\"=====003_Dragon520636771063_=====\"" + enter + enter;
                        SendBufferstr += "This is a multi-part message in MIME format." + enter + enter;
                    }
                    else
                    {
                        SendBufferstr += "This is a multi-part message in MIME format." + enter + enter;
                        SendBufferstr += "--=====001_Dragon520636771063_=====" + enter;
                        SendBufferstr += "Content-Type: multipart/alternative;" + enter;//内容格式和分隔符
                        SendBufferstr += "	boundary=\"=====003_Dragon520636771063_=====\"" + enter + enter;
                    }
                    SendBufferstr += "--=====003_Dragon520636771063_=====" + enter;
                    SendBufferstr += "Content-Type: text/plain;" + enter;
                    SendBufferstr += ((Charset == "") ? ("	charset=\"iso-8859-1\"") : ("	charset=\"" + Charset.ToLower() + "\"")) + enter;
                    SendBufferstr += "Content-Transfer-Encoding: base64" + enter + enter;
                    SendBufferstr += Base64Encode("邮件内容为HTML格式，请选择HTML方式查看") + enter + enter;
                    SendBufferstr += "--=====003_Dragon520636771063_=====" + enter;
                    SendBufferstr += "Content-Type: text/html;" + enter;
                    SendBufferstr += ((Charset == "") ? ("	charset=\"iso-8859-1\"") : ("	charset=\"" + Charset.ToLower() + "\"")) + enter;
                    SendBufferstr += "Content-Transfer-Encoding: base64" + enter + enter;
                    SendBufferstr += Base64Encode(Body) + enter + enter;
                    SendBufferstr += "--=====003_Dragon520636771063_=====--" + enter;
                }
                else
                {
                    if (Attachments.Count != 0)
                    {
                        SendBufferstr += "--=====001_Dragon303406132050_=====" + enter;
                    }
                    SendBufferstr += "Content-Type: text/plain;" + enter;
                    SendBufferstr += ((Charset == "") ? ("	charset=\"iso-8859-1\"") : ("	charset=\"" + Charset.ToLower() + "\"")) + enter;
                    SendBufferstr += "Content-Transfer-Encoding: base64" + enter + enter;
                    SendBufferstr += Base64Encode(Body) + enter;
                }
                if (Attachments.Count != 0)
                {
                    for (int i = 0; i < Attachments.Count; i++)
                    {
                        string filepath = (string)Attachments[i];
                        SendBufferstr += "--=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====" + enter;
                        SendBufferstr += "Content-Type: text/plain;" + enter;
                        SendBufferstr += "	name=\"=?" + Charset.ToUpper() + "?B?" + Base64Encode(filepath.Substring(filepath.LastIndexOf("\\") + 1)) + "?=\"" + enter;
                        SendBufferstr += "Content-Transfer-Encoding: base64" + enter;
                        SendBufferstr += "Content-Disposition: attachment;" + enter;
                        SendBufferstr += "	filename=\"=?" + Charset.ToUpper() + "?B?" + Base64Encode(filepath.Substring(filepath.LastIndexOf("\\") + 1)) + "?=\"" + enter + enter;
                        SendBufferstr += GetStream(filepath) + enter + enter;
                    }
                    SendBufferstr += "--=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====--" + enter + enter;
                }
                SendBufferstr += enter + "." + enter;
                if (!Dialog(SendBufferstr, "错误信件信息"))
                    return false;
                SendBufferstr = "QUIT" + enter;
                if (!Dialog(SendBufferstr, "断开连接时错误"))
                    return false;
                ns.Close();
                tc.Close();
                return true;
            }
            /// <summary>
            /// 是否合法的email
            /// </summary>
            /// <param name="strIn">测试的字符串</param>
            /// <returns>true,合法， false：不合法</returns>
            public static bool IsValidEmail(string strIn)
            {
                // Return true if strIn is in valid e-mail format.
                return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            }
            /// <summary>
            /// 将输入的email列表转换成数组，email地址间用分号分割.
            /// </summary>
            /// <param name="EL">Email</param>
            /// <returns>数组，包含转化后的结果。</returns>
            public static string[] ToEmailList(string EL)
            {
                string delimStr = ";";
                char[] delimiter = delimStr.ToCharArray();
                return EL.Split(delimiter);
            }
            #endregion
            //Methods中的方法为对外提供的接口，通过对AddAttachment方法的调用，可以为发送的邮件添加附件（可以多个）；
            //通过对AddRecipient方法的调用，可以添加一组收件人。通过对Send（重载）方法的调用就可以将邮件发送出去；
            //在发送邮件时，要通过属性设置将必要的参数设置。如：mailserver,mailserverport等信息。
            #region Methods

            /// <summary>
            /// 添加邮件附件
            /// </summary>
            /// <param name="FilePath">附件绝对路径</param>
            public void AddAttachment(params string[] FilePath)
            {
                if (FilePath == null)
                {
                    throw (new ArgumentNullException("FilePath"));
                }
                for (int i = 0; i < FilePath.Length; i++)
                {
                    Attachments.Add(FilePath[i]);
                }
            }
            /// <summary>
            /// 添加一组收件人（不超过recipientmaxnum个），参数为字符串数组
            /// </summary>
            /// <param name="Recipients">保存有收件人地址的字符串数组（不超过recipientmaxnum个）</param>	
            public bool AddRecipient(params string[] Recipients)
            {
                if (Recipient == null)
                {
                    Dispose();
                    throw (new ArgumentNullException("Recipients"));
                }

                for (int i = 0; i < Recipients.Length; i++)
                {
                    string recipient = Recipients[i].Trim();
                    if (recipient == String.Empty)
                    {
                        Dispose();
                        throw (new ArgumentNullException("Recipients[" + i + "]"));
                    }
                    if (recipient.IndexOf("@") == -1)
                    {
                        Dispose();
                        throw (new ArgumentException("Recipients.IndexOf(\"@\")==-1", "Recipients"));
                    }
                    if (!AddRecipient(recipient))
                    {
                        return false;
                    }
                }
                return true;
            }
            /// <summary>
            /// 发送邮件方法，所有参数均通过属性设置。
            /// </summary>
            public bool Send()
            {
                if (mailserver.Trim() == "")
                {
                    throw (new ArgumentNullException("Recipient", "必须指定SMTP服务器"));
                }
                return SendEmail();
            }
            /// <summary>
            /// 发送邮件方法
            /// </summary>
            /// <param name="smtpserver">smtp服务器信息，如"username:password@www.smtpserver.com:25"，也可去掉部分次要信息，如"www.smtpserver.com"</param>
            public bool Send(string smtpserver)
            {
                MailDomain = smtpserver;
                return Send();
            }

            /// <summary>
            /// 发送邮件方法
            /// </summary>
            /// <param name="smtpserver">smtp服务器信息，如"username:password@www.smtpserver.com:25"，也可去掉部分次要信息，如"www.smtpserver.com"</param>
            /// <param name="from">发件人mail地址</param>
            /// <param name="fromname">发件人姓名</param>
            /// <param name="to">收件人地址</param>
            /// <param name="toname">收件人姓名</param>
            /// <param name="html">是否HTML邮件</param>
            /// <param name="subject">邮件主题</param>
            /// <param name="body">邮件正文</param>
            public bool Send(string smtpserver, string from, string fromname, string to, string toname, bool html, string subject, string body)
            {
                MailDomain = smtpserver;
                From = from;
                FromName = fromname;
                AddRecipient(to);
                RecipientName = toname;
                Html = html;
                Subject = subject;
                Body = body;
                return Send();
            }

            public bool Send(string smtpserver, string from, string fromname, string[] to, string toname, bool html, string subject, string body)
            {
                MailDomain = smtpserver;
                From = from;
                FromName = fromname;
                AddRecipient(to);
                RecipientName = toname;
                Html = html;
                Subject = subject;
                Body = body;
                return Send();
            }
            #endregion
        }
        #endregion


        #region 获取指定WEB页面
        /// <summary>
        /// 获取指定WEB页面
        /// </summary>
        /// <param name="strurl">URL</param>
        /// <returns>string</returns>
        public static string GetWebUrl(string strurl)
        {
            try
            {

                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData(strurl);
                //string pageHtml = Encoding.UTF8.GetString(pageData);
                string pageHtml = Encoding.Default.GetString(pageData);
                return pageHtml;

            }
            catch (WebException webEx)
            {
                return "error_GetWebUrl:" + webEx.ToString();
            }

        }
        #endregion



        #region GET方法获取页面
        /// GET方法获取页面
        /// 函数名:GetUrl	
        /// 功能描述:GET方法获取页面	
        /// 处理流程:
        /// 算法描述:
        /// 作 者: 杨栋
        /// 日 期: 2006-11-19 12:00
        /// 修 改: 2007-01-29 17:00
        /// 修 改: 2008-08-06 15:30
        /// 日 期:
        /// 版 本:
        #region GetUrl(String url, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)
        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="AutoRedirect">是否自动跳转</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type"> 请求类型 http https </param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url)
        {
            string outcookieheader = "";
            return GetUrl(url, "", out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, int timeout)
        {
            string outcookieheader = "";
            return GetUrl(url, "", out outcookieheader, "", true, "", "", "", timeout, "", "", "");
        }


        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, out string outcookieheader)
        {
            return GetUrl(url, "", out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader)
        {
            return GetUrl(url, cookieheader, out cookieheader, "", true, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="AutoRedirect">是否自动跳转</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader, bool AutoRedirect)
        {
            return GetUrl(url, cookieheader, out cookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http https</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string Header_UserAgent, string http_type)
        {
            return GetUrl(url, cookieheader, out outcookieheader, "", true, Header_UserAgent, http_type, "", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http https</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string Header_Referer, string cookieheader, out string outcookieheader, string Header_UserAgent, string http_type)
        {
            return GetUrl(url, cookieheader, out outcookieheader, Header_Referer, true, Header_UserAgent, http_type, "", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http https</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string Header_Referer, string cookieheader, out string outcookieheader, string Header_UserAgent, string http_type, string encoding)
        {
            //return GetUrl(url,cookieheader,out outcookieheader, Header_Referer, AutoRedirect, Header_UserAgent, http_type, encoding,0);
            return GetUrl(url, cookieheader, out outcookieheader, Header_Referer, true, Header_UserAgent, http_type, encoding, 0, "", "", "");
        }


        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="AutoRedirect">是否自动跳转</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http https</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="NetworkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="NetworkCredentialPassword">密码 身份验证密码</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)
        {
            outcookieheader = string.Empty;
            if ((http_type == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = AutoRedirect;
                if (Header_Referer.Length > 2)
                {
                    req.Referer = Header_Referer;
                }
                if (Header_UserAgent.Length < 2)
                {
                    Header_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)"; ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((NetworkCredentialName.Length > 0) || (NetworkCredentialPassword.Length > 0))
                {
                    NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache;//增加请求身份验证信息
                }
                req.UserAgent = Header_UserAgent;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");
                //req.ServicePoint.Expect100Continue = false;
                //ServicePointManager.Expect100Continue = false;
                //为请求加入cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;
                }
                Stream ReceiveStream = null;
                //try
                res = (HttpWebResponse)req.GetResponse();

                string Res_ContentEncoding = res.ContentEncoding.ToLower();
                if (Res_ContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new GZipStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (Res_ContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new DeflateStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    ReceiveStream = res.GetResponseStream();

                }

                //try
                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//获得cookie
                if (outcookieheader.Length < 2)
                {
                    //try
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                }
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                if (encoding.Trim().Length > 2)
                {
                    encodestr = encoding;
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="AutoRedirect">是否自动跳转</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http https</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="NetworkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="NetworkCredentialPassword">密码 身份验证密码</param>
        /// <param name="HttpExpect100Continue">HTTP100Continue</param>
        /// <param name="ServicePointManagerExpect100Continue">服务100Continue</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword, bool HttpExpect100Continue, bool ServicePointManagerExpect100Continue)
        {
            outcookieheader = string.Empty;
            if ((http_type == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = AutoRedirect;
                if (Header_Referer.Length > 2)
                {
                    req.Referer = Header_Referer;
                }
                if (Header_UserAgent.Length < 2)
                {
                    Header_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)"; ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((NetworkCredentialName.Length > 0) || (NetworkCredentialPassword.Length > 0))
                {
                    NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache;//增加请求身份验证信息
                }
                req.UserAgent = Header_UserAgent;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");

                if (HttpExpect100Continue == false)
                {
                    req.ServicePoint.Expect100Continue = false;
                }

                if (ServicePointManagerExpect100Continue == false)
                {
                    ServicePointManager.Expect100Continue = false;
                }

                //为请求加入cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;
                }
                Stream ReceiveStream = null;
                //try
                res = (HttpWebResponse)req.GetResponse();

                string Res_ContentEncoding = res.ContentEncoding.ToLower();
                if (Res_ContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new GZipStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (Res_ContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new DeflateStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    ReceiveStream = res.GetResponseStream();

                }

                //try
                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//获得cookie
                if (outcookieheader.Length < 2)
                {
                    //try
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                }
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                if (encoding.Trim().Length > 2)
                {
                    encodestr = encoding;
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #endregion



        #region POST方法获取页面
        /// POST方法获取页面
        /// 函数名:PostUrl	
        /// 功能描述:POST方法获取页面	
        /// 处理流程:
        /// 算法描述:
        /// 作 者: 杨栋
        /// 日 期: 2006-11-19 12:00
        /// 修 改: 2007-01-29 17:00
        /// 修 改: 2008-08-06 15:00
        /// 日 期:
        /// 版 本:
        #region PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)



        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList)
        {
            string outcookieheader = "";
            return PostUrl(url, paramList, "", out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">post方式请求页面</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输出cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader)
        {
            return PostUrl(url, paramList, cookieheader, out cookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">post方式请求页面</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="outcookieheader">输入cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, out string outcookieheader)
        {
            return PostUrl(url, paramList, "", out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader)
        {
            return PostUrl(url, paramList, cookieheader, out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, string Header_Referer)
        {
            return PostUrl(url, paramList, cookieheader, out cookieheader, Header_Referer, true, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="AutoRedirect">是否自动跳转</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, string Header_Referer, bool AutoRedirect)
        {
            return PostUrl(url, paramList, cookieheader, out cookieheader, Header_Referer, AutoRedirect, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer)
        {
            return PostUrl(url, paramList, cookieheader, out outcookieheader, Header_Referer, true, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="AutoRedirect">是否自动跳转</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect)
        {//return PostUrl(url, paramList, cookieheader, out outcookieheader, Header_Referer, AutoRedirect, Header_UserAgent, http_type, encoding);
            return PostUrl(url, paramList, cookieheader, out outcookieheader, Header_Referer, AutoRedirect, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// post主函数
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="AutoRedirect">是否自动跳转</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type"> 请求类型 http https </param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="NetworkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="NetworkCredentialPassword">密码 身份验证密码</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)
        {
            outcookieheader = string.Empty;
            if ((http_type == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.AllowAutoRedirect = AutoRedirect;
                if (Header_Referer.Length > 2)
                {
                    req.Referer = Header_Referer;
                }
                if (Header_UserAgent.Length < 2)
                {
                    Header_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)"; ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((NetworkCredentialName.Length > 0) || (NetworkCredentialPassword.Length > 0))
                {
                    NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache;//增加请求身份验证信息
                }
                req.UserAgent = Header_UserAgent;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");
                req.MaximumResponseHeadersLength = 1024;//石坤杰 09-03-28 17:00 新增加
                //req.ServicePoint.Expect100Continue = false;
                //ServicePointManager.Expect100Continue = false;
                //为请求加入cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;
                }
                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = null;
                    //try
                    newStream = req.GetRequestStream();
                    //
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                //取得返回的响应
                //res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = null;
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if (outcookieheader.Length < 2)
                {
                    //try
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");
                }

                //res = (HttpWebResponse)req.GetResponse();
                //Stream ReceiveStream = res.GetResponseStream();                

                string Res_ContentEncoding = res.ContentEncoding.ToLower();
                if (Res_ContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new GZipStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (Res_ContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new DeflateStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    ReceiveStream = res.GetResponseStream();

                }


                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                if (encoding.Trim().Length > 2)
                {
                    encodestr = encoding;
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }

            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        /// <summary>
        /// post主函数
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="AutoRedirect">是否自动跳转</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type"> 请求类型 http https </param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="NetworkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="NetworkCredentialPassword">密码 身份验证密码</param>
        /// <param name="HttpExpect100Continue">HTTP100Continue</param>
        /// <param name="ServicePointManagerExpect100Continue">服务100Continue</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword, bool HttpExpect100Continue, bool ServicePointManagerExpect100Continue)
        {
            outcookieheader = string.Empty;
            if ((http_type == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.AllowAutoRedirect = AutoRedirect;
                if (Header_Referer.Length > 2)
                {
                    req.Referer = Header_Referer;
                }
                if (Header_UserAgent.Length < 2)
                {
                    Header_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)"; ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((NetworkCredentialName.Length > 0) || (NetworkCredentialPassword.Length > 0))
                {
                    NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache;//增加请求身份验证信息
                }
                req.UserAgent = Header_UserAgent;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");
                req.MaximumResponseHeadersLength = 1024;//石坤杰 09-03-28 17:00 新增加

                if (HttpExpect100Continue == false)
                {
                    req.ServicePoint.Expect100Continue = false;
                }

                if (ServicePointManagerExpect100Continue == false)
                {
                    ServicePointManager.Expect100Continue = false;
                }

                //为请求加入cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;
                }
                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = null;
                    //try
                    newStream = req.GetRequestStream();
                    //
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                //取得返回的响应
                //res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = null;
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if (outcookieheader.Length < 2)
                {
                    //try
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");
                }

                //res = (HttpWebResponse)req.GetResponse();
                //Stream ReceiveStream = res.GetResponseStream();                

                string Res_ContentEncoding = res.ContentEncoding.ToLower();
                if (Res_ContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new GZipStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (Res_ContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new DeflateStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    ReceiveStream = res.GetResponseStream();

                }


                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                if (encoding.Trim().Length > 2)
                {
                    encodestr = encoding;
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }

            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion


        #endregion
        //NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
        //CredentialCache myCache = new CredentialCache();
        //myCache.Add(new Uri(url), "Basic", myCred);

        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //req.Credentials = myCache;//增加请求身份验证信息
        //怎讲代理设置

        #region 获取图片

        /// GET方法获取页面
        /// 函数名:GetImage	
        /// 功能描述:GET方法获取图片	
        /// 处理流程:
        /// 算法描述:
        /// 作 者: 杨栋
        /// 日 期: 2006-11-21 09:00
        /// 修 改:2006-12-05 17:00
        /// 修 改: 2007-01-29 17:00 
        /// 修 改: 2008-08-06 16:00
        /// 日 期:
        /// 版 本:
        /// 
        #region GetImage(String url, string cookieheader, out string outcookieheader, string Header_Referer, string Header_UserAgent, string http_type, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)


        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader)
        {
            return GetImage(url, cookieheader, out cookieheader, "", "", "http", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string Header_Referer)
        {
            return GetImage(url, cookieheader, out outcookieheader, Header_Referer, "", "http", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, string Header_Referer)
        {
            return GetImage(url, cookieheader, out cookieheader, Header_Referer, "", "http", 0, "", "", "");
        }


        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http 或 https</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, string Header_Referer, string Header_UserAgent, string http_type)
        {
            return GetImage(url, cookieheader, out cookieheader, Header_Referer, Header_UserAgent, http_type, 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader)
        {
            //return GetImage(url, cookieheader, out outcookieheader, Header_Referer,  Header_UserAgent, http_type,  timeout);
            return GetImage(url, cookieheader, out outcookieheader, "", "", "", 0, "", "", "");
        }



        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http 或 https</param>
        /// <param name="timeout">超时时间 ms  输入0则为默认时间</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="NetworkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="NetworkCredentialPassword">密码 身份验证密码</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string Header_Referer, string Header_UserAgent, string http_type, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)
        {
            outcookieheader = string.Empty;
            if ((http_type == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                if (Header_Referer.Length > 2)
                {
                    req.Referer = Header_Referer;
                }
                if (Header_UserAgent.Length < 2)
                {
                    Header_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)"; ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((NetworkCredentialName.Length > 0) || (NetworkCredentialPassword.Length > 0))
                {
                    NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache;//增加请求身份验证信息
                }
                req.UserAgent = Header_UserAgent;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");
                //req.ServicePoint.Expect100Continue = false;
                //ServicePointManager.Expect100Continue = false;
                // req.Method = "GET";
                //req.AllowAutoRedirect = false;
                //req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                //req.Headers.Add("Accept-Encoding", "gzip, deflate");

                //req.KeepAlive = true;

                //req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                //为请求加入cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;
                }
                Stream ReceiveStream = null;
                //try
                res = (HttpWebResponse)req.GetResponse();

                string Res_ContentEncoding = res.ContentEncoding.ToLower();
                if (Res_ContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new GZipStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (Res_ContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new DeflateStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    ReceiveStream = res.GetResponseStream();

                }
                ReceiveStream = res.GetResponseStream();
                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//获得cookie
                if (outcookieheader.Length < 2)
                {
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));

                }

                //string len = res.Headers["Content-Length"];

                //if ((len == null)||(len=="")||(len=="0"))
                //{
                //    len = "102400";
                //}
                //byte[] mybytes = new byte[int.Parse(len)];

                //int count = ReceiveStream.Read(mybytes, 0, int.Parse(len));

                //byte[] image = new byte[count];

                //Array.Copy(mybytes, image, count);

                MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int sz = ReceiveStream.Read(buffer, 0, 1024);
                    if (sz == 0) break;
                    ms.Write(buffer, 0, sz);
                }
                ms.Position = 0;
                byte[] image = ms.GetBuffer();


                return image;

            }
            catch (Exception e)
            {
                //strResult = e.ToString();
                return null;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            //return strResult;
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http 或 https</param>
        /// <param name="timeout">超时时间 ms  输入0则为默认时间</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="NetworkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="NetworkCredentialPassword">密码 身份验证密码</param>
        /// <param name="HttpExpect100Continue">HTTP100Continue</param>
        /// <param name="ServicePointManagerExpect100Continue">服务100Continue</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string Header_Referer, string Header_UserAgent, string http_type, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword, bool HttpExpect100Continue, bool ServicePointManagerExpect100Continue)
        {
            outcookieheader = string.Empty;
            if ((http_type == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                if (Header_Referer.Length > 2)
                {
                    req.Referer = Header_Referer;
                }
                if (Header_UserAgent.Length < 2)
                {
                    Header_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)"; ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((NetworkCredentialName.Length > 0) || (NetworkCredentialPassword.Length > 0))
                {
                    NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache;//增加请求身份验证信息
                }
                req.UserAgent = Header_UserAgent;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");

                if (HttpExpect100Continue == false)
                {
                    req.ServicePoint.Expect100Continue = false;
                }

                if (ServicePointManagerExpect100Continue == false)
                {
                    ServicePointManager.Expect100Continue = false;
                }

                // req.Method = "GET";
                //req.AllowAutoRedirect = false;
                //req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                //req.Headers.Add("Accept-Encoding", "gzip, deflate");

                //req.KeepAlive = true;

                //req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                //为请求加入cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;
                }
                Stream ReceiveStream = null;
                //try
                res = (HttpWebResponse)req.GetResponse();

                string Res_ContentEncoding = res.ContentEncoding.ToLower();
                if (Res_ContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new GZipStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (Res_ContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new DeflateStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    ReceiveStream = res.GetResponseStream();

                }
                ReceiveStream = res.GetResponseStream();
                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//获得cookie
                if (outcookieheader.Length < 2)
                {
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));

                }

                //string len = res.Headers["Content-Length"];

                //if ((len == null)||(len=="")||(len=="0"))
                //{
                //    len = "102400";
                //}
                //byte[] mybytes = new byte[int.Parse(len)];

                //int count = ReceiveStream.Read(mybytes, 0, int.Parse(len));

                //byte[] image = new byte[count];

                //Array.Copy(mybytes, image, count);

                MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int sz = ReceiveStream.Read(buffer, 0, 1024);
                    if (sz == 0) break;
                    ms.Write(buffer, 0, sz);
                }
                ms.Position = 0;
                byte[] image = ms.GetBuffer();


                return image;

            }
            catch (Exception e)
            {
                //strResult = e.ToString();
                return null;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            //return strResult;
        }

        #endregion

        #endregion

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   //   Always   accept   
            return true;
        }



        # region 取得页面 viewState 并进行编码
        /// <summary>
        /// 取得页面 viewState 并进行编码
        /// </summary>
        /// <param name="pageCode"></param>
        /// <returns></returns>
        public static string GetViewState(string pageCode)
        {
            string strViewState = string.Empty;

            int start = pageCode.IndexOf("__VIEWSTATE");

            if (start == -1)
            {//未找到
                return string.Empty;
            }

            start = pageCode.IndexOf("value=", start);
            start = pageCode.IndexOf("\"", start);
            start += 1;//取得“后的第一个字符的位置

            int end = pageCode.IndexOf("\"", start); //最后一个”的前一个字符的位置
            if (end == -1)
            {
                return string.Empty;
            }

            try
            {
                strViewState = pageCode.Substring(start, end - start);
                //进行URL 编码 ， 实际可以不用吧。
                strViewState = System.Web.HttpUtility.UrlEncode(strViewState, System.Text.Encoding.Default);
            }
            catch//(ArgumentOutOfRangeException ex)
            {
                //不存在或ViewState 不正确
                //ex.Message;
                strViewState = null;
            }

            return strViewState;
        }
        #endregion


    }
}
