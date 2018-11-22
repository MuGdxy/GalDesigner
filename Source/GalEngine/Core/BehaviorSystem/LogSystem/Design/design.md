# Design

- LogComponent: record and store log.
- ConsoleLogSystem: print ot solve the LogComponent.

## LogComponent

GameObject can contain LogComponent to record log and print log(by log system). We can record log by this way.

```C#
logComponent.Log("[time] is {0}", "null");
```

output: [20xx/xx/xx] is null.

## ConsoleLogSystem

Sub Behavior System to print or solve the logs in LogComponent.

## BaseLogFormat

The base log format support:

- [time]: repalce [time] to real time.
- [object]: repalce [object] to object's name.