//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once
struct YUVICfg
{
public:
	unsigned char Step;
	float IndexRangeDiv;
	float UVDiv;

	YUVICfg(unsigned char Step)
    {
        if (Step > 224)
            throw "Step is must be 224 or smaller!";
        if (Step < 16)
            throw "Step is must be 16 or more!";

        YUVICfg::Step = Step;
        YUVICfg::IndexRangeDiv = (float)((Step * Step) - 1.0f);
        YUVICfg::UVDiv = (256.0f / Step);
    }
};

