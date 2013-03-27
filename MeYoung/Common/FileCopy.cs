using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    /// <summary>
    /// FileCopy 的摘要说明。
    /// </summary>
    public class FileCopy
    {
        private static Object errtxtLock = new object();
        private static Object txtLock = new object();
        /// <summary>
        /// 文件拷贝 0 源文件不存在 1拷贝成功 -1拷贝失败
        /// </summary>
        /// <param name="SFile">源文件地址 全称 含文件名</param>
        /// <param name="TPath">目标文件夹名称 不含文件名</param>
        /// <param name="TFileName">新文件名</param>
        public static int FilesCopy(string SFile, string TPath, string TFileName)
        {
            int ret = 1;
            if (!File.Exists(SFile))
            {
                return 0;
            }
            try
            {
                if (!Directory.Exists(TPath))
                {
                    Directory.CreateDirectory(TPath);
                }
                if (File.Exists(TPath + "\\" + TFileName))//如果文件存在，取消文件只读属性
                {
                    File.SetAttributes(TPath + "\\" + TFileName, FileAttributes.Normal);
                }
                File.Copy(SFile, TPath + "\\" + TFileName, true);
            }
            catch
            {
                ret = -1;
            }
            return ret;
        }

        public static void directoryCopy(string sourceDirectory, string targetDirectory)
        {
            //			if (!Directory.Exists(sourceDirectory) || !Directory.Exists(targetDirectory)) 
            if (!Directory.Exists(sourceDirectory))
            {
                return;
            }
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] fileInfo = sourceInfo.GetFiles();

            foreach (FileInfo fiTemp in fileInfo)
            {
                if (File.Exists(targetDirectory + "\\" + fiTemp.Name))//如果文件存在，取消文件只读属性
                {
                    File.SetAttributes(targetDirectory + "\\" + fiTemp.Name, FileAttributes.Normal);
                }
                File.Copy(sourceDirectory + "\\" + fiTemp.Name, targetDirectory + "\\" + fiTemp.Name, true);
            }

            DirectoryInfo[] diInfo = sourceInfo.GetDirectories();

            foreach (DirectoryInfo diTemp in diInfo)
            {
                string sourcePath = diTemp.FullName;
                string targetPath = diTemp.FullName.Replace(sourceDirectory, targetDirectory);
                Directory.CreateDirectory(targetPath);
                directoryCopy(sourcePath, targetPath);
            }
        }


        public static void directoryBackup(string sourceDirectory, string targetDirectory, string backupDirectory)
        {
            if (!Directory.Exists(sourceDirectory) || !Directory.Exists(targetDirectory))
            //			if (!Directory.Exists(sourceDirectory)) 
            {
                return;
            }

            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }


            DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] fileInfo = sourceInfo.GetFiles();

            foreach (FileInfo fiTemp in fileInfo)
            {
                //				File.Copy(sourceDirectory + "\\" + fiTemp.Name, targetDirectory + "\\" + fiTemp.Name, true);
                if (File.Exists(targetDirectory + "\\" + fiTemp.Name))//如果文件存在，则备份
                {
                    File.Copy(targetDirectory + "\\" + fiTemp.Name, backupDirectory + "\\" + fiTemp.Name, true);
                }
            }

            DirectoryInfo[] diInfo = sourceInfo.GetDirectories();

            foreach (DirectoryInfo diTemp in diInfo)
            {
                string sourcePath = diTemp.FullName;
                string targetPath = diTemp.FullName.Replace(sourceDirectory, targetDirectory);
                string backupPath = diTemp.FullName.Replace(sourceDirectory, backupDirectory);
                //				Directory.CreateDirectory(targetPath);
                Directory.CreateDirectory(backupPath);

                //				directoryCopy(sourcePath,targetPath);
                directoryBackup(sourcePath, targetPath, backupPath);
            }
        }


        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="sourceDirectory"></param>
        public static void deldirectory(string sourceDirectory)
        {
            //			if (!Directory.Exists(sourceDirectory) || !Directory.Exists(targetDirectory)) 
            if (!Directory.Exists(sourceDirectory))
            {
                return;
            }

            DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] fileInfo = sourceInfo.GetFiles();

            foreach (FileInfo fiTemp in fileInfo)
            {
                if (File.Exists(sourceDirectory + "\\" + fiTemp.Name))//如果文件存在，取消文件只读属性
                {
                    File.SetAttributes(sourceDirectory + "\\" + fiTemp.Name, FileAttributes.Normal);
                }
                File.Delete(sourceDirectory + "\\" + fiTemp.Name);
            }

            DirectoryInfo[] diInfo = sourceInfo.GetDirectories();

            foreach (DirectoryInfo diTemp in diInfo)
            {
                string sourcePath = diTemp.FullName;
                DirectoryInfo sourcePathInfo = new DirectoryInfo(sourcePath);
                deldirectory(sourcePath);
                sourcePathInfo.Delete();
            }
        }

        public static void FileNameSel(string sourceDirectory, string thatpaths, string thismessage, out string AllPath, out string message)
        {
            AllPath = thatpaths;
            message = thismessage;

            if (!Directory.Exists(sourceDirectory))
            {
                return;
            }

            DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] fileInfo = sourceInfo.GetFiles();

            foreach (FileInfo fiTemp in fileInfo)
            {
                if (fiTemp.Name.IndexOf(".cs") != -1)
                {
                    if (AllPath.IndexOf("\\" + fiTemp.Name) == -1)
                    {
                        AllPath += sourceDirectory + "\\" + fiTemp.Name + "\r\n";
                    }
                    else
                    {
                        int i = AllPath.IndexOf(fiTemp.Name);
                        string temp = AllPath.Substring(0, i + fiTemp.Name.Length);
                        int j = temp.LastIndexOf("\r\n");
                        if (j != -1)
                        {
                            temp = temp.Substring(j, i + fiTemp.Name.Length - j);
                        }
                        message += temp + "\r\n";
                        message += sourceDirectory + "\\" + fiTemp.Name + "\r\n";
                    }
                }
            }

            DirectoryInfo[] diInfo = sourceInfo.GetDirectories();

            foreach (DirectoryInfo diTemp in diInfo)
            {
                string sourcePath = diTemp.FullName;
                DirectoryInfo sourcePathInfo = new DirectoryInfo(sourcePath);
                FileNameSel(sourcePath, AllPath, message, out AllPath, out message);
            }
        }

        public static void FileTextNameSel(string sourceDirectory, string thatpaths, string thismessage, out string AllPath, out string message)
        {
            AllPath = thatpaths;
            message = thismessage;

            if (!Directory.Exists(sourceDirectory))
            {
                return;
            }

            DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] fileInfo = sourceInfo.GetFiles();

            foreach (FileInfo fiTemp in fileInfo)
            {
                if (fiTemp.Name.ToLower().IndexOf(".aspx.cs") != -1)
                {

                    string filestr = File.ReadAllText(sourceDirectory + "\\" + fiTemp.Name);
                    int ii = filestr.IndexOf(":");
                    if (ii != -1)
                    {
                        filestr = filestr.Substring(0, ii).Trim();
                        int jj = filestr.LastIndexOf(" ");
                        if (jj != -1)
                        {
                            filestr = "=" + filestr.Substring(jj).Trim() + ":";
                        }
                    }

                    if ((filestr.Length > 0) && (filestr.Length < 30))
                    {
                        if (AllPath.IndexOf(filestr) == -1)
                        {
                            AllPath += sourceDirectory + "\\" + fiTemp.Name + filestr + "\r\n";
                        }
                        else
                        {
                            int i = AllPath.IndexOf(filestr);
                            string temp = AllPath.Substring(0, i + filestr.Length);
                            int j = temp.LastIndexOf("\r\n");
                            if (j != -1)
                            {
                                temp = temp.Substring(j, i + filestr.Length - j);
                            }
                            message += temp + "\r\n";
                            message += sourceDirectory + "\\" + fiTemp.Name + filestr + "\r\n";
                        }
                    }
                }
            }

            DirectoryInfo[] diInfo = sourceInfo.GetDirectories();

            foreach (DirectoryInfo diTemp in diInfo)
            {
                string sourcePath = diTemp.FullName;
                DirectoryInfo sourcePathInfo = new DirectoryInfo(sourcePath);
                FileTextNameSel(sourcePath, AllPath, message, out AllPath, out message);
            }
        }


        //记录操作日志
        public static void SaveTxtLog(string context)
        {
            lock (txtLock)
            {
                string fileName = DateTime.Now.Day.ToString() + "_V.txt";//生成粗略文件名
                string FileName = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString();//生成文件夹名称
                string FilePath = System.Web.HttpContext.Current.Server.MapPath("/logs/" + FileName + "/");//生成文件存放路径
                if (Directory.Exists(FilePath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(FilePath);
                }
                FileStream fs = new FileStream(FilePath + fileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(DateTime.Now.ToString() + ":" + context + "\r\n");
                sw.Close();
                fs.Close();
            }

        }

        //记录错误日志
        public static void SaveETxtLog(string context)
        {
            lock (errtxtLock)
            {
                string fileName = DateTime.Now.Day.ToString() + "_E.txt";//生成粗略文件名
                string FileName = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString();//生成文件夹名称
                string FilePath = System.Web.HttpContext.Current.Server.MapPath("/logs/" + FileName + "/");//生成文件存放路径
                if (Directory.Exists(FilePath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(FilePath);
                }
                FileStream fs = new FileStream(FilePath + fileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(DateTime.Now.ToString() + ":" + context + "\r\n");
                sw.Close();
                fs.Close();
            }

        }

        public static string ReadTxtFile(string files)
        {
            string txtbody = "";
            files = System.Web.HttpContext.Current.Server.MapPath(files);
            if (File.Exists(files) == false)
            {
                return "";
            }

            FileStream fs = new FileStream(files, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            txtbody = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return txtbody;
        }

    }
}
