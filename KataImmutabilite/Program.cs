// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using KataImmutabilite.Application;
using KataImmutabilite.IO;
using KataImmutabilite.Models;

string[] playerList = ["Theo", "Julian", "Sebastien"];
var engine = IO.InitializeGameEngine(playerList.ToImmutableHashSet());

var askQuestion = (Question question) =>
{
    Console.WriteLine($"{question.Type}: {question.Text}");
    var answer = Console.ReadLine();
    Console.WriteLine($"Answer was {question.Answer} and you answered {answer} is it correct ? (Y/N)");
    bool isValid = Console.ReadKey().Key == ConsoleKey.Y;
    return isValid;
};

var endState = engine.RunGame(Console.WriteLine, askQuestion);
Console.WriteLine($"Player {endState.GetCurrentPlayer().Name} won the game");