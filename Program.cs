using Spectre.Console;
using System.Text.Json;

var words = GetWords();

if (words is null)
{
	throw new InvalidOperationException("Could not deserialize words.");
}

var randomWord = GetRandomWord();
var guesses = 0;

while (guesses < 6)
{
	var word = GetWordFromInput();

	if (word.Length != 5) HandleInvalidWordLength(word);
	if (words.Contains(word))
	{
		var isWon = HandleWordCheck(word);
		if (isWon is not null) break;
		return;
	}

	AnsiConsole.Markup("[red]That word is not in the dictionary.[/]");
	AnsiConsole.WriteLine();
}

string[]? GetWords()
{
	var rawJson = File.ReadAllText("words.json");
	var words = JsonSerializer.Deserialize<string[]>(rawJson);
	return words;
}

string GetRandomWord()
{
	var random = new Random();
	var randomWord = words[random.Next(0, words.Length)];
	return randomWord;
}

string GetWordFromInput()
{
	return AnsiConsole.Ask<string>("Enter a [green]five[/] letter word?").ToUpper();
}

void HandleInvalidWordLength(string word)
{
	AnsiConsole.Markup($"[red]Error[/] [yellow]{word}[/] is not a 5 letter word.");
	AnsiConsole.WriteLine();
}

bool? HandleWordCheck(string word)
{
	var wordMatches = 0;
	for (var i = 0; i < randomWord.Length; i++)
	{
		wordMatches = HandleCharCheck(word, randomWord, wordMatches, i);
	}
	AnsiConsole.WriteLine();
	if (wordMatches == randomWord.Length)
	{
		AnsiConsole.Markup("[green]You win![/]");
		return true;
	};
	if (guesses == 5 && wordMatches != randomWord.Length)
	{
		AnsiConsole.Markup("[red]You lose![/]");
		return false;
	}

	AnsiConsole.WriteLine();
	guesses++;
	return null;
}

int HandleCharCheck(string word, string randomWord, int wordMatches, int i)
{
	if (randomWord[i] == word[i])
	{
		AnsiConsole.Markup($"[green]{word[i]}[/]");
		wordMatches++;
	}
	else if (randomWord.Contains(word[i]))
	{
		AnsiConsole.Markup($"[yellow]{word[i]}[/]");
	}
	else
	{
		AnsiConsole.Markup($"{word[i]}");
	}

	return wordMatches;
}