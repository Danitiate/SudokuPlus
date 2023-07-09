using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuPlus
{
    public class Solver
    {
        MainWindow Main;

        public Solver(MainWindow main)
        {
            Main = main;
        }

        public bool CheckSolution(bool showMessage = true)
        {
            Main.Result.Content = "";
            var nonEmptyCells = Main.SudokuGrid.Children.OfType<Label>().Where(c => c.Content != null).ToList();
            foreach (var cell in nonEmptyCells)
            {
                var labelCell = ((Label)cell);
                var relatedCells = Main.GetRelatedCells(labelCell);
                if (relatedCells.Any(c => (int)c.Content == (int)labelCell.Content))
                {
                    if (showMessage)
                    {
                        Main.Result.Content = "Something is not right";
                    }
                    return false;
                }
            }
            if ((string)Main.Result.Content == "" && showMessage)
            {
                Main.Result.Content = "Everything looks good";
                if (nonEmptyCells.Count() == 81)
                {
                    Main.Result.Content = "Wow! Amazing! You won!!!";
                    return true;
                }
            }
            return false;
        }

        public void SolvePuzzle()
        {
            for (int i = 0; i < 10; i++)
            {
                SolveIndividualCells();
                SolveRows();
                SolveColumns();
                SolveGrids();
                var done = CheckSolution(showMessage: false);
                if (done)
                {
                    Main.Result.Content = "How does it feel to be a cheater?";
                    return;
                }
                else if(i == 9)
                {
                    ForceSolve();
                }
            }
        }

        private void SolveRows()
        {
            for (int row = 0; row < 9; row++)
            {
                var cellsInRow = Main.SudokuGrid.Children.OfType<Label>().Where(c => c.Content == null && Grid.GetRow(c) == row).ToList();
                SolveCellsForGroup(cellsInRow);
            }
        }

        private void SolveColumns()
        {
            for (int column = 0; column < 9; column++)
            {
                var cellsInColumn = Main.SudokuGrid.Children.OfType<Label>().Where(c => c.Content == null && Grid.GetColumn(c) == column).ToList();
                SolveCellsForGroup(cellsInColumn);
            }
        }

        private void SolveGrids()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    var cellsInGrid = Main.SudokuGrid.Children.OfType<Label>().Where(c => c.Content == null && Grid.GetColumn(c) / 3 == column && Grid.GetRow(c) / 3 == row).ToList();
                    SolveCellsForGroup(cellsInGrid);
                }
            }
        }

        private void SolveCellsForGroup(List<Label> cells)
        {
            List<Cell> cellsInGroup = new List<Cell>();
            foreach (var cell in cells)
            {
                var newCell = new Cell(cell);
                var relatedCells = Main.GetRelatedCells(cell);
                foreach (var relatedCell in relatedCells)
                {
                    var value = (int)relatedCell.Content;
                    if (newCell.PossibleValues.Contains(value))
                    {
                        newCell.PossibleValues.Remove(value);
                    }
                }
                cellsInGroup.Add(newCell);
            }
            for (int i = 1; i <= 9; i++)
            {
                int counter = 0;
                foreach (var cell in cellsInGroup)
                {
                    if (cell.PossibleValues.Contains(i))
                    {
                        counter++;
                    }
                }
                if (counter == 1)
                {
                    var uniqueValuedCell = cellsInGroup.Where(c => c.PossibleValues.Contains(i)).First();
                    uniqueValuedCell.Label.Content = i;
                }
            }
        }

        private void SolveIndividualCells()
        {
            var emptyCells = Main.SudokuGrid.Children.OfType<Label>().Where(c => c.Content == null).ToList();
            foreach (var cell in emptyCells)
            {
                List<int> possibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                var relatedCells = Main.GetRelatedCells(cell);
                foreach (var relatedCell in relatedCells)
                {
                    var value = (int)relatedCell.Content;
                    if (possibleValues.Contains(value))
                    {
                        possibleValues.Remove(value);
                    }
                }
                if (possibleValues.Count() == 1)
                {
                    cell.Content = possibleValues.First();
                }
            }
        }

        private void ForceSolve()
        {
            List<Cell> possibleToForceCells = new List<Cell>();
            //Start by finding all empty cells with only 2 options
            var emptyCells = Main.SudokuGrid.Children.OfType<Label>().Where(c => c.Content == null).ToList();
            foreach(var cell in emptyCells)
            {
                var newCell = new Cell(cell);
                var relatedCells = Main.GetRelatedCells(cell);
                foreach (var relatedCell in relatedCells)
                {
                    var value = (int)relatedCell.Content;
                    if (newCell.PossibleValues.Contains(value))
                    {
                        newCell.PossibleValues.Remove(value);
                    }
                }
                if(newCell.PossibleValues.Count == 2)
                {
                    possibleToForceCells.Add(newCell);
                }
            }
            if(possibleToForceCells.Count == 0)
            {
                return;
            }
            //Assign first value. If fails, assign the other
            var firstTryCell = possibleToForceCells.First();
            firstTryCell.Label.Content = firstTryCell.PossibleValues.First();
            SolvePuzzle();
            if (emptyCells.All(c => c.Content != null))
            {
                return;
            }
            else
            {
                foreach(var cell in emptyCells)
                {
                    cell.Content = null;
                }
            }
            firstTryCell.Label.Content = firstTryCell.PossibleValues.Last();
            SolvePuzzle();
        }
    }
}
