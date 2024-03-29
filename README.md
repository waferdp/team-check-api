# TeamCheck
Team Performance Check evaluates Team Performance according to the four stages of team development.
The test is composed of 25 questions that are averaged across the team.

## Setup
### .NET

Install .NET 6  

### MongoDB
Install MongoDB  
*or* 
1. Install Docker desktop
2. Pull docker MongoDB image
```
docker pull mongo
```
3. Run image and expose standard ports
```
docker run -d -p 27017-27019:27017-27019 --name mongodb mongo
```

## How to build, run etc

### Restoring all packages
```
dotnet restore
```
### Building project
```
dotnet build Api/
```
### Run project
```
dotnet run --project Api
```
### Run database (docker container)
```
docker start mongodb
```

### Publish project
```
dotnet publish project ./Api
```


## Calculating score
Use the api/team-assessment endpoint to get a team average over a period.
TeamCheck currently only supports one team.

Total score:
| Score   | Group's stage |
| ------: | ------------- |
| >85     | Performing    |
| 70 - 84 | Norming       |
| <70     | Forming* or Storming** |

\* The group is in Forming Stage if members are tentative, polite and somewhat passive.  
\*\* The group is in Storming Stage if members are disagreeing with each other or the leader.  
Read more about the stages on [Wikipedia](https://en.wikipedia.org/wiki/Tuckman%27s_stages_of_group_development)

## Available online
### Backend Swagger documentation (this repo)
https://jacob-team-check.azurewebsites.net

### Frontend (team-check-web)
http://waferdp.github.io/team-check-web
