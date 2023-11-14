using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;
using System.Text.RegularExpressions;
using Moves;

public class MoveDataParser {
    private static readonly char[] BracketChar = { '(', ')' };
    private static readonly string RegexStr = "\\([^)]*\\)";
    private Regex _effectVariableRegex;

    public MoveDataParser() {
        _effectVariableRegex = new Regex(RegexStr);
    }

    public void Parse(string jsonText) {
        JSONObject moveJsonObject = new JSONObject(jsonText);

        int numOfMoves = moveJsonObject.list.Count;
        for (int i = 0; i < numOfMoves; ++i) {
            JSONObject jsonObj = moveJsonObject.list[i];
            
            MoveInfo moveInfo = new MoveInfo() {
                moveID = "asdasd"
            };
        }
    }
}
