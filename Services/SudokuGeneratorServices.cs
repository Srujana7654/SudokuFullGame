using System;

public class SudokuGeneratorService
{
    private readonly Random random = new Random();

    public int[,] GenerateNewGame(int numbersToRemove)
    {
        int[,] grid = GenerateRandomSudoku();
        RemoveNumbers(grid, numbersToRemove);
        return grid;
    }

    private int[,] GenerateRandomSudoku()
    {
        int[,] grid = new int[9, 9];
        FillDiagonal(grid);
        FillRemaining(grid, 0, 3);
        return grid;
    }

    private void FillDiagonal(int[,] grid)
    {
        for (int box = 0; box < 9; box += 3)
        {
            FillBox(grid, box, box);
        }
    }

    private void FillBox(int[,] grid, int row, int col)
    {
        int num;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                do
                {
                    num = random.Next(1, 10);
                } while (!IsValid(grid, row + i, col + j, num));

                grid[row + i, col + j] = num;
            }
        }
    }

    private bool IsValid(int[,] grid, int row, int col, int num)
    {
      
        for (int x = 0; x < 9; x++)
            if (grid[row, x] == num)
                return false;

       
        for (int x = 0; x < 9; x++)
            if (grid[x, col] == num)
                return false;

       
        int startRow = row - row % 3;
        int startCol = col - col % 3;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (grid[i + startRow, j + startCol] == num)
                    return false;

        return true;
    }

    private bool FillRemaining(int[,] grid, int row, int col)
    {
        if (col >= 9 && row < 8)
        {
            row++;
            col = 0;
        }
        if (row >= 9 && col >= 9)
            return true;

        if (row < 3)
        {
            if (col < 3)
                col = 3;
        }
        else if (row < 6)
        {
            if (col == (row / 3) * 3)
                col += 3;
        }
        else
        {
            if (col == 6)
            {
                row++;
                col = 0;
                if (row >= 9)
                    return true;
            }
        }

        for (int num = 1; num <= 9; num++)
        {
            if (IsValid(grid, row, col, num))
            {
                grid[row, col] = num;
                if (FillRemaining(grid, row, col + 1))
                    return true;

                grid[row, col] = 0;
            }
        }
        return false;
    }

    private void RemoveNumbers(int[,] grid, int count)
    {
        int cellsRemoved = 0;
        while (cellsRemoved < count)
        {
            int row = random.Next(9);
            int col = random.Next(9);
            if (grid[row, col] != 0)
            {
                grid[row, col] = 0;
                cellsRemoved++;
            }
        }
    }

   
    public void PrintGrid(int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.Write(grid[i, j] + " ");
                if ((j + 1) % 3 == 0 && j < 8) Console.Write("| ");
            }
            Console.WriteLine();
            if ((i + 1) % 3 == 0 && i < 8)
            {
                Console.WriteLine("---------------------");
            }
        }
    }
}

