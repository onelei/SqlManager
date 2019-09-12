using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    public class StringUtil
    {
        private static StringBuilder SharedStringBuilder = new StringBuilder();
        private static StringBuilder internalStringBuilder = new StringBuilder();

        public static StringBuilder GetShareStringBuilder()
        {
            SharedStringBuilder.Remove(0, SharedStringBuilder.Length);
            return SharedStringBuilder;
        }

        public static string Concat(string s1, string s2, string s3)
        {
            internalStringBuilder.Remove(0, internalStringBuilder.Length);
            internalStringBuilder.Append(s1);
            internalStringBuilder.Append(s2);
            internalStringBuilder.Append(s3);
            return internalStringBuilder.ToString();
        }

        public static string Concat(string s1, string s2, string s3, string s4)
        {
            internalStringBuilder.Remove(0, internalStringBuilder.Length);
            internalStringBuilder.Append(s1);
            internalStringBuilder.Append(s2);
            internalStringBuilder.Append(s3);
            internalStringBuilder.Append(s4);
            return internalStringBuilder.ToString();
        }
    }
}
