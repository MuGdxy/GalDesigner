# GalEngine.DebugLayer

When some errors occur, we need to show this error to user.
There is a way to report a error or warning to user.

## About Type and Message

We have ErrorType and WarningType. We use this to tell DebugLayer which error occurs.
If we show error to user, we need known what we need show. So we need to define a MessageText to each errors or warnings.

A text is a string. But we can use `{number}` to set data.
For example:

```C#
    DebugLayer.ReportError(ErrorType, value1, value2);
```

If we assume the text is `Error: we need {1} , but we get {2}.`
The result of text is `Error: we need value1 , but we get value2.`
The `value1` and `value2` is variable.

## About Assert

```C#
    DebugLayer.Assert(bool condition, Error or Warning, ...)
```
If the condition is true, we will report this error or warning.

## About Add Erorr or Warning Type

We can add our own errorType or warningType.
To do this ,we need the TypeID(**If the ID is used, we will cover the old**), MessageText.

For example:

```C#
    DebugLayer.RegisterError(errorType, "We get error {1} in line {2}.");
```

## About DebugCommand

We can send some command by using DebugCommand to our game. 
And you can use `Tab` to enable or disable it.

### Support Command

There are some Command that we support:

Command | Format | Description 
------- | ------ | -----------
Set     | Set value's name value | Set value, the value must be in GlobalValue or GlobalConfig.
Get     | Get value's name       | Get value, the value must be in GlobalValue or GlobalConfig.
Clear   | Clear                  | Clear Command.




### Custom Command

You can support your command by using `DebugCommand.CommandAnalyser`.

```C#
    public delegate bool CommandHandle(string command);
    //the result is used for telling the DebugCommand if the command is right
    //ture means it is a command, false means it is not a command.

    public static event CommandHandle CommandAnalyser;

    //example
    DebugCommand.CommandAnalyser += Solution;

    private static bool Solution(string command)
    {
        return false;
    }
```