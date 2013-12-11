using System;
using System.IO;
using System.Reflection;

namespace CloudBox.General
{
    public class CBGeneral
    {
        public static string GetCurrentPath()
        {
            string t_sPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            t_sPath = new Uri(t_sPath).LocalPath;
            return t_sPath;
        }
        public static string GetFullPath(string fileName)
        {
            string t_sCurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            fileName = t_sCurrentDirectory + "/" + fileName;
            string newName = new Uri(fileName).LocalPath;
            return newName;
        }
    }

    public class CBDirectory
    {
        /// <summary>
        /// Get current directory
        /// </summary>
        /// <returns>Full path string for current directory</returns>
        public static string GetCurrentDirectory()
        {
            string t_sCurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            t_sCurrentDirectory = new Uri(t_sCurrentDirectory).LocalPath;
            return t_sCurrentDirectory;
        }

        /// <summary>
        /// Get currently working directory
        /// </summary>
        /// <returns>Full path string for working directory</returns>
        public static string GetWorkingDirectory()
        {
            string t_sCurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            t_sCurrentDirectory = new Uri(t_sCurrentDirectory).LocalPath;
            // When C++ call this function, it will remove FULL_PATH/"bin/debug" or FULL_PATH/"bin/release"
            if (t_sCurrentDirectory.ToLower().IndexOf("bin") >= 0)
            {
                t_sCurrentDirectory = t_sCurrentDirectory.Substring(0, t_sCurrentDirectory.ToLower().IndexOf("bin"));
            }
            return t_sCurrentDirectory;
        }

        /// <summary>
        /// Get full file name with working directory
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>A full path string</returns>
        public static string GetFullPath(string fileName)
        {
            string t_sCurrentDirectory = GetWorkingDirectory();
            string t_sFullName = string.Format("{0}/{1}", t_sCurrentDirectory, fileName);
            t_sFullName = new Uri(t_sFullName).LocalPath;
            return t_sFullName;
        }

        /// <summary>
        /// Get full file name with pass directory
        /// </summary>
        /// <param name="directory">Directory</param>
        /// <param name="fileName">File name</param>
        /// <returns>A full path file string</returns>
        public static string GetFullPath(string directory, string fileName)
        {
            string t_sFullName = string.Format("{0}/{1}", directory, fileName);
            t_sFullName = new Uri(t_sFullName).LocalPath;
            return t_sFullName;
        }
    }

}
