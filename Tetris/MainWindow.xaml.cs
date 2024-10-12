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
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GAMESPEED = 700;// millisecond
        List<System.Media.SoundPlayer> soundList = new List<System.Media.SoundPlayer>();
        DispatcherTimer timer;
        Random shapeRandom;
        private int rowCount = 0;
        private int columnCount = 0;
        private int leftPos = 0;
        private int downPos = 0;
        private int currentTetrominoWidth;
        private int currentTetrominoHeigth;
        private int currentShapeNumber;
        private int nextShapeNumber;
        private int tetrisGridColumn;
        private int tetrisGridRow;
        private int rotation = 0;
        private bool gameActive = false;
        private bool nextShapeDrawed = false;
        private int[,] currentTetromino = null;
        private bool isRotated = false;
        private bool bottomCollided = false;
        private bool leftCollided = false;
        private bool rightCollided = false;
        private bool isGameOver = false;
        private int gameSpeed;
        private int levelScale = 60;// every 60 second increase level by 1 until 10
        private double gameSpeedCounter = 0;
        private int gameLevel = 1;
        private int gameScore = 0;
        private static Color O_TetrominoColor = Colors.GreenYellow;
        private static Color I_TetrominoColor = Colors.Red;
        private static Color T_TetrominoColor = Colors.Gold;
        private static Color S_TetrominoColor = Colors.Violet;
        private static Color Z_TetrominoColor = Colors.DeepSkyBlue;
        private static Color J_TetrominoColor = Colors.Cyan;
        private static Color L_TetrominoColor = Colors.LightSeaGreen;
        List<int> currentTetrominoRow = null;
        List<int> currentTetrominoColumn = null;

        // Color for shape tetromino
        Color[] shapeColor = {  O_TetrominoColor,I_TetrominoColor,
                                T_TetrominoColor,S_TetrominoColor,
                                Z_TetrominoColor,J_TetrominoColor,
                                L_TetrominoColor
                             };

        // ---------
        string[] arrayTetrominos = { "","O_Tetromino" , "I_Tetromino_0",
                                        "T_Tetromino_0","S_Tetromino_0",
                                        "Z_Tetromino_0","J_Tetromino_0",
                                        "L_Tetromino_0"
                                   };

        // arrays of tetromino shape
        //---- O Tetromino------------
        public int[,] O_Tetromino = new int[2, 2] { { 1, 1 },  // * *
                                                    { 1, 1 }}; // * *

        //---- I Tetromino------------
        public int[,] I_Tetromino_0 = new int[2, 4] { { 1, 1, 1, 1 }, { 0, 0, 0, 0 } };// * * * *

        public int[,] I_Tetromino_90 = new int[4, 2] {{ 1,0 },   // *  
                                                       { 1,0 },  // *
                                                       { 1,0 },  // *
                                                       { 1,0 }}; // *
        //---- T Tetromino------------
        public int[,] T_Tetromino_0 = new int[2, 3] {{0,1,0},    //    * 
                                                     {1,1,1}};   //  * * *

        public int[,] T_Tetromino_90 = new int[3, 2] {{1,0},     //  * 
                                                      {1,1},     //  * *
                                                      {1,0}};

        public int[,] T_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * *
                                                       {0,1,0}}; //   * 

        public int[,] T_Tetromino_270 = new int[3, 2] {{0,1},    //   * 
                                                       {1,1},    // * *
                                                       {0,1}};   //   *  

        //---- S Tetromino------------
        public int[,] S_Tetromino_0 = new int[2, 3] {{0,1,1},    //   * *
                                                     {1,1,0}};   // * *

        public int[,] S_Tetromino_90 = new int[3, 2] {{1,0},     // *
                                                      {1,1},     // * *
                                                      {0,1}};    //   *

        //---- Z Tetromino------------
        public int[,] Z_Tetromino_0 = new int[2, 3] {{1,1,0},    // * *
                                                     {0,1,1}};   //   * *

        public int[,] Z_Tetromino_90 = new int[3, 2] {{0,1},     //   *
                                                      {1,1},     // * *
                                                      {1,0}};    // *

        //---- J Tetromino------------
        public int[,] J_Tetromino_0 = new int[2, 3] {{1,0,0},    // * 
                                                     {1,1,1}};   // * * *

        public int[,] J_Tetromino_90 = new int[3, 2] {{1,1},     // * * 
                                                      {1,0},     // *
                                                      {1,0}};    // * 

        public int[,] J_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * * 
                                                       {0,0,1}}; //     *

        public int[,] J_Tetromino_270 = new int[3, 2] {{0,1},    //   * 
                                                       {0,1},    //   *
                                                       {1,1 }};  // * *

        //---- L Tetromino------------
        public int[,] L_Tetromino_0 = new int[2, 3] {{0,0,1},    //     * 
                                                     {1,1,1}};   // * * *

        public int[,] L_Tetromino_90 = new int[3, 2] {{1,0},     // *  
                                                      {1,0},     // *
                                                      {1,1}};    // * *

        public int[,] L_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * * 
                                                       {1,0,0}}; // *

        public int[,] L_Tetromino_270 = new int[3, 2] {{1,1},    // * * 
                                                       {0,1},    //   *
                                                       {0,1 }};  //   *

        public object Task { get; private set; }


        public MainWindow()
        {
            InitializeComponent();
            gameSpeed = GAMESPEED;
            //created event for key press
            KeyDown += MainWindow_KeyDown;
            // init timer
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, gameSpeed); // 700 millisecond
            timer.Tick += Timer_Tick;
            tetrisGridColumn = tetrisGrid.ColumnDefinitions.Count;
            tetrisGridRow = tetrisGrid.RowDefinitions.Count;
            shapeRandom = new Random();
            currentShapeNumber = shapeRandom.Next(1, 8);
            nextShapeNumber = shapeRandom.Next(1, 8);
            nextTxt.Visibility = levelTxt.Visibility = GameOverTxt.Visibility = Visibility.Collapsed;
            // Add the 2 wav sound in list
            soundList.Add(new System.Media.SoundPlayer(Properties.Resources.collided));
            soundList.Add(new System.Media.SoundPlayer(Properties.Resources.deleteLine));
        }

        // Key event method for moving shape down,rigth,left and rotation
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

            if (!timer.IsEnabled) { return; }
            switch (e.Key.ToString())
            {
                case "Up":
                    rotation += 90;
                    if (rotation > 270) { rotation = 0; }
                    shapeRotation(rotation);
                    break;
                case "Down":
                    downPos++;
                    break;
                case "Right":
                    // Check if collided
                    TetroCollided();
                    if (!rightCollided) { leftPos++; }
                    rightCollided = false;
                    break;
                case "Left":
                    // Check if collided
                    TetroCollided();
                    if (!leftCollided) { leftPos--; }
                    leftCollided = false;
                    break;
            }
            moveShape();
        }

        // Rotation tetrominos 
        private void shapeRotation(int _rotation)
        {
            // Check if collided
            if (rotationCollided(rotation))
            {
                rotation -= 90;
                return;
            }

            if (arrayTetrominos[currentShapeNumber].IndexOf("I_") == 0)
            {
                if (_rotation > 90) { _rotation = rotation = 0; }
                currentTetromino = getVariableByString("I_Tetromino_" + _rotation);
            }
            else if (arrayTetrominos[currentShapeNumber].IndexOf("T_") == 0)
            {
                currentTetromino = getVariableByString("T_Tetromino_" + _rotation);
            }
            else if (arrayTetrominos[currentShapeNumber].IndexOf("S_") == 0)
            {
                if (_rotation > 90) { _rotation = rotation = 0; }
                currentTetromino = getVariableByString("S_Tetromino_" + _rotation);
            }
            else if (arrayTetrominos[currentShapeNumber].IndexOf("Z_") == 0)
            {
                if (_rotation > 90) { _rotation = rotation = 0; }
                currentTetromino = getVariableByString("Z_Tetromino_" + _rotation);
            }
            else if (arrayTetrominos[currentShapeNumber].IndexOf("J_") == 0)
            {
                currentTetromino = getVariableByString("J_Tetromino_" + _rotation);
            }
            else if (arrayTetrominos[currentShapeNumber].IndexOf("L_") == 0)
            {
                currentTetromino = getVariableByString("L_Tetromino_" + _rotation);
            }
            else if (arrayTetrominos[currentShapeNumber].IndexOf("O_") == 0) // Do not rotate this
            {
                return;
            }

            isRotated = true;
            addShape(currentShapeNumber, leftPos, downPos);
        }

        // Timer tick method for moving shape down
        private void Timer_Tick(object sender, EventArgs e)
        {
            downPos++;
            moveShape();
            if (gameSpeedCounter >= levelScale)
            {
                if (gameSpeed >= 50)
                {
                    gameSpeed -= 50;
                    gameLevel++;
                    levelTxt.Text = "Level: " + gameLevel.ToString();
                }
                else { gameSpeed = 50; }
                timer.Stop();
                timer.Interval = new TimeSpan(0, 0, 0, 0, gameSpeed);
                timer.Start();
                gameSpeedCounter = 0;
            }
            gameSpeedCounter += (gameSpeed / 1000f);

        }

        // Button start stop clicked method
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (isGameOver)
            {
                tetrisGrid.Children.Clear();
                nextShapeCanvas.Children.Clear();
                GameOverTxt.Visibility = Visibility.Collapsed;
                isGameOver = false;
            }
            if (!timer.IsEnabled)
            {
                if (!gameActive) { scoreTxt.Text = "0"; leftPos = 3; addShape(currentShapeNumber, leftPos); }
                nextTxt.Visibility = levelTxt.Visibility = Visibility.Visible;
                levelTxt.Text = "Level: " + gameLevel.ToString();
                timer.Start();
                startStopBtn.Content = "Stop Game";
                gameActive = true;
            }
            else
            {
                timer.Stop();
                startStopBtn.Content = "Start Game";
            }
        }

        // Add new shape tetromino in grid
        private void addShape(int shapeNumber, int _left = 0, int _down = 0)
        {
            // Remove previous position of tetromino
            removeShape();
            currentTetrominoRow = new List<int>();
            currentTetrominoColumn = new List<int>();
            Rectangle square = null;
            if (!isRotated)
            {
                currentTetromino = null;
                currentTetromino = getVariableByString(arrayTetrominos[shapeNumber].ToString());
            }
            int firstDim = currentTetromino.GetLength(0);
            int secondDim = currentTetromino.GetLength(1);
            currentTetrominoWidth = secondDim;
            currentTetrominoHeigth = firstDim;
            // This is only for I Tetromino
            if (currentTetromino == I_Tetromino_90)
            {
                currentTetrominoWidth = 1;
            }
            else if (currentTetromino == I_Tetromino_0) { currentTetrominoHeigth = 1; }
            //------------------------------------
            for (int row = 0; row < firstDim; row++)
            {
                for (int column = 0; column < secondDim; column++)
                {
                    int bit = currentTetromino[row, column];
                    if (bit == 1)
                    {
                        square = getBasicSquare(shapeColor[shapeNumber - 1]);
                        tetrisGrid.Children.Add(square);
                        square.Name = "moving_" + Grid.GetRow(square) + "_" + Grid.GetColumn(square);
                        if (_down >= tetrisGrid.RowDefinitions.Count - currentTetrominoHeigth)
                        {
                            _down = tetrisGrid.RowDefinitions.Count - currentTetrominoHeigth;
                        }
                        Grid.SetRow(square, rowCount + _down);
                        Grid.SetColumn(square, columnCount + _left);
                        currentTetrominoRow.Add(rowCount + _down);
                        currentTetrominoColumn.Add(columnCount + _left);

                    }
                    columnCount++;
                }
                columnCount = 0;
                rowCount++;
            }
            columnCount = 0;
            rowCount = 0;
            if (!nextShapeDrawed)
            {
                drawNextShape(nextShapeNumber);
            }
        }

        // Add new shape in new location
        private void moveShape()
        {
            leftCollided = false;
            rightCollided = false;

            // Check if collided
            TetroCollided();
            if (leftPos > (tetrisGridColumn - currentTetrominoWidth))
            {
                leftPos = (tetrisGridColumn - currentTetrominoWidth);
            }
            else if (leftPos < 0) { leftPos = 0; }

            if (bottomCollided)
            {
                shapeStoped();
                return;
            }
            addShape(currentShapeNumber, leftPos, downPos);
        }

        // Check collided if rotated tetromino 
        private bool rotationCollided(int _rotation)
        {
            if (checkCollided(0, currentTetrominoWidth - 1)) { return true; }//Bottom collision 
            else if (checkCollided(0, -(currentTetrominoWidth - 1))) { return true; }// Top collision
            else if (checkCollided(0, -1)) { return true; }// Top collision
            else if (checkCollided(-1, currentTetrominoWidth - 1)) { return true; }// Left collision
            else if (checkCollided(1, currentTetrominoWidth - 1)) { return true; }// Right collision
            return false;
        }

        // Check if collided in sides , bottom and other shapes 
        private void TetroCollided()
        {
            bottomCollided = checkCollided(0, 1);
            leftCollided = checkCollided(-1, 0);
            rightCollided = checkCollided(1, 0);
        }


    }
}
