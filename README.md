# VinculacionBackend

## Entities

### Class
* Id(`long`): Unique database identifier.
* Code(`string`): Unique code set by the enterprise to represent a specific class.
* Name(`string`): The class's established name.

### Faculty
* Id(`long`): Unique database identifier.
* Name(`string`): The faculty's established name.

### Hour
* Id(`long`): Unique database identifier.
* Amount(`int`): The amount of hours added.
* SectionProject(`SectionProject`): The associated Section-Project relationship (Hours need to be specified a `Project` and the `Section` during which hours were earned).
* User(`User`): The `User` to whom the hours belong. 

### Major
* Id(`long`): Unique database identifier.
* MajorId(`string`): Unique string given by the enterprise to represent a specific major.
*  Name(`string`): The major's established name.
*  Faculty(`Faculty`): The faculty to which the major belongs.

### Period
* Id(`long`): Unique database identifier.
* Number(`int`): The period number of the stored period (1-4).
* Year(`int`): The year on which the period was created.
* FromDate(`string`): Starting date of the period.
* ToDate(`string`): Date on which the period finalized.
* IsCurrent(`bool`): Flag used by the system to know which period's 'active' at a given moment.

### Project
* Id(`long`): Unique database identifier.
* ProjectId(`string`): Unique string established by the enterprise to represent a specific project.
* Name(`string`): The project's specified name.
* Description(`string`): A brief explanation of the project's requirements, goals, initial budget, etc.
* IsDeleted(`bool`): Flag used to mark deleted Project objects in the database.
* BeneficiarieOrganization(`string`): Name of the organization that the project's destined for.




