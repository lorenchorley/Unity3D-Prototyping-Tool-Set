# StrangeIoC-Updated

This project is an update of StrangeIoC by ThirdMotion (https://github.com/strangeioc), a now unmaintained project. 

Most pull requests from the original project have been merged, and a number of bug fixes made for the signal messaging system, as well as:

 * Equivalent example projects for signals
 * Debugging options (Print event/signal dispatch to console, named binders, readout of bindings per binder in inspector)
 * Listen & dispatch extensions for events and signals to make handling requests with multiple return events/signals easier to use and read
 * Added UniRx (Rx for Unity) library for easier concurrency handling
 * UniRx customisations for events and signals (to be able to use them like any other observable)