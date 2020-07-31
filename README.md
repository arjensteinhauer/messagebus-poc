# Setting up your local development environment

## Introduction

This article provides instructions on setting up the local development environment for the Message Bus Proof of Concept (MB) solution. The instructions mentioned in this article assume that you have access to the resources mentioned below.

### Install Visual Studio 2019
Make sure to enable the following features: 
- .NET desktop development
- Azure development
- Node.js development
- .NET Core cross-platform development

[Download](https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.1.201-windows-x64-installer) and install the .NET Core 3.1.201 SDK

### Install Node.js
[Download](https://nodejs.org/dist/v10.15.3/node-v10.15.3-x64.msi) and install Node.js and make sure `node` and `npm` commands are added to your `PATH`.

### Setup RabbitMQ server
The MB solution uses [RabbitMQ](https://www.rabbitmq.com/) as its message broker to communicate between clients and services during local development.

RabbitMQ can be installed in two ways:
1. In a Docker container
2. On your Windows OS as a Windows Service

#### Install on Docker
Assuming you have installed the latest version of docker, you can run a local RabbitMQ instance inside a Docker container.
- Switch to Linux Containers
- Execute `docker run -d --hostname rabbitmq --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management`. This creates a local RabbitMQ instance which can be reached at `rabbitmq://localhost` and uses the default port `5672`. It has the Management Plugin installed by default. *)
- Connect to the Management Plugin using http://localhost:15672  to confirm that the instance is up and running (username: guest, password: guest).
- Change the `ConnectionString:MessageBus` appSetting to `rabbitmq://localhost`.
*) port 15672 can be changed if required.

**Troubleshooting:**  
- Execute the commands as Administrator
- Restart the Docker deamon

#### Install on Windows as a Windows Service

- Download and install [RabbitMQ Server](https://dl.bintray.com/rabbitmq/all/rabbitmq-server/3.7.7/rabbitmq-server-3.7.7.exe)
- When prompted, download and install [Erlang](http://erlang.org/download/otp_win64_21.0.1.exe)
- Search and run **RabbitMQ Command Prompt (sbin dir)** from the Windows start menu
- Run the following command to enable the [RabbitMQ Management Plugin](https://www.rabbitmq.com/management.html): `rabbitmq-plugins enable rabbitmq_management`
- Check if RabbitMQ is running by navigating to http://localhost:15672/ in a browser window (use _guest_ when prompted for a **Username** and **Password**)

### Install Visual Studio 2019 Extensions

**SwitchStartupProject (https://bitbucket.org/thirteen/switchstartupproject/src)**
Using the _PH.sln.startup.json_ file to allow custom startup project groups (i.e. Payment Client & Microservice projects).

**Open Command Line (https://marketplace.visualstudio.com/items?itemName=MadsKristensen.OpenCommandLine)** 
Allows executing .bat files from Solution Explorer

**Add New File (https://marketplace.visualstudio.com/items?itemName=MadsKristensen.AddNewFile)**
Makes it easy to add new files to solution using Shift+F2

**SonarLint (https://marketplace.visualstudio.com/items?itemName=SonarSource.SonarLintforVisualStudio2019)**
Provides helpful messages/warning regarding code style, quality etc.

**CodeMaid (https://marketplace.visualstudio.com/items?itemName=SteveCadwallader.CodeMaid)**
Cleans and structures code on file save.


### Clone the solution from GitHub

- 
- 


### Build and run the solution using Visual Studio 2019

- Open a VS2019 command prompt window
- Open the solution
- Build the solution
