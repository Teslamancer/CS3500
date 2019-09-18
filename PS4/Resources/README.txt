Hunter Schmidt - 9/16/19
I think I am going to write a private class to represent cells as self-contained "units" which can store a text formula and/or a value. The Spreadsheet object itslef will store a DependencyGraph
that maps each cell to its dependents and dependees.

Currently I am using updated versions of Formula and DependencyGraph that pass all of the Grading Tests.
