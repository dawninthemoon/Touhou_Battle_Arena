using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Photon.Pun;

public class GameMain : MonoBehaviour {

    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;
    [SerializeField] private GridSelector _gridSelector;
    [SerializeField] private MoveButtonControl _moveButtonControl;
    private PlayerMoveReceiver _playerMoveReceiver;

    private void Awake() {
        var reciver = PhotonNetwork.Instantiate("PlayerMoveReceiver", Vector3.zero, Quaternion.identity);
        _playerMoveReceiver = reciver.GetComponent<PlayerMoveReceiver>();
    }

    private async UniTaskVoid Start() {
        _moveButtonControl.SetButtonInteraction(false);

        if (PlayerMoveReceiver.MyColor == TeamColor.BLUE) {
            await SelectInitialRowcol();
            await WaitForOpponentSelecting();
        }
        else {
            await WaitForOpponentSelecting();
            await SelectInitialRowcol();
        }
        
        _moveButtonControl.SetButtonInteraction(true);
    }

    private async UniTask SelectInitialRowcol() {
        Rowcol initialRowcol = await _gridSelector.SelectGrid();
        _characterControl.PlaceCharacter(PlayerMoveReceiver.MyColor, initialRowcol);
        _playerMoveReceiver.InitialSelectComplete(PlayerMoveReceiver.MyColor, initialRowcol);
    }

    private async UniTask<Rowcol> WaitForOpponentSelecting() {
        await UniTask.WaitUntil(() => _playerMoveReceiver.CheckInitializeCompleted(PlayerMoveReceiver.OpponentColor));
        
        Rowcol initialRowcol = _playerMoveReceiver.GetInitialRowcol(PlayerMoveReceiver.OpponentColor);
        _characterControl.PlaceCharacter(PlayerMoveReceiver.OpponentColor, initialRowcol);

        return initialRowcol;
    }
}
