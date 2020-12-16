using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace FileUtilities.CsvToJson
{
    class Program
    {
        static void Main(string[] args)
        { 
            var app = new CommandLineApplication();
            app.Name = "CsvToJson";
            app.Description = "Converrt CSV files to JSON.";

            app.HelpOption("-?|-h|--help");

            var targetFile = app.Argument("target", "Target CSV file to convert.");
            var outputDirectory = app.Argument("output", "Directory to send the ouput JSON file.");

            app.OnExecute(() =>
            {
                // Read in CSV
                var lines = File.ReadAllLines(targetFile.Value);

                var isFirstRow = true;

                var propertyNames = new List<string>();
                var mappedData = new List<ExpandoObject>();

                // Write the CSV lines into the program
                foreach (string line in lines)
                {
                    var splits = line.Split(',').ToList();

                    if (isFirstRow)
                    {
                        foreach (var split in splits)
                        {
                            propertyNames.Add(split);
                        }
                    }
                    else
                    {
                        dynamic expando = new ExpandoObject();
                        var p = expando as IDictionary<String, object>;

                        foreach (var split in splits)
                        {
                            var index = splits.IndexOf(split);

                            p[propertyNames[index]] = split;
                        }

                        mappedData.Add(p as ExpandoObject);
                    }

                    isFirstRow = false;
                }

                // Serialize to JSON
                string json = JsonConvert.SerializeObject(mappedData);

                // Write to a new JSON file
                var fileName = $"CsvToJson_{DateTime.Now.Ticks}";
                string filePath = @$"{outputDirectory.Value}\{fileName}.json";

                using (StreamWriter outputFile = new StreamWriter(filePath))
                {
                    outputFile.WriteLine(json);
                }

                return 0;
            });

            try
            {
                // This begins the actual execution of the application
                Console.WriteLine("ConsoleArgs app executing...");
                app.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                // You'll always want to catch this exception, otherwise it will generate a messy and confusing error for the end user.
                // the message will usually be something like:
                // "Unrecognized command or argument '<invalid-command>'"
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to execute application: {0}", ex.Message);
            }

        }
    }
}
