using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class GameTest : MonoBehaviour {
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private CharacterTest _reimuPrefab, _marisaPrefab;
    private PhotonView _photonView;
    private GridControl _gridControl;
    public static int TurnPlayer;
    private Dictionary<KeyCode, Rowcol> _keyDirectionDictionary;
    private CharacterTest _myCharacter;
    private int _playerID;

    private void Awake() {
        _photonView = GetComponent<PhotonView>();
        _gridControl = GetComponent<GridControl>();

        _playerID = 0;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ID")) {
            _playerID = (int)PhotonNetwork.LocalPlayer.CustomProperties["ID"];
            if (!_photonView.IsMine) {
                _playerID = 1 - _playerID;
            }
        }

        if (_photonView.IsMine) {
            _keyDirectionDictionary = new Dictionary<KeyCode, Rowcol>();

            _keyDirectionDictionary.Add(KeyCode.W, new Rowcol(-1, 0));
            _keyDirectionDictionary.Add(KeyCode.A, new Rowcol(0, -1));
            _keyDirectionDictionary.Add(KeyCode.S, new Rowcol(1, 0));
            _keyDirectionDictionary.Add(KeyCode.D, new Rowcol(0, 1));
        }
    }

    private void Start() {
        if (_photonView.IsMine) {
            _gridControl.InitializeGrid(_tilePrefab);
            GameObject.Find("Sibal").GetComponent<TMP_Text>().text = _playerID.ToString();
        }

        CharacterTest prefab = (_playerID == 0) ? _reimuPrefab : _marisaPrefab;
        _myCharacter = Instantiate(prefab);
        Rowcol coord = new Rowcol(1, 0);
        if (!_photonView.IsMine) {
            _myCharacter.GetComponent<SpriteRenderer>().flipX = true;
            coord = new Rowcol(1, 3);
        }
        _myCharacter.MoveImmediate(_gridControl.RowcolToPosition(coord), coord);
    }
    
    private void Update() {
        if (_photonView.IsMine && (TurnPlayer == _playerID) && Input.anyKeyDown) {
            if (_keyDirectionDictionary.TryGetValue(RieslingUtils.ExKey.GetPressedKey(), out Rowcol rc)) {
                Rowcol next = _myCharacter.Curr + rc;
                if (_gridControl.IsValidRowcol(next)) {
                    _photonView.RPC("MoveMyCharacter", RpcTarget.All, next.row, next.column);
                }
            }
        }
    }

    [PunRPC]
    private void MoveMyCharacter(int row, int column) {
        if (!_photonView.IsMine) {
            column = 3 - column;
        }
        _myCharacter.MoveImmediate(_gridControl.RowcolToPosition(row, column), new Rowcol(row, column));
        TurnPlayer = (TurnPlayer + 1) % 2;
    }
}
