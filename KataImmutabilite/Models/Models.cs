using System.Collections.Immutable;

namespace KataImmutabilite.Models;

public enum QuestionType
{
    Geography,
    Entertainment,
    History,
    ArtAndLiterature,
    ScienceAndTechnology,
    SportsAndLeisure
}

public record Player(string Name, ImmutableHashSet<QuestionType> Tokens, int BoardIndex);

public record BoardTile(ImmutableList<QuestionType> QuestionTypes, bool IsBlocking);

public record Board(ImmutableList<Player> Players, ImmutableList<BoardTile> Tiles);

public record Question(QuestionType Type, string Text, string Answer);

public record GameEngine
(
    ImmutableDictionary<QuestionType, ImmutableList<Question>> Questions,
    Board Board, 
    int CurrentPlayerIndex,
    bool IsFinished
);