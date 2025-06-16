**DebugL 시스템은 정적(static) 클래스로, 프로젝트 어디에서든 쉽게 사용할 수 있습니다.**
# 🚀 사용법 (Usage)

### 1. 초기화 (Initialization)
게임을 시작할 때 ``DebugL`` 시스템을 **한 번만 초기화**해야 합니다. 보통 ``GameManager``와 같은 스크립트의 ``Awake()`` 또는 ``Start()`` 메서드에서 호출하는 것이 좋습니다.

```cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        // 선택 사항: 로그 파일 경로 및 설정 변경 (Initialize() 호출 전)
        // DebugL.LogFilePath = Application.persistentDataPath + "/MyCustomLogs";
        // DebugL.LogFileName = "CustomGameLog.txt";
        // DebugL.LogTypesToSave = LogType.Error | LogType.Exception | LogType.Warning; 
        // DebugL.EnableUnityConsoleOutput = true; 

        DebugL.Initialize();

        DebugL.Log("DebugL 시스템이 초기화되었습니다.");
    }
}
```

### 2. 로그 사용 (Logging)
``Unity``의 ``Debug`` 클래스 대신 ``DebugL``을 사용하여 로그를 출력합니다. 모든 로그에는 ``[DebugL]`` **접두사**가 자동으로 붙습니다.

```cs
using UnityEngine;
using System;

public class MyGameScript : MonoBehaviour
{
    void Start()
    {
        DebugL.Log("Hello, DebugL!");
        DebugL.LogWarning("이것은 경고입니다.");
        DebugL.LogError("이것은 에러입니다!");

        try
        {
            throw new Exception("테스트 예외 발생!");
        }
        catch (Exception e)
        {
            DebugL.LogException(e);
        }
    }
}
```
### 3. 로그 파일 확인 (Check Log File)
로그 파일은 기본적으로 ``Application.persistentDataPath`` 경로에 ``[DebugL] Log.txt`` 이름으로 저장됩니다.

### 4. 로그 지우기 (Clear Logs)
메모리에 쌓인 로그 목록을 지우려면 다음을 호출합니다:

```cs
DebugL.ClearLogs();
```

# ⚙️ 설정 (Configuration)
``DebugL`` 시스템의 동작은 다음 **정적(static) 속성**들을 통해 설정할 수 있습니다. 이 설정들은 ``DebugL.Initialize()`` 메서드를 **호출하기 전에** 변경해야 합니다.

- ``DebugL.LogFilePath``: (string) 로그 파일이 저장될 폴더 경로를 설정합니다. ``null`` 또는 경로를 지정하지 않으면 유니티의 ``Application.persistentDataPath``를 기본 경로로 사용합니다.
  - **예시**: ``DebugL.LogFilePath = Application.persistentDataPath + "/MyGameLogs";``
- ``DebugL.LogFileName``: (string) 생성될 로그 파일의 이름을 설정합니다. 기본값은 ``"[DebugL] Log.txt"``입니다.
  - **예시**: ``DebugL.LogFileName = "Session_Log.txt";``
- ``DebugL.LogTypesToSave``: (``LogType`` 플래그) 파일로 저장할 로그의 종류를 설정합니다. 여러 타입을 동시에 저장하고 싶다면 | (비트 OR) 연산자를 사용해 조합할 수 있습니다.
  - **기본값**: ``LogType.Error | LogType.Exception`` (에러와 예외만 저장)
  - **예시**: ``DebugL.LogTypesToSave = LogType.Error | LogType.Exception | LogType.Warning;`` (에러, 예외, 경고만 저장)
- ``DebugL.EnableUnityConsoleOutput``: (bool) ``true``로 설정하면 ``DebugL``을 통해 출력된 로그가 유니티 콘솔에도 표시됩니다. ``false``로 설정하면 로그가 파일에만 저장되고 콘솔에는 출력되지 않습니다.
  - **기본값**: ``true``

