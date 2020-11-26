using System;
using CryptoCAD.Core.DES;

namespace CryptoCAD.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] text = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xE7 };
            byte[] key = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            byte[] output = new byte[8];
            byte[,] schedule = new byte[16, 6];

            DESService.KeySchedule(key, schedule, DESService.ENCRYPT);
            DESService.Crypt(text, output, ToJaggedArray(schedule));
            System.Console.Write("Encrypt Output: ");
            PrintText(output);

            DESService.KeySchedule(key, schedule, DESService.DECRYPT);
            DESService.Crypt(output, text, ToJaggedArray(schedule));
            System.Console.Write("Decrypt Output: ");
            PrintText(text);

            //var p = G(0b_1111, 62, 45);

            System.Console.WriteLine("Hello World!");
        }

		private static void PrintText(byte[] hash)
		{
			for (int i = 0; i < 8; ++i)
				System.Console.Write("{0:x2} ", hash[i]);

			System.Console.WriteLine();
		}

		private static T[][] ToJaggedArray<T>(T[,] twoDimensionalArray)
		{
			int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
			int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
			int numberOfRows = rowsLastIndex + 1;

			int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
			int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
			int numberOfColumns = columnsLastIndex + 1;

			T[][] jaggedArray = new T[numberOfRows][];

			for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
			{
				jaggedArray[i] = new T[numberOfColumns];

				for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
				{
					jaggedArray[i][j] = twoDimensionalArray[i, j];
				}
			}

			return jaggedArray;
		}

		private static uint G(byte a, int b, int c)
        {
			return (uint)(((a >> (7 - (b % 8))) & 0x01) << (c));
		}
	}
}
