using System;

namespace SQL
{
    public class SqlPlayerData : SqlData
    {
        private const string BaseName = "Player";
        public SqlPlayerData(string userName) : base()
        {
            this.tableName = BaseName + userName;
            base.LoadData();
        }
    }
}
