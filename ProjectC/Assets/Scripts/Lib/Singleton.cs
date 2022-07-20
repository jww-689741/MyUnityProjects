using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 싱글톤 제네릭 클래스
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shuttDownFlag = false;
    private static object _lock = new object();
    private static T instance;


    public static T Instance
    {
        get
        {
            // 싱글톤 셧다운 상태일 경우
            if(shuttDownFlag)
            {
                Debug.Log("[Singleton] Instance '" + typeof(T) + "' 이(가) 종료되었습니다.");
                return null;
            }
            lock (_lock)
            {
                instance = (T)FindObjectOfType(typeof(T));
                // 인스턴스가 생성되지 않았을 경우
                if (instance == null)
                {
                    var newSingleton = new GameObject();
                    instance = newSingleton.AddComponent<T>();
                    newSingleton.name = typeof(T).ToString() + "_Singleton";
                    DontDestroyOnLoad(newSingleton);
                }
            }
            return instance;
        }
    }

    private void OnApplicationQuit()
    {
        shuttDownFlag = true;
    }

    private void OnDestroy()
    {
        shuttDownFlag = true;
    }
}
