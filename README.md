# BLOGS ENGINE PROJECT

- DB -> SQL SERVER

  - Generate your own DB connection string

  - Run the following commands to setup your example data

- TABLES CREATION

  ```
     CREATE TABLE tblUsers(Id INT PRIMARY KEY IDENTITY(1,1), UserName VARCHAR(20), Role INT)

     CREATE TABLE tblBlogs(Id INT PRIMARY KEY IDENTITY(1,1), Title VARCHAR(70), Content VARCHAR(250), DateOfPublishing DATE, AuthorId INT, Status BIT, FOREIGN KEY (AuthorId) REFERENCES tblUsers(Id))

     CREATE TABLE tblComments(Id INT PRIMARY KEY IDENTITY(1,1), Content VARCHAR(200), AuthorId INT, BlogId INT , FOREIGN KEY (AuthorID) REFERENCES tblUsers(Id), FOREIGN KEY (BlogId) REFERENCES tblBlogs(Id))

     --- Users that match with our InMemoryDatabase for users
     INSERT INTO tblUsers VALUES('JorgeEditor', 0);
     INSERT INTO tblUsers VALUES('JorgeWriter', 1);
     INSERT INTO tblUsers VALUES('JorgePublic', 2);

     --- approved blogs
     INSERT INTO tblBlogs VALUES('Some weird Title', 'This is a content test', '2022/04/04', 2 , 1);
     INSERT INTO tblBlogs VALUES('Some weird Title 2', 'This is a content test 2', '2022/04/04', 2, 1);
     INSERT INTO tblBlogs VALUES('Some weird Title 3', 'This is a content test 3', '2022/04/04', 2, 1);


     ---- Pending blogs
     INSERT INTO tblBlogs VALUES('Some Pending post 1', 'This is a content test for a pending post', '2022/04/04', 2 , 2);
     INSERT INTO tblBlogs VALUES('pending post 2', 'This is a content test 2', '2022/04/03', 2, 2);
     INSERT INTO tblBlogs VALUES('pending post 3', 'This is a content test 3', '2022/04/03', 2, 2);


     ---- Rejected blogs
     INSERT INTO tblBlogs VALUES('Some Pending post 1', 'This is a content test for a rejected post', '2022/04/04', 2 , 0)INSERT INTO tblBlogs VALUES('rejected post 2', 'rejected 2', '2022/04/03', 2, 0);
     INSERT INTO tblBlogs VALUES('rejected post 3', 'rejected 3', '2022/04/03', 2, 0);
  ```

# Application config

- Open the solution using MS Visual Studio
- Replace the following entry on file -> local.settings.json using your own connection string

  -     "SQLConnectionString": "Server=DESKTOP-EK93PDP;Database=db_blogs; User ID=sa;Password=jlBlogs1995;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

- Run the application. Set the AZ function as your startup project

# API TESTS

### TOKENS NEEDED

Id: 1
UserName: JorgeEditor
Role: Editor
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjEiLCJVc2VyTmFtZSI6IkpvcmdlRWRpdG9yIiwiUm9sZSI6IkVkaXRvciJ9.dsvd_XaBQy30aE1IXNttUwqmL1anpgMXzwN60g7Y2lo

Id: 2
UserName: JorgeWriter
Role: Writer
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjIiLCJVc2VyTmFtZSI6IkpvcmdlV3JpdGVyIiwiUm9sZSI6IldyaXRlciJ9.M-hdAg8lb\_\_GIZjW_8es5c96J1yemA6QflY3WPxtGWc

Id: 3
UserName: JorgePublic
Role: Public
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjMiLCJVc2VyTmFtZSI6IkpvcmdlUHVibGljIiwiUm9sZSI6IlB1YmxpYyJ9.5HaeUHEAs22VSw_PuK7T-ysjXvmbyCncCkGHjyUR3Ng

### ENDPOINTS TESTING SAMPLES

- GET ALL BLOGS -- ALL ROLES
  http://localhost:7071/api/blogs

- GET MY OWN BLOGS -- WRITER
  http://localhost:7071/api/blogs?owned=true

- GET PENDING BLOGS -- EDITOR
  http://localhost:7071/api/blogs?pending=true

- POST CREATE COMMENT -- ALL ROLES
  http://localhost:7071/api/blogs/5/comments
  body:

          {
              "content": "This is a comment's content test!!! "
          }

- POST CREATE SAMPLE BLOG -- WRITERS
  http://localhost:7071/api/blogs
  body:

          {
              "title": "This was created from API 2",
              "content": "This is a content test for a created post 2",
              "dateOfPublishing": "2022-04-04T00:00:00"
          }

- PUT UPDATE SAMPLE BLOG -- WRITERS
  http://localhost:7071/api/blogs/6
  body:

          {
              "title": "This blog has been patched 2",
              "content": "This is a content test for a patched blog 2",
              "dateOfPublishing": "2022-04-04T00:00:00"
          }

- PUT APPROVE SAMPLE BLOG -- EDITOR
  http://localhost:7071/api/blogs/4/approve

- DELETE REJECT SAMPLE BLOG -- EDITOR
  http://localhost:7071/api/blogs/4/reject
