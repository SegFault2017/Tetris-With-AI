## Table of contents

- [Tetris-With-Ai](#evolutionary-tetris)
- [Demo](#demo)
- [Architechture](#architeture)
- [Support](#support)
- [Used Tech](#used-tech)
- [Acknowledgement](#acknowledgement)
- [AI](#ai)
- [More](#more)


## Tetri with AI
This AI uses evolutionary technique to improve over time. Through selection, crossover, and mutation, the AI will learn to paly [Tetris](https://en.wikipedia.org/wiki/Tetris) in as few moves as possible.


## Demo
![Part 1](demo/demo.gif)


## Architecture
About the MVC architecture pattern
Advantages and disadvantages of MVC
advantage:

- Low coupling, high cohesion: A system is divided into a presentation layer, a business logic layer, and a data access layer through the MVC framework. For example, only the view layer needs to be changed without recompiling the model and controller code, and the business of an application The change of process or business rules only needs to change the model layer without modifying the code of the view layer and the controller layer.

- High reusability: The data of the model can be accessed through different view layers, only the data format needs to be processed in the controller layer, without the need to modify the code of the model layer.

- Maintainability: Separate the business layer, view layer, and data layer to make the code easier to maintain

Problems that may be caused by MVC or similar frameworks in actual development:

- The code is cumbersome: a very simple logic, which has been encapsulated many times, needs to be indexed in multiple code files, and the reading efficiency is extremely low.

- Project confusion: The inertia of some less professional programs makes them not really understand the principles of MVC or these frameworks. Their goal is to get the functions out. They either bypass the framework and intersperse a lot of calls, or Copy a function of others as a whole, remove the logic, leave the skeleton, and then fill in your own logic, regardless of whether the skeleton is applicable. This greatly increases the confusion of the project

## More
Many design patterns are to learn their ideas and solutions.
We should think and ask questions about design patterns and frameworks:
What needs does this framework apply to? What problem was solved?
Under what circumstances should I use it, and when should I not use it? What problem does he bring?
Is it suitable for my project, my team?
Should I use the project as a whole, or should I use it for some partial requirements?





## Support 
This build supports IOS/Android touch gesture and also joystick. 


## Used Tech
Project used:
- Unity: 2020.2.6
- C#


# AI
If you are interested in the AI part. You can checkout [this](https://github.com/SegFault2017/EvoluationaryTetris) out.

## Acknowledgement
Cerfiticate of course completetion from Udemy.

## More
- A [web app]() version is avalible here.
- [Is it possible to Play Tetris forever](https://tetris.fandom.com/wiki/Playing_forever)

