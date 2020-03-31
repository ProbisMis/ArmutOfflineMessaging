## ArmutOfflineMessaging ##

### Installation ###

* type `git clone https://github.com/ProbisMis/clevel-blog-app.git projectname` to clone the repository 
* type `cd projectname`
* Open Solution .sln with Visual Studio 2019 or any other 
* Open Package-Manager Console and type `Update-Database`. This should initiliase tables. 
* To Verify go to  View - SQL Server Object Explorer - Connect to mssqllocaldb under  `Databases - System Databases - master - Tables`
* Run ISS Express.

(TEST) Open Postman Set Enviroment variable {{url}} to your localhost and port eg. http://localhost:13420
 * type `cd projectname`
 * open up postman .json file

### Presentation ###
API Routes List
![routes](/images/routes1.png)

Register
![register](/images/register.png)

A sample register error, user already exist
![register](/images/registererror1.png)

Login
![Login](/images/login.png)

A sample response for login will return user data
![user data](/images/loginresult.png)

A sample response for login error
![user data](/images/loginerror1.png)

Add Friend
![addfriend](/images/addfriend.png)

All Friends
![friends](/images/allfriends.png)

Start Chat 
![Chat1](/images/allfriends.png)

Send Message 
![Chat2](/images/message1.png)


Send Message - swap users
![Chat3](/images/message2.png)


Get Message - If reciever recieves messages via GetChat function. Read status is updated. According to whose reading.
![Chat4](/images/message3.png)


### Include ###

* [NLog] for logging 
* [EntityFramework] for database query
* [Postman API Endpoints] for testing the application 

### Features ###

* Basic Authentication (registration, login)
* User can add friends.
* User can block friends.
* Two users can start a chat.
* One user may block other, blocked user messages will not be recieved.
* Messages have read time. User can see if the reciever read it yet.
