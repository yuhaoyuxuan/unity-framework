//==========================
// - Author:      slf         
// - Date:        2021/08/05 17:01:40	
// - Description: 抽象单例基类
//==========================

namespace Slf
{
    public abstract class Singleton<T> where T : new()
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}
