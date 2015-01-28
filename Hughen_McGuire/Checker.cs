using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

// Authors: Hughen and McGuire
// Date:    4/22/2014
// Holds the attributes of a checker piece

namespace Hughen_McGuire
{
    class Checker
    {
        private int diameter;
        private int owner;
        private int locationX = 0,
                    locationY = 0;

        private Ellipse checker = null;

        private Color color;

        // Constructor initializing the checker's diameter and color
        public Checker(int checkerDiameter, Color checkerColor)
        {
            diameter = checkerDiameter;
            color = checkerColor;
        }

        #region Getters and setters
        // Set the owner of a checker
        public void setOwner(int checkerOwner)
        {
            owner = checkerOwner;
        }

        // Get the owner of a checker
        public int getOwner()
        {
            return owner;
        }

        // Set the location of the checker
        public void setLocation(int xCoordinate, int yCoordinate)
        {
            locationX = xCoordinate;
            locationY = yCoordinate;
        }

        // Set the color of the checker
        public void setColor(Color checkerColor)
        {
            color = checkerColor;
        }

        // Get the color of the checker
        public Color getColor()
        {
            return color;
        } 
        #endregion

        #region Draw a checker
        // Draw a checker onto the game canvas
        public void Draw(Canvas gameCanvas)
        {
            if (checker != null)
                gameCanvas.Children.Remove(checker);
            else
                checker = new Ellipse();
            checker.Height = diameter;
            checker.Width = diameter;
            Canvas.SetBottom(checker, locationY);
            Canvas.SetLeft(checker, locationX);
            SolidColorBrush brush = new SolidColorBrush(color);
            checker.Fill = brush;
            gameCanvas.Children.Add(checker);
        } 
        #endregion
    }
}