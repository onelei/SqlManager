using System;

namespace SQL
{
    public sealed class PlayerSaveUtil
    {
        public static void SetString(string key, string value)
        {
            SqlManager.Instance.SetString(ESqlType.Player, key, value);
        }

        public static void SetInt(string key, int value)
        {
            SqlManager.Instance.SetInt(ESqlType.Player, key, value);
        }

        public static void SetLong(string key, long value)
        {
            SqlManager.Instance.SetLong(ESqlType.Player, key, value);
        }

        public static void SetFloat(string key, float value)
        {
            SqlManager.Instance.SetFloat(ESqlType.Player, key, value);
        }

        public static string GetString(string key, string defaultValue = "")
        {
            return SqlManager.Instance.GetString(ESqlType.Player, key, defaultValue);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            return SqlManager.Instance.GetInt(ESqlType.Player, key, defaultValue);
        }

        public static long GetLong(string key, long defaultValue = 0)
        {
            return SqlManager.Instance.GetLong(ESqlType.Player, key, defaultValue);
        }

        public static float GetFloat(string key, float defaultValue = 0)
        {
            return SqlManager.Instance.GetFloat(ESqlType.Player, key, defaultValue);
        }

        public static void DeleteValue(string key)
        {
            SqlManager.Instance.DeleteValue(ESqlType.Player, key);
        }
    }
}
