# Overview
A Blog app that is a sample of development using ASP.NET MVC and Entity Framework, etc.

# Technology Used/Focused
- ASP.NET MVC (Not ASP.NET Core MVC)
- Entity Framework
  - Code First
  - Automatic migration
    - [Note] Initial data registration is not implemented.
- Authentication, Authorization function by Membership Framework
  - MembershipProvider
  - RoleProvider
  - [Note] For simplicity of implementation of MembershipProvider and RoleProvider, it does not check the matching with the DB value, but implements it by checking with the hard-coded fixed value.  
  Refer to [TodoApp](https://github.com/ExiadiceJinTree/ASP.NetMVC-EntityFrameworkSample.TodoApp) repository for a sample implementation of MembershipProvider and RoleProvider, which checks the matching with DB value and the implementation including user management function.
- Bootstrap
  - Apply the design as a whole
  - Header navigation bar, hamburger button/menu
  - On the screen after signing in, the body element is divided into a left main content area and a right side menu area.  
A category list is displayed in the right side menu area.
  - Each item in the item list as follows is displayed as a Panel component:
    - Each article on the article list screen
    - Each article in the article list on the category detail screen
    - Each comment in the comment list on the article details screen
- Handling of date/time
  - The date/time handled / saved by the server code and DB is UTC date/time.  
    Only when the date/time value of DB is displayed on the screen, it is converted to the date/time of each client locale.
  - Moment.js & ISO8601 format
    - ISO8601 format date/time character string and Moment.js library are used as a method to display the date/time to be displayed on the screen by converting from UTC to client locale.
- Others
  - Input candidates are displayed in the category name input text box on the article registration / edit screen by using the list attribute and datalist tag.
  - In each article item on the article list screen and each article item in the article list on the category details screen, the article body is displayed in a plurality of lines and omitted.
  - On each screen that displays the entire article body, the article body text is properly wrapped and displayed in a block, and whitespaces (line-break, etc.) in the article body itself are displayed correctly.

# Implementation Function
- Function about Article
  - Post(Registeration), Edit, Delete of Article (for Administrators)
  - Display(List, Details) of Article
- Function about Article's Category
  - Display(List, Details) of Category
    - On the Category details screen, a list of articles that belong to the category is displayed.
  - [Note] Categories are created/updated/deleted internally when a article is created/updated/deleted.
- Function about Article's Comment
  - Post(Registeration) of Comment
  - Delete of Comment (for Administrators)

- Authentication, Authorization function
  - SignIn, SignOut
  - [Note] SignUp is not implemented.
  - [Note] Password hashing is not implemented.
- [Note] User and Role management function are not implemented.
