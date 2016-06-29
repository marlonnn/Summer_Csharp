using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Zip
{
    public class FileInfoHelper
    {
        /// <summary>
        /// 查找指定文件夹下面指定后缀名的最新文件
        /// </summary>
        /// <param name="baseDirectory">根文件夹</param>
        /// <param name="fileFolder">指定的文件夹</param>
        /// <param name="filter">过滤条件 如"*.ncf"文件</param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string baseDirectory, string fileFolder, string filter)
        {
            string path = baseDirectory + fileFolder;
            if (path != null)
            {
                FileInfo fileInfo;
                try
                {
                    DirectoryInfo folder = new DirectoryInfo(path);
                    fileInfo = (from f in folder.GetFiles(filter)
                                orderby f.LastWriteTime descending
                                select f).First();
                }
                catch (Exception e)
                {
                    return null;
                }

                return fileInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查找指定文件夹下面所有的文件信息
        /// </summary>
        /// <param name="baseDirectory">根文件夹</param>
        /// <param name="fileFolder">指定查找的文件夹</param>
        /// <returns></returns>
        public static FileInfo[] GetFileInfo(string baseDirectory, string fileFolder)
        {
            DirectoryInfo folder = new DirectoryInfo(baseDirectory + fileFolder);
            try
            {
                FileInfo[] files = folder.GetFiles();
                return files;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据条件查找出指定文件夹下的所有文件信息
        /// </summary>
        /// <param name="baseDirectory">根文件夹</param>
        /// <param name="fileFolder">查找的文件夹</param>
        /// <param name="filter">过滤条件，如：文件名"config"、文件后缀".txt"</param>
        /// <param name="isNamefilter">是否是以文件名为过滤条件</param>
        /// <returns></returns>
        public static FileInfo[] GetFileInfo(string baseDirectory, string fileFolder, string[] filter, bool isNamefilter)
        {
            DirectoryInfo folder = new DirectoryInfo(baseDirectory + fileFolder);
            try
            {
                FileInfo[] files;
                if (isNamefilter)
                {
                    files = folder.GetFiles().Where(f => filter.Contains(f.Name)).ToArray();
                }
                else
                {
                    files = folder.GetFiles().Where(f => filter.Contains(f.Extension.ToLower())).ToArray();
                }

                return files;
            }
            catch (Exception e)
            {

                return null;
            }
        }

    }
}
