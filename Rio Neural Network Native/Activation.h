//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once
#include <math.h>

enum class ActivationTypes : int
{
	INVALID = 0,

	SIGMOID,
	TANH,
	RELU,
	LRELU,
};

//Base class of all activations functions
class Activation
{
public:
	virtual float Transfer(float activation)
	{
		throw "Called method in abstract base class: \"Activation\"\n";
	}

	virtual float TransferDerivative(float output)
	{
		throw "Called method in abstract base class: \"Activation\"\n";
	}
};