using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves {
    public class Move_ReimuBasicAttack : MoveBase {
        public Move_ReimuBasicAttack(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            _executionAreas.Add(new ExecutionArea());
            _executionAreas[0].Add(new Rowcol(1, 0));
            _executionAreas[0].Add(new Rowcol(1, 1));
            _executionAreas[0].Add(new Rowcol(0, 1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[1].Add(new Rowcol(-1, 0));
            _executionAreas[1].Add(new Rowcol(-1, 1));
            _executionAreas[1].Add(new Rowcol(0, 1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[2].Add(new Rowcol(-1, 0));
            _executionAreas[2].Add(new Rowcol(-1, -1));
            _executionAreas[2].Add(new Rowcol(0, -1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[3].Add(new Rowcol(1, 0));
            _executionAreas[3].Add(new Rowcol(1, -1));
            _executionAreas[3].Add(new Rowcol(0, -1)); 
        }

        public override void Execute(int areaIndex) {
            ExecutionArea area = _executionAreas[areaIndex];
            foreach (Rowcol rc in area.Rowcols) {
            }
        }
    }
}
