# WordleSolver
WorldeSolver is a simple tool used to see if a bot can solve wordle puzzles using some basic algorithms and weighting.

## Concept

The progam uses a dictionay of known English words, and performs some basic character serches and eliminate possible options using
loops over the available data to generate a list of all possible options. From the list of remaining words, each word is weigted
based on the commonality of the letters in the word.

The word with the heighest score is selected as the next choice.
