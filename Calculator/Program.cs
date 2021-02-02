using System;
using System.Collections.Generic;

namespace Calculator
{
    public class Program
    {
        public static bool isRunning = true;  // Is the program running?
        public static List<string> history = new List<string>(); // Saves the history of entered commands.

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Calculator!\n");
            PrintMenu();

            while (isRunning)  // This variable will be changed to false when user enters "quit".
            {
                string input = AskCommand();
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

        static void PrintMenu ()
        {
            Console.WriteLine("You can use the 4 normal ways of math, + - * /  but also the following commands:");
            Console.WriteLine("C or F - Convert to Celcius or Fahrenheit.");
            Console.WriteLine("MARCUS - The answer to the question about life, universe and everything.");
            Console.WriteLine("BMI - Lets you calculate your bmi score.");
            Console.WriteLine("NEWTON - Lets you calculate Newtons law of force.");
            Console.WriteLine("LIST - List the earlier commands that have been entered.");
            Console.WriteLine("HELP - Print this help menu again.");
            Console.WriteLine("QUIT - Quit the calculator.");
        }

        public static double? Compute(double number1, double number2, string op)
        {
            // I decided to use a nullable double as return value. Then I an use NULL to show that something went wrong.
            double? result = 0.0;
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
                    if (number2 == 0)
                    {
                        Console.WriteLine("ERROR: You can't divide by zero!");
                        result = null;
                    }
                    result = number1 / number2;
                    break;
                default:
                    Console.WriteLine("Illegal operator: " + op);
                    result = null;
                    break;
            }
            return result;
        }

        public static string AskCommand()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }

        public static void HandleNumber (double input)
        {

        }

        public static void HandleCommand (string input)
        {
            input = input.ToLower();
            switch (input) { 
                case "help":
                    PrintMenu();
                    break;
                case "quit":
                    isRunning = false;
                    break;
                case "marcus":
                    HandleNumber(42);
                    break;
                case "bmi":
                    CalcBmi();
                    break;
                case "newton":
                    CalcNewton();
                    break;
                case "list":
                    ShowList();
                    break;
                default:
                    Console.WriteLine("Syntax error. Please try again.");
                    break;

            }
        }


        public static void ShowList()
        {
            foreach (string s in history)
            {
                Console.WriteLine(s);
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
            history.Add("BMI(" + length + "/" + weight + ") => " + bmi);
        }

        /// <summary>
        /// Asks questions of mass and acceleration and the calculates the resulting force.
        /// </summary>
        public static void CalcNewton()
        {
            double mass = AskNumber("Mass? (kg): ");
            double acc = AskNumber("Acceleration? (m/s): ");
            double force = Math.Round (mass * acc, 2);
            Console.WriteLine("The resulting force is: " + force + " Newton");
            history.Add("Newtons 2nd(" + mass + "/" + acc + ") => " + force + " Newton.");
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
