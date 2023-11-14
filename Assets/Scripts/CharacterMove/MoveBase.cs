using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves {
    [System.Serializable]
    public class MoveInfo {
        public string moveID;
        public string moveName;
        public int cost;
        public int value;
        public string description;
        public bool isRelativeForCharacter;
        public string[] commandName;
        public Dictionary<string, string[]> variables;
    }

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
        public List<ExecutionArea> GetExecutionArea(IsometricGrid<GameObject> grid) {
            return _executionAreas;
        }
    }
}