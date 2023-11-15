using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Moves;

namespace Commands {
    public interface IBattleCommand {
        UniTaskVoid Execute(ExecutionArea executionArea, string[] variables);
    }

    public class BattleCommand {
        public class Damage : IBattleCommand {
            public async UniTaskVoid Execute(ExecutionArea executionArea, string[] variables) {
                await UniTask.Yield();
            }
        }
    }
}
