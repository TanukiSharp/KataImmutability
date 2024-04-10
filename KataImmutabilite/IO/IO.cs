using System.Collections.Immutable;
using KataImmutabilite.Application;
using KataImmutabilite.Models;

namespace KataImmutabilite.IO;

public static class IO
{
    public static int ThrowDice6()
    {
        return Random.Shared.Next(1, 7);
    }
    
    private static ImmutableDictionary<QuestionType, ImmutableList<Question>> InitializeQuestions()
        => ImmutableDictionary.CreateRange(
        new[]
            {
                KeyValuePair.Create(QuestionType.Geography, ImmutableList.Create(
                    new Question(QuestionType.Geography, "What is the capital of France?", "Paris"),
                    new Question(QuestionType.Geography, "What is the capital of Spain?", "Madrid")
                )),
                KeyValuePair.Create(QuestionType.Entertainment, ImmutableList.Create(
                    new Question(QuestionType.Entertainment, "Who directed the movie 'Pulp Fiction'?", "Quentin Tarantino"),
                    new Question(QuestionType.Entertainment,
                        "Who played the character of Harry Potter in the movie series?", "Daniel Radcliffe")
                )),
                KeyValuePair.Create(QuestionType.History, ImmutableList.Create(
                    new Question(QuestionType.History, "In which year did the Titanic sink?", "1912"),
                    new Question(QuestionType.History, "Who was the first president of the United States?",
                        "George Washington")
                )),
                KeyValuePair.Create(QuestionType.ArtAndLiterature, ImmutableList.Create(
                    new Question(QuestionType.ArtAndLiterature, "Who painted the Mona Lisa?", "Leonardo da Vinci"),
                    new Question(QuestionType.ArtAndLiterature, "Who wrote the novel 'Moby Dick'?", "Herman Melville")
                )),
                KeyValuePair.Create(QuestionType.ScienceAndTechnology, ImmutableList.Create(
                    new Question(QuestionType.ScienceAndTechnology, "What is the most famous Einstein equation?", "e = mc^2"),
                        new Question(QuestionType.ScienceAndTechnology, "When did the Apollo mission land on the moon for the first time?", "21 july 1969"),
                        new Question(QuestionType.ScienceAndTechnology, "What is the Apollo mission number that landed on the moon for the first time?", "Apollo 11")
                )),
                KeyValuePair.Create(QuestionType.SportsAndLeisure, ImmutableList.Create(
                    new Question(QuestionType.SportsAndLeisure, "What is Usain Bolt's current time for the 100m sprint?", "9s58"),
                    new Question(QuestionType.SportsAndLeisure, "Who won the fifa world cup in 1998?", "France")
                ))
            }
        );
   
    public static ImmutableList<QuestionType> ShuffleQuestionTypes()
    {
        return Enum.GetValues<QuestionType>().OrderBy(_ => Random.Shared.Next()).ToImmutableList();
    }

    public static ImmutableList<BoardTile> InitializeBoardTiles()
        => ShuffleQuestionTypes().SelectMany(
            challengeType => ShuffleQuestionTypes()
                .Select(
                    qt => new BoardTile(ImmutableList.Create(qt), false)
                )
                .Where(bt => bt.QuestionTypes.All(qt => qt != challengeType))
                .Append(new BoardTile(ImmutableList.Create(challengeType, challengeType), true))
                .ToImmutableList()
            )
            .Prepend(new BoardTile(ImmutableList<QuestionType>.Empty, false))
            .Append(new BoardTile(ShuffleQuestionTypes(), true)).ToImmutableList();

    public static Board InitializeBoard(ImmutableHashSet<string> playerNames)
        => new Board(
            playerNames.Select(name => new Player(name, ImmutableHashSet<QuestionType>.Empty, 0)).ToImmutableList(),
            InitializeBoardTiles()
        );

    public static GameEngine InitializeGameEngine(ImmutableHashSet<string> playerNames)
        => new GameEngine(InitializeQuestions(), InitializeBoard(playerNames), 0, false);
    
    public static Question GetQuestion(this GameEngine engine, QuestionType type)
    {
        var randomNumber = Random.Shared.Next(engine.Questions[type].Count);
        return engine.Questions[type][randomNumber];
    }
    
    public static GameEngine RunGame(this GameEngine engine, Action<string> print, Func<Question, bool> askQuestionFunc)
    {
        if (engine.IsFinished)
        {
            return engine;
        }

        var diceThrow = ThrowDice6();
        print($"Player {engine.GetCurrentPlayer().Name} rolled a {diceThrow}");
        var lastIndex = engine.GetCurrentPlayer().BoardIndex;
        var movedState = engine.MoveCurrentPlayer(diceThrow);
        var newIndex = engine.GetCurrentPlayer().BoardIndex;
        print($"Player {engine.GetCurrentPlayer().Name} moved from {lastIndex} to {newIndex}");
        var questionTypes = engine.GetCurrentTile().QuestionTypes;
        var hasFailed = questionTypes.Any(qt => !askQuestionFunc(engine.GetQuestion(qt)));
        var questionState = hasFailed ? movedState.DenyTile() : movedState.GrantTile();
        return RunGame(questionState, print, askQuestionFunc);
    }
}