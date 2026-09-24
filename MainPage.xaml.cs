namespace MauiApp1;

public partial class MainPage : ContentPage
{
    private readonly string[] _board = new string[9];
    private string _currentPlayer = "X";
    private bool _gameFinished;

    public MainPage()
    {
        InitializeComponent();
        ResetBoard();
    }

    private void OnCellClicked(object? sender, EventArgs e)
    {
        if (_gameFinished || sender is not Button button || !string.IsNullOrEmpty(button.Text))
            return;

        var index = GetButtonIndex(button);
        if (index < 0)
            return;

        _board[index] = _currentPlayer;
        button.Text = _currentPlayer;
        button.IsEnabled = false;

        if (CheckForWinner())
        {
            StatusLabel.Text = $"Player {_currentPlayer} wins!";
            _gameFinished = true;
            return;
        }

        if (_board.All(cell => !string.IsNullOrEmpty(cell)))
        {
            StatusLabel.Text = "It's a draw!";
            _gameFinished = true;
            return;
        }

        _currentPlayer = _currentPlayer == "X" ? "O" : "X";
        StatusLabel.Text = $"Player {_currentPlayer}'s turn";
    }

    private void OnResetClicked(object? sender, EventArgs e)
    {
        ResetBoard();
    }

    private void ResetBoard()
    {
        Array.Fill(_board, string.Empty);
        _currentPlayer = "X";
        _gameFinished = false;
        StatusLabel.Text = "Player X's turn";

        foreach (var cell in new[] { Cell00, Cell01, Cell02, Cell10, Cell11, Cell12, Cell20, Cell21, Cell22 })
        {
            cell.Text = string.Empty;
            cell.IsEnabled = true;
        }
    }

    private int GetButtonIndex(Button button)
    {
        var buttonName = button.StyleId;
        if (string.IsNullOrEmpty(buttonName))
            buttonName = button.AutomationId;

        return buttonName switch
        {
            "Cell00" => 0,
            "Cell01" => 1,
            "Cell02" => 2,
            "Cell10" => 3,
            "Cell11" => 4,
            "Cell12" => 5,
            "Cell20" => 6,
            "Cell21" => 7,
            "Cell22" => 8,
            _ => -1
        };
    }

    private bool CheckForWinner()
    {
        int[,] winningLines =
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8},
            {0, 4, 8}, {2, 4, 6}
        };

        for (int i = 0; i < winningLines.GetLength(0); i++)
        {
            int a = winningLines[i, 0];
            int b = winningLines[i, 1];
            int c = winningLines[i, 2];

            if (!string.IsNullOrEmpty(_board[a]) && _board[a] == _board[b] && _board[a] == _board[c])
                return true;
        }

        return false;
    }
}
