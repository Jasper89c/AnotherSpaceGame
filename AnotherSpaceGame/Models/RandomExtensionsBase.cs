// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace AnotherSpaceGame.Models
{
    public static class RandomExtensionsBase
    {
        // Changed access modifier from private to internal to fix CS0122
        internal const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    }

    // Moved RandomExtensions to a top-level static class to fix CS1109
    public static class RandomExtensions
    {
        public static string RandomString(this Random random, int length)
        {
            return new string(Enumerable.Repeat(RandomExtensionsBase.Characters, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}