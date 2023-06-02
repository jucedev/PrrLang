﻿namespace PrrCompiler.CodeAnalysis;

public readonly struct TextSpan
{
    public int Start { get; }
    public int Length { get; }
    public int End => Start + Length;

    public TextSpan(int start, int length)
    {
        Start = start;
        Length = length;
    }
}