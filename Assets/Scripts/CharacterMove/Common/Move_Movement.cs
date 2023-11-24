using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Moves {
    public class Move_Movement : MoveBase {
        public static readonly string MoveID = "Move_Movement";
        public Move_Movement(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            _executionAreas.Add(new ExecutionArea());
            _executionAreas[0].Add(new Rowcol(-1, 0)); // Up

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[1].Add(new Rowcol(0, 1)); // Right

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[2].Add(new Rowcol(1, 0)); // Down

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[3].Add(new Rowcol(0, -1)); // Left
        }

        public override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            foreach (Rowcol rc in area.Rowcols) {
                sharedData.CharcaterCtrl.MoveCharacter(caster, rc);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        }
    }
}
