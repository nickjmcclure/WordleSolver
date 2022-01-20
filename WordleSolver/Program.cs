using WorldSolver.Common;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var solver = new Solver(5);
solver.AddGuess("arose");
solver.AddGuess("noily");
solver.AddGuess("coign");


//solver.AddCorrect(0, 'p');
solver.AddCorrect(1, 'o');
solver.AddCorrect(2, 'i');
//solver.AddCorrect(3, 's');
//solver.AddCorrect(4, 'e');

solver.AddPresent('n');

//solver.AddAbsent('a');



var output = solver.GetOptions();
Console.WriteLine(output.Count);

Console.WriteLine(solver.GetGuess());

/*
Console.WriteLine("all words---");
foreach (var item in output)
{
    Console.WriteLine(item);
}
*/