# GalEngine.Script 

For making game, we need make some objects by ourselves.
So we need a way to do this. There are why we use the script to make game.

In GalEngine, Script is not like traditional script. It likes a logic diagram.
The mainbody of script is state. It contains the logic code, the next state, the variables and so on.

**The code style are similar to Python, In fact we use `tab` for distinguishing code sentences.**

## Variable

We can have our variables in our scripts.
It has four kinds and we can define it in the beginning of script.

### Accessibility

As a variable, some can be read in other script, some only can read in this script.
So we define a keyword to do this.

- `Local`: Only can be read in this script. We will create and initalize it(**In new script**) firstly after we enter a new script and we will destory it after we leave this script.
- `Global`: Can be read in any scripts. We will create it and initalize it in the beginning of game. And we will destory it after game end or game close.

## State

## About Scripts

For a game, we can have more than one script.
So we need a file to tell the GalEngine that what scripts we will use.
The `.gsIndex` file will do this.

There is a example:

```gsIndex

[Script1.gs] 
[Script2.gs] 
...

```