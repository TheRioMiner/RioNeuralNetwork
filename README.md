# Rio Neural Network
Simple and performance neural network _.NET_ library

Its use native C++ dll for __AVX2__ acceleration!

Supported x64 and x86 platforms.

 
# Autoencoders
In this test 5620 examples in epoch, epoch time is ~66 (±2) seconds.

From this - 66 / 5620 = __~0,0117437__ sec = __~11.7__ ms for 1 example in training mode.

In image below, results of training in __2400__ epochs! Reached error per epoch: __~270__.
[![](https://i.imgur.com/gcpUCJY.png)](https://i.imgur.com/gcpUCJY.png)

 
# Simple digit recognition
Even for classical neural networks with direct distribution, the recognition of numbers is _not very effective_.

But I decided to do a test with partial MNIST dataset. _(only 10000 images for train and 1000 for test)_

In this test network trained in 41 epochs. Reached __test__ error per epoch: ~16 (error for test dataset!).

Layers cfg: __256 sigmoid (_input_), 256 tanh, 256 tanh, 256 tanh, 10 sigmoid (_output_)__


## Training
[![](https://i.imgur.com/mBWkb2s.png)](https://i.imgur.com/mBWkb2s.png)

## Test
[![](https://i.imgur.com/zBGF166.png)](https://i.imgur.com/zBGF166.png)


 
# Genetic algorithm
_Tests coming soon ;)_
