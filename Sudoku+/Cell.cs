using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Text;

namespace SudokuPlus
{
    public class Cell
    {
        public Label Label;
        public List<int> PossibleValues;

        public Cell(Label label)
        {
            Label = label;
            PossibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }
    }
}
