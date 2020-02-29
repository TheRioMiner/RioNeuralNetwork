//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once
#include "Activation.h"


class SigmoidActivation : public Activation
{
	float Transfer(float activation);
	float TransferDerivative(float output);
};

class TanhActivation : public Activation
{
	float Transfer(float activation);
	float TransferDerivative(float output);
};

class ReLUActivation : public Activation
{
	float Transfer(float activation);
	float TransferDerivative(float output);
};

class LReLUActivation : public Activation
{
	float Transfer(float activation);
	float TransferDerivative(float output);
};


class Activations
{
public:
	static SigmoidActivation* Sigmoid;
	static TanhActivation* Tanh;
	static ReLUActivation* ReLU;
	static LReLUActivation* LReLU;
};