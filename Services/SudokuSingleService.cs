namespace SudokuFullGame.Services
{
    public class SudokuStateService
    {
        private int[,] _solution;

        private int[,] currentBoard = new int[9, 9];
        private int[,] initialBoard = new int[9, 9];
        private int[,] playerAnswers = new int[9, 9];
        private int[,] solution = new int[9, 9];
        private int[,] board = new int[9, 9];





        public void UpdateCell(int row, int col, int value)
        {
            if (initialBoard[row, col] == 0)
            {
                board[row, col] = value;
                playerAnswers[row, col] = value;
            }
        }
        public int[,] GetPlayerAnswers()
        {
            return playerAnswers;
        }


        public void SetPlayerAnswers(int[,] answers)
        {
            playerAnswers = (int[,])answers.Clone();
        }

        public int[,] GetCurrentBoard()
        {
            int[,] currentBoard = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (playerAnswers[i, j] != 0)
                        currentBoard[i, j] = playerAnswers[i, j];
                    else
                        currentBoard[i, j] = initialBoard[i, j];
                }
            }
            return currentBoard;
        }
        public void SetCurrentBoard(int[,] board)
        {
            currentBoard = (int[,])board.Clone();
        }


        public void SetInitialBoard(int[,] board)
        {
            initialBoard = (int[,])board.Clone();
        }

        public int[,] GetInitialBoard()
        {
            return (int[,])initialBoard.Clone();
        }
        public void SetSolution(int[,] sol)
        {
            solution = (int[,])sol.Clone();
            _solution = solution;
        }

        public int[,] GetSolution()
        {
            return (int[,])solution.Clone();
            return _solution;
        }

        public void RestartGame()
        {
            currentBoard = (int[,])initialBoard.Clone();
        }

        public void ResetToInitialState()
        {
            currentBoard = (int[,])initialBoard.Clone();
        }
        public void GenerateNewGame()
        {
           
           
        }

    }
}