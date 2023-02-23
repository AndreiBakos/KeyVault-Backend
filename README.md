 # What is KeyVault?

KeyVault is a web application where you can: 
- Create an account.
- Authenticate into your account 
  - Authentication is made with a JWT so your session is secure :)
- When logged in you can create your own secrets
- You can also create groups
- Invite other users on your groups
- Other users can invite you as well in their groups
- If you are the group creator you can manage users
- You and your colleagues can add secrets in shared groups.
- You can remove you group

# how to set up the backend

For this app I used `MySql Server` so something else won't work.

- MySql Server: `https://dev.mysql.com/downloads/mysql/`
- MySql Workbench: `https://dev.mysql.com/downloads/mysql/`

### Setting up the database:
1) Open MySql workbench and create schema `KeyVault`
1) Grab the `TableCreatorQuery.sql` file and paste the code in the query window and run the query
1) You will see that you have the tables created

### Setting up the secret keys:
1) Open the project in your editor and go to `appsettings.json`
1) There you will see that the connection string and keys are not set
1) Grab the connection string from mysql and put it in the appsettings
1) For the `Key` and `Iv` you will need to put 2 random generated guids
1) For the `JWT` -> `Secret` you'll need to put a random string

That's it :)

P.S: Check out the backend code as well (you'll need it): `https://github.com/AndreiBakos/KeyVault-Frontend`
