using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Internal
{
    public static class UnixPath
    {
        public static readonly char DirectorySeparatorChar = '/';
        public static readonly char AltDirectorySeparatorChar = '/';
        public static readonly char PathSeparator = ':';
        public static readonly char VolumeSeparatorChar = '/';

        public static string ToUnixPath(this string path)
        {
            return path.Replace('\\', '/');
        }

        public static string FileName(string path)
        {
            int index = path.LastIndexOfAny(new char[] { '\\', '/' });
            if (index >= 0)
            {
                path = path.Substring(index + 1);
            }
            return path;
        }

        public static string DirectoryName(string path)
        {
            int index = path.LastIndexOfAny(new char[] { '\\', '/' });
            if (index >= 0)
            {
                path = path.Substring(0, index);
            }
            return path;
        }

        //public static string Combine(params string[] parts)
        //{
        //    StringBuilder path = new StringBuilder();
           
        //    foreach (string part in parts)
        //    {
        //        path.Append('/').Append(part.Trim('\\', '/'));
        //    }
        //    path.Replace('\\', '/');
        //    return path.ToString();
        //}

        public static string Combine(string path1, params string[] paths)
        {
            if (path1 == null)
            {
                throw new ArgumentNullException("path1");
            }
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }
            //if (path1.IndexOfAny(UnixPath._InvalidPathChars) != -1)
            //{
            //    throw new ArgumentException("Illegal characters in path", "path1");
            //}
            int num = path1.Length;
            int num2 = -1;
            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i] == null)
                {
                    throw new ArgumentNullException($"paths[{i}]");
                }
                //if (paths[i].IndexOfAny(UnixPath._InvalidPathChars) != -1)
                //{
                //    throw new ArgumentException("Illegal characters in path", "paths[" + i + "]");
                //}
                if (UnixPath.IsPathRooted(paths[i]))
                {
                    num = 0;
                    num2 = i;
                }
                num += paths[i].Length + 1;
            }
            StringBuilder stringBuilder = new StringBuilder(num);
            if (num2 == -1)
            {
                stringBuilder.Append(path1);
                num2 = 0;
            }
            for (int j = num2; j < paths.Length; j++)
            {
                UnixPath.Combine(stringBuilder, paths[j]);
            }
            return stringBuilder.ToString();
        }

        private static void Combine(StringBuilder path, string part)
        {
            if (path.Length > 0 && part.Length > 0)
            {
                char c = path[path.Length - 1];
                if (c != UnixPath.DirectorySeparatorChar && c != UnixPath.AltDirectorySeparatorChar && c != UnixPath.VolumeSeparatorChar)
                {
                    path.Append(UnixPath.DirectorySeparatorChar);
                }
            }
            path.Append(part);
        }

        public static bool IsPathRooted(string path)
        {
            return path != null && path.Length != 0 && path[0] == UnixPath.DirectorySeparatorChar;
        }

    }
}
