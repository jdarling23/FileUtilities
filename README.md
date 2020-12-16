# FileUtilities
This project contains tools to work with files via the Command line in Windows.

## CsvToJson
This command converts CSV files to JSON files

To install, clone the repo, build the solution, and then navigate to the directory that contains the ```.csproj``` file. Run the following command.

```dotnet tool install --global --add-source ./nupkg FileUtilities.CsvToJson```

. See the below code for an example for how to use the tool.

```csvtojson C:/targetFile.csv C:/OutputDirectory```
