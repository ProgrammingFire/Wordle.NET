using Spectre.Console;
using System.Text.Json;

var rawJson = File.ReadAllText("words.json");
var words = JsonSerializer.Deserialize<string[]>(rawJson);

if (words is null)
{
	throw new InvalidOperationException("Could not deserialize words.");
}

var random = new Random();
var randomWord = words[random.Next(0, words.Length)];
var guesses = 0;

while (guesses < 6)
{
	var word = AnsiConsole.Ask<string>("Enter a [green]five[/] letter word?").ToUpper();

	if (word.Length != 5)
	{
		AnsiConsole.Markup($"[red]Error[/] [yellow]{word}[/] is not a 5 letter word.");
		AnsiConsole.WriteLine();
		continue;
	}
	if (words.Contains(word))
	{
		var wordMatches = 0;
		for (var i = 0; i < randomWord.Length; i++)
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
		}
		AnsiConsole.WriteLine();
		if (wordMatches == randomWord.Length)
		{
			AnsiConsole.Markup("[green]You win![/]");
			break;
		}
		if (guesses == 5 && wordMatches != randomWord.Length)
		{
			AnsiConsole.Markup("[red]You lose![/]");
			break;
		}
		AnsiConsole.WriteLine();
		guesses++;
	}
	else
	{
		AnsiConsole.Markup("[red]That word is not in the dictionary.[/]");
		AnsiConsole.WriteLine();
	}
}
