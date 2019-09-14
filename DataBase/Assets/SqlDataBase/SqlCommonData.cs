using System;

namespace SQL
{
    public class SqlCommonData : SqlData
    {
        private const string BaseName = "Common";
        public SqlCommonData(string userName) : base()
        {
            this.tableName = BaseName + userName;
            base.LoadData();
        }
    }
}
