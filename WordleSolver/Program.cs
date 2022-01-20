using WorldSolver.Common;



// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var solver = new Solver(5);
solver.AddGuess("arose");
solver.AddGuess("noily");
solver.AddGuess("coign");


//solver.AddKnown(0, 'p');
solver.AddKnown(1, 'o');
solver.AddKnown(2, 'i');
//solver.AddKnown(3, 's');
//solver.AddKnown(4, 'e');



solver.AddContains('n');
//solver.AddContains('o');
//solver.AddContains('d');
//solver.AddContains('y');
//solver.AddContains('l');

solver.AddInvalid('a');
solver.AddInvalid('r');
solver.AddInvalid('s');
solver.AddInvalid('e');
solver.AddInvalid('l');
solver.AddInvalid('y');
///solver.AddInvalid('l');
//solver.AddInvalid('y');
solver.AddInvalid('c');
solver.AddInvalid('g');


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