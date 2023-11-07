**1.**
In InitialCreate.migration file I've added CompanyEmployeeTitleConstraint indexed view with clustered unique index
PK_CompanyEmployeeTitleConstraint to validate Company-Title constraint at Database layer.

**2.**
I've used MS SQL Server for speedup developing process, because i am more familiar with it.
If u want - i can rewrite an application with Postgree, but it will take additional time.

**3.**
I've covered with tests only a few thins for saving time.

**4.**
I've used domain notifications for writing in a SystemLog table.
Events could be improved, but in my opinion it is OK for solving our problem.

