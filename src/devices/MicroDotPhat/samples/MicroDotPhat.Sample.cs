// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;
using System.Threading;
using Iot.Device.MicroDotPhat;

Console.WriteLine("Hello MicroDotPhat Sample!");

MicroDotPhat microDot = new MicroDotPhat();

microDot.ClearAll();
Thread.Sleep(250);
microDot.ShowString("     1");
Thread.Sleep(250);
microDot.ShowString("    12");
Thread.Sleep(250);
microDot.ShowString("   123");
Thread.Sleep(250);
microDot.ShowString("  1234");
Thread.Sleep(250);
microDot.ShowString(" 12345");
Thread.Sleep(250);
microDot.ShowString("123456");
Thread.Sleep(250);
microDot.ShowString("234567");
Thread.Sleep(250);
microDot.ShowString("345678");
Thread.Sleep(250);
microDot.ShowString("456789");
Thread.Sleep(250);
microDot.ShowString("567890");
Thread.Sleep(250);
microDot.ShowString("67890 ");
Thread.Sleep(250);
microDot.ShowString("7890  ");
Thread.Sleep(250);
microDot.ShowString("890   ");
Thread.Sleep(250);
microDot.ShowString("90    ");
Thread.Sleep(250);
microDot.ShowString("0     ");
Thread.Sleep(250);
microDot.ClearAll();
Thread.Sleep(250);
microDot.ShowString("123456");
Thread.Sleep(250);
microDot.ShowCharacterAtPosition(5, '7'); // Display now shows: 723456
Thread.Sleep(250);
microDot.ShowCharacterAtPosition(0, '9'); // Display now shows: 723459microDot.ClearAll();
Thread.Sleep(250);
microDot.ClearAll();
Thread.Sleep(250);
microDot.Dispose();
