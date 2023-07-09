# SudokuPlus
In April 2020 I was inspired to create a WPF program to play and solve any Sudoku puzzle. 
The main objective was to create an algorithm that solves any puzzle.
Project was abandoned and is thus unfinished as I wanted to remake the whole thing using react, vue or angular. 


## Mechanics
- You can select a button with your cursor and add numbers to the grid
- "Solve all" will fill the blanks for most scenarios. Tested and verified using the hardest difficulty (evil) on https://sudoku.com/evil/

## Known issues
- All bugs, defects and features are listed under `MainWindow.xaml.cs`
```
/**
 * TODO
 * - Fix Undo and delete background set to white defect
 * - Fix wrong input foreground bug
 * - Bruteforce when solution no longer works
 * - Add support for keyboard
 * - Save states for undo / redo button
 * - Pencil markings
 * - Solve 1 button functionality
 * - Preset games + Ability to browse
 * - Timer + personal best tracker
 * - Difficulty slider
 * - Darkmode settings
 * - Improve keyPad border GUI
 * - More sudoku types
 **/
```

## Usage

```
# Run program
dotnet run --project Sudoku+
```


## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.
