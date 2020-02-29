//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#include "Activations.h"

SigmoidActivation* Activations::Sigmoid = new SigmoidActivation();
TanhActivation* Activations::Tanh = new TanhActivation();
ReLUActivation* Activations::ReLU = new ReLUActivation();
LReLUActivation* Activations::LReLU = new LReLUActivation();


float SigmoidActivation::Transfer(float activation)
{
	return 1.0f / (1.0f + expf(-activation));
}

float SigmoidActivation::TransferDerivative(float output)
{
	return output * (1.0f - output);
}


float TanhActivation::Transfer(float activation)
{
	return tanhf(activation);
}

float TanhActivation::TransferDerivative(float output)
{
	return 1.0f - (output * output);
}


float ReLUActivation::Transfer(float activation)
{
	if (activation > 0.0f)
		return activation;
	else
		return 0.0f;
}

float ReLUActivation::TransferDerivative(float output)
{
	if (output > 0.0f)
		return 1.0f;
	else
		return 0.0f;
}


float LReLUActivation::Transfer(float activation)
{
	if (activation > 0.0f)
		return activation;
	else
		return 0.01f * activation;
}

float LReLUActivation::TransferDerivative(float output)
{
	if (output > 0.0f)
		return 1.0f;
	else
		return 0.01f * output;
}