namespace ConsoleApp;



public class Singleton<T> where T : new()//保留一份静态的对象而又无需把类声明为静态类，因为静态类无法使用实例化对象
    {
        private static T instance;
 
        public static T Instance
        {
            get
            {
                return Equals(instance, default(T)) ? (instance = new T()) : instance;
            }
        }
    }
