using FibonacciTest.Model;
using System.Text;

namespace FibonacciTest
{
    internal class Program
    {
        private string _userName = "";
        private int _sequenceCount;
        private string _output = "";

        static void Main(string[] args)
        {
            var program = new Program();

            program.DisplayRunPrompt();
            program.GenerateFibonacciSequence();
            program.SaveResult();

        }

        private void DisplayRunPrompt()
        {
            Console.WriteLine("Welcome to the Fibonacci Generator!");
            _userName = PromptForUsername();

            _sequenceCount = PromptForSequenceCount();
        }

        private string PromptForUsername()
        {
            string input;
            do
            {
                Console.Write("Please enter your username:");
                input = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        private int PromptForSequenceCount()
        {
            string input;
            int sequenceCount;
            do
            {
                Console.Write("Please enter the number of Fibonacci sequence numbers to generate:");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out sequenceCount) || sequenceCount <= 0);

            return sequenceCount;
        }

        private void GenerateFibonacciSequence()
        {
                Console.WriteLine($"Generating {_sequenceCount} Fibonacci numbers for {_userName}:");

                int first = 0, second = 1, next;
                StringBuilder outputSb = new StringBuilder();

                for (int i = 0; i < _sequenceCount; i++)
                {
                    if (i <= 1)
                        next = i;
                    else
                    {
                        next = first + second;
                        first = second;
                        second = next;
                    }
                    outputSb.Append($"{next}, ");
                }
                _output = outputSb.ToString().TrimEnd(' ',','); 
        }

        private void SaveResult()
        {
            using (var dbContext = new FibonacciContext())
            {
                var runSummary = new RunSummary() { SequenceCount = _sequenceCount, UserName = _userName, RunDateTime = DateTime.Now };
                dbContext.RunSummary.Add(runSummary);
                dbContext.SaveChanges();

                dbContext.RunOutput.Add(new RunOutput() { RunId = runSummary.RunId, Output = _output });
                dbContext.SaveChanges();
            }
        }
    }
}
