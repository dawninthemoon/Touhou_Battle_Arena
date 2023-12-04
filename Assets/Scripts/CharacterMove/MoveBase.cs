using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
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
        public List<ExecutionArea> GetExecutionArea() {
            return _executionAreas;
        }
        public abstract UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData);
    
        protected bool AttackAt(TeamColor color, Rowcol target, int damage, GridControl gridControl, CharacterControl characterControl) {
            bool enemyHit = false;

            GridObject obj = gridControl.GetObject(TeamColor.NONE, target);
            GridObject obj2 = gridControl.GetObject(ExTeamColor.GetOpponentColor(color), target);

            PlayerCharacter character = characterControl.GetCharacterByColor(color);
            character.OnCharacterAttack();

            obj?.ReceiveDamage(damage);
            if (obj2 != null) {
                obj2.ReceiveDamage(damage);
                characterControl.GainEnergy(Info.cost / 2, color);
                enemyHit = true;
            }
            return enemyHit;
        }

        public bool AttackAtWithTriggerName(TeamColor color, Rowcol target, int damage, GridControl gridControl, CharacterControl characterControl, string triggerName) {
            bool enemyHit = false;
            GridObject obj = gridControl.GetObject(TeamColor.NONE, target);
            GridObject obj2 = gridControl.GetObject(ExTeamColor.GetOpponentColor(color), target);

            PlayerCharacter character = characterControl.GetCharacterByColor(color);
            character.SetTrigger(triggerName);

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