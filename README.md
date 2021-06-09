# macrypt
Research C# blockchain originally written for a school project
![image](https://user-images.githubusercontent.com/55022497/121388312-adc48600-c94b-11eb-9ba6-60fa6ef1d0d6.png)

## How to run

### dotnet CLI

To run macrypt with dotnet cli, do
```shell
dotnet run
```
in the root folder.

### Visual Studio

Open the project file and run macrypt.

## Networking and connecting to other nodes

You can interact with a macrypt node via JSON RPC calls.   
For example, to get the latest block you would send a get request like this:   
```shell
curl http://localhost:6475/api/blocks/latest
```   
Which will give you the latest block in JSON format.

To send transactions you would send a post request like this:   
```shell
curl -d '{"from":"alice","to":"bob","amount":1000000000000,"fee":0}' -H "Content-Type: application/json" -X POST http://localhost:6475/api/add
```     
![image](https://user-images.githubusercontent.com/55022497/121389181-6f7b9680-c94c-11eb-9f90-49f7a4701094.png)

### Connecting to other nodes
*SOON*
