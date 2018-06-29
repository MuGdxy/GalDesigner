# GalEngine.Script.ProcessUnit

The ProcessUnit is used to process the script.

## How does it run?

A process unit has a stack to record the scripts that we need process.

At current script, we will analyse the code and execute it. 
When we jump to an script but current script is not end, we will used the stack to record it.
Before we analyse an sentence , we will check the stack to make sure the script is newest.

## Main ProcessUnit

This process unit is used to process the events or common script. Most of scripts are ran by this process unit.

## Story ProcessUnit

This process unit is used to process the story script. It is ran in the game scene.

## How to analyer a file?

- Find all script block in the file.
- Find all sentence in the script block.
- Build an expression to analyer it.