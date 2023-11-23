using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public abstract class MoveBase {
        protected List<ExecutionArea> _executionAreas;
        public MoveInfo Info {
            get;
            private set;
        }
        public MoveBase(MoveInfo info) {
            Info = info;
            _executionAreas = new List<ExecutionArea>();
        }
        public abstract void InitializeExecutionArea();
        public List<ExecutionArea> GetExecutionArea() {
            return _executionAreas;
        }
        public abstract UniTask Execute(TeamColor caster, int areaIndex, SharedData sharedData);
    }
}