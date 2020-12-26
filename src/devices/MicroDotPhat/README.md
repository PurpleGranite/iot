# Pimoroni MicroDot pHAT
The [MicroDot pHAT](https://shop.pimoroni.com/products/microdot-phat) is an LED Dot-matrix display HAT for the
Raspberry Pi / Pi Zero. This pHAT uses three IS31FL3730 I2C LED Matrix Controllers to manage six 5x7 dot character
matrices, each matrix has an additional decimal point as well.

This is a farily simple binding for now, allowing display of individual characters on a specific module, or a
fixed-length string across all 6 modules.  The binding may be enhanced in the future to include more advanced
features, such as setting / clearing individual pixels and smooth scrolling of a text buffer.

The binding includes `DotFont`, which is a 1:1 reproduction of the same [font in Pimoroni's Python Library](https://github.com/pimoroni/microdot-phat/blob/master/library/microdotphat/font.py).

## Binding Notes
Usage is very straightforward, since the IS31FL3730's used on the pHAT are at fixed addresses on the I2C bus, the
binding will take care of initialising these for you.

```cs
MicroDotPhat microDot = new MicroDotPhat();
microDot.ClearAll();
microDot.ShowString("123456");
```

You can also set characters at individual positions on the display.
```cs
MicroDotPhat microDot = new MicroDotPhat();
microDot.ClearAll();
microDot.ShowString("123456");
microDot.ShowCharacterAtPosition(5, '7'); // Display now shows: 723456
microDot.ShowCharacterAtPosition(0, '9'); // Display now shows: 723459
```
