using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves {
    public class Move_ReimuSkill2 : MoveBase {
        public Move_ReimuSkill2(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            int maxLen = 6;
            for (int directionIdx = 0; directionIdx < Rowcol.directions.Length; ++directionIdx) {
                _executionAreas.Add(new ExecutionArea());
                for (int len = 0; len < maxLen; ++len) {
                    Rowcol rc = Rowcol.Zero + Rowcol.directions[directionIdx] * len;
                    _executionAreas[directionIdx].Add(rc);
                }
            }
        }

        public override void Execute(TeamColor caster, int areaIndex, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            foreach (Rowcol rc in area.Rowcols) {
            }
        }
    }
}
