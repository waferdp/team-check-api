# TeamCheck
Team Performance Check evaluates Team Performance according to the four stages of team development.
The test is composed of 25 questions that are averaged across the team.

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
