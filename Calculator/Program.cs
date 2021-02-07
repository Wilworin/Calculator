using System;
using System.Collections.Generic;

namespace Calculator
{
    public class Program
    {
        // Yes, I'm using global variables. Perhaps not the prettiest way but it's simple atleast. :)
        public static bool isRunning = true;  // Is the program running?
        public static List<string> history = new List<string>(); // Saves the history of entered commands.
        public static string currentFormula = ""; // Saves the current formula. When it's finished it will be moved to history.
        public static double currentSum = 0.0; // Saves the current sum. When it's finished it will be moved to history.
        public static string lastCommand = ""; // Saves the last command to be entered (or # if it was a number)

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Calculator!\n");
            PrintMenu();

            // Note: All calculations are done with doubles so you can enter decimals if you wish. I had some issues with the
            // decimal delimiter sometimes having to be entered as a comma and sometimes as a dot. I believe that the Swedish
            // way is a comma but since Visual Studio is running English I need to code it with a dot. So, I coded with a dot
            // but when running it I had to enter with a comma. I'm running an English Windows but with keyboard and localisation
            // as Swedish.

            // This is the main loop. It keeps displaying a prompt and asking for input. The TryParse is used to detect if it's
            // a number or a command and then the corresponding method is called: HandleNumber or HandleCommand.

            while (isRunning)  // This variable will be changed to false when user enters "quit" or "exit".
            {
                string input = AskCommand(); // Displays a prompt and asks for input.
                bool isNumber = Double.TryParse(input, out double number);
                if (isNumber)
                {
                    HandleNumber(number);
                }
                else
                {
                    HandleCommand(input);
                }
            }
        }


        /// <summary>
        /// Prints the help menu
        /// </summary>
        static void PrintMenu ()
        {
            Console.WriteLine("You can use the 4 normal ways of math, + - * /  but also the following commands:");
            Console.WriteLine("= - Shows the current formula.");
            Console.WriteLine("C or F - Convert the current number to Celcius or Fahrenheit.");
            Console.WriteLine("PI - The mathematical number called PI.");
            Console.WriteLine("MARCUS - The answer to the question about life, universe and everything.");
            Console.WriteLine("RICHARD - A double MARCUS.");
            Console.WriteLine("DEVIL - The number of the Beast!");
            Console.WriteLine("BMI - Lets you calculate your bmi score.");
            Console.WriteLine("NEWTON - Lets you calculate Newtons law of force.");
            Console.WriteLine("LIST - List the earlier commands that have been entered.");
            Console.WriteLine("HELP - Print this help menu again.");
            Console.WriteLine("QUIT - Quit the calculator.");
        }


        /// <summary>
        /// Does the calculations of the formulas that the user inputs.
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <param name="op">The operator that should be used.</param>
        /// <returns>The result of the operation as a double.</returns>
        public static double Compute(double number1, double number2, string op)
        {
            double result = 0.0;
            switch (op)
            {
                case "+":
                    result = number1 + number2;
                    break;
                case "-":
                    result = number1 - number2;
                    break;
                case "*":
                    result = number1 * number2;
                    break;
                case "/":
                    // I noticed that division by 0 doesn't give any exceptions when working with doubles.
                    // Thus I have to create that Exception manually.
                    if (number2 == 0)
                    {
                        throw new DivideByZeroException("ERROR: You can't divide by zero!");
                    }
                    result = number1 / number2;
                    break;
                default:  // If for some reason the operator is something else.
                    throw new ArgumentException("Illegal operator: " + op);
            }
            return result;
        }


        /// <summary>
        /// Shows a prompt and then asks the user for a command.
        /// </summary>
        /// <returns></returns>
        public static string AskCommand()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }


        /// <summary>
        /// If the input is a number, this method gets called.
        /// </summary>
        /// <param name="input">The number that the user have entered.</param>
        /// <param name="special">An optional string that defaults to empty but should contain the string if a command like
        ///                       marcus, richard, devil or pi is entered.</param>
        public static void HandleNumber (double input, string special = "")
        {
            if (currentFormula == "")
            {
                currentSum = input;
                currentFormula = HandleSpecial(input, special);
            }
            else
            {
                if (lastCommand == "#")
                {
                    AddFormulaToList();
                    currentSum = input;
                    currentFormula = HandleSpecial(input, special);
                }
                else
                {
                    currentFormula += " " + lastCommand + " " + HandleSpecial(input, special);
                    try
                    {
                        currentSum = Compute(currentSum, input, lastCommand);
                    }
                    catch (DivideByZeroException)
                    {
                        Console.WriteLine("You cannot divide by zero!");
                        ClearFormula();
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Illegal operator!");
                        ClearFormula();
                    }
                    Console.WriteLine(currentFormula + " = " + currentSum);
                }
            }
            lastCommand = "#";  // Last command is a number
        }


        /// <summary>
        /// If the input is a non-number, this method gets called. It switches through all allowed commands.
        /// </summary>
        /// <param name="input"></param>
        public static void HandleCommand (string input)
        {
            input = input.ToLower();  // Ensures that the user can use both upper and lower caps.
            switch (input) { 
                case "help":
                    PrintMenu();
                    break;
                case "exit":
                case "quit":
                    isRunning = false;
                    break;
                case "marcus":
                    HandleNumber(42, "MARCUS"); // If user says MARCUS, we calls the number method with 42 and marcus as parameter.
                    break;
                case "richard":
                    HandleNumber(84, "RICHARD"); // If user says RICHARD, we calls the number method with 84 and richard as parameter.
                    break;
                case "devil":
                    HandleNumber(666, "DEVIL"); // If user says devil, we calls the number method with 666 and devil as parameter.
                    break;
                case "pi":
                    HandleNumber(Math.PI, "PI");
                    break;
                case "bmi":
                    AddFormulaToList(); // Adds the earlier formula to history before starting to ask for bmi.
                    CalcBmi();
                    break;
                case "newton":
                    AddFormulaToList(); // Adds the earlier formula to history before starting to ask for newton.
                    CalcNewton();
                    break;
                case "list":
                    ShowList();
                    break;
                case "c":
                case "f":
                    CalcTemp(input);
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                    if (lastCommand != "#")
                    {
                        Console.WriteLine("You need to enter a number before a math operator.");
                    }
                    else
                    {
                        lastCommand = input;
                    }
                    break;
                case "=": // Prints the current formula without adding to history.
                    if (currentFormula != "")
                    {
                        Console.WriteLine(currentFormula + (currentFormula != currentSum.ToString() ? " = " + currentSum : " "));
                    }
                    break;
                default:
                    Console.WriteLine("Not a valid command. Please try again.");
                    break;

            }
        }


        /// <summary>
        /// Converts the temperature between Celsius and Fahrenheit.
        /// </summary>
        /// <param name="tempChoice">Should be c or f depending on what the user want to convert into.</param>
        public static void CalcTemp (string tempChoice)
        {
            if (currentSum == 0)
            {
                Console.WriteLine("You first need to have a number to convert.");
            }
            else
            {
                double temp = currentSum;

                // Adds the current formula to history if it's not empty or a single number (that should be converted).
                if (currentFormula != "" && (currentFormula != currentSum.ToString()))
                {
                    AddFormulaToList();
                }
                if (tempChoice == "c")
                {
                    currentFormula = (Math.Round(temp,1)).ToString() + " Fahrenheit => ";
                    temp -= 32;
                    temp /= 1.8;
                    temp = Math.Round(temp, 1);
                    currentFormula += temp + " Celsius.";
                    Console.WriteLine(temp + " degrees Celsius.");
                }
                else if (tempChoice == "f")
                {
                    currentFormula = (Math.Round(temp, 1)).ToString() + " Celsius => ";
                    temp *= 1.8;
                    temp += 32;
                    temp = Math.Round(temp, 1);
                    currentFormula += temp + " Fahrenheit.";
                    Console.WriteLine(temp + " degrees Fahrenheit.");
                }
                history.Add(currentFormula);
                ClearFormula();
            }
        }


        /// <summary>
        /// Clears all the variables that holds the current formula.
        /// </summary>
        public static void ClearFormula ()
        {
            currentFormula = "";
            currentSum = 0;
            lastCommand = "";
        }


        /// <summary>
        /// Adds the current formula to the history-list.
        /// </summary>
        public static void AddFormulaToList ()
        {
            if (currentFormula != "")
            {
                history.Add(currentFormula + " = " + currentSum);
            }
            ClearFormula();
        }


        /// <summary>
        /// Prints out the history of commands the user have previously entered.
        /// </summary>
        public static void ShowList()
        {
            foreach (string s in history)
            {
                Console.WriteLine(s);
            }
            if (currentFormula != "")
            {
                Console.WriteLine(currentFormula + (currentFormula != currentSum.ToString() ? " = " + currentSum : " "));
            }
        }


        /// <summary>
        /// Returns the string "MARCUS", "RICHARD" or "DEVIL" if called as them, otherwise input.ToString.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Specialstring or the number converted to a string.</returns>
        public static string HandleSpecial (double input, string special)
        {
            if (special.Length > 0)
            {
                return special;
            }
            else
            {
                return input.ToString();
            }
        }


        /// <summary>
        /// Asks questions of length and weight and then calculates the bmi score.
        /// </summary>
        public static void CalcBmi ()
        {
            double length = AskNumber("Length? (cm): ");
            double weight = AskNumber("Weight? (kg): ");
            length = length / 100; // Converts to meters
            double bmi = Math.Round((weight / (length * length)), 2);
            Console.WriteLine("Your BMI is: " + bmi);
            if (bmi < 18.5)
            {
                Console.WriteLine("You are underweight.");
            }
            else if (bmi < 25)
            {
                Console.WriteLine("You are of normal weight.");
            }
            else if (bmi < 30)
            {
                Console.WriteLine("You are overweight.");
            }
            else
            {
                Console.WriteLine("You are obese!");
            }
            history.Add("BMI(" + length + "m/" + weight + "kg) => " + bmi);
        }


        /// <summary>
        /// Asks questions of mass and acceleration and then calculates the resulting force.
        /// </summary>
        public static void CalcNewton()
        {
            double mass = AskNumber("Mass? (kg): ");
            double acc = AskNumber("Acceleration? (m/s): ");
            double force = Math.Round (mass * acc, 2);
            Console.WriteLine("The resulting force is: " + force + " Newton");
            history.Add("Newtons 2nd(" + mass + "kg/" + acc + "m/s) => " + force + " Newton.");
        }


        /// <summary>
        /// Keeps asking a question over and over until the response is a number.
        /// </summary>
        /// <param name="question">The question to ask.</param>
        /// <returns>The number that the user answered with.</returns>
        public static double AskNumber(string question)
        {
            bool isNumber = false;
            double output = 0.0;
            while (!isNumber)
            {
                Console.Write(question);
                string input = Console.ReadLine();
                isNumber = Double.TryParse(input, out output);
                if (!isNumber)
                {
                    Console.WriteLine("You need to enter a number. Please try again.");
                }
            }
            return output;
        }
    }
}
