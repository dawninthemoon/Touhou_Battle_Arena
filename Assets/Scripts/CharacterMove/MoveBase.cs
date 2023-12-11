using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public abstract class MoveBase {
        protected List<ExecutionArea> _executionAreas;
        protected string _effectName;
        public MoveInfo Info {
            get;
            private set;
        }
        public MoveBase(MoveInfo info) {
            Info = info;
            _effectName = Info.moveID.Replace("Move", "Effect");
            _executionAreas = new List<ExecutionArea>();
        }
        public abstract void InitializeExecutionArea();
        public List<ExecutionArea> GetExecutionArea() {
            return _executionAreas;
        }
        public async UniTask StartExecute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            if (Info.animTriggerName != null) {
                PlayerCharacter character = sharedData.CharcaterCtrl.GetCharacterByColor(caster);
                character.SetAnimationTrigger(Info.animTriggerName);
            }
            await Execute(caster, areaIndex, origin, sharedData);
        }

        protected abstract UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData);
    
        protected bool AttackAt(TeamColor color, Rowcol target, int damage, GridControl gridControl, CharacterControl characterControl) {
            bool enemyHit = false;

            GridObject obj = gridControl.GetObject(TeamColor.NONE, target);
            GridObject obj2 = gridControl.GetObject(color.GetOpponent(), target);

            obj?.ReceiveDamage(damage);
            if (obj2 != null) {
                obj2.ReceiveDamage(damage);
                characterControl.GainEnergy(Info.cost / 2, color);
                enemyHit = true;
            }
            return enemyHit;
        }
    }
}