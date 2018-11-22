# Design

- LogPrint: print the log.
- LogFormat: about the format. 
- KeySetting: setting of "key to value".
- Log: log text(combine from log element).

## KeySetting

Map [key] to [value]. We also can save some other information to make rich log.

- format: [key0] [key1] ... {0} {2} ....
- output: [method0_output] [method1_output] ... context[0] context[2]

for example: 
- input : Today is [time].
- output: Today is 20xx/xx/xx.

## LogFormat

A set of KeySetting. It is used for format and making. We can use LogFormat to make log.

## LogPrint

Apply log and output it.


