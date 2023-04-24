class Program
{
    static void Main()
    {
        string[] words =
        {
            "sweden", "malmo", "umeaa"
        };
        
        while (true)
        {
            Play(words);

            var userChoice = ReadInput("Do you want to continue (y/n)? ", 'y', 'n');
            if(userChoice == 'n')
                break;
        }

        Console.WriteLine();
        Console.WriteLine("Goodbye boring soul!");
    }

    static void Play(string[] words)
    {
        var word = GetRandomWord(words);
        var mask = CreateMask(word);
        var attemptsLeft = 6;
        var usedLetters = string.Empty; 
        
        while (true)
        {
            ShowUi(mask, usedLetters, attemptsLeft);
            
            // check win condition
            if (!mask.Contains('_'))
            {
                Console.WriteLine("You rock!");
                Console.WriteLine();
                return;
            }
            
            // check lose condition
            if (attemptsLeft == 0)
            {
                Console.WriteLine("You ran out of attempts");
                Console.WriteLine($"The word was {word}");
                Console.WriteLine();
                return;
            }

            var guessedLetter = ReadInput("Your guess: ");
            
            if(usedLetters.Contains(guessedLetter))
                continue;

            usedLetters += $"{guessedLetter} ";

            if (word.Contains(guessedLetter))
                UpdateMask(mask, word, guessedLetter);
            else
                attemptsLeft--;
        }
    }

    static char ReadInput(string message, params char[] acceptableLetters) //params keywords https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params
    {
        Console.Write(message);
        return ReadLetter(acceptableLetters);
    }
    
    static char ReadLetter(params char[] acceptableLetters)
    {
        char letter = default;

        while (true)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter && letter != default)
            {
                Console.WriteLine();
                return letter;
            }
            
            if (key.Key == ConsoleKey.Backspace && letter != default)
            {
                letter = default;
                Console.Write("\b \b");
                continue;
            }
            
            if(!char.IsLetter(key.KeyChar) || letter != default)
                continue;

            if (acceptableLetters.Length == 0 || acceptableLetters.Contains(key.KeyChar))
            {
                letter = char.ToLower(key.KeyChar);
                Console.Write(letter);
            }
        }
    }
    
    static void ShowUi(char[] mask, string usedLetters, int attemptsLeft)
    {
        Console.Clear();
        Console.WriteLine(string.Join(' ', mask));
        Console.WriteLine();
        Console.WriteLine($" Used letters: {usedLetters}");
        Console.WriteLine($"Attempts left: {attemptsLeft}");
        Console.WriteLine();
    }
    
    private static char[] CreateMask(string word)
    {
        return new string('_', word.Length).ToCharArray(); // treat string like a object 
    }

    static void UpdateMask(char[] mask, string word, char guessedLetter)
    {
        for (var i = 0; i < word.Length; i++)
            if (word[i] == guessedLetter)
                mask[i] = guessedLetter;
    }
    
    static string GetRandomWord(string[] words) 
    {
        Random random = new Random();
        return words[random.Next(words.Length)];
    }
}