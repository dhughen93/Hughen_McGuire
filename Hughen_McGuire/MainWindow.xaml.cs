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

// Authors: Hughen and McGuire
// Date:    4/22/2014
// This application simulates a traditional connect four game.

namespace Hughen_McGuire
{
    public partial class MainWindow : Window
    {
        public const int COLUMNS = 7,
                         ROWS    = 6;

        public int player = 1;          // Start the game with player 1
        public int player1Total = 0,    // The number of wins for player 1
                   player2Total = 0;    // The number of wins for player 2
        public int gameFirstPlayer = 1; // The first player in the current game

        Checker[,] checkers = new Checker[COLUMNS, ROWS];

        public MainWindow()
        {
            InitializeComponent();

            mainWindow.MinHeight = 628;
            mainWindow.MinWidth = 485;
            playerTurnBlock.Text = " It's player " + player.ToString() + "'s turn to go.";
            playerTurnBlock.Foreground = Brushes.Red;
            newGame();
        }

        #region Draw the board
        // Draw the empty game board
        public void newGame()
	    {
            player1TotalBlock.Text = player1Total.ToString();
            player2TotalBlock.Text = player2Total.ToString();

		    for (int rowCounter = 0; rowCounter < ROWS; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < COLUMNS; columnCounter++)
                {
                    Point checkerLocation = new Point((65 * columnCounter + 10), (65 * rowCounter + 10));
                    checkers[columnCounter, rowCounter] = new Checker(60, Colors.Navy);
                    checkers[columnCounter, rowCounter].setLocation((int)checkerLocation.X, (int)checkerLocation.Y);
                    checkers[columnCounter, rowCounter].Draw(gameCanvas);
                }
            }
	    }
        #endregion

        #region Game board clicks
        // Click the game board
        private void clickBoard(object sender, MouseButtonEventArgs e)
        {
            Point mouseLocation = e.GetPosition(gameCanvas);
            int column = (int)(mouseLocation.X / 65);
            placeChecker(column);
        }
        #endregion

        #region Refresh the game board
        // Redraw the board
        public void refreshBoard()
        {
            for (int rowCounter = 0; rowCounter < ROWS; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < COLUMNS; columnCounter++)
                {
                    Point checkerLocation = new Point((65 * columnCounter + 10), (65 * rowCounter + 10));
                    checkers[columnCounter, rowCounter].setLocation((int)checkerLocation.X, (int)checkerLocation.Y);
                    checkers[columnCounter, rowCounter].Draw(gameCanvas);
                }
            }
        }
        #endregion

        #region Place a checker on the game board
        // Drop a token onto the game board
        public void placeChecker(int column)
        {
            int row = 0;

            try
            {
                // Check for stalemate
                if (checkStalemate() == true)
                {
                    MessageBox.Show("Stalemate; no winner.\nGame will restart.");
                    newGame();
                }

                else
                {
                    // Check if a column is full
                    if (checkers[column, row].getColor() != Colors.Navy && checkers[column, row + 1].getColor() != Colors.Navy && checkers[column, row + 2].getColor() != Colors.Navy && checkers[column, row + 3].getColor() != Colors.Navy && checkers[column, row + 4].getColor() != Colors.Navy && checkers[column, row + 5].getColor() != Colors.Navy)
                        MessageBox.Show("That column is full");

                    else
                    {
                        for (int rowCounter = 0; rowCounter < ROWS; rowCounter++)
                        {
                            // Checks to see if column is given a value too high
                            if (column > COLUMNS - 1)
                                throw new ArgumentOutOfRangeException();

                            // Check for an empty slot
                            if (checkers[column, rowCounter].getColor() == Colors.Navy)
                            {
                                // Red checker if player one
                                if (player == 1)
                                {
                                    checkers[column, rowCounter].setColor(Colors.Red);
                                    checkers[column, rowCounter].setOwner(player);
                                    refreshBoard();
                                    if (addWin() == true)
                                    {
                                        player = 1;
                                        playerTurnBlock.Foreground = Brushes.Red;
                                        playerTurnBlock.Text = " It's player " + player.ToString() + "'s turn to go.";
                                        gameFirstPlayer = 1;
                                    }
                                    else
                                    {
                                        player = 2;
                                        playerTurnBlock.Foreground = Brushes.Black;
                                        playerTurnBlock.Text = " It's player " + player.ToString() + "'s turn to go.";
                                    }
                                    break;
                                }

                                // Black checker if player two
                                else if (player == 2)
                                {
                                    checkers[column, rowCounter].setColor(Colors.Black);
                                    checkers[column, rowCounter].setOwner(player);
                                    refreshBoard();
                                    if (addWin() == true)
                                    {
                                        player = 2;
                                        playerTurnBlock.Foreground = Brushes.Black;
                                        playerTurnBlock.Text = " It's player " + player.ToString() + "'s turn to go.";
                                        gameFirstPlayer = 2;
                                    }
                                    else
                                    {
                                        player = 1;
                                        playerTurnBlock.Foreground = Brushes.Red;
                                        playerTurnBlock.Text = " It's player " + player.ToString() + "'s turn to go.";
                                    }
                                    break;
                                }
                            }
                        } 
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("I'm sorry, but that click was too far to the right. Try again!");
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("I'm sorry, but that click was too far to the right. Try again!"); 
            }
            
        }
        #endregion

        #region Add a win if there is one
        // Add 1 to a player's total if a winner is found
        public bool addWin()
        {
            // Check for a winner
            if (checkWin() == true)
            {
                MessageBox.Show("Game over! You win, Player " + player.ToString() + "!!!");
                if (player == 1)
                    player1Total++;
                else if (player == 2)
                    player2Total++;

                newGame();

                return true;
            }
            
            return false;
        }
        #endregion

        #region Check for a stalemate
        // Check for a stalemate
        public bool checkStalemate()
        {
            int column = 0,
                row = 0;
            try
            {
                if ((checkers[column, row].getColor() != Colors.Navy && checkers[column, row + 1].getColor() != Colors.Navy && checkers[column, row + 2].getColor() != Colors.Navy && checkers[column, row + 3].getColor() != Colors.Navy && checkers[column, row + 4].getColor() != Colors.Navy && checkers[column, row + 5].getColor() != Colors.Navy) &&
                    (checkers[column + 1, row].getColor() != Colors.Navy && checkers[column + 1, row + 1].getColor() != Colors.Navy && checkers[column + 1, row + 2].getColor() != Colors.Navy && checkers[column + 1, row + 3].getColor() != Colors.Navy && checkers[column + 1, row + 4].getColor() != Colors.Navy && checkers[column + 1, row + 5].getColor() != Colors.Navy) &&
                    (checkers[column + 2, row].getColor() != Colors.Navy && checkers[column + 2, row + 1].getColor() != Colors.Navy && checkers[column + 2, row + 2].getColor() != Colors.Navy && checkers[column + 2, row + 3].getColor() != Colors.Navy && checkers[column + 2, row + 4].getColor() != Colors.Navy && checkers[column + 2, row + 5].getColor() != Colors.Navy) &&
                    (checkers[column + 3, row].getColor() != Colors.Navy && checkers[column + 3, row + 1].getColor() != Colors.Navy && checkers[column + 3, row + 2].getColor() != Colors.Navy && checkers[column + 3, row + 3].getColor() != Colors.Navy && checkers[column + 3, row + 4].getColor() != Colors.Navy && checkers[column + 3, row + 5].getColor() != Colors.Navy) &&
                    (checkers[column + 4, row].getColor() != Colors.Navy && checkers[column + 4, row + 1].getColor() != Colors.Navy && checkers[column + 4, row + 2].getColor() != Colors.Navy && checkers[column + 4, row + 3].getColor() != Colors.Navy && checkers[column + 4, row + 4].getColor() != Colors.Navy && checkers[column + 4, row + 5].getColor() != Colors.Navy) &&
                    (checkers[column + 5, row].getColor() != Colors.Navy && checkers[column + 5, row + 1].getColor() != Colors.Navy && checkers[column + 5, row + 2].getColor() != Colors.Navy && checkers[column + 5, row + 3].getColor() != Colors.Navy && checkers[column + 5, row + 4].getColor() != Colors.Navy && checkers[column + 5, row + 5].getColor() != Colors.Navy) &&
                    (checkers[column + 6, row].getColor() != Colors.Navy && checkers[column + 6, row + 1].getColor() != Colors.Navy && checkers[column + 6, row + 2].getColor() != Colors.Navy && checkers[column + 6, row + 3].getColor() != Colors.Navy && checkers[column + 6, row + 4].getColor() != Colors.Navy && checkers[column + 6, row + 5].getColor() != Colors.Navy))
                    return true;
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("ERROR CHECKING STALEMATE");
            }
            
            return false;
        }
        #endregion

        #region Check for a game winner
        // Check for a winner
        public bool checkWin()
        {
            try
            {
                // Check for four in a row horizontally
                for (int rowCounter = 0; rowCounter < ROWS; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < COLUMNS - 3; columnCounter++)
                    {
                        if ((checkers[columnCounter, rowCounter].getOwner() == player) && (checkers[columnCounter + 1, rowCounter].getOwner() == player) && (checkers[columnCounter + 2, rowCounter].getOwner() == player) && (checkers[columnCounter + 3, rowCounter].getOwner() == player))
                            return true;
                    }
                }

                // Check for four in a row vertically
                for (int rowCounter = 0; rowCounter < ROWS - 3; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < COLUMNS; columnCounter++)
                    {
                        if ((checkers[columnCounter, rowCounter].getOwner() == player) && (checkers[columnCounter, rowCounter + 1].getOwner() == player) && (checkers[columnCounter, rowCounter + 2].getOwner() == player) && (checkers[columnCounter, rowCounter + 3].getOwner() == player))
                            return true;
                    }
                }

                // Check for four in a row diagonally (top left to bottom right)
                for (int rowCounter = 0; rowCounter < ROWS - 3; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < COLUMNS - 3; columnCounter++)
                    {
                        if ((checkers[columnCounter, rowCounter].getOwner() == player) && (checkers[columnCounter + 1, rowCounter + 1].getOwner() == player) && (checkers[columnCounter + 2, rowCounter + 2].getOwner() == player) && (checkers[columnCounter + 3, rowCounter + 3].getOwner() == player))
                            return true;
                    }
                }

                // Check for four in a row diagonally (bottom left to top right)
                for (int rowCounter = 0; rowCounter < ROWS - 3; rowCounter++)
                {
                    for (int columnCounter = 3; columnCounter < COLUMNS; columnCounter++)
                    {
                        if ((checkers[columnCounter, rowCounter].getOwner() == player) && (checkers[columnCounter - 1, rowCounter + 1].getOwner() == player) && (checkers[columnCounter - 2, rowCounter + 2].getOwner() == player) && (checkers[columnCounter - 3, rowCounter + 3].getOwner() == player))
                            return true;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("ERROR CHECKING WIN");
            }
            return false;
        }
        #endregion

        #region Quit the game
        // Quit the game and close the window
        private void quitGame(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Restart a match
        // Restart the current match
        private void restartMatch(object sender, RoutedEventArgs e)
        {
            player1Total = 0;
            player2Total = 0;

            player = 1;
            playerTurnBlock.Foreground = Brushes.Red;
            playerTurnBlock.Text = " It's player " + player.ToString() + "'s turn to go.";

            newGame();
        } 
        #endregion

        #region Restart a game
        // Restart the game
        private void restartGame(object sender, RoutedEventArgs e)
        {
            gameCanvas.Children.Clear();

            if (gameFirstPlayer == 2)
            {
                player = 2;
                playerTurnBlock.Foreground = Brushes.Black;
                playerTurnBlock.Text = " It's player " + player.ToString() + "'s turn to go.";
            }
            else
            {
                player = 1;
                playerTurnBlock.Foreground = Brushes.Red;
                playerTurnBlock.Text = " It's player " + player.ToString() + "'s turn to go.";
            }

            newGame();
        }
        #endregion

        #region Help button
        // Game help information
        private void btnHelpclick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("HUGHEN AND MCGUIRE CONNECT-FOUR: HELP\n----------------------------------------------------\n\nWelcome to Hughen and McGuire Connect-Four, designed and developed by David Hughen and Ethan McGuire as a programming project for Mr. Geary's CS 364 class at Pensacola Christian College\n\nThis application is meant for two players. Each player, on his turn, must select the column into which he wishes to place a checker. The two players may do this until there is a winner - when there are four same colored checkers in a row. Player 1 plays first in the beginning game of a match. The first player of all other games is determined by the winner of the last game.\n\nWhen the application is opened, a new match is started. You can restart any game in the match by hitting the Restart Game button, or you can restart the entire match by hitting the Restart Match button. The match lasts as long as you wish to play it.\n\nGood luck!");
        }
        #endregion

        #region Cool Info button
        // Show connect four history
        private void btnInfoClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("HUGHEN AND MCGUIRE CONNECT-FOUR: COOL INFORMATION\n----------------------------------------------------------------------\n\nConnect Four was first sold officially and under its now popular and trademarked name \"Connect Four\" in 1974 by Milton Bradley. Its biggest publisher is Hasbro games. The game itself, as we know it today at least, was invented by Howard Wexler in the 1940's; this is contrary to the disproved rumour that singer David Bowie came up with the game. Connect Four belongs to the \"abstract strategy\" genre of games, which means that the player does not have to rely on luck to win and the game does not rely on a theme. Other games in the abstract strategy genre are chess, checkers, and mancala. Other names for Connect Four include Four in a Row, Find Four, and Plot Four. The board does not have to be 7X6, although that is the most common variation; other sizes are 8X7, 9X7, and 10X7. There are also other variations on the gameplay, including Pop Out, Pop 10, 5 in a Row, and Power Up.");
        } 
        #endregion
    }
}