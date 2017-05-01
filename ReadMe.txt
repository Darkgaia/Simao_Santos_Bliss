Developed using asp.net core on visual studio 2015.
Requires MailKit. To install open visual studio, go to tools, open NuGet Package Manager and select Package manager console. Pass the following into the console to install: "Install-Package MailKit"
Recommended using a RESTClient to test the API since theres no frontend setup.

Microsoft SQL Server was used to store data.
Table Question:
question_id, int not null. Primary Key (Is Identity)
question, nchar(100), not null
image_url, nchar(100), not null
thumb_url, nchar(100), not null
timePosted, datatime, not null

Table Choice:
id, int, not null, Primary Key (Is Identity)
question_id, int, not null, Foreign Key
choice, nchar(20), not null
votes, int, not null

To connect visual studio to the database, open visual studio, go to View and select Server Explorer. On Data Connections, right click and select add Connection. On Server Name add your Server Name used on SQL Server Managment Studio. Then select the database in the combobox below.
The name I gave to the database was Blissapp.
After connecting, open the property of the database on visual studio, and copy connection string to conString which is located in QuestionController.cs.
How to use:

/api/ServerStatus

Get- Returns status of the server ("OK" or "NOT OK" only)
Put- Change the status of the server ("OK" or "NOT OK" only). Body must go only "OK" or "NOT OK". Header must be set to application/json

/api/Question

GET- Returns, if available, 10 questions if no query setting is added.

Query parameters: (Allows none or several, execept question_filter, which must be by itself as a parameter)
limit : how many you wish to receive
offset : from were you wish to start to receive
filter : filters both question and choice. Must have a question to then move on to choice
question_filter: only accepts "english". Returns 10 questions, if available

POST- Adds a question to the list

Example of Body for POST-
{
  "image_url": "https://dummyimage.com/600x400/000/fff.png&text=question+1+image+(600x400)",
  "thumb_url": "https://dummyimage.com/120x120/000/fff.png&text=question+1+image+(120x120)",
  "question": "Favourite programming language?",
  "choices": [
    {
      "choice": "Swift",
      "votes": 1
    },
    {
      "choice": "Python",
      "votes": 0
    },
    {
      "choice": "Objective-C",
      "votes": 0
    },
    {
      "choice": "Ruby",
      "votes": 0
    }
  ]
}



/api/Question/{id}
GET - Returns, if exists, a specific question 
PUT - Update the whole question

Example of Body for PUT-
{
  "image_url": "https://dummyimage.com/600x400/000/fff.png&text=question+1+image+(600x400)",
  "thumb_url": "https://dummyimage.com/120x120/000/fff.png&text=question+1+image+(120x120)",
  "question": "Favourite programming language?",
  "choices": [
    {
      "choice": "Swift",
      "votes": 1
    },
    {
      "choice": "Python",
      "votes": 0
    },
    {
      "choice": "Objective-C",
      "votes": 0
    },
    {
      "choice": "Ruby",
      "votes": 0
    }
  ]
}

/api/Share
POST - Sends an email with the content passed on query

Query Parameters(Requires Both)
dest: email to who we wish to share the link
content: message sent to user.(link in this case)
