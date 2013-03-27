using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using System.Web;

namespace Common
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        RAR = 8297,
        jpg = 255216,
        exe = 7790,
        xml = 6063,
        html = 6033,
        aspx = 239187,
        cs = 117115,
        js = 119105,
        txt = 210187,
        sql = 255254
    }
    /// <summary>
    /// 图片检测类
    /// </summary>
    public static class FileValidation
    {
        /// <summary>
        /// 是否图片
        /// </summary>
        public static bool IsPicture(FileStream fs)
        {
            FileExtension[] fileEx = { FileExtension.jpg, FileExtension.GIF, FileExtension.BMP, FileExtension.PNG };
            try
            {
                BinaryReader reader = new BinaryReader(fs);
                string fileClass;
                byte buffer;
                byte[] b = new byte[2];
                buffer = reader.ReadByte();
                b[0] = buffer;
                fileClass = buffer.ToString();
                buffer = reader.ReadByte();
                b[1] = buffer;
                fileClass += buffer.ToString();
                reader.Close();
                foreach (FileExtension fe in fileEx)
                {
                    if (Int32.Parse(fileClass) == (int)fe) return true;
                }        
                return false;
            }
            catch
            {
                return false;
            }  
        }
    }
    /// <summary> 
    /// 功能：上传文件操作(主要用于图片上传);
    /// </summary> 
    public class ImgUp
    {
        private int _Error = 0;//返回上传状态。 
        private int _MaxSize = 20 * 1024 * 1024;// 20480000;//最大单个上传文件 (默认)
        private string _FileType = "jpg/gif/bmp/png/xls/doc/docx/pdf/rar/zip/txt/data_submit/apk";//所支持的上传类型用"/"隔开 
        private string _SavePath = System.Web.HttpContext.Current.Server.MapPath("/uploadfiles/");//保存文件的实际路径 
        private int _SaveType = 0;//上传文件的类型，0代表自动生成文件名 
        private HtmlInputFile _FormFile;//上传控件。 
        private string _InFileName = "";//非自动生成文件名设置。 
        private string _OutFileName = "";//输出文件名。 
        private bool _IsCreateImg = true;//是否生成缩略图。 
        private bool _Iss = false;//是否有缩略图生成.
        private int _Height = 0;//获取上传图片的高度 
        private int _Width = 0;//获取上传图片的宽度 
        private int _sHeight = 120;//设置生成缩略图的高度 
        private int _sWidth = 160;//设置生成缩略图的宽度
        private bool _IsDraw = true;//设置是否加水印
        private int _DrawStyle = 0;//设置加水印的方式０：文字水印模式，１：图片水印模式,2:不加
        private int _DrawString_x = 10;//绘制文本的Ｘ坐标（左上角）
        private int _DrawString_y = 10;//绘制文本的Ｙ坐标（左上角）
        private string _AddText = "河北省通信建设有限公司 www.hebccc.com";//设置水印内容
        private string _Font = "宋体";//设置水印字体
        private int _FontSize = 12;//设置水印字大小
        private int _FileSize = 0;//获取已经上传文件的大小
        private string _CopyIamgePath = System.Web.HttpContext.Current.Server.MapPath("/images/logo.gif");//图片水印模式下的覆盖图片的实际地址

        /// <summary>
        /// Error返回值，1、没有上传的文件。2、类型不允许。3、大小超限。4、未知错误。0、上传成功。 
        /// </summary>
        public int Error
        {
            get { return _Error; }
        }
        /// <summary>
        /// 最大单个上传文件
        /// </summary>
        public int MaxSize
        {
            get { return _MaxSize; }
            set { _MaxSize = value; }
        }
        /// <summary>
        /// 所支持的上传类型用"/"隔开 
        /// </summary>
        public string FileType
        {
            set { _FileType = value; }
        }
        /// <summary>
        /// //保存文件的实际路径 
        /// </summary>
        public string SavePath
        {
            set { _SavePath = System.Web.HttpContext.Current.Server.MapPath(value); }
            get { return _SavePath; }
        }
        /// <summary>
        /// 上传文件的类型，0代表自动生成文件名
        /// </summary>
        public int SaveType
        {
            set { _SaveType = value; }
        }
        /// <summary>
        /// 上传控件
        /// </summary>
        public HtmlInputFile FormFile
        {
            set { _FormFile = value; }
        }
        /// <summary>
        /// //非自动生成文件名设置。
        /// </summary>
        public string InFileName
        {
            set { _InFileName = value; }
        }
        /// <summary>
        /// 输出文件名
        /// </summary>
        public string OutFileName
        {
            get { return _OutFileName; }
            set { _OutFileName = value; }
        }
        /// <summary>
        /// 是否有缩略图生成.
        /// </summary>
        public bool Iss
        {
            get { return _Iss; }
        }
        /// <summary>
        /// //获取上传图片的宽度
        /// </summary>
        public int Width
        {
            get { return _Width; }
        }
        /// <summary>
        /// //获取上传图片的高度
        /// </summary>
        public int Height
        {
            get { return _Height; }
        }
        /// <summary>
        /// 设置缩略图的宽度
        /// </summary>
        public int sWidth
        {
            get { return _sWidth; }
            set { _sWidth = value; }
        }
        /// <summary>
        /// 设置缩略图的高度
        /// </summary>
        public int sHeight
        {
            get { return _sHeight; }
            set { _sHeight = value; }
        }
        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        public bool IsCreateImg
        {
            get { return _IsCreateImg; }
            set { _IsCreateImg = value; }
        }
        /// <summary>
        /// 是否加水印
        /// </summary>
        public bool IsDraw
        {
            get { return _IsDraw; }
            set { _IsDraw = value; }
        }
        /// <summary>
        /// 设置加水印的方式０：文字水印模式，１：图片水印模式,2:不加
        /// </summary>
        public int DrawStyle
        {
            get { return _DrawStyle; }
            set { _DrawStyle = value; }
        }
        /// <summary>
        /// 绘制文本的Ｘ坐标（左上角）
        /// </summary>
        public int DrawString_x
        {
            get { return _DrawString_x; }
            set { _DrawString_x = value; }
        }
        /// <summary>
        /// 绘制文本的Ｙ坐标（左上角）
        /// </summary>
        public int DrawString_y
        {
            get { return _DrawString_y; }
            set { _DrawString_y = value; }
        }
        /// <summary>
        /// 设置文字水印内容
        /// </summary>
        public string AddText
        {
            get { return _AddText; }
            set { _AddText = value; }
        }
        /// <summary>
        /// 设置文字水印字体
        /// </summary>
        public string Font
        {
            get { return _Font; }
            set { _Font = value; }
        }
        /// <summary>
        /// 设置文字水印字的大小
        /// </summary>
        public int FontSize
        {
            get { return _FontSize; }
            set { _FontSize = value; }
        }
        public int FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
        }
        /// <summary>
        /// 图片水印模式下的覆盖图片的实际地址
        /// </summary>
        public string CopyIamgePath
        {
            set { _CopyIamgePath = System.Web.HttpContext.Current.Server.MapPath(value); }
        }

        //获取文件的后缀名 
        private string GetExt(string path)
        {

            return Path.GetExtension(path);
        }
        //获取输出文件的文件名。 
        private string FileName(string Ext)
        {
            if (_SaveType == 0 || _InFileName.Trim() == "")
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + Ext;
            else
                return _InFileName;
        }
        //检查上传的文件的类型，是否允许上传。 
        private bool IsUpload(string Ext)
        {
            Ext = Ext.Replace(".", "");
            bool b = false;
            string[] arrFileType = _FileType.Split('/');
            foreach (string str in arrFileType)
            {
                if (str.ToLower() == Ext.ToLower())
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        //缩略图按比例缩放大小
        private void mtSetSize(ref Int32 iWidth_, ref Int32 iHeight_, Int32 iSetWidth, Int32 iSetHeight)
        {
            Single iWidth = (Single)iWidth_;
            Single iHeight = (Single)iHeight_;
            if (iWidth > iHeight)
            {
                if (iWidth > iSetWidth)
                {
                    iHeight = iHeight - (iHeight / (iWidth / (iWidth - iSetWidth)));
                    iWidth = iSetWidth;
                }
            }
            if (iWidth < iHeight)
            {
                if (iHeight > iSetHeight)
                {
                    iWidth = iWidth - (iWidth / (iHeight / (iHeight - iSetHeight)));
                    iHeight = iSetHeight;
                }
            }
            if (iWidth == iHeight)
            {
                if (iHeight > iSetHeight)
                {
                    iHeight = iSetHeight;
                    iWidth = iSetWidth;
                }
            }
            iWidth_ = (Int32)iWidth;
            iHeight_ = (Int32)iHeight;
        } // end private static void mtSetSize


        //int_Height int_Width 指定高度和指定宽度 input_Imgfile,out_ImgFile为原图片和缩小后图片的路径,out_FileName为输出文件名
        private void Thumbnail(int int_Width, int int_Height, FileStream input_ImgFile, string out_ImgFile, string out_FileName)
        {

            System.Drawing.Image oldimage = System.Drawing.Image.FromStream(input_ImgFile);
            float New_Width; // 新的宽度    
            float New_Height; // 新的高度    
            float Old_Width, Old_Height; //原始高宽    
            int flat = 0;//标记图片是不是等比    


            int xPoint = 0;//若果要补白边的话，原图像所在的x，y坐标。    
            int yPoint = 0;
            //判断图片    

            Old_Width = (float)oldimage.Width;
            Old_Height = (float)oldimage.Height;

            if ((Old_Width / Old_Height) > ((float)int_Width / (float)int_Height)) //当图片太宽的时候    
            {
                New_Height = Old_Height * ((float)int_Width / (float)Old_Width);
                New_Width = (float)int_Width;
                //此时x坐标不用修改    
                yPoint = (int)(((float)int_Height - New_Height) / 2);
                flat = 1;
            }
            else if ((oldimage.Width / oldimage.Height) == ((float)int_Width / (float)int_Height))
            {
                New_Width = int_Width;
                New_Height = int_Height;
            }
            else
            {
                New_Width = (int)oldimage.Width * ((float)int_Height / (float)oldimage.Height);  //太高的时候   
                New_Height = int_Height;
                //此时y坐标不用修改    
                xPoint = (int)(((float)int_Width - New_Width) / 2);
                flat = 1;
            }


            // ＝＝＝缩小图片＝＝＝    
            System.Drawing.Image thumbnailImage = oldimage.GetThumbnailImage((int)New_Width, (int)New_Height, null, IntPtr.Zero);
            Bitmap bm = new Bitmap(thumbnailImage);

            if (flat != 0)
            {
                Bitmap bmOutput = new Bitmap(int_Width, int_Height);
                Graphics gc = Graphics.FromImage(bmOutput);
                SolidBrush tbBg = new SolidBrush(Color.White);
                gc.FillRectangle(tbBg, 0, 0, int_Width, int_Height); //填充为白色    

                gc.DrawImage(bm, xPoint, yPoint, (int)New_Width, (int)New_Height);
                // bmOutput.Save(out_ImgFile);  
                bmOutput.Save(out_ImgFile + out_FileName.Split('.').GetValue(0).ToString() + "_s." + out_FileName.Split('.').GetValue(1).ToString());
            }
            else
            {
                //bm.Save(out_ImgFile);  
                bm.Save(out_ImgFile + out_FileName.Split('.').GetValue(0).ToString() + "_s." + out_FileName.Split('.').GetValue(1).ToString());

            }

        }


        //上传主要部分。 
        public void Open()
        {

            HttpPostedFile hpFile = _FormFile.PostedFile;
            if (hpFile == null || hpFile.FileName.Trim() == "")
            {
                _Error = 1;
                return;
            }

            string Ext = GetExt(hpFile.FileName);
            // Common.common.msgscript("", Ext);
            if (!IsUpload(Ext))
            {
                _Error = 2;
                return;
            }

            int iLen = hpFile.ContentLength;
            if (iLen > _MaxSize)
            {
                _Error = 3;
                return;
            }

            try
            {

                if (!Directory.Exists(_SavePath))
                    Directory.CreateDirectory(_SavePath);
                byte[] bData = new byte[iLen];
                hpFile.InputStream.Read(bData, 0, iLen);
                string FName;
                FName = FileName(Ext);
                string TempFile = "";
                if (_IsDraw)
                {
                    TempFile = FName.Split('.').GetValue(0).ToString() + "_temp." + FName.Split('.').GetValue(1).ToString();
                }
                else
                {
                    TempFile = FName;
                }
                FileStream newFile = new FileStream(_SavePath + TempFile, FileMode.Create);
                newFile.Write(bData, 0, bData.Length);
                newFile.Flush();
                int _FileSizeTemp = hpFile.ContentLength;

                if (_IsDraw)
                {
                    if (_DrawStyle == 0)
                    {
                        System.Drawing.Image Img1 = System.Drawing.Image.FromStream(newFile);

                        Bitmap bm = new Bitmap(Img1);
                        Graphics g = Graphics.FromImage(bm);
                        //Graphics g = Graphics.FromImage(Img1);



                        //g.DrawImage(bm, 100, 100, bm.Width, bm.Height);
                        Font f = new Font(_Font, _FontSize);
                        Brush b = new SolidBrush(Color.Red);
                        Font fw = new Font(_Font, _FontSize, System.Drawing.FontStyle.Bold);
                        Brush bw = new SolidBrush(Color.White);
                        string addtext = _AddText;
                        //g.DrawString(addtext, f, b, _DrawString_x, _DrawString_y);


                        g.DrawString(addtext, fw, bw, bm.Width - 250 + 1, bm.Height - 20 + 1);
                        g.DrawString(addtext, fw, bw, bm.Width - 250 - 1, bm.Height - 20 - 1);
                        g.DrawString(addtext, fw, bw, bm.Width - 250 + 1, bm.Height - 20 - 1);
                        g.DrawString(addtext, fw, bw, bm.Width - 250 - 1, bm.Height - 20 + 1);

                        g.DrawString(addtext, fw, b, bm.Width - 250, bm.Height - 20);
                        g.Dispose();
                        bm.Save(_SavePath + FName);
                        bm.Dispose();


                    }
                    else
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(newFile);
                        System.Drawing.Image copyImage = System.Drawing.Image.FromFile(_CopyIamgePath);

                        Bitmap bm = new Bitmap(image);
                        Graphics g = Graphics.FromImage(bm);
                        //Graphics g = Graphics.FromImage(image);
                        g.DrawImage(copyImage, new Rectangle(bm.Width - copyImage.Width - 5, bm.Height - copyImage.Height - 5, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                        g.Dispose();
                        bm.Save(_SavePath + FName);
                        bm.Dispose();

                    }
                }

                try
                {
                    ////获取图片的高度和宽度
                    //System.Drawing.Image Img = System.Drawing.Image.FromStream(newFile);
                    //_Width = Img.Width;
                    //_Height = Img.Height;

                    //mtSetSize(ref _Width, ref _Height, _sWidth, _sHeight);

                    //生成缩略图部分 
                    if (_IsCreateImg)
                    {
                        ////如果上传文件小于15k，则不生成缩略图。 
                        //if (iLen > 15360)
                        //{

                        //    System.Drawing.Image newImg = Img.GetThumbnailImage(_Width, _Height, null, System.IntPtr.Zero);
                        //    newImg.Save(_SavePath + FName.Split('.').GetValue(0).ToString() + "_s." + FName.Split('.').GetValue(1).ToString());
                        //    newImg.Dispose();
                        //    _Iss = true;
                        //}


                        //按比例生成固定大小的缩略图，比例不等主动补白边
                        Thumbnail(_sWidth, _sHeight, newFile, _SavePath, FName);
                        _Iss = true;

                    }
                    if (_IsDraw)
                    {
                        if (File.Exists(_SavePath + FName.Split('.').GetValue(0).ToString() + "_temp." + FName.Split('.').GetValue(1).ToString()))
                        {
                            newFile.Dispose();
                            File.Delete(_SavePath + FName.Split('.').GetValue(0).ToString() + "_temp." + FName.Split('.').GetValue(1).ToString());
                        }
                    }
                }
                catch { }
                newFile.Close();
                newFile.Dispose();
                _OutFileName = FName;
                _FileSize = _FileSizeTemp;
                _Error = 0;
                return;
            }
            catch (Exception e)
            {
                //throw e;
                _Error = 4;
                return;
            }

        }
    }
}
