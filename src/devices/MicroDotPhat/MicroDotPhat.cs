// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.I2c;
using Iot.Device.IS31FL3730;

namespace Iot.Device.MicroDotPhat
{
    /// <summary>
    /// Add documentation here
    /// </summary>
    public class MicroDotPhat : IDisposable
    {
        private I2cDevice _matrix01Device;
        private I2cDevice _matrix23Device;
        private I2cDevice _matrix45Device;

        private IS31FL3730.IS31FL3730 _matrix01;
        private IS31FL3730.IS31FL3730 _matrix23;
        private IS31FL3730.IS31FL3730 _matrix45;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroDotPhat"/> class.
        /// </summary>
        public MicroDotPhat()
        {
            IS31FL3730.DriverConfiguration configuration = new DriverConfiguration()
            {
                IsShutdown = false,
                IsAudioInputEnabled = false,
                Layout = MatrixLayout.Matrix8by8,
                Mode = MatrixMode.Both,
                DriveStrength = DriveStrength.Drive45ma
            };

            _matrix01Device = I2cDevice.Create(new I2cConnectionSettings(1, IS31FL3730.IS31FL3730.DefaultI2cAddress));
            _matrix23Device = I2cDevice.Create(new I2cConnectionSettings(1, IS31FL3730.IS31FL3730.DefaultI2cAddress + 1));
            _matrix45Device = I2cDevice.Create(new I2cConnectionSettings(1, IS31FL3730.IS31FL3730.DefaultI2cAddress + 2));

            _matrix01 = new IS31FL3730.IS31FL3730(_matrix01Device, configuration);
            _matrix23 = new IS31FL3730.IS31FL3730(_matrix23Device, configuration);
            _matrix45 = new IS31FL3730.IS31FL3730(_matrix45Device, configuration);
        }

        /// <summary>
        /// Display a character at the specified position (5-0, Left-Right)
        /// </summary>
        /// <param name="position">Display position to set.</param>
        /// <param name="character">Character to display.</param>
        public void ShowCharacterAtPosition(int position, char character)
        {
            switch (position)
            {
                case 0:
                    _matrix01.SetMatrix(MatrixMode.Matrix1Only, DotFont.GetCharacter(character));
                    break;
                case 1:
                    _matrix01.SetMatrix(MatrixMode.Matrix2Only, RotateCharacter(DotFont.GetCharacter(character)));
                    break;
                case 2:
                    _matrix23.SetMatrix(MatrixMode.Matrix1Only, DotFont.GetCharacter(character));
                    break;
                case 3:
                    _matrix23.SetMatrix(MatrixMode.Matrix2Only, RotateCharacter(DotFont.GetCharacter(character)));
                    break;
                case 4:
                    _matrix45.SetMatrix(MatrixMode.Matrix1Only, DotFont.GetCharacter(character));
                    break;
                case 5:
                    _matrix45.SetMatrix(MatrixMode.Matrix2Only, RotateCharacter(DotFont.GetCharacter(character)));
                    break;
            }
        }

        /// <summary>
        /// Display a specific string, must be exactly 6 characters long.
        /// </summary>
        /// <param name="value">String to display, must be exactly 6 characters long.</param>
        public void ShowString(string value)
        {
            if (value.Length != 6)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value supplied must be exactly 6 characters long.");
            }

            int position = 0;
            foreach (char character in value.ToCharArray())
            {
                ShowCharacterAtPosition(position, character);
                position++;
            }
        }

        /// <summary>
        /// Clear all 6 matrices.
        /// </summary>
        public void ClearAll()
        {
            _matrix01.SetMatrix(MatrixMode.Both, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            _matrix23.SetMatrix(MatrixMode.Both, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            _matrix45.SetMatrix(MatrixMode.Both, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        /// <summary>
        /// Rotate a character for display on matrices 1, 3 and 5 (Lord only knows why they wired it this way!)
        /// </summary>
        /// <param name="character">Character matrix to rotate.</param>
        /// <returns>Rotated matrix.</returns>
        private byte[] RotateCharacter(byte[] character)
        {
            byte s1 = (byte)(((character[4] & 0b00000001) << 4) | ((character[3] & 0b00000001) << 3) | ((character[2] & 0b00000001) << 2) | ((character[1] & 0b00000001) << 1) | (character[0] & 0b00000001));
            byte s2 = (byte)(((character[4] & 0b00000010) << 3) | ((character[3] & 0b00000010) << 2) | ((character[2] & 0b00000010) << 1) | (character[1] & 0b00000010) | ((character[0] & 0b00000010) >> 1));
            byte s3 = (byte)(((character[4] & 0b00000100) << 2) | ((character[3] & 0b00000100) << 1) | (character[2] & 0b00000100) | ((character[1] & 0b00000100) >> 1) | ((character[0] & 0b00000100) >> 2));
            byte s4 = (byte)(((character[4] & 0b00001000) << 1) | (character[3] & 0b00001000) | ((character[2] & 0b00001000) >> 1) | ((character[1] & 0b00001000) >> 2) | ((character[0] & 0b00001000) >> 3));
            byte s5 = (byte)((character[4] & 0b00010000) | ((character[3] & 0b00010000) >> 1) | ((character[2] & 0b00010000) >> 2) | ((character[1] & 0b00010000) >> 3) | ((character[0] & 0b00010000) >> 4));
            byte s6 = (byte)(((character[4] & 0b00100000) >> 1) | ((character[3] & 0b00100000) >> 2) | ((character[2] & 0b00100000) >> 3) | ((character[1] & 0b00100000) >> 4) | ((character[0] & 0b00100000) >> 5));
            byte s7 = (byte)(((character[4] & 0b01000000) >> 2) | ((character[3] & 0b01000000) >> 3) | ((character[2] & 0b01000000) >> 4) | ((character[1] & 0b01000000) >> 5) | ((character[0] & 0b01000000) >> 6));

            return new byte[] { s1, s2, s3, s4, s5, s6, s7 };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _matrix01.Dispose();
            _matrix23.Dispose();
            _matrix45.Dispose();

            _matrix01Device.Dispose();
            _matrix23Device.Dispose();
            _matrix45Device.Dispose();
        }
    }
}
