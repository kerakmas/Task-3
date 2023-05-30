using System;
using System.Security.Cryptography;

public class RockPaperScissorsGame
{
    private static readonly string[] Moves = { "rock", "paper", "scissors", "lizard", "Spock" };

    public static void Main(string[] args)
    {
        if (args.Length < 3 || args.Length % 2 == 0)
        {
            Console.WriteLine("Incorrect number of arguments. Please provide an odd number of non-repeating strings.");
            Console.WriteLine("Example: dotnet run rock paper scissors lizard Spock");
            return;
        }

        Console.WriteLine("HMAC key: " + GenerateKey());
        Console.WriteLine("Available moves:");
        for (int i = 0; i < args.Length; i++)
        {
            Console.WriteLine((i + 1) + " - " + args[i]);
        }
        Console.WriteLine("0 - exit");
        Console.WriteLine("? - help");

        int userMove = GetUserMove(args.Length);
        if (userMove == 0)
        {
            Console.WriteLine("Exiting the game.");
            return;
        }

        string userMoveString = args[userMove - 1];
        string computerMove = GenerateComputerMove(args.Length);
        Console.WriteLine("Your move: " + userMoveString);
        Console.WriteLine("Computer move: " + computerMove);

        int result = DetermineWinner(userMove, args.Length);
        if (result == 0)
        {
            Console.WriteLine("It's a draw!");
        }
        else if (result > 0)
        {
            Console.WriteLine("You win!");
        }
        else
        {
            Console.WriteLine("Computer wins!");
        }

        Console.WriteLine("HMAC key: " + GenerateKey());
    }

    private static string GenerateKey()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] keyBytes = new byte[32];
            rng.GetBytes(keyBytes);
            return BitConverter.ToString(keyBytes).Replace("-", "");
        }
    }

    private static int GetUserMove(int numMoves)
    {
        int userMove;
        do
        {
            Console.Write("Enter your move: ");
            string input = Console.ReadLine();
            if (input == "?")
            {
                DisplayHelpTable(numMoves);
                continue;
            }
            if (int.TryParse(input, out userMove) && userMove >= 0 && userMove <= numMoves)
            {
                return userMove;
            }
            Console.WriteLine("Invalid input. Please try again.");
        } while (true);
    }

    private static void DisplayHelpTable(int numMoves)
    {
        Console.WriteLine("Move | Win  | Lose | Draw");
        Console.WriteLine("--------------------------");
        for (int i = 1; i <= numMoves; i++)
        {
            string move = Moves[i - 1];
            string win = Moves[(i % numMoves)];
            string lose = Moves[(i + 1) % numMoves];
            Console.WriteLine($"{move,-4} | {win,-4} | {lose,-4} | Draw");
        }
    }

    private static string GenerateComputerMove(int numMoves)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] randomNumber = new byte[1];
            rng.GetBytes(randomNumber);
            return Moves[randomNumber[0] % numMoves];
        }
    }

    private static int DetermineWinner(int userMove, int numMoves)
    {
        int halfMoves = numMoves / 2;
        int computerMove = (userMove + halfMoves) % numMoves;
        if (userMove == computerMove)
        {
            return 0; // Draw
        }
        else if ((userMove > computerMove && userMove - computerMove <= halfMoves) || (userMove < computerMove && computerMove - userMove > halfMoves))
        {
            return 1; // User wins
        }
        else
        {
            return -1; // Computer wins
        }
    }
}
