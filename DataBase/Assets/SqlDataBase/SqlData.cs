using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Text;

namespace SQL
{
    public abstract class SqlData
    {
        private const string PATH_DATABASE = "dataBase.db";

        /// <summary>
        /// 数据库连接
        /// </summary>
        private SqliteConnection SqlConnection;
        /// <summary>
        /// 数据库命令
        /// </summary>
        private SqliteCommand SqlCommand;
        /// <summary>
        /// 数据库读取
        /// </summary>
        private SqliteDataReader SqlDataReader;

        private Hashtable dataHashTable = new Hashtable();
        public string tableName;

        private static readonly string _KEY = "key";
        private static readonly string _VALUE_TYPE = "valueType";
        private static readonly string _VALUE_INT = "valueInt";
        private static readonly string _VALUE_STRING = "valueString";
        private static readonly string _VALUE_FLOAT = "valueFloat";
        private static readonly string _VALUE_LONG = "valueLong";

        private readonly List<string> COL_NAMES = new List<string>() { _KEY, _VALUE_TYPE, _VALUE_INT, _VALUE_STRING, _VALUE_FLOAT, _VALUE_LONG };
        private readonly List<string> COL_TYPES = new List<string>() { "TEXT", "INTEGER", "INTEGER", "TEXT", "REAL", "TEXT" };

        private enum EDataType
        {
            String = 1,
            Int = 2,
            Long = 3,
            Float = 4,
        }

        /// <summary>
        /// 建立数据库连接
        /// </summary>
        public SqlData()
        {
            try
            {
                SqlConnection = new SqliteConnection(GetDataPath(PATH_DATABASE));
                SqlConnection.Open();
                SqlCommand = SqlConnection.CreateCommand();
            }
            catch (System.Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public SqliteDataReader ExecuteReader(string command)
        {
#if UNITY_EDITOR
            Debug.Log("SQL:ExecuteReader " + command);
#endif
            SqlCommand.CommandText = command;
            SqlDataReader = SqlCommand.ExecuteReader();
            return SqlDataReader;
        }

        public void ExecuteNonQuery(string command)
        {
#if UNITY_EDITOR
            Debug.Log("SQL:ExecuteNonQuery " + command);
#endif
            SqlCommand.CommandText = command;
            SqlCommand.ExecuteNonQuery();
        }

        public bool ExecuteScalar(string command)
        {
#if UNITY_EDITOR
            Debug.Log("SQL:ExecuteScalar " + command);
#endif
            SqlCommand.CommandText = command;
            int result = System.Convert.ToInt32(SqlCommand.ExecuteScalar());
            Debug.Log("result "+ result);
            return (result > 0);
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void Close()
        {
            if (SqlCommand != null)
            {
                SqlCommand.Dispose();
                SqlCommand = null;
            }

            if (SqlDataReader != null)
            {
                SqlDataReader.Close();
                SqlDataReader = null;
            }

            if (SqlConnection != null)
            {
                SqlConnection.Close();
                SqlConnection = null;
            }
        }

        #region SQL
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="col"></param>
        /// <param name="colType"></param>
        public void SQL_CreateTable(List<string> col, List<string> colType)
        {
            // CREATE TABLE table_name(column1 type1, column2 type2, column3 type3,...);
            if (col.Count != colType.Count)
            {
                Debug.LogError("col Count != col Type Count.");
                return;
            }

            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();

            stringBuilder.Append("Create Table ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" (");

            for (int i = 0; i < col.Count; i++)
            {
                stringBuilder.Append(col[i]);
                stringBuilder.Append(" ");
                stringBuilder.Append(colType[i]);
                if (i != col.Count - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append(")");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SQL_Insert_String(string key, string value)
        {
            // INSERT INTO table_name(column1, column2, column3,...) VALUES(value1, value2, value3,...);
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("insert into ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" (");
            stringBuilder.Append(_KEY);
            stringBuilder.Append(",");
            stringBuilder.Append(_VALUE_TYPE);
            stringBuilder.Append(",");
            stringBuilder.Append(_VALUE_STRING);

            stringBuilder.Append(") values ('");
            stringBuilder.Append(key);
            stringBuilder.Append("',");
            stringBuilder.Append(((int)EDataType.String).ToString());
            stringBuilder.Append(",'");
            stringBuilder.Append(value);
            stringBuilder.Append("')");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SQL_Insert_Int(string key, int value)
        {
            // INSERT INTO table_name(column1, column2, column3,...) VALUES(value1, value2, value3,...);
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("insert into ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" (");
            stringBuilder.Append(_KEY);
            stringBuilder.Append(",");
            stringBuilder.Append(_VALUE_TYPE);
            stringBuilder.Append(",");
            stringBuilder.Append(_VALUE_INT);

            stringBuilder.Append(") values ('");
            stringBuilder.Append(key);
            stringBuilder.Append("',");
            stringBuilder.Append(((int)EDataType.Int).ToString());
            stringBuilder.Append(",");
            stringBuilder.Append(value);
            stringBuilder.Append(")");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SQL_Insert_Long(string key, long value)
        {
            // INSERT INTO table_name(column1, column2, column3,...) VALUES(value1, value2, value3,...);
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("insert into ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" (");
            stringBuilder.Append(_KEY);
            stringBuilder.Append(",");
            stringBuilder.Append(_VALUE_TYPE);
            stringBuilder.Append(",");
            stringBuilder.Append(_VALUE_LONG);

            stringBuilder.Append(") values ('");
            stringBuilder.Append(key);
            stringBuilder.Append("',");
            stringBuilder.Append(((int)EDataType.Long).ToString());
            stringBuilder.Append(",");
            stringBuilder.Append(value);
            stringBuilder.Append(")");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SQL_Insert_Float(string key, float value)
        {
            // INSERT INTO table_name(column1, column2, column3,...) VALUES(value1, value2, value3,...);
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("insert into ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" (");
            stringBuilder.Append(_KEY);
            stringBuilder.Append(",");
            stringBuilder.Append(_VALUE_TYPE);
            stringBuilder.Append(",");
            stringBuilder.Append(_VALUE_FLOAT);

            stringBuilder.Append(") values ('");
            stringBuilder.Append(key);
            stringBuilder.Append("',");
            stringBuilder.Append(((int)EDataType.Float).ToString());
            stringBuilder.Append(",");
            stringBuilder.Append(value);
            stringBuilder.Append(")");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void SQL_Delete(string key)
        {
            // DELETE FROM table_name WHERE some_column = some_value;
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("delete from ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" where ");
            stringBuilder.Append(_KEY);
            stringBuilder.Append(" = '");
            stringBuilder.Append(key);
            stringBuilder.Append("'");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SQL_Update_String(string key, string value)
        {
            // UPDATE table_name SET column1 = value1, column2 = value2,... WHERE some_column = some_value;
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("update ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" set ");
            stringBuilder.Append(_VALUE_STRING);
            stringBuilder.Append("='");
            stringBuilder.Append(value);

            stringBuilder.Append("' where ");
            stringBuilder.Append(_KEY);
            stringBuilder.Append("='");
            stringBuilder.Append(key);
            stringBuilder.Append("'; ");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SQL_Update_Int(string key, int value)
        {
            // UPDATE table_name SET column1 = value1, column2 = value2,... WHERE some_column = some_value;
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("update ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" set ");
            stringBuilder.Append(_VALUE_INT);
            stringBuilder.Append("=");
            stringBuilder.Append(value);

            stringBuilder.Append(" where ");
            stringBuilder.Append(_KEY);
            stringBuilder.Append("='");
            stringBuilder.Append(key);
            stringBuilder.Append("'; ");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SQL_Update_Long(string key, long value)
        {
            // UPDATE table_name SET column1 = value1, column2 = value2,... WHERE some_column = some_value;
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("update ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" set ");
            stringBuilder.Append(_VALUE_LONG);
            stringBuilder.Append("=");
            stringBuilder.Append(value);

            stringBuilder.Append(" where ");
            stringBuilder.Append(_KEY);
            stringBuilder.Append("='");
            stringBuilder.Append(key);
            stringBuilder.Append("'; ");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SQL_Update_Float(string key, float value)
        {
            // UPDATE table_name SET column1 = value1, column2 = value2,... WHERE some_column = some_value;
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("update ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" set ");
            stringBuilder.Append(_VALUE_LONG);
            stringBuilder.Append("=");
            stringBuilder.Append(value);

            stringBuilder.Append(" where ");
            stringBuilder.Append(_KEY);
            stringBuilder.Append("='");
            stringBuilder.Append(key);
            stringBuilder.Append("'; ");
            ExecuteNonQuery(stringBuilder.ToString());
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool SQL_Select(string key)
        {
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("select ");
            stringBuilder.Append(key);
            stringBuilder.Append(" from ");
            stringBuilder.Append(tableName);
            try
            {
                SqlDataReader = ExecuteReader(stringBuilder.ToString());
                if (SqlDataReader != null)
                    return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return false;
        }

        public void LoadData()
        {
            if (!ExistTable(tableName))
            {
                //table 不存在
                SQL_CreateTable(COL_NAMES, COL_TYPES);
                return;
            }

            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("select * from ");
            stringBuilder.Append(tableName);
            SqliteDataReader dataReader = ExecuteReader(stringBuilder.ToString());
            while (dataReader.Read())
            {
                string key = dataReader.GetString(dataReader.GetOrdinal(_KEY));
                int dataType = dataReader.GetInt32(dataReader.GetOrdinal(_VALUE_TYPE));
                switch ((EDataType)dataType)
                {
                    case EDataType.Float:
                        dataHashTable[key] = dataReader.GetFloat(dataReader.GetOrdinal(_VALUE_TYPE));
                        break;
                    case EDataType.Int:
                        dataHashTable[key] = dataReader.GetInt32(dataReader.GetOrdinal(_VALUE_INT));
                        break;
                    case EDataType.Long:
                        dataHashTable[key] = dataReader.GetInt64(dataReader.GetOrdinal(_VALUE_LONG));
                        break;
                    case EDataType.String:
                        dataHashTable[key] = dataReader.GetString(dataReader.GetOrdinal(_VALUE_STRING));
                        break;
                    default:
                        break;
                }
            }
            dataReader.Close();
        }

        /// <summary>
        /// 表格是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool ExistTable(string tableName)
        {
            // SELECT COUNT(*) FROM sqlite_master;
            StringBuilder stringBuilder = StringUtil.GetShareStringBuilder();
            stringBuilder.Append("SELECT COUNT(*) FROM sqlite_master where type='table' and name='");
            stringBuilder.Append(tableName);
            stringBuilder.Append("';");
            return ExecuteScalar(stringBuilder.ToString());
        }

        public string GetDataPath(string databasePath)
        {
#if UNITY_EDITOR 
            return StringUtil.Concat("data source=", Application.streamingAssetsPath, "/", databasePath);
#endif
#if UNITY_ANDROID
            return StringUtil.Concat("URI=file:", Application.persistentDataPath, "/", databasePath);
#endif
#if UNITY_IOS
            return StringUtil.Concat("data source=", Application.persistentDataPath, "/", databasePath);
#endif
        }
        #endregion

        public void SetValue_String(string key, string value)
        {
            if (ContainsKey(key))
            {
                SQL_Update_String(key, value);
            }
            else
            {
                SQL_Insert_String(key, value);
            }
            dataHashTable[key] = value;
        }

        public void SetValue_Int(string key, int value)
        {
            if (ContainsKey(key))
            {
                SQL_Update_Int(key, value);
            }
            else
            {
                SQL_Insert_Int(key, value);
            }
            dataHashTable[key] = value;
        }

        public void SetValue_Long(string key, long value)
        {
            if (ContainsKey(key))
            {
                SQL_Update_Long(key, value);
            }
            else
            {
                SQL_Insert_Long(key, value);
            }
            dataHashTable[key] = value;
        }

        public void SetValue_Float(string key, float value)
        {
            if (ContainsKey(key))
            {
                SQL_Update_Float(key, value);
            }
            else
            {
                SQL_Insert_Float(key, value);
            }
            dataHashTable[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return dataHashTable.ContainsKey(key);
        }

        public string GetValue_String(string key, string defaultValue = "")
        {
            if (dataHashTable.ContainsKey(key))
            {
                return System.Convert.ToString(dataHashTable[key]);
            }
            return defaultValue;
        }

        public int GetValue_Int(string key, int defaultValue = 0)
        {
            if (dataHashTable.ContainsKey(key))
            {
                return System.Convert.ToInt32(dataHashTable[key]);
            }
            return defaultValue;
        }

        public long GetValue_Long(string key, long defaultValue = 0)
        {
            if (dataHashTable.ContainsKey(key))
            {
                return System.Convert.ToInt64(dataHashTable[key]);
            }
            return defaultValue;
        }

        public float GetValue_Float(string key, float defaultValue = 0)
        {
            if (dataHashTable.ContainsKey(key))
            {
                return System.Convert.ToSingle(dataHashTable[key]);
            }
            return defaultValue;
        }

        public void DeleteValue(string key)
        {
            if (dataHashTable.ContainsKey(key))
            {
                dataHashTable.Remove(key);
            }
            SQL_Delete(key);
        }
    }
}