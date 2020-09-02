//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once

enum class ThreadingMode : int
{
    /// <summary>
    /// Invalid threading mode
    /// </summary>
    INVALID = -1,

    /// <summary>
    /// Default threading mode
    /// </summary>
    DEFAULT = 1/*SINGLETHREAD*/,

    /// <summary>
    /// Maximal value of threads to use, this value is number of logical cores in system.
    /// </summary>
    MAX = 0,

    /// <summary>
    /// Dont use multi-threading in computation
    /// </summary>
    SINGLETHREAD = 1,

    T2 = 2,
    T3 = 3,
    T4 = 4,
    T5 = 5,
    T6 = 6,
    T7 = 7,
    T8 = 8,
    T9 = 9,
    T10 = 10,
    T11 = 11,
    T12 = 12,
    T13 = 13,
    T14 = 14,
    T15 = 15,
    T16 = 16,
};