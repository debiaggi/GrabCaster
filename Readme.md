# GrabCaster

Author: Nino Crudele (@ninocrudele)  
Blog: http://ninocrudele.me  
Info: http://GrabCaster.io  

GrabCaster is designed to allow end users, developers and IT users to integrate information and data into a variety of applications and systems, simply and easily.
GrabCaster eliminates all existing barriers by providing a new dynamic and easy way to integrate technologies and software.
GrabCaster includes the building blocks developers need to integrate successful, any kind of software and technology, send data across the network and internet, plug-and-play applications, vertical applications and operating systems, everything from track-and-trace to asset tracking, inventory control and system readiness and more, the only limit is our capacity for invention.
GrabCaster connects any kind of technology, custom programs, operating system stacks, it works with any existing line of business applications such as Enterprise Resource Planning (ERP) systems, Warehouse Management Systems (WMS), hardware devices and more specialized vertical software. This flexibility allows it to work seamlessly and in almost all cases automatically with minimal implementation required.
More information at http://grabcaster.io

# Prepare the development environment
Clone the repository.  
Open GrabCaster.sln in Visual Studio and build the solution.  
Execute the cmd script PrepareDevEnvironment.cmd under the Develoment Script solution folder.  
Now the development environment is ready to run in debug mode.  

# Packaging
To prepare a new GrabBaster msi package build the Setup project in the GrabCaster solution.

# Licenses

#### Microsoft Roslyn is used in GrabCaster.Framework.Engine component

Microsoft Roslyn is licensed under the Apache license as described here http://www.apache.org/licenses/
Microsoft Roslyn binaries are linked into the GrabCaster Framework distribution allowed under the license terms found here https://github.com/dotnet/roslyn/blob/master/License.txt.

#### StackExchange.Redis is used in GrabCaster.Framework.Dcp.Redis component

StackExchange.Redis is licensed under the MIT license as described here https://opensource.org/licenses/MIT
StackExchange.Redis binaries are linked into the GrabCaster Framework distribution allowed under the license terms found here https://github.com/StackExchange/StackExchange.Redis/blob/master/LICENSE.
