//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;

namespace Rio_Neural_Network
{
    [Flags]
    public enum FileFlags
    {
        /// <summary>
        /// With this flag file will be hashed at save and every load it will be check hash of file that its not corrupted
        /// </summary>
        Hash = 1,

        /// <summary>
        /// With this flag in file will be added learn info as "LearnRate", "Alpha", current "Epoch", "ExampleIndex" and etc useful information in training of neural network.
        /// </summary>
        LearnInfo = 2,

        /// <summary>
        /// With this flag in file will be writted a weights of every layer of neural network (Its necessarily flag in using neural network!)
        /// </summary>
        Weights = 4,

        /// <summary>
        /// With this flag in file will be writted a weights momentum (inertia of weights change) of every layer of neural network.
        /// </summary>
        WeightsMomentum = 8,

        /// <summary>
        /// With this flag in file will be writted a neuron outputs of every layer of neural network. (Optional Flag!)
        /// </summary>
        Output = 16,

        /// <summary>
        /// With this flag in file will be writted a neuron errors of every layer of neural network. (Optional Flag!)
        /// </summary>
        Errors = 32,
        


        /// <summary>
        /// Save neural network configuration as trained and ready for use.
        /// </summary>
        SAVE_FOR_RELEASE = Hash | Weights,

        /// <summary>
        /// Save neural network configuration as trained and ready for use. (With learn info)
        /// </summary>
        SAVE_FOR_RELEASE_WITH_INFO = Hash | Weights | LearnInfo,

        /// <summary>
        /// Save neural network configuration as not trained and need to be trained.
        /// </summary>
        SAVE_FOR_FURTHER_LEARN = Hash | LearnInfo | Weights | WeightsMomentum,


        SAVE_FOR_FURTHER_LEARN_MIN = Hash | LearnInfo | Weights,

        /// <summary>
        /// Save neural network configuration in full internal state. (Not recommended)
        /// </summary>
        SAVE_FULL_STATE = Hash | LearnInfo | Weights | WeightsMomentum | Output | Errors,

        /// <summary>
        /// Default flag if you save neural network in file
        /// </summary>
        DEFAULT = SAVE_FOR_RELEASE_WITH_INFO,
    }
}
