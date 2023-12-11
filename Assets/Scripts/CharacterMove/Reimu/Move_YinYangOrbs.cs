using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_YinYangOrbs : MoveBase {
        private static readonly string DamageVariableKey = "d1";
        private static readonly string DamageVariable2Key = "d2";
        private EffectConfig _cachedEffectConfig;

        public Move_YinYangOrbs(MoveInfo info) : base(info) {
            InitializeExecutionArea();
            _cachedEffectConfig = new EffectConfig();
        }

        public override void InitializeExecutionArea() {
            ExecutionArea area = new ExecutionArea();
            area.Add(Rowcol.Zero);
            for (int directionIdx = 0; directionIdx < Rowcol.Directions.Length; ++directionIdx) {
                area.Add(Rowcol.Directions[directionIdx]);
            }

            _executionAreas.Add(area);
        }

        protected override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            int damage1 = int.Parse(Info.variables[DamageVariableKey][0]);
            int damage2 = int.Parse(Info.variables[DamageVariable2Key][0]);

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                int finalDamage = rc.Equals(Rowcol.Zero) ? damage2 : damage1;
                bool hit = AttackAt(
                    caster,
                    target, 
                    finalDamage, 
                    sharedData.GridCtrl, 
                    sharedData.CharcaterCtrl
                );
                PlayerCharacter obj = sharedData.GridCtrl.GetObject(caster.GetOpponent(), target) as PlayerCharacter;
                EffectTarget effectTarget = new EffectTarget(obj, sharedData.GridCtrl.RowcolToPoint(target));
                _cachedEffectConfig.Add(effectTarget);
                
                sharedData.GridCtrl.HighlightTile(target);
                sharedData.GridCtrl.HighlightObjectExcept(caster, target);
            }

            PlayerCharacter p = sharedData.CharcaterCtrl.GetCharacterByColor(caster);
            await sharedData.EffectCtrl.StartExecuteEffect(_effectName, p, _cachedEffectConfig, sharedData);

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                sharedData.GridCtrl.RemoveHighlightTile(target);
                sharedData.GridCtrl.RemoveHighlightObjectExcept(caster, target);
            }
        }
    }
}
