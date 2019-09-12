using System;
using System.Collections.Generic; 

namespace SQL
{
    public enum ESqlType
    {
        Player,
    }

    public class SqlManager
    {
        private static SqlManager _Instance;
        public static SqlManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SqlManager();
                }
                return _Instance;
            }
        }

        private SqlPlayerData playerDataBase;

        private Dictionary<ESqlType, SqlData> sqlDatas = new Dictionary<ESqlType, SqlData>();

        public void OnAwake()
        {
            sqlDatas.Clear();
            sqlDatas.Add(ESqlType.Player, new SqlPlayerData(string.Empty));
        }

        public void OnDestroy()
        {
            Dictionary<ESqlType, SqlData>.Enumerator enumerator = sqlDatas.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.Close();
            }
            enumerator.Dispose();
        }

        public void SetString(ESqlType eSqlType, string key, string values)
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                sqlData.SetValue_String(key, values);
            }
        }

        public void SetInt(ESqlType eSqlType, string key, int value)
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                sqlData.SetValue_Int(key, value);
            }
        }

        public void SetLong(ESqlType eSqlType, string key, long value)
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                sqlData.SetValue_Long(key, value);
            }
        }

        public void SetFloat(ESqlType eSqlType, string key, float value)
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                sqlData.SetValue_Float(key, value);
            }
        }

        public string GetString(ESqlType eSqlType, string key, string defaultValue = "")
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                return sqlData.GetValue_String(key, defaultValue);
            }
            return defaultValue;
        }

        public int GetInt(ESqlType eSqlType, string key, int defaultValue = 0)
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                return sqlData.GetValue_Int(key, defaultValue);
            }
            return defaultValue;
        }

        public float GetFloat(ESqlType eSqlType, string key, float defaultValue = 0)
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                return sqlData.GetValue_Float(key, defaultValue);
            }
            return defaultValue;
        }

        public long GetLong(ESqlType eSqlType, string key, long defaultValue = 0)
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                return sqlData.GetValue_Long(key, defaultValue);
            }
            return defaultValue;
        }

        public void DeleteValue(ESqlType eSqlType, string keys)
        {
            SqlData sqlData;
            if (sqlDatas.TryGetValue(eSqlType, out sqlData))
            {
                sqlData.DeleteValue(keys);
            }
        }
    }
}
