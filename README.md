# HarvestConsole
A console tool for a card game that ingests card data and uses it to format printable cards and perform data-driven card analysis

A few code pointers:

Card data schema:
https://github.com/morganmaddren/HarvestConsole/blob/master/HarvestConsole/CardData.cs

Example formatter that takes in a card data and draws the card to a PDF:
https://github.com/morganmaddren/HarvestConsole/blob/master/HarvestConsole/Formatters/SpellFormatter.cs

String tokenizer used for typesetting the text portion of cards:
https://github.com/morganmaddren/HarvestConsole/blob/master/HarvestConsole/Typesetting/StringTokenizer.cs

Command to calculate card power budgets and power utilization based on balance parameters and card power approximation formula:
https://github.com/morganmaddren/HarvestConsole/blob/master/HarvestConsole/Commands/BalanceCommand.cs
