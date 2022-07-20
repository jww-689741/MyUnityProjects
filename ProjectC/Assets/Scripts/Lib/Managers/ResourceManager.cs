using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    // 리소스 로드
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    // 프리팹 생성
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}"); // 프리팹을 설정한 경로로 로드
        if (prefab == null)
        {
            Debug.Log($"프리팹 경로가 잘못되어 로드 할 수 없습니다 : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    // 프리팹 제거
    public void Destroy(GameObject target)
    {
        if(target == null)
        {
            return;
        }
        Object.Destroy(target);
    }
}
