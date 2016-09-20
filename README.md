# VinculacionBackend

## Installation 

To install the project on your computer, simply use `git clone` on your terminal.

If you prefer, you can also just download the compressed project.

After downloading the project, use the package installer to restore the missing packages. After this you're ready to use the program!

## Migrations

To perform a new migration on the existing database you simply need to use the commands
* `Enable-Migrations -Force`
* `Add-Migration MigrationName`
* `Update-Database`

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

### Role
* Id(`long`): Unique database identifier.
* Name(`string`): The role's specified name.

### Section
* Id(`long`): Unique database identifier.
* Code(`string`): Unique string given by the enterprise to represent a specific section.
* Class(`Class`): The class to which the section belongs.
* Period(`Period`): The period during which the section will be available.
* User(`User`): The Professor in charge of the section.

### User
* Id(`long`): Unique database identifier.
* AccountId(`string`): Unique alphanumeric string assigned to represent the user.
* Name(`string`): The user's specified name.
* Password(`string`): The user's account password.
* Major(`Major`): The major to which the user belongs.
* Campus(`string`): String to indicate the campus to which the user assists.
* Email(`string`): The user's email address.
* CreationDate(`DateTime`): The date and time at which the user was added to the system.
* ModificationDate(`DateTime`): The date and time when changes were performed on the user.
* Finiquiteado(`bool`): Flag to indicate if a student's hours have been settled (Has enough hours and has signed off on his worked hours).
* Status(`Status`): Indicates the current state of an account.

### MajorUser
Entity that handles the relationships between Majors and Users
* Id(`long`): Unique database identifier.
* Major(`Major`): Major to which the User belongs.
* User(`User`): User that forms part of a specific major.

### ProjectMajor
Entity that handles the relationship between projects and majors.
* Id(`long`): Unique database identifier.
* Project(`long`): A project restricted to be worked on by members of a specific major.
* Major(`long`): A major that's allowed to work on a given project.

### SectionProject
Entity that handles the relationship between sections and projects.
* Id(`long`): Unique database identifier.
* Section(`Section`): Section that's worked or is working on a specific project.
* Project(`Projects`): Project on which a specific section has worked or is currently working on.
* IsApproved(`bool`): Flag that indicates if a section working on a project has been approved by the enterprise.
* Description(`string`): Brief explanation and listing of the work details.
* Cost(`double`): The amount spent on working on the project by the section.

### SectionUser
Entity that handles the relationships between sections and users.
* Id(`long`): Unique database identifier.
* Section(`Section`): A section to which the user belongs.
* User(`User`): A user that forms part of a specific section.

### UserRole
Entity that handles the roles of a specific user.
* Id(`long`): Unique database identifier.
* User(`User`): user that possesses a specific role.
* Role(`Role`): A role assigned to a specific user.
