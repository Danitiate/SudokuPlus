using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


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

namespace SudokuPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int SelectedValue = 0;
        private Solver Solver;
        private List<Label> Moves = new List<Label>();

        public MainWindow()
        {
            InitializeComponent();
            foreach (var child in SudokuGrid.Children)
            {
                var cell = ((Label)child);
                var column = Grid.GetColumn(cell);
                var row = Grid.GetRow(cell);
                if (cell.Content == null)
                {
                    cell.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) => SudokuCellClick(sender);
                }
                else
                {
                    cell.FontWeight = FontWeight.FromOpenTypeWeight(750);
                }
            }
            Solver = new Solver(this);
        }

        private void SudokuCellClick(object sender)
        {
            var cell = ((Label)sender);
            cell.Foreground = Brushes.Black;
            CheckValidity(cell);
            if (SelectedValue == 0)
            {
                cell.Content = null;
            }
            else
            {
                cell.Content = SelectedValue;
                cell.Background = Brushes.Yellow;
            }
            Moves.Add(cell);
        }

        private void CheckValidity(Label cell)
        {
            var relatedCells = GetRelatedCells(cell);
            foreach(var relatedCell in relatedCells)
            {
                if((int)relatedCell.Content == SelectedValue)
                {
                    relatedCell.Foreground = Brushes.Red;
                    cell.Foreground = Brushes.Red;
                }
            }
        }

        public List<Label> GetRelatedCells(Label cell)
        {
            var cellColumn = Grid.GetColumn(cell);
            var cellRow = Grid.GetRow(cell);
            return SudokuGrid.Children.OfType<Label>().Where(c => c.Content != null && c != cell && (Grid.GetColumn(c) == cellColumn || Grid.GetRow(c) == cellRow || Grid.GetColumn(c) / 3 == cellColumn / 3 && Grid.GetRow(c) / 3 == cellRow / 3)).ToList();
        }

        private void KeyPadButtonClick(object sender, RoutedEventArgs e)
        {
            var button = ((Button)sender);
            var content = button.Content;
            if (content != null)
            {
                foreach (var element in KeyPad.Children)
                {
                    var child = ((Button)element);
                    child.IsEnabled = true;
                }
                if ((string)content == "DEL")
                {
                    SelectedValue = 0;
                    button.IsEnabled = false;
                }
                else if ((string)content == "UNDO")
                {
                    var previousMove = Moves.LastOrDefault();
                    if (previousMove != null)
                    {
                        Moves.Remove(previousMove);
                        previousMove.Content = null;
                        previousMove.Background = Brushes.White;
                    }
                }
                else
                {
                    SelectedValue = Int32.Parse((string)content);
                    var previousSelectedCells = SudokuGrid.Children.OfType<Label>().Where(c => c.Background == Brushes.Yellow).ToList();
                    var sameValueCells = SudokuGrid.Children.OfType<Label>().Where(c => c.Content != null && (int)c.Content == SelectedValue).ToList();
                    foreach(var cell in sameValueCells)
                    {
                        cell.Background = Brushes.Yellow;
                    }
                    foreach(var cell in previousSelectedCells)
                    {

                        cell.Background = Brushes.White;
                    }
                    button.IsEnabled = false;
                }
            }
        }

        private void CheckButtonClick(object sender, RoutedEventArgs e)
        {
            Solver.CheckSolution();
        }

        private void SolveAllButtonClick(object sender, RoutedEventArgs e)
        {
            Solver.SolvePuzzle();
        }
    }
}
