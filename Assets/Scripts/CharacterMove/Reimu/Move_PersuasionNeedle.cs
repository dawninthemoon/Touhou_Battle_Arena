using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_PersuasionNeedle : MoveBase {
        private static readonly string DamageVariableKey = "d1";
        private EffectConfig _cachedEffectConfig;

        public Move_PersuasionNeedle(MoveInfo info) : base(info) {
            InitializeExecutionArea();
            _cachedEffectConfig = new EffectConfig();
        }

        public override void InitializeExecutionArea() {
            int maxLen = 6;
            for (int directionIdx = 0; directionIdx < Rowcol.Directions.Length; ++directionIdx) {
                _executionAreas.Add(new ExecutionArea());
                for (int len = 0; len < maxLen; ++len) {
                    Rowcol rc = Rowcol.Zero + Rowcol.Directions[directionIdx] * len;
                    _executionAreas[directionIdx].Add(rc);
                }
            }
        }

        protected override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            int damage = int.Parse(Info.variables[DamageVariableKey][0]);

            Rowcol target = Rowcol.Zero;
            _cachedEffectConfig.AreaIndex = areaIndex;
            foreach (Rowcol rc in area.Rowcols) {
                target = origin + rc;

                sharedData.GridCtrl.HighlightTile(target);
                sharedData.GridCtrl.HighlightObjectExcept(caster, target);

                if (AttackAt(caster, target, damage, sharedData.GridCtrl, sharedData.CharcaterCtrl)) {
                    break;
                } 
            }

            Rowcol finalTarget = target;
            finalTarget.Clamp(6, 6);
            Vector3 pos = sharedData.GridCtrl.RowcolToPoint(target);
            EffectTarget effectTarget = new EffectTarget(null, pos);
            _cachedEffectConfig.Add(effectTarget);

            PlayerCharacter p = sharedData.CharcaterCtrl.GetCharacterByColor(caster);
            await sharedData.EffectCtrl.StartExecuteEffect(_effectName, p, _cachedEffectConfig, sharedData);

            foreach (Rowcol rc in area.Rowcols) {
                target = origin + rc;
                sharedData.GridCtrl.RemoveHighlightTile(target);
                sharedData.GridCtrl.RemoveHighlightObjectExcept(caster, target);
            }
            _cachedEffectConfig.Reset();
        }
    }
}
