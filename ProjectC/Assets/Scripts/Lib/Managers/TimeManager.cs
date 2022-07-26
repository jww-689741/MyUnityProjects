using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    // 타이머 실행 중 매개변수가 없는 액션을 실행
    public IEnumerator TimerForAction(float interval, float limit, Action processAction = null, Action endAction = null)
    {
        var waitForSeconds = new WaitForSeconds(interval);
        var current = 0.0f;
        Debug.Log($"타이머 시작(설정 시간 : {limit})");
        while (true)
        {
            current += interval;
            if (current > limit)
            {
                Debug.Log($"타이머 종료(설정 시간 : {limit})");
                if(endAction != null) endAction.Invoke();
                yield break;
            }
            Debug.Log($"타이머 진행중(소요 시간 : {(int)current - 1})");
            if (processAction != null) processAction.Invoke();
            yield return waitForSeconds;
        }
    }
}
