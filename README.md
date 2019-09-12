# Unity数据存储Sqlite的使用

## 是什么

Sqlite是是一个SQL数据库引擎。

## 为什么

之前的项目中基本都是使用PlayerPrefs来存储数据，但是由于后期数据结构复杂的情况下，比如存储一个List的结构数据，通常是自己控制写法添加逗号，分号来将数据分隔开。如果哪里写错了，整个数据就会错位导致数据错乱，维护起来非常麻烦。由于服务器的数据存储都是使用数据库SQL，因此打算在前端也使用数据库存储数据。

## 怎么做

#### 前期准备

要在Unity中使用Sqlite需要将Mono.Data.Sqlite.dll，System.Data.dll， Sqlite3.dll 三个文件放入Plugins文件夹下。

##### Mono.Data.Sqlite.dll

在Unity的Editor安装目录下“ Editor\Data\Mono\lib\mono\2.0\ Mono.Data.Sqlite.dll”

##### System.Data.dll

在Unity的Editor安装目录下“ Editor\Data\Mono\lib\mono\2.0\ System.Data.dll”

##### Sqlite3.dll

在Sqlite的官网下载对应的版本即可“ https://www.sqlite.org/download.html ”

![1568293365829](https://github.com/onelei/ThreadFileWriter/blob/master/Images/1568293365829.png)

#### 实现

下载一个数据库查看软件Navicat Premium，通过它创建一个数据库database.db，将其放入Unity的“StreamingAssets”目录下。

![1568294082437](https://github.com/onelei/ThreadFileWriter/blob/master/Images/1568294082437.png)

![1568293982940](https://github.com/onelei/ThreadFileWriter/blob/master/Images/1568293982940.png)

数据库一般是增删改查这几项功能。

新建SqliteConnection，SqliteCommand，SqliteDataReader三个变量。

```C#
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
```

建立数据库连接

```C#
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
```

执行查询语句

```C#
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
```

创建表格

```C#
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
```

 插入

```C#
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
```

删除

```C#
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
```

更新

```C#
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
```

查找

```C#
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
```

表格是否存在

```C#
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
```

数据库在不同平台下的路径

```C#
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
```



由于数据在表格中，因此我们需要使用一个HashTable来存储这些数据，一开始先载入数据库，然后读取出里面的所有数据将其存入HashTable中，后面增删改查的时候不仅针对HashTable进行操作，还要针对数据库进行操作。

```C#
private Hashtable dataHashTable = new Hashtable();
```

我们存储数据的时候不一定都是String类型，同时还需要Int，Long，Float等类型，因此需要预先设置好数据库的列名，同时还需要一个数据类型字段来表明存储的数据类型，具体如下

![1568295147722](https://github.com/onelei/ThreadFileWriter/blob/master/Images/1568295147722.png)

```C#
   	private enum EDataType
        {
            String = 1,
            Int = 2,
            Long = 3,
            Float = 4,
        }
```

加载表格

```C#
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
```

设置Value

```C#
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
```

删除Value

```C#
	public void DeleteValue(string key)
        {
            if (dataHashTable.ContainsKey(key))
            {
                dataHashTable.Remove(key);
            }
            SQL_Delete(key);
        }
```

关闭数据库

```C#
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
```

整个数据库相关操作已经完成，新建一个SqlManager统一管理即可

```C#
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
```

由于使用的时候需要调用的函数名太长，可以新建一个工具类封装一下

![1568295539751](https://github.com/onelei/ThreadFileWriter/blob/master/Images/1568295539751.png)

新建一个测试函数

```C#
using UnityEngine; 

namespace SQL
{
    public class TestSqlData : MonoBehaviour
    {
        private void Awake()
        {
            SqlManager.Instance.OnAwake();
        }

        private void OnDestroy()
        {
            SqlManager.Instance.OnDestroy();
        }

        // Use this for initialization
        void Start()
        {
            SaveUtil.SetString("name", "onelei");
            SaveUtil.SetInt("score", 99);

            Debug.Log("name的值为 " + SaveUtil.GetString("name"));
            Debug.Log("score的值为 " + SaveUtil.GetString("score"));

            //SaveUtil.DeleteValue("score");
            Debug.Log("score的值为 " + SaveUtil.GetInt("score")); 
        }

    }
}
```

运行Unity，发现表格数据已经生成

![1568295690452](https://github.com/onelei/ThreadFileWriter/blob/master/Images/1568295690452.png)

同时通过C#，也将数据读取了出来

![1568295883460](https://github.com/onelei/ThreadFileWriter/blob/master/Images/1568295883460.png)

具体源码可以通过Github查看： https://github.com/onelei/SqlManager
