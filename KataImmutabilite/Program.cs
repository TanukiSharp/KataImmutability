﻿// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using KataImmutabilite.IO;
using KataImmutabilite.Models;

string[] playerList = ["Theo", "Julian", "Sebastien"];
var engine = IO.InitializeGameEngine(playerList.ToImmutableHashSet());

var askQuestion = (Question question) =>
{
    Console.WriteLine(question.Text);
    var answer = Console.ReadLine();
    
    return false;
};

var endState = engine.RunGame(Console.WriteLine, askQuestion);
// Pas challenge:
// poser la question
// vérifier la réponse
// Si bonne réponse rejouer
// Challenge:
// poser 2 questions
// vérifier les réponses
// si bonne réponse, ajouter le token
// Grand challenge
// poser 6 question (1 chaque type)
// si bonne réponse => return engine with currentPlayer.AddToken(QuestionType)
// passer au joueur suivant
        
// condition victoire