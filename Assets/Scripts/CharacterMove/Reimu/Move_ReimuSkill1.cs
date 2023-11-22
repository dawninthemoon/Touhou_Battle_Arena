using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves {
    public class Move_ReimuSkill1 : MoveBase {
        public Move_ReimuSkill1(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            ExecutionArea area = new ExecutionArea();
            area.Add(Rowcol.Zero);
            area.Add(new Rowcol(1, 1));
            area.Add(new Rowcol(-1, 1));
            area.Add(new Rowcol(1, -1));
            area.Add(new Rowcol(-1, -1));

            _executionAreas.Add(area);

        }

        public override void Execute(TeamColor caster, int areaIndex, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            foreach (Rowcol rc in area.Rowcols) {
            }
        }
    }
}
