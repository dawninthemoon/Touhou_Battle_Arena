using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves {
    public class Move_Movement : MoveBase {
        public Move_Movement(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            _executionAreas.Add(new ExecutionArea());
            _executionAreas[0].Add(new Rowcol(-1, 0)); // Up

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[1].Add(new Rowcol(0, 1)); // Right

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[2].Add(new Rowcol(0, 1)); // Down

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[3].Add(new Rowcol(-1, 0)); // Left
        }

        public void Execute(int areaIndex) {
            ExecutionArea area = _executionAreas[areaIndex];
            foreach (Rowcol rc in area.Rowcols) {
                
            }
        }
    }
}