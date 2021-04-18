using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GuessingGame
{
    class Program
    {
        // ***************************************************************
        //
        // Title: Guessing Game
        // Application Type: Console
        // Description: CIT 110 - Capstone Project - Number Guessing Game
        // Author: Vorpagel, Ryan
        // Dated Created: 4/09/2021
        // Last Modified: 4/18/2021
        //
        // ***************************************************************

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;

            DisplayWelcomeScreen();
            DisplayMenuScreen();

            DisplayClosingScreen();

        }


        #region Application Setup

        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;


            (int minValue, int maxValue, int numGuesses) gameSettings;
            gameSettings.minValue = 0;
            gameSettings.maxValue = 0;
            gameSettings.numGuesses = 0;
            

            (int gamesWon, int gamesLost) gameStats;
            gameStats.gamesWon = 0;
            gameStats.gamesLost = 0;

            do
            {
                DisplayScreenHeader("\t\tMain Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Instructions for the Game");
                Console.WriteLine("\tb) Change Console Theme");
                Console.WriteLine("\tc) Change Game Settings");
                Console.WriteLine("\td) Play Game");
                Console.WriteLine("\te) View Statistics");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {


                    case "a":
                        GameInstructions();
                        break;

                    case "b":
                        DisplaySetTheme();
                        break;

                    case "c":
                        gameSettings = GameSettings();
                        break;

                    case "d":
                        gameStats = GuessingGame(gameSettings);
                        break;

                    case "e":
                        GameStatistics(gameStats);
                        break;

                    case "q":
                        
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }


        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t          Welcome to the Number Guessing Game!");
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t         Designed by: Ryan Vorpagel for CIT 110");
            Console.WriteLine();
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t      Press any key to continue.");
            Console.ReadKey();
        }

        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t" + headerText);
            Console.WriteLine();
        }

        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"\t\t\t\t     Press any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t     Thank you for stopping by!!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        #endregion

        #region GussingGame

        static (int gamesWon, int gamesLost) GuessingGame((int minValue, int maxValue, int numGuesses) gameSettings)
        {
           
                     
            int userGuess;
            bool play = true;
            string newGame;

            (int gamesWon, int gamesLost) gameStats;
            gameStats.gamesWon = 0;
            gameStats.gamesLost = 0;

            Console.Clear();
            DisplayScreenHeader("\t    The Guessing Game");

            //
            // do/while loop to run game until play is false
            //
            do
            {
                //
                // number of attemtpts and random number are reset for every new game
                //
                int randNum = RandomNumber(gameSettings.minValue, gameSettings.maxValue);
                int numAttempts = 0;

                //
                // loop used to continue the game after the users guess attempt
                //
                while (true)
                {
                    
                    ValidGuessInt("What number am I thinking of? : ", gameSettings.maxValue, gameSettings.minValue, out userGuess);
                    string highLow = HigherOrLower(userGuess, randNum);

                    //
                    // If users guess is correct we tell them and add to the games won
                    //
                    if (userGuess == randNum)
                    {
                        Console.WriteLine($"That is correct! My number was {randNum}!");
                        gameStats.gamesWon++;
                        break;
                    }
                    //
                    // If users guess is incorrect, print higher or lower via the HigherOrLower method
                    //
                    if (userGuess != randNum)
                    {
                        Console.WriteLine(highLow);
                        numAttempts++;
                    }
                    //
                    // If users reaches their maximum attempts we tell them and add to the gameslost count
                    //
                    if (numAttempts == gameSettings.numGuesses)
                    {
                        Console.WriteLine();
                        Console.WriteLine("*********************************************************");
                        Console.WriteLine($"*  I'm sorry, you have reached your limit of attempts!  *");
                        Console.WriteLine($"*     The random number I was thinking of is {randNum}         *");
                        Console.WriteLine("*********************************************************");
                        Console.WriteLine();
          
                        gameStats.gamesLost++;
                        break;
                    }

                    //
                    // Tell user how many attempts they have left
                    //
                    Console.WriteLine($"You have {gameSettings.numGuesses - numAttempts} attempts left. Try again!");
                    Console.WriteLine();
                }

                //
                // ask user if they want to play again. If yes, reset num attempts and random number and run again. If no, set play to false to break out of loop
                //
                Console.WriteLine();
                PlayAgain("Do you want to play again?", out newGame);

                    if (newGame == "yes")
                    {
                        play = true;
                        Console.Clear();
                    }
                    else 
                    {
                        play = false;
                    }

            } while (play == true);

            //
            // display the users statistics after they finish playing
            //
            GameStatistics(gameStats);

            
            return gameStats;
        }

        
        static int RandomNumber(int min, int max)
        {
            //
            // Generate a random number between the specified range
            // 

            Random randm = new Random();
            int randomNum = randm.Next(min,max);

            return randomNum;
        }
        static bool ValidGuessInt (string question, int maxVal, int minVal, out int validInt)
        {
            bool isValid = false;
            validInt = 0;

            Console.Write(question);

            //
            // Validate the integer entered by the user for their guess. Verify that its an integer and between the min and max range of the random number
            //

            while (!isValid)
            {
                if (!int.TryParse(Console.ReadLine(), out validInt))
                {
                    Console.WriteLine("I'm sorry, you must enter an integer! Please try again.");
                    Console.WriteLine();
                    Console.Write(question);
                }
                else if (validInt < minVal || validInt > maxVal)
                {
                    Console.WriteLine($"I'm sorry! You must enter an integer between {minVal} and {maxVal}. Please try again.");
                    Console.WriteLine();
                    Console.Write(question);
                }
                else
                {
                    isValid = true;
                }
            }
            return true;
        }

        static string HigherOrLower (int guess, int random)
        {
            string highLow;

            //
            // Determine if users guess is higher or lower than the random number and set high/low to user message
            //

            if (guess > random)
            {
                highLow = "The random number is lower than your guess";
            }
            else
            {
                highLow = "The random number is higher than your guess";
            }

            return highLow;
        }

        static void GameInstructions()
        {
            Console.Clear();
            DisplayScreenHeader("How to play the Guessing Game");

            Console.WriteLine("This is a simple game of guess the number!");
            Console.WriteLine();
            Console.WriteLine("First, adjust the settings to your liking in the 'Change Game Settings' page");
            Console.WriteLine("Next, select the 'Play Game' option from the menu");
            Console.WriteLine("Finally, once in the game, make your guess!");
            Console.WriteLine();
            Console.WriteLine("The game will generate a random number within the values that you supplied");
            Console.WriteLine("The game will then tell you if the number is higher, or lower, than your guess");
            Console.WriteLine("Keep guessing until you have reached your guess limit, or until you guess the number correctly!");
            Console.WriteLine("Once complete, you can view your results in the 'View Statistics' page.");
            Console.WriteLine();
            Console.WriteLine("Have fun and enjoy!");


            DisplayMenuPrompt("Main Menu");
        }

        static (int minValue, int maxValue, int numGuesses) GameSettings()
        {
            (int minValue, int maxValue, int numGuesses) gameSettings;
            gameSettings.minValue = 0;
            gameSettings.maxValue = 0;
            gameSettings.numGuesses = 0;
            bool isValid = false;
            bool isValid2 = false;
            

            //
            //Get game settings from user and validate them
            //
            Console.Clear();
            ValidIntSettings("Please enter the minimum value for the random number: ", out gameSettings.minValue);
            ValidIntSettings("Please enter the maximum value for the random number: ", out gameSettings.maxValue);
           
            //
            // If user tries to input a number lower than the minimum number throw an error and ask again
            //
            while (!isValid)
            {
                if (gameSettings.maxValue <= gameSettings.minValue)
                {
                    Console.WriteLine("I'm sorry, your maximum value must be greater than your minimum value");
                    Console.WriteLine();
                    ValidIntSettings("Please enter the maximum value for the random number: ", out gameSettings.maxValue);
                }
                else
                {
                    isValid = true;
                }
            }
            ValidIntSettings("Please enter the number of attempts you would like to have: ", out gameSettings.numGuesses);

            //
            // If user attempts to enter 0 or a negative number for attempts, throw error and ask again
            //
            while (!isValid2)
            {
                if (gameSettings.numGuesses <= 0)
                {
                    Console.WriteLine("I'm sorry, your number of attempts must be greater than 0");
                    Console.WriteLine();
                    ValidIntSettings("Please enter the number of attempts you would like to have: ", out gameSettings.numGuesses);
                }
                else
                {
                    isValid2 = true;
                }
            }
            

            //
            // Echo users values
            //

            Console.WriteLine();
            Console.WriteLine($"Minimum value of the random number: {gameSettings.minValue}");
            Console.WriteLine($"Maximum value of the random number: {gameSettings.maxValue}");
            Console.WriteLine($"Number of attempts before answer is given: {gameSettings.numGuesses}");
            


            DisplayMenuPrompt("Main Menu");

            return gameSettings;
        }

        static bool ValidIntSettings(string question, out int validInt)
        {
            bool isValid = false;
            validInt = 0;

            Console.Write(question);

            //
            // Loop to validate that input given for game settings is an integer and greater than 0
            //
            while (!isValid)
            {
                if (!int.TryParse(Console.ReadLine(), out validInt))
                {
                    Console.WriteLine("I'm sorry, you must enter an integer! Please try again.");
                    Console.WriteLine();
                    Console.Write(question);
                }
                else if (validInt < 0)
                {
                    Console.WriteLine("I'm sorry, you must enter an number greater than 0");
                    Console.WriteLine();
                    Console.Write(question);
                }
                else
                {
                    isValid = true;
                }
            }

            return true;
        }

        static void GameStatistics((int gamesWon, int gamesLost) gameStats)
        {
            Console.Clear();
            DisplayScreenHeader("         Guessing Game Results");

            //
            // Display user statistic for game
            //
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t******************************************");
            Console.WriteLine("\t\t\t\t\t*                                        *");
            Console.WriteLine($"\t\t\t\t\t*    You won {gameStats.gamesWon} games and lost {gameStats.gamesLost} games!   *");
            Console.WriteLine("\t\t\t\t\t*                                        *");
            Console.WriteLine("\t\t\t\t\t******************************************");

            DisplayMenuPrompt("Main Menu");
        }

        static string PlayAgain(string question, out string answer)
        {
            answer = null;
            bool isValid = false;

            //
            // loop to verify that user only enters yes or no for play again prompt
            //
            while (!isValid)
            {
                Console.WriteLine(question);
                answer = Console.ReadLine().ToLower();

                if (answer == "yes")
                {
                    isValid = true;
                }
                else if (answer == "no")
                {
                    isValid = true;
                }

                else
                {
                    Console.WriteLine("I'm sorry, you must enter yes or no");
                    Console.WriteLine();
                    isValid = false;
                }
            }
            return answer;
        }



        #endregion

        #region ThemeChange

        static void DisplaySetTheme()
        {


            DisplayReadAndSetTheme();

            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;
            string exceptionMessage;


            DisplayScreenHeader("Set Application Colors!");

            Console.WriteLine($"\tCurrent foreground (Text) color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background (Console) color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tWould you like to change the current theme (yes or no)?");
            if (Console.ReadLine().ToLower() == "yes")
            {
                do
                {

                    //
                    // List available theme options for user
                    //

                    Console.WriteLine();
                    Console.WriteLine("Valid Options: ");
                    Console.WriteLine();
                    Console.WriteLine("\tblack");
                    Console.WriteLine("\twhite");
                    Console.WriteLine("\tblue or darkblue");
                    Console.WriteLine("\tcyan or darkcyan");
                    Console.WriteLine("\tgray or darkgray");
                    Console.WriteLine("\tgreen or darkgreen");
                    Console.WriteLine("\tmagenta or darkmagenta");
                    Console.WriteLine("\tred or darkred");
                    Console.WriteLine("\tyellow or darkyellow");
                    Console.WriteLine();
                    Console.WriteLine();


                    themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                    themeColors.backgroundColor = GetConsoleColorFromUser("background");

                    //
                    // set new theme
                    //

                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set Application Colors!");

                    //
                    // Echo user's choices
                    //

                    Console.WriteLine($"\tNew foreground (Text) color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew background (Console) color: {Console.BackgroundColor}");

                    //
                    // Ask user if they want to save this theme
                    //

                    Console.WriteLine();
                    Console.Write("\tIs this the theme you would like?");

                    //
                    // Validate that the file was written correctly
                    //

                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        exceptionMessage = WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                        if (exceptionMessage == "Complete")
                        {
                            Console.WriteLine("\n\tNew theme written to data file.\n");
                        }
                        else
                        {
                            Console.WriteLine("\n\tNew theme not written to data file.");
                            Console.WriteLine($"\t*** {exceptionMessage} ***\n");
                        }
                    }

                } while (!themeChosen);
            }
            DisplayContinuePrompt();
        }

        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;


            //
            // Ask user what theme they would like and verify thats a valid color
            //

            do
            {
                Console.Write($"\tEnter a value for the {property}:");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\tI'm sorry, that is not a valid option. Please try again\n");
                }
                else
                {
                    validConsoleColor = true;
                }

            } while (!validConsoleColor);

            return consoleColor;
        }

        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeData(out string exceptionMessage)
        {
            string dataPath = @"Data/Theme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor = ConsoleColor.White;
            ConsoleColor backgroundColor = ConsoleColor.Black;

            //
            // Try to read theme data from file, give user error message if failed
            //

            try
            {
                themeColors = File.ReadAllLines(dataPath);
                if (Enum.TryParse(themeColors[0], true, out foregroundColor) &&
                    Enum.TryParse(themeColors[1], true, out backgroundColor))
                {
                    exceptionMessage = "Complete";
                }
                else
                {
                    exceptionMessage = "Data file incorrectly formated.";
                }
            }
            catch (DirectoryNotFoundException)
            {
                exceptionMessage = "Unable to locate the folder for the data file. Please verify directory exists";
            }
            catch (Exception)
            {
                exceptionMessage = "Unable to read data file.";
            }

            return (foregroundColor, backgroundColor);
        }

        static string WriteThemeData(ConsoleColor foreground, ConsoleColor background)
        {
            string dataPath = @"Data/Theme.txt";
            string exceptionMessage = "";

            //
            // Verify that program can access and write to file. Give user error message if failed
            //

            try
            {
                File.WriteAllText(dataPath, foreground.ToString() + "\n");
                File.AppendAllText(dataPath, background.ToString());
                exceptionMessage = "Complete";
            }
            catch (DirectoryNotFoundException)
            {
                exceptionMessage = "Unable to locate the folder for the data file. Please verify directory exists";
            }
            catch (Exception)
            {
                exceptionMessage = "Unable to write to data file.";
            }

            return exceptionMessage;
        }

        static void DisplayReadAndSetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            string exceptionMessage;

            //
            // read theme from data and set theme
            //
            themeColors = ReadThemeData(out exceptionMessage);
            if (exceptionMessage == "Complete")
            {
                Console.ForegroundColor = themeColors.foregroundColor;
                Console.BackgroundColor = themeColors.backgroundColor;
                Console.Clear();

                DisplayScreenHeader("       Import Theme from Data File");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("\n\t\t\t\t         Theme read from data file Successfully.\n");
            }
            else
            {
                DisplayScreenHeader("\t\t\t      Import Theme from Data File");
                Console.WriteLine("\n\t\t\t\t     Could not retreive theme data from data file.");
                Console.WriteLine();
                Console.WriteLine($"\t\t*** {exceptionMessage} ***\n");
            }
            DisplayContinuePrompt();
        }


        #endregion
    }
}



