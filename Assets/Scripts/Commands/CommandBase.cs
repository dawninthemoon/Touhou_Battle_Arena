using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Moves;

namespace Commands {
    public interface IBattleAction {
        UniTaskVoid Execute();
    }

    public struct DamageAction : IBattleAction {
        private Rowcol _targetRowcol;
        private int _damage;

        public DamageAction(Rowcol target, int damage) {
            _targetRowcol = target;
            _damage = damage;
        }

        public async UniTaskVoid Execute() {
            
            await UniTask.Yield();
        }
    }
}
