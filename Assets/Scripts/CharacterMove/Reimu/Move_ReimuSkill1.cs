using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves {
    public class Move_ReimuSkill1 : MoveBase {
        public Move_ReimuSkill1(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            
        }

        public override void Execute(TeamColor caster, int areaIndex, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            foreach (Rowcol rc in area.Rowcols) {
            }
        }
    }
}