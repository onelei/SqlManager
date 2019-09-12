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
