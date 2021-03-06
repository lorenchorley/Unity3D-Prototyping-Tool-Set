# Unity3D Prototyping Tool Set

## What
 * A project in development!
 * A fast game prototyping and production tool set for Unity3D
 * A set of frameworks and tools for fast game prototyping and production
 * A continuously updated project, as my needs change or as I find new tools that fit my needs
 * A well organised project structure to use as a base to start any new game
 
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
 * One no-configuration-necessary Unity component that manages all events
 * Easy network synchronisation (In progress)
 * Dead simple session replays
 * Stupidly simple serialisation and loading of game state (In progress)
 * Elementary do and undo (gameplay rollback) (In progress)
 
## Docs and references
 * <Link to video> <summary of points in video>
 
## Drawbacks
 * What: A fast and simple Event Sourcing implementation built for the gaming context with multiplayer and replay capabilities in mind.
 * Why: 
 * Isolation of game entities with access via commands and queries
 * Projections for processing the game state in part or in it's entirety
 * Easy serialisation of game state
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
 * Centralised organisation of game entities 
 * Strict control mechanisms and information access via Commands and Queries 
 * Integrates well with the other frameworks (In progress)
 
## Docs and references

## Drawbacks
 * Very slight overhead, but maybe no more than any system I might half-heartedly slap together for a midnight prototype.
 
## Development progress
 * In progress
 
## Notes
 * Need to check out https://github.com/sschmid/Entitas-CSharp

 
 
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
 * Implementing Full Serializer as secondary development serialiser

 
 
Photon extensions
==================

## What
 * A set of guides and tools for Photon to integrate with the other systems in this package

## Why 
 * Easier, faster multiplayer

## Features
 * Distribution and synchronisation of state (from Event Source) and commands (From IoC or elsewhere) (In progress)
 * Guidelines for easy request and confirm communication between multiple decentralised players
 * Event rollback and ping management (In progress)

## Docs and references

## Drawbacks

## Development progress
 * In progress

 
 
Menu system 
===========
## What
 * A well thought out, simple, versatile menu system
 * Adapted from https://github.com/YousicianGit/UnityMenuSystem
 
## Why 

## Features
 * Integrated with StrangeIoC for maximum reusability
 * One script and one prefab for each menu screen
 
## Docs and references

## Drawbacks
 * May not be as versatile as it could be out of the box

## Development progress

 

Input system
=======================================
## What
 * Cross-platform and cross-device input system
 
## Why 
 * Because it's a tricky system to make and every game needs one

## Features
 * IoC, ES integrated (In progress)
 * Customisable key mappings (In progress)
 * Blended mouse/touch functions (In progress)

## Docs and references

## Drawbacks
 * May not be as flexible as could be desired for a certain platform
 
## Development progress
 * In progress
 
## Notes
 * Could replace with TouchScript, not quite the same thing though. Doesn't create a common interface between mouse and touch

 
 
Examples
=======================================
 
## Basic Entity Management

Give an idea about several other functions like serialisation, persistence, event sourcing and projections, 

## Basic Input Management
## Super Simple Network Game

This example is where most of the frameworks in this project are combined in a single simple game. The game is so simple to be uninteresting, but it shows how to connect all the individual frameworks and managers through the IoC framework. It includes network synchronisation, event sourcing, input, entity and menu management as well some examples of the use of Rx and other smaller helper methods and extensions.

## IoC

The examples that come with the StrangeIoC framework

## Rx

A set of Rx examples including those that come with the UniRx implementation
 
 
 
Links to other projects or pages that could be handy
====================================================

## Quality free assets: http://www.procedural-worlds.com/blog/best-free-unity-assets-categorised-mega-list/
 
 
Other included projects
========================
 * Unity Toolbag - https://github.com/nickgravelyn/unitytoolbag 
 * https://github.com/thefuntastic/Unity3d-Finite-State-Machine
 * https://github.com/neuecc/LINQ-to-GameObject-for-Unity
 * Autosaver - from the Asset Store (Updated slightly)
 * https://github.com/bitcake/bitstrap
 * https://bitbucket.org/UnityUIExtensions/unity-ui-extensions
 * LeanTween - from the Asset Store
 * ThreadNinja - from the Asset Store
 
 
 
 
 
 