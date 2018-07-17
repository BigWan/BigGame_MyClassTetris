using UnityEngine;

/// <summary>
/// MonoBehaviour单例
/// </summary>
/// <typeparam name="T">继承 MonoBehaviour 的类型 </typeparam>
public class UnitySingleton<T> :MonoBehaviour where T :Component {

    private static T _instance;
    public static T Instance{
        get{
            if(_instance == null){
                CreateInstance();
            }
            return _instance;
        }
    }
    public static void CreateInstance(){
        _instance = FindObjectOfType(typeof(T)) as T;
        if(_instance == null){
            GameObject o = new GameObject {
                hideFlags = HideFlags.HideAndDontSave
            };
            _instance = o.AddComponent<T>();
        }
    }
}