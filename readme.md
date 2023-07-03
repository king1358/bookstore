# ABOUT PROJECT

<h3>This is my personal project to practice for .NET, reactjs, train thinking while working<br>
Project consists of 2 parts, front-end and back-end
</h3>
<b>Front-end</b>: use ReactJS and edit it based on the available template (https://github.com/Akshatjalan/Book-store-Reactjs)<br>
<b>Back-end</b>: use ASP.NET with SQL Server.<br>

# FEATURE

| Name                       |     |
| -------------------------- | --- |
| Get all book               | ✅  |
| Get book by id             | ✅  |
| Add book to cart           | ✅  |
| Update amount book in cart | ✅  |
| Remove book from cart      | ✅  |
| Login                      | ✅  |
| Create account             | ✅  |
| Check out                  | ✅  |
| View Order                 | ✅  |

# CHECKOUT

- Using paypal account sandbox for testing.

# BUILD AND RUN

- Database:

  - Require: SQL Server 16, Microsoft SQL Server Management Studio(or other tool can run file script)
  - Open file Script.sql and run
  - Run file: province.sql,district.sql,ward.sql after run script.sql
  - Create user with username is sa2, password: svcntt and grant permission as dba on BOOKSTORE

- Front-end:

  - Require: Node, back-end is working, VSCode
  - Open folder "Book-store-Reactjs" with VSCode
  - Open terminal and run "npm instal"
  - Run "npm run start"

- Back-end:
  - Require: .NET 6 or higher, Microsoft Visual Studio
  - Open file "ASP_Book_API.sln" in API_Book -> ASP_Book_API
  - Run(Debug) in IIS Express
  - Appsetting:
    - At ConnectionStrings -> database: change <your_server> to server name of database local.
    - At GHN -> token and shopId: you follow the steps given at: https://api.ghn.vn/home/docs/detail
    - At PayPal -> username and password: you follow the steps given at: https://developer.paypal.com/api/rest/authentication/
