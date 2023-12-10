using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour {
    private void Awake() {
        // 이름을 통해 이펙트를 출력.
        // 각 이펙트는 어떻게 저장할까?
        // 변수는 테이블로, 세부 구현은 클래스로.
        // ScriptableObject를 써도 될듯? -> 아트가 수정하기 쉽게
        // 이펙트에 필요한 종속성들은 어떻게 할까?

        // EffectControl에서 매개변수로 넘기는 게 이상적.
        // SharedData를 받아오는 게 좋을듯

        // 로드는 어떻게 하지?
        // 일단 모든 이펙트를 로드하고 구현하고,
        // 나중에 게임이 좀 만들어지면
        // 현재 게임에서 사용되는 캐릭터들의 이펙트 + 공용 이펙트만 메모리에 로드
    }

    public void StartExecuteEffect(string effectName) {
        
    }
}
