using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;
using System.Text.RegularExpressions;
using Moves;

public class MoveDataParser {
    private static readonly string MoveIDColumn = "MoveID";
    private static readonly string CharacterKeyColumn = "CharacterKey";
    private static readonly string NameKorColumn = "MoveNameKor";
    private static readonly string CostColumn = "Cost";
    private static readonly string ValueColumn = "Value";
    private static readonly string SpellSpeedColumn = "SpellSpeed";
    private static readonly string DescriptionColumn = "Description";
    private static readonly string IsRelativeColumn = "IsRelativeForCharacter";
    private static readonly string AnimTriggerColumn = "AnimTriggerName";
    private static readonly string VariablesColumn = "Variables";
    private static readonly string ButtonIndexColumn = "ButtonIndex";

    private static readonly char ColonChar = ':';
    private static readonly char CommaChar = ',';
    private static readonly string BlankStr = "";
    private static readonly char[] BracketChar = { '(', ')' };
    private static readonly string RegexStr = "\\([^)]*\\)";
    private Regex _commandVariableRegex;

    public MoveDataParser() {
        _commandVariableRegex = new Regex(RegexStr);
    }

    public MoveInfo[] Parse(string jsonText) {
        JSONObject moveJsonObject = new JSONObject(jsonText);

        int numOfMoves = moveJsonObject.list.Count;
        MoveInfo[] moves = new MoveInfo[numOfMoves];
        for (int i = 0; i < numOfMoves; ++i) {
            JSONObject jsonObj = moveJsonObject.list[i];

            string commands = jsonObj.GetField(VariablesColumn).stringValue;
            Dictionary<string, string[]> commandsDictionary = new Dictionary<string, string[]>();
            if (commands != null) {
                string[] splitedCommands = commands.Split(ColonChar);
                foreach (string splitedCommand in splitedCommands) {
                    string commandName = _commandVariableRegex.Replace(splitedCommand, BlankStr);
                    string variableStr = _commandVariableRegex.Match(splitedCommand).ToString();
                    variableStr = variableStr.Trim(BracketChar);
                    
                    string[] variables = variableStr.Split(CommaChar);
                    commandsDictionary.Add(commandName, variables);
                }
            }
            
            MoveInfo moveInfo = new MoveInfo() {
                moveID = jsonObj.GetField(MoveIDColumn).stringValue,
                characterKey = jsonObj.GetField(CharacterKeyColumn).stringValue,
                moveName = jsonObj.GetField(NameKorColumn).stringValue,
                cost = jsonObj.GetField(CostColumn).intValue,
                value = jsonObj.GetField(ValueColumn).intValue,
                spellSpeed = jsonObj.GetField(SpellSpeedColumn).intValue,
                description = jsonObj.GetField(DescriptionColumn).stringValue,
                isRelativeForCharacter = jsonObj.GetField(IsRelativeColumn).boolValue,
                animTriggerName = jsonObj.GetField(AnimTriggerColumn).stringValue,
                variables = commandsDictionary,
                buttonIndex = jsonObj.GetField(ButtonIndexColumn).intValue
            };

            moves[i] = moveInfo;
        }
        return moves;
    }
}
