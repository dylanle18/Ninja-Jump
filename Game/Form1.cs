//Dylan Le
//Game
//ICS 3U1
//12/15/2017
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace Game
{
    public partial class Form1 : Form
    {
        Label playLabel, infoLabel, exitLabel; //variables for the menu buttons
        Image menuImage, titleImage, ExitImage; //the menu backgoung
        Rectangle menuRectangle, titleRectangle; //rectangle for the all the menu picture
        Timer mainTimer; //sets timer
        Timer menuTimer; //sets timer
        Timer playTimer; //sets timer
        Label yesLabel, noLabel; //varibles for the exit labels
        Rectangle[] platforms = new Rectangle[5]; //creates array for platforms
        Image clouds; //variable for the cloud pic
        Random randomNumberMaker = new Random();
        Image ninjaleft, ninjaright; //variable for the ninja pic
        Rectangle player; //variable for the player rec
        Image enemyninja; //variable for the enemy pic
        Rectangle[] enemy = new Rectangle[2]; //variable for the enemy rec
        Timer hitTimer; //variable for hit timer
        Image sky; //variable for sky pic
        Rectangle playBackground; //variable for play back rec
        Label scoreLabel, highscoreLabel; //variable for score and hs labels
        Timer scoreTimer; //timer for score
        Image InfoImage; //variable for info image
        Label backLabel; //creates label to go back
        int score = 0, highscore; //variable for actualy score and hs
        bool canMove = false; //creates a bool to see whether a player is allowed to move
        int dx; //variable for dx
        int speed = 13; //variables for speed
        bool isRight = false;

        StreamReader fileReader; //creates file writer
        StreamWriter fileWriter; //creates file reader

        //JUMPING
        int yLimit; //variable fore y limit
        int upper, gravity; //variable for upper and gravity
        bool jumping; //variable for jumping
        Timer jumpTimer; //variable for the jump timer

        SoundPlayer clickSound; //variable for the click sound
        SoundPlayer bounceSound; //variable for the bounce sound
        SoundPlayer hitSound; //variable for the hit sound
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //THE FORM
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            this.Size = new Size(800, 600); //changes the size of the form
            this.MaximizeBox = false; //so you cant maximize the form
            this.MinimizeBox = false; //so you cant minimize the form
            this.MaximumSize = this.Size; //sets the maximum size of the form
            this.MinimumSize = this.Size; //sets the minimum size of the form
            this.Text = "NINJA JUMP"; //changes the name of the form
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            sky = Image.FromFile(Application.StartupPath + @"\Images\skytospace.png", true); //sets the picture to the sky

            ninjaleft = Image.FromFile(Application.StartupPath + @"\Images\ninjaleft.png", true); //sets the picture to the ninja left

            ninjaright = Image.FromFile(Application.StartupPath + @"\Images\ninjaleft.png", true); //sets the picture to the ninja right

            ninjaright.RotateFlip(RotateFlipType.Rotate180FlipY); //flips the image

            enemyninja = Image.FromFile(Application.StartupPath + @"\Images\enemy.png", true); //sets the picture to the enemy

            clouds = Image.FromFile(Application.StartupPath + @"\Images\cloud.png", true); //sets the picture to the platforms

            ExitImage = Image.FromFile(Application.StartupPath + @"\Images\ExitImage.png", true); //sets the picture to the menu image variable

            menuImage = Image.FromFile(Application.StartupPath + @"\Images\MenuBack.jpg", true); //sets the picture to the menu image variable
            menuRectangle = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height); //creates the rectangle

            titleImage = Image.FromFile(Application.StartupPath + @"\Images\NinjaJump.png", true); //sets the picture to the title image varible
            titleRectangle = new Rectangle(-10, -10, titleImage.Width, titleImage.Height); //creates the rectangle

            InfoImage = Image.FromFile(Application.StartupPath + @"\Images\InfoImage.png", true); //sets the picture to the info image variable

            ExitImage = Image.FromFile(Application.StartupPath + @"\Images\ExitImage.png", true); //sets the picture to the menu image variable

            mainTimer = new Timer(); //creates the timer
            mainTimer.Tick += MainTimer_Tick; //sets up the timer method
            mainTimer.Interval = 1000 / 60; //sets the timer interval
            mainTimer.Start(); //starts the timer

            menuTimer = new Timer(); //creates the timer
            menuTimer.Tick += MenuTimer_Tick; //sets up the timer method
            menuTimer.Interval = 1000 / 30; //sets the timer interval

            scoreTimer = new Timer(); //creates the timer
            scoreTimer.Tick += ScoreTimer_Tick;  //sets up the timer method
            scoreTimer.Interval = 1000; //sets the timer interval

            playTimer = new Timer(); //creates the timer
            playTimer.Tick += PlayTimer_Tick; //sets up the timer method
            playTimer.Interval = 1000 / 60; //sets the timer interval

            hitTimer = new Timer(); //creates the timer
            hitTimer.Tick += HitTimer_Tick; //sets up the timer method
            hitTimer.Interval = 1000 / 60; //sets the timer interval

            KeyDown += Form1_KeyDown; //creates method for key up
            KeyUp += Form1_KeyUp; //creates the method for key down

            AddMenu(); //calls for the addMenu method

            yLimit = this.ClientSize.Height; //sets the ylimit
            
            jumping = false; //makes you not jumping to start

            //sets up jump timer
            jumpTimer = new Timer();
            jumpTimer.Interval = 1000 / 60;
            jumpTimer.Tick += JumpTimer_Tick;

            this.Paint += Form1_Paint; //sets up the paint method
            this.DoubleBuffered = true; //makes it so the form buffers

            clickSound = new SoundPlayer(); //creates the click sound
            clickSound.SoundLocation = Application.StartupPath + @"\Sounds\click.wav"; //find the sound in the debug folder
            bounceSound = new SoundPlayer(); //creates the bounce sound
            bounceSound.SoundLocation = Application.StartupPath + @"\Sounds\bounce.wav"; //find the sound in the debug folder
            hitSound = new SoundPlayer(); //creates the hit sound
            hitSound.SoundLocation = Application.StartupPath + @"\Sounds\hit.wav"; //find the sound in the debug folder
        }

        //MENU METHODS
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AddMenu() //method to add the menu
        {
            menuTimer.Start(); //starts the timer

            playLabel = new Label(); //creates the play label
            infoLabel = new Label(); //creates the info label
            exitLabel = new Label(); //creates the exit label

            playLabel.MouseClick += PlayLabel_MouseClick; //creates the mouse click method for play label
            infoLabel.MouseClick += InfoLabel_MouseClick; //creates the mouse click method for info label
            exitLabel.MouseClick += ExitLabel_MouseClick; //creates the mouse click method for exit label

            playLabel.MouseEnter += PlayLabel_MouseEnter; //creates the mouse enter method for play label
            playLabel.MouseLeave += PlayLabel_MouseLeave; //creates the mouse leave method for play label

            infoLabel.MouseEnter += InfoLabel_MouseEnter; //creates the mouse enter method for info label
            infoLabel.MouseLeave += InfoLabel_MouseLeave; //creates the mouse leave method for play label

            exitLabel.MouseEnter += ExitLabel_MouseEnter; //creates the mouse enter method for exit label
            exitLabel.MouseLeave += ExitLabel_MouseLeave; //creates the mouse leave method for play label

            this.Controls.Add(playLabel); //adds the play label to the form
            this.Controls.Add(infoLabel); //adds the info label to the form
            this.Controls.Add(exitLabel); //adds the exit label to the form

            playLabel.Size = new Size(200, 70); //sets the size of the play label
            infoLabel.Size = new Size(220, 70); //sets the size of the info label
            exitLabel.Size = new Size(200, 70); //sets the size of the exit label

            playLabel.Location = new Point(this.ClientSize.Width / 2 - 60, 150 + 25); //changes the location of the play label
            infoLabel.Location = new Point(this.ClientSize.Width / 2 - 60, this.ClientSize.Height / 2 - infoLabel.Height / 2 + 50); //changes the location of the play label
            exitLabel.Location = new Point(this.ClientSize.Width / 2 - 60, this.ClientSize.Height - exitLabel.Height - 150 + 75); //changes the location of the play label

            playLabel.Text = "PLAY"; //sets the text of the play label
            infoLabel.Text = "INFO"; //sets the text of the info label
            exitLabel.Text = "EXIT"; //sets the text of the exit label

            playLabel.Font = new Font("Mistral", 50, FontStyle.Bold); //changes the font and size of the play label
            infoLabel.Font = new Font("Mistral", 50, FontStyle.Bold); //changes the font and size of the info label
            exitLabel.Font = new Font("Mistral", 50, FontStyle.Bold); //changes the font and size of the exit label

            playLabel.ForeColor = Color.White; //changes the colour of play label
            infoLabel.ForeColor = Color.White; //changes the colour of info label
            exitLabel.ForeColor = Color.White; //changes the colour of exit label

            playLabel.BackColor = Color.Transparent; //makes the backgorund of the play label transparent
            infoLabel.BackColor = Color.Transparent; //makes the backgorund of the info label transparent
            exitLabel.BackColor = Color.Transparent; //makes the backgorund of the exit label transparent

            menuRectangle.Size = new Size(this.ClientSize.Width, this.ClientSize.Height); //sets the size of the rectangle
            menuRectangle.Location = new Point(0, 0); //moves the rectangle
            titleRectangle.Location = new Point(-10, -10); //moves the rectangle
        }

        private void MenuTimer_Tick(object sender, EventArgs e) //method for the menu timer
        {
            menuRectangle.Size = new Size(menuRectangle.Width + 2, menuRectangle.Height + 2); //increases the size of the picture
            menuRectangle.X -= 1; //moves the picture left
            menuRectangle.Y -= 1; //moves the picture up

            if (menuRectangle.Width >= (this.ClientSize.Width + 1000))  //if the size of the picture is a certain size
            {
                menuRectangle.Size = this.Size; //set the size back down to the form size
                menuRectangle.X = 0; //sets the x to 0
                menuRectangle.Y = 0; //sets the y to 0
            }
        }

        private void MainTimer_Tick(object sender, EventArgs e) //method for the main timer
        {
            this.Invalidate(); //redraws the form
        }

        private void Form1_Paint(object sender, PaintEventArgs e) //method to paint rectangles
        {
            e.Graphics.DrawImage(sky, playBackground);
            e.Graphics.DrawImage(menuImage, menuRectangle); //draws the menu image in the rectangle
            e.Graphics.DrawImage(titleImage, titleRectangle); //draws the title image in the rectangle

            if (dx < 0 || isRight == false) //if the player is moving left
            {
                e.Graphics.DrawImage(ninjaleft, player); //draws the ninja to the player rec
                isRight = false; //sets is right to false
            }
            if (dx > 0 || isRight == true) //if the player is moving right
            {
                e.Graphics.DrawImage(ninjaright, player); //draws the ninja to the player rec
                isRight = true; //sets is right to true
            }
            for (int i = 0; i < 5; i++) //loops 5 times
            {
                e.Graphics.DrawImage(clouds, platforms[i]); //draws the cloud in the rectangle
            }  
            for (int i = 0; i < 2; i++) //loops 2 times
            {
                e.Graphics.DrawImage(enemyninja, enemy[i]); //draws the enemy ninja to the enemy rec
            }
        }

        private void ExitLabel_MouseLeave(object sender, EventArgs e) //method for when the mouse leaves the label
        {
            exitLabel.Location = new Point(this.ClientSize.Width / 2 - 60, this.ClientSize.Height - exitLabel.Height - 150 + 75); //changes the location of the play label
            exitLabel.Text = "EXIT"; //sets the text of the exit label
        }

        private void ExitLabel_MouseEnter(object sender, EventArgs e) //method for when the mouse enters the label
        {
            clickSound.Play();
            exitLabel.Location = new Point(this.ClientSize.Width / 2 - 90, this.ClientSize.Height - exitLabel.Height - 150 + 75); //changes the location of the play label
            exitLabel.Text = "E X I T"; //sets the text of the exit label
        }

        private void InfoLabel_MouseLeave(object sender, EventArgs e) //method for when the mouse leaves the label
        {
            infoLabel.Location = new Point(this.ClientSize.Width / 2 - 60, this.ClientSize.Height / 2 - infoLabel.Height / 2 + 50); //changes the location of the play label
            infoLabel.Text = "INFO"; //sets the text of the info label
        }

        private void InfoLabel_MouseEnter(object sender, EventArgs e) //method for when the mouse enters the label
        {
            clickSound.Play();
            infoLabel.Location = new Point(this.ClientSize.Width / 2 - 90, this.ClientSize.Height / 2 - infoLabel.Height / 2 + 50); //changes the location of the play label
            infoLabel.Text = "I N F O"; //sets the text of the info label
        }

        private void PlayLabel_MouseLeave(object sender, EventArgs e) //method for when the mouse leaves the label
        {
            playLabel.Location = new Point(this.ClientSize.Width / 2 - 60, 150 + 25); //changes the location of the play label
            playLabel.Text = "PLAY"; //sets the text of the play label
        }

        private void PlayLabel_MouseEnter(object sender, EventArgs e) //method for when the mouse enters the label
        {
            clickSound.Play();
            playLabel.Location = new Point(this.ClientSize.Width / 2 - 90, 150 + 25); //changes the location of the play label
            playLabel.Text = "P L A Y"; //sets the text of the play label
        }

        private void ExitLabel_MouseClick(object sender, MouseEventArgs e) //method for when the exit label is clicked
        {
            this.BackgroundImage = ExitImage; //changes the backgorund image
            RemoveMenu(); //calls on the remove menu method
            RemovePlay(); //removes the play screen
            AddExit(); //calls for the exit method
        }

        private void InfoLabel_MouseClick(object sender, MouseEventArgs e) //method for when the info label is clicked
        {
            this.BackgroundImage = InfoImage;
            RemoveMenu(); //calls on the remove menu method
            AddInfo();
        }

        private void PlayLabel_MouseClick(object sender, MouseEventArgs e) //method for when the play label is clicked
        {
            RemoveMenu(); //calls on the remove menu method
            RemovePlay(); //removes the play screen
            AddPlay(); //calls for the play method
        }

        private void RemoveMenu() //method to remove the menu
        {
            this.Controls.Remove(playLabel); //removes the play label
            this.Controls.Remove(infoLabel); //removes the info label
            this.Controls.Remove(exitLabel); //removes the exit label

            menuRectangle.Location = new Point(1000, 1000); //relocates the menu rectangle
            titleRectangle.Location = new Point(1000, 1000); //relocates the title rectangle

            menuTimer.Stop(); //stops the timer
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //PLAY METHODS
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AddPlay() //main method for the actualy game
        {
            jumping = false; //sets jumping to false

            if (File.Exists(Application.StartupPath + @"\data.txt")) //checks if the file exists
            {
                fileReader = new StreamReader(Application.StartupPath + @"\data.txt"); //opens the file reader
                highscore = Convert.ToInt32(fileReader.ReadLine()); //reads the highscore and sets it to the variable
                fileReader.Close(); //closes the file reader
            }
            else //else
            {
                fileWriter = new StreamWriter(Application.StartupPath + @"\data.txt"); //open the fle writer
                fileWriter.WriteLine("0"); //writes 0 in the file
                fileWriter.Close(); //closes the file
            }

            scoreLabel = new Label(); //creates score label
            score = 0; //sets score to 0
            scoreLabel.Text = "SCORE:" + score; //sets the text of the score label
            this.Controls.Add(scoreLabel); //adds it to the form
            scoreLabel.Font = new Font("Mistral", 30, FontStyle.Bold); //changes the font of the label
            scoreLabel.Size = new Size(1000, 40); //changes the size
            scoreLabel.ForeColor = Color.White; //sets text to while
            scoreLabel.BackColor = Color.Transparent; //sets background to transparent
            scoreLabel.Top = 40; //moves the label

            highscoreLabel = new Label(); //creates the highscore label
            highscoreLabel.Text = "HIGHSCORE:" + highscore; //sets the text of the label
            this.Controls.Add(highscoreLabel); //adds the label to the form
            highscoreLabel.Font = new Font("Mistral", 30, FontStyle.Bold); //changes the fond of the label
            highscoreLabel.Size = new Size(1000, 40); //changes the size
            highscoreLabel.ForeColor = Color.White; //sets the text to white
            highscoreLabel.BackColor = Color.Transparent; //sets background to transparent

            canMove = true; //sets can move to true

            playBackground = new Rectangle(0, -5400, 800, 6000); //moves the play backgorund

            player = new Rectangle(this.ClientSize.Width / 2 - ninjaleft.Width / 6, this.ClientSize.Height - ninjaleft.Height / 3, ninjaleft.Width / 3, ninjaleft.Height / 3); //creates the rec for the player
            
            for (int i = 0; i < 5; i++) //loops 5 times
            {
                platforms[i] = new Rectangle(randomNumberMaker.Next(700), i * 170  - 800, clouds.Width / 3, clouds.Height / 3); //creates the rectangle
            }

            enemy[0] = new Rectangle(randomNumberMaker.Next(700), -800, enemyninja.Width / 3, enemyninja.Height / 3); //creates rec for enemy 1
            enemy[1] = new Rectangle(randomNumberMaker.Next(700), -100, enemyninja.Width / 3, enemyninja.Height / 3); //creates rec for enemy 2
        }

        private void RemovePlay() //method for removing the play
        {
            canMove = false; //sets can move to false

            this.Controls.Remove(scoreLabel); //removes score label
            this.Controls.Remove(highscoreLabel); //removes high score label
            playBackground.Location = new Point(1000, 1000); //moves the background rec
            player.Location = new Point(1000, 1000); //moves the player
            for (int i = 0; i < 5; i++) //loops 5 times
            {
                platforms[i].Location = new Point(1000, 1000); //moves all the platforms
            }

            for (int i = 0; i < 2; i++) //loops 2 times
            {
                enemy[i].Location = new Point(1000, 1000); //moves all the enemies
            }
        }
        
        private void ScoreTimer_Tick(object sender, EventArgs e) //timer for score
        {
            score += 1; //adds 1 to the score
            scoreLabel.Text = "SCORE:" + score; //changes text of the score label

            if (score > highscore) //if the score is greater then the highscore
            {
                highscore = score; //makes highscore = score
                highscoreLabel.Text = "HIGHSCORE:" + highscore; //changes the text of the high score
                fileWriter = new StreamWriter(Application.StartupPath + @"\data.txt"); //opens the file writer
                fileWriter.WriteLine(highscore); //writes in the high score
                fileWriter.Close(); //closes the file writer
            }
        }

        private void HitTimer_Tick(object sender, EventArgs e) //method for hit timer
        {
            enemy[0].Y += 4; //moves the enemies down 6 pixel

            enemy[1].Y += 6; //moves the enemies down 4 pixel

            if (player.IntersectsWith(enemy[0]) || player.IntersectsWith(enemy[1]) || player.Bottom >= this.ClientSize.Height) //if the player touches an enemy for touches the bottom of the form
            {
                hitSound.Play(); //stops the timer
                hitTimer.Stop(); //stops the timer
                playTimer.Stop(); //stops the timer
                jumpTimer.Stop(); //stops the timer
                scoreTimer.Stop(); //stops the timer
                MessageBox.Show("GAME OVER"); //shows game over in a message box
                RemovePlay(); //calls on the remove play method
                RemoveMenu(); //calls on the remove menu method
                AddMenu(); //calls the add menu method
            }
        }

        private void JumpTimer_Tick(object sender, EventArgs e) //method for jump timer
        {
            //if you are jumping
            if (jumping)
            {
                scoreTimer.Start(); //starts the score timer

                hitTimer.Start(); //starts the hit timer

                //if you are not yet at the ylimit i.e. you are

                //still falling

                if (player.Bottom <= yLimit)
                {
                    //continue with jump
                    jumping = true;
                    //move player up by the upper value
                    player.Y -= upper;
                    //move player down by the gravity value
                    player.Y += gravity;
                    //if you still have a positive upper value
                    //reduce the upper value by 1 to slowly

                    //reach the peak of your jump

                    if (upper > 0)
                    {
                        --upper;
                    }
                    //if you hit the yLimit
                    if (player.Bottom >= yLimit)
                    {
                        //stop jumping
                        jumping = false;
                        //set the bottom of the player to the
                        //yLimit

                        player.Y = yLimit - player.Height;
                        //force upper to 0

                        upper = 0;

                        //force gravity to 0

                        gravity = 0;

                    }
                }
            }

            for (int i = 0; i < 5; i++) //loops 5 times
            {
                //when space key is pushed
                if (player.IntersectsWith(platforms[i]) && player.Bottom >= platforms[i].Top + 25)
                {
                    bounceSound.Play();
                    //start jumping
                    jumping = true;
                    //set an upper value
                    upper = 50;
                    //set a gavity value
                    gravity = 32;
                }
            }
        }

        private void PlayTimer_Tick(object sender, EventArgs e) //method for the play timer
        {
            player.X += dx; //moves the player

            if (player.Y <= 0) //if player is over the top of the form
            {
                upper = 0; //sets upper to 0

                gravity = 20; //sets gravity to 20

                bounceSound.Play(); //plays the bounce sound
            }

            if (playBackground.Y != 0) //if backgound isnt y = 0
            {
                playBackground.Y += 1; //move the background down 1 pixel
            }

            if (player.X + player.Width / 2 < 0) //if the middle of the player is <= the left side of form
            {
                player.X = this.ClientSize.Width - player.Width / 2; //tp the player to the right side
            }

            if (player.X + player.Width / 2 > this.ClientSize.Width) //if the middle of the player is >= the right side of form
            {
                player.X = 0 - player.Width / 2; //tp the player to the left side
            }

            for (int i = 0; i < 5; i++) //loops 6 times
            {
                platforms[i].Y += 3; //moves all the platforms down 3 pixels
                if (platforms[i].Y > 700) //if a platform is > then 700 pixels on y
                {
                    platforms[i].Location = new Point(randomNumberMaker.Next(700), -200); //tps platform to random x and -130 y
                }
            }

            for (int i = 0; i < 2; i++) //loops 2 times
            {
                if (enemy[i].Y > 1000) //if enemy is > then 700 pixels on y
                {
                    enemy[i].Location = new Point(randomNumberMaker.Next(700), -800); //tp the enemy to a random x location and -800 y
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) //key up method
        {
            if (e.KeyCode.Equals(Keys.Left) || e.KeyCode.Equals(Keys.A)) //if keys left or a are up the dx is 0
            {
                dx = 0; //sets value of dx
            }
            if (e.KeyCode.Equals(Keys.Right) || e.KeyCode.Equals(Keys.D)) //if keys right or d are up dx is 0
            {
                dx = 0; //sets value of dx
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) //key down method
        {
            if (canMove == true)
            {
                if (e.KeyCode.Equals(Keys.Left) || e.KeyCode.Equals(Keys.A)) //if keys left or a are down dx is -speed
                {
                    dx = -speed; //sets value of dx
                    playTimer.Start(); //starts the timer
                    jumpTimer.Start(); //starts the timer
                }
                if (e.KeyCode.Equals(Keys.Right) || e.KeyCode.Equals(Keys.D)) //if keys right or d are down dx is speed
                {
                    dx = speed; //sets value of dx
                    playTimer.Start(); //starts the timer
                    jumpTimer.Start(); //starts the timer
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //INFO METHODS
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AddInfo()
        {
            backLabel = new Label(); //creates yes label
            this.Controls.Add(backLabel); //adds the label to the form
            backLabel.Text = "BACK"; //changes the text for the yes label
            backLabel.Font = new Font("Mistral", 25, FontStyle.Bold); //changes font and size of the yes label
            backLabel.ForeColor = Color.White; //changes text colour of yes label
            backLabel.BackColor = Color.Transparent; //changes the back colour to be tranparent of yes label
            backLabel.Size = new Size(150, 70); //changes the size of the yes label
            backLabel.Location = new Point(100, 70); //moves the yes label to the proper location

            backLabel.MouseClick += BackLabel_MouseClick; //creates method for back label click
            backLabel.MouseEnter += BackLabel_MouseEnter; //creates method for back label enter
            backLabel.MouseLeave += BackLabel_MouseLeave; //creates method for back label leave
        }

        private void BackLabel_MouseLeave(object sender, EventArgs e) //method for mouse leave for back label
        {
            backLabel.Text = "BACK"; //changes the text of the no label
        }

        private void BackLabel_MouseEnter(object sender, EventArgs e) //method for mouse eneter for back label
        {
            clickSound.Play();
            backLabel.Text = "B A C K"; //changes the text of the no label
        }

        private void BackLabel_MouseClick(object sender, MouseEventArgs e) //method for mouse click for back label
        {
            this.Controls.Remove(backLabel); //removes the yes label
            AddMenu(); //calls on the add menu method
            menuTimer.Start(); //starts the menu timer
            this.BackgroundImage = null; //removes the form backgound
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //EXIT METHODS
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AddExit() //method to add the exit thing
        {
            yesLabel = new Label(); //creates yes label
            noLabel = new Label(); //creates no label

            this.Controls.Add(yesLabel); //adds the label to the form
            this.Controls.Add(noLabel); //adds the label to the form

            yesLabel.MouseClick += YesLabel_MouseClick; //creates mouse click method for yes label
            yesLabel.MouseEnter += YesLabel_MouseEnter; //creates mouse enter method for yes label
            yesLabel.MouseLeave += YesLabel_MouseLeave; //creates mouse leave method for yes label

            noLabel.MouseClick += NoLabel_MouseClick; //creates mouse click method for no label
            noLabel.MouseEnter += NoLabel_MouseEnter; //creates mouse enter method for no label
            noLabel.MouseLeave += NoLabel_MouseLeave; //creates mouse leave method for no label

            yesLabel.Text = "YES"; //changes the text for the yes label
            noLabel.Text = "NO"; //changes the text for the no label

            yesLabel.Font = new Font("Mistral", 50, FontStyle.Bold); //changes font and size of the yes label
            noLabel.Font = new Font("Mistral", 50, FontStyle.Bold); //changes font and size of the o label

            yesLabel.ForeColor = Color.White; //changes text colour of yes label
            noLabel.ForeColor = Color.White; //changes text colour of no label

            yesLabel.BackColor = Color.Transparent; //changes the back colour to be tranparent of yes label
            noLabel.BackColor = Color.Transparent; //changes the back colour to be tranparent of no label

            yesLabel.Size = new Size(150, 70); //changes the size of the yes label
            noLabel.Size = new Size(150, 70); //changes the size of the no label

            yesLabel.Location = new Point(75, this.ClientSize.Height / 2 - 15); //moves the yes label to the proper location
            noLabel.Location = new Point(this.ClientSize.Width - 190, this.ClientSize.Height / 2 - 15); //moves the no label to the proper location
        }

        private void NoLabel_MouseLeave(object sender, EventArgs e) //method for mouse leaving no label
        {
            noLabel.Text = "NO"; //changes the text of the no label
            noLabel.Location = new Point(this.ClientSize.Width - 190, this.ClientSize.Height / 2 - 15); //moves the no label to the proper location
        }

        private void NoLabel_MouseEnter(object sender, EventArgs e) //method for mouse entering no label
        {
            clickSound.Play();
            noLabel.Text = "N O"; //changes the text of the no label
            noLabel.Location = new Point(this.ClientSize.Width - 200, this.ClientSize.Height / 2 - 15); //moves the no label to the proper location
        }

        private void NoLabel_MouseClick(object sender, MouseEventArgs e) //method for mouse clicking no label
        {
            this.Controls.Remove(yesLabel); //removes the yes label
            this.Controls.Remove(noLabel); //removes the no label
            AddMenu(); //calls on the add menu method
            menuTimer.Start(); //starts the menu timer
            this.BackgroundImage = null; //removes the form backgound
        }

        private void YesLabel_MouseLeave(object sender, EventArgs e) //method for mouse leaving no label
        {
            yesLabel.Text = "YES"; //changes the text of the yes label
            yesLabel.Location = new Point(75, this.ClientSize.Height / 2 - 15); //moves the yes label to the proper location
        }

        private void YesLabel_MouseEnter(object sender, EventArgs e) //method for mouse entering no label
        {
            clickSound.Play();
            yesLabel.Text = "Y E S"; //changes the text of the yes label
            yesLabel.Location = new Point(65, this.ClientSize.Height / 2 - 15); //moves the yes label to the proper location
        }

        private void YesLabel_MouseClick(object sender, MouseEventArgs e) //method for mouse clicking no label
        {
            Application.Exit(); //exits the application
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
