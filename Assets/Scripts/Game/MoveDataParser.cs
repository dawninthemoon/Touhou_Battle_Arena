using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;
using System.Text.RegularExpressions;
using Moves;

public class MoveDataParser {
    private static readonly string IDColumn = "MoveID";
    private static readonly string NameColumn = "MoveName";
    private static readonly string CostColumn = "Cost";
    private static readonly string ValueColumn = "Value";
    private static readonly string DescriptionColumn = "Description";
    private static readonly string IsRelativeColumn = "IsRelativeForCharacter";
    private static readonly string CommandsColumn = "Commands";
    private static readonly string ProductionNameColumn = "ProductionName";

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

            string commands = jsonObj.GetField(CommandsColumn).stringValue;
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
                moveID = jsonObj.GetField(IDColumn).stringValue,
                moveName = jsonObj.GetField(NameColumn).stringValue,
                cost = jsonObj.GetField(CostColumn).intValue,
                value = jsonObj.GetField(ValueColumn).intValue,
                description = jsonObj.GetField(DescriptionColumn).stringValue,
                isRelativeForCharacter = jsonObj.GetField(IsRelativeColumn).boolValue,
                commandsInfo = commandsDictionary,
                productionName = jsonObj.GetField(ProductionNameColumn).stringValue
            };

            moves[i] = moveInfo;
        }
        return moves;
    }
}