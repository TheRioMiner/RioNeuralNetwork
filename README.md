# Rio Neural Network
Simple and performance neural network .NET library

Its use native C++ dll for AVX2 acceleration!

 
# Autoencoders
In this test 5620 examples in epoch, epoch time is 66 (±2) seconds.

From this - 66 / 5620 = ~0,0117437 sec = ~11.7 ms for 1 example in training mode.

In image below, results of training in 2400 epochs! Reached error per epoch: ~270.
[![](https://i.imgur.com/gcpUCJY.png)](https://i.imgur.com/gcpUCJY.png)

 
# Simple digit recognition
Even for classical neural networks with direct distribution, the recognition of numbers is _not very effective_.

But I decided to do a test with partial MNIST dataset. _(only 10000 images for train and 1000 for test)_

In this test network trained in 41 epochs. Reached __test__ error per epoch: ~14 (error for test dataset!).

Layers cfg: __256 sigmoid (_input_), 256 tanh, 256 tanh, 256 tanh, 10 sigmoid (_output_)__


## Training
[![](https://i.imgur.com/mBWkb2s.png)](https://i.imgur.com/mBWkb2s.png)

## Test
[![](https://i.imgur.com/zBGF166.png)](https://i.imgur.com/zBGF166.png)


 
# Genetic algorithm
_Tests coming soon ;)_
