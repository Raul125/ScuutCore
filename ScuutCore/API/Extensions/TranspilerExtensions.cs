namespace ScuutCore.API.Extensions;

using HarmonyLib;
using NorthwoodLib.Pools;
using System;
using System.Collections.Generic;

/// <summary>
/// An extension class for handling transpilers.
/// </summary>
public static class TranspilerExtensions
{
    /// <summary>
    /// Creates a code instruction list from the list pool using the given instructions.
    /// </summary>
    /// <param name="instructions">The original instructions enumerable.</param>
    /// <param name="newInstructions">The new list of code instructions.</param>
    public static List<CodeInstruction> BeginTranspiler(this IEnumerable<CodeInstruction> instructions)
    {
        return ListPool<CodeInstruction>.Shared.Rent(instructions);
    }

    /// <summary>
    /// Returns an enumerable of code instructions and returns the instructions list to the pool.
    /// </summary>
    /// <param name="newInstructions">The instructions list to return to the pool.</param>
    /// <returns>Enumerable representing the finished code of the transpiler.</returns>
    public static IEnumerable<CodeInstruction> FinishTranspiler(this List<CodeInstruction> newInstructions)
    {
        for (int i = 0; i < newInstructions.Count; i++)
        {
            yield return newInstructions[i];
        }

        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }

    /// <summary>
    /// Finds the Nth instruction index that matches the specified predicate.
    /// </summary>
    /// <param name="instructions">The instructions to search.</param>
    /// <param name="n">The Nth value to find.</param>
    /// <param name="predicate">The predicate to match for.</param>
    /// <returns>The index of the Nth instruction that matches the predicate, or -1 if not found.</returns>
    public static int FindNthInstruction(this List<CodeInstruction> instructions, int n, Predicate<CodeInstruction> predicate)
    {
        if (n <= 0)
        {
            return -1;
        }

        for (int i = 0; n < instructions.Count; i++)
        {
            if (!predicate(instructions[i]))
            {
                continue;
            }

            if (--n == 0)
            {
                return i;
            }
        }

        return -1;
    }
}
