# Brick Destroyer
This is my Unity project for the tutorial "Submission: Data persistence in a new repo" from the mision  "Manage scene flow and data" from  the "Junior Programmer " pathway at "Unity Learn".

The goal the tutorial is to to add saving of data between scenes and between sessions in a game.

Link to tutorial: https://learn.unity.com/tutorial/submission-data-persistence-in-a-new-repo

Link to game: 

Enjoy the game!

## Unity concepts
- Persistence between session including C# List<T> class
- Persistence between scenes
- Singleton design pattern
- Textmeshpro package
- Localization package
- AudioSource and AudioClip
- ParticleSystem

## Other concepts

- Language picker widget (My first version)
- TextMeshPro

## The game
This is a simple retro breakout-style game in which you are getting points depending on how many bricks you destroy by a ball that you control by af paddle.

(Image)

## Development environment
Unity Editor v2020.3.32f1 running on Manjaro Linux.

I made a lot of comments in the code for anyone who wants to learn how the code works.

## Scenes
I made some game scenes:
- Menu scene
- Highscore scene
- Settings scene
- and a Main game scene

### Menu
- Contains Inputfield to enter the same og the player
- The Inputfield is populated wirh the same og the last player
- A "New game" button that strips the text in the Inputfield for whitespace and done other unwanted characters
- If the name is empty then a dialog is show that asks for a valid name
- If the name is validated as ok then the "Main" scene is loadet
- The game remembers all players name and record score
- A "Highscore" button that loads the highscore scene
- A "Settings" button that loads the settings scene
- A "Quit" button If the game is running as a native Linux application

### Highscore
- Shows a list of the best ten players sorted from the best to the tenth best player
- If fewer than ten players have played the list show all the players records
- A "Return button" to the "Menu" scene

### Settings
- Shows a "Reset highscore" button that deletes all players that have been playing except if a name is entered in the inputfield. That player gets its record reset
- Pressing the "Reset highscore" button opens a "are you sure?" dialog
- Shows a custom "Language picker" widget in which you can select between two languages: English and Danish
- The selected Language is remembered between game sessions
- A "Return button" to the "Menu" scene

### Main
- I made some fancier materials for the bricks
- I added the game mechanic "levels" to the game. When all bricks are destroyed a new set of bricks spawn. Depending on the level the paddle gets smaller and the ball runs faster to a certain point. New bricks with more points at higher levels and at even higher levels the bricks get randomized. The higher level the smaller paddle and faster ball to a certain limit
- You can see the player name, the current points and the all time record for the player in the UI
- You can see the name and record of the best player ever played this game (unless the highscore has been reset)

## What could be enhanced?
- The LanguagePicker could have a better implementation
- Better UI graphics in the "Main" scene would be nice
- The code can be optimized for speed and memory some places but since this is a small game it is not necessary
- Some code can properly be optimized using better design but this version is based on my current knowledge of Unity and C# (.Net)

