# Drama
It's what you get when a bunch of actors try to host a game.

### What is it?
Drama is a server emulator for version 1.12.1 of my favorite online RPG.

### Why make it?
Lots of reasons!
- To learn how to write a scalable sockets application.
- To learn how to work with the actor system programming paradigm.
- To learn what goes into the development of an online game server.

So yeah, mostly lots of learning.

### Who wrote it?
So far, just me, Soulson. If you think the project sounds interesting, go ahead
and fork the repository and play around with it. I'll even accept pull requests
if our interests are aligned. And then I'll have to rewrite this paragraph.

### Gimme some more details.
##### Platform?
Drama is written in C# and targets .NET Core 1.1.

That way, it can be executed natively on Windows, Linux, or Mac with zero
platform-specific code. C# is also a high-productivity language, allowing fast
development.

##### Framework?
Drama is written on top of the Orleans framework.

This framework provides many benefits. First of all, Orleans is an *actor
system*. Actor systems are known for their resilience and their sidestepping
of many common pitfalls in highly parallel and distributed programming.
Additionally, using Orleans gives Drama clustering support basically for free.

##### Paradigm?
Actor model, with a healthy side of OOP and a touch of SOA where it makes
sense. IRandomService and ITimeService are good examples of that.

##### License?
Most of Drama is licensed under the GNU Affero General Public License v3.

###### You said "most" of Drama is AGPLv3. What isn't?
There are a few files that have been used from other open source projects, and
I have chosen to maintain the original licenses of these files. Currently, the
following come to mind:
- The Drama.Host.Providers namespace is adapted from OrleansContrib.MongoDB
and is licensed under the X11 MIT license.

Whenever a different license is in use, it will be included in the folder of
files to which it applies.

## Architecture
A Drama installation is made up of a three types of mandatory programs, plus
an optional one.

##### Mandatory
- Host
- Auth Gateway
- Shard Gateway

##### Optional
- Database Server

> "Wait, the database is optional?" you're probably thinking. Yes. Orleans
> provides an interface called Storage Providers, and all persistence in Drama
> is handled through this interface. Currently, a file system implementation is
> provided. A MongoDB implementation is half-written, and a RDBMS provider
> could be implemented in the future. The filesystem implementation makes it
> easy for anyone to clone, compile, and run the server, without any
> complicated setup routine before getting started.

#### Host
Drama.Host is the Orleans Silo Host that contains most of the logic involved in
a Drama server installation. You can run one or multiple Host applications in a
single installation. Each Host must be able to communicate with all other
Hosts.

The Host application is where your realms "live." This relationship is many-to-
many. You could have one Host running four realms, or four Hosts running one
realm, or thirteen Hosts running six realms. The state and logic of each realm
will be distributed among each Host.

Advanced installations can use a heterogenous multi-Host configuration, where
one set of Hosts is tasked with authentication-related Grains and another set
is configured to host shard-related Grains. This would slightly improve data
locality, but several shard Hosts would be required to saturate a single auth
Host.

#### Auth Gateway
Drama.Auth.Gateway is a TCP server that accepts the "realm server" protocol.
While the Host application handles the actual requests, the Auth Gateway
provides an endpoint for the client to connect to when making these requests.
Like the relationship between Hosts and realms, the relationship between Auth
Gateways and Hosts is many-to-many. You could have one Auth Gateway and four
Hosts, or four Auth Gateways and one Host, or three Auth Gateways and twelve
Hosts. Each Auth Gateway must be able to connect to at least one Host.

#### Shard Gateway
Drama.Shard.Gateway is a TCP server that accepts the "world server" protocol.
While the virtual realms live inside the Hosts, the Shard Gateway provides an
endpoint for the client to connect to when communicating with a realm. Shard
Gateways are also many-to-many with Hosts.

> These gateways are handy for mitigating threats such as DDOS. Gateways are
> relatively lightweight applications, and nonsense packets will not be
> forwarded by Gateways to Hosts.

## Getting Started
Getting started with Drama is easy!

1. Grab a copy of Visual Studio Community 2017 if you don't already have it.
This software is available gratis from Microsoft for development of open-source
projects.
2. When installing Visual Studio, enable the "Cross-Platform Development with
.NET Core" workload.
3. Clone the repository from GitHub.
4. Open Drama.sln in Visual Studio. In Solution Properties, indicate that Drama
has multiple startup projects, and configure Drama.Host, Drama.Auth.Gateway,
and Drama.Shard.Gateway to all start when debugging.
5. Start. When the Host finishes loading, press enter on both Gateway apps to
start them.
6. In the Auth Gateway window, create yourself an account and shard, like this:
> account.create AccountName Password
> shard.create Drama 127.0.0.1 8085 PvP
7. Open a client and log in!
