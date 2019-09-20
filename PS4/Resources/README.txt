Hunter Schmidt - 9/19/19
I think I am going to write a private class to represent cells as self-contained "units" which can store a text formula and/or a value. The Spreadsheet object itslef will store a DependencyGraph
that maps each cell to its dependents and dependees.
I am using the GetCellsToRecalculate method to get a list of a cell and all its dependents to return when setting cell values, and as it throws a
CircularException when a new Formula causes a circular dependency, I am catching that exception to revert changes made to the offending cell,
then re-throwing the exception.

Currently I am using updated versions of Formula and DependencyGraph that pass all of the Grading Tests.
