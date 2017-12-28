# Unity3D Prototyping Tool Set

## What
 * A project in development!
 * A fast game prototyping and production tool set for Unity3D
 * A set of frameworks and tools for fast game prototyping and production
 * A continuously updated project, as my needs change or as I find new tools that fit my needs
 
## Why 
 * I found that I spend most of my time on a new project reinventing the same things over and over again, so:
 * I want a global framework that gives good context for code and separation of concerns, and lets me reuse code as much as possible
 * I want to be able to prototype an idea as quickly and easily as possible
 * I want to use the same prototyping framework to go straight into production with a minimum of changes
 * I want all essential functions for a game ready to go when I start a new project. I just want to link things together and give them a bit of a style
 * I want to work at a high level of abstraction as much as possible
 
## Features
 * An Inversion of Control framework with Dependency Injection (Provided the glue for the other features)
 * A reactive extensions (Rx) framework
 * An event sourcing framework
 * An entity management framework
 * A serialisation and persistence framework
 * Extensions and guides for Photon multiplayer
 * A menu system
 * A multi-platform input system
 * All with example projects and all integrated via IoC for plug and play-like functionality

## Drawbacks
 * May take a bit of time to get to know the tools, but once learned will create a far better programmer
 * Adds a bit of overhead but more than makes up for it with the development speed-up


StrangeIoC - Updated
==========
## What
 * An inversion of control, dependency injection, MVC framework with view mediators (for streamlined view-model interaction) and services (for external-to-application interaction)
 * An update of StrangeIoC by ThirdMotion (https://github.com/strangeioc), a now unmaintained project. (Most pull requests from the original project have been merged, and a number of bug fixes made for the signal messaging system)
## Why 
 * Code reuse
 * Organisation taken care of, no need to reinvent for each new project or each new part of a project
 * Decoupling of ________ from ______ (IoC reasons)
 * <IoC reasons>
## Features
 * <StrangeIoC features>
## Additional Features
 * StrictSignal for speed optimised strongly-typed events
 * Debugging options (Print event/signal dispatch to console, named binders, readout of bindings per binder from inspector)
 * Listen & dispatch extensions for events and signals to make handling requests with multiple return events/signals easier to use and read
 * Equivalent example projects for signals
## Docs and references
 * <website>
## Drawbacks
 * Small overhead, though it's mainly on startup and messaging overhead can be largely avoided using signals framework with as many StrictSignals as possible.
## Development progress
 * Finished
 
UniRx
=====
## What
 * UniRx is _____
 * UniRx (Rx for Unity) library for easier concurrency handling, event streaming, etc...
## Why 
 * <reasons Rx>
## Features
 * Added documentation from http://reactivex.io for supported commands. For use with inline documentation viewing tools like Intellisense in Visual Studio
 * Added decision tree like that on http://reactivex.io/documentation/operators.html updated for UniRx specific command naming
 * Added implementations for several Rx extensions that UniRx misses
 * UniRx customisations for events and signals (to be able to use them like any other observable)
 * <features Rx>
## Docs and references
 * <good reference for this type of Rx>
## Drawbacks
 * Very minimal overhead for massive time savings
## Development progress
 * Finished

 
Event sourcing framework
==============
## What
 * Event Sourcing module
## Why 
 * Often how to store events that occur in the game is a critical issue that needs to be managed
## Features
 * Easy network synchronisation
 * Dead simple session replays
## Docs and references
 * <Link to video> <summary of points in video>
## Drawbacks
 * What: A fast and simple Event Sourcing implementation built for the gaming context with multiplayer and replay capabilities in mind.
 * Why: 
 * Isolation of game entities with access via commands and queries
 * Projections for processing the game state in part or in it's entirety
 * easy Seriaisation of game state
 * easy Game state persistence and sharing
 * Example projects
## Development progress
 * Largely finished

 
Entity Management framework
==============
## What
 * Entity management
 * Commands, queries and projections framework
## Why 
 * One feature I always find myself reinventing multiple times for each project is a way to store a list of game entities, a way to create, delete and access them, a set of operations on them, and a way to iterate over them.
 * Another is how to store events that occur in the game.
## Features
 * Centralised organisation of game entities with strict control mechanisms and information access
 * Elementary serialisation of game state
## Docs and references

## Drawbacks
 * Very slight overhead, but maybe no more than any system I might half-heartedly slap together for a midnight prototype.
## Development progress
 * In progress

 
Serialisation and persistence framework
==============
## What
 * A framework to serialise game state, persist or transmit it, and deserialise it back to how it was
## Why 
 * Because every game can be saved, to a disk, to a server, to the cloud, wherever... but it has to be saved, and then loaded again too!
 * Multiplayer games often need a way of transmitting game state
## Features
 * Tools and guidelines for object serialisation in development and in production
 * Tools to save and load serialisable objects from a disk (for saved games) or through a network (for multiplayer)
 * https://forum.unity.com/threads/best-way-to-save-game-state.153411/
## Docs and references
 * See example project
## Drawbacks
## Development progress
 * In progress

 
Photon extensions
==================
## What
## Why 
## Features
## Docs and references
## Drawbacks
## Development progress
 * In progress

 * What: A set of guides and tools for Photon to integrate with the other systems in this package
 * Why: Easier, faster multiplayer
 * ES state and commands easily distributed and synchronised
 * Guidelines for easy request and confirm communication between multiple decentralised players
 * Event rollback and ping management 

 
Menu system 
===========
## What
 * from ___
## Why 
## Features
 * Integrated with StrangeIoC for maximum reusability
## Docs and references
## Drawbacks
## Development progress
 * In progress


Input system
=======================================
## What
 * Cross-platform and cross-device input system
## Why 
## Features
 * IoC, ES integrated 
 * Customisable key mappings
 * Blended mouse/touch functions
## Docs and references
## Drawbacks
 * May not be as flexible as could be desired for a certain platform
## Development progress
 * In progress
