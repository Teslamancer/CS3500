Repository for Michael Blum and Hunter Schmidt CS 3500 Fall 2019

This repository is a collection of problem sets designed to create a spreadsheet program in C#.
10/4
(Hunter) Done: Added comments to Help Window. Added Help Item for SEditing Cell Contents.
10/3
(Hunter) Done: Fixed Window Name Updating on "open". Added Help window launch. Added Help Content. Implemented Autosave Tool.
10/2
(Hunter) Done: Made any invalid formula entry display cell as "#INVALID". Made "Open" option loop on invalid file until "cancel" is selected in messagebox.
Fixed Value box. Fixe "Cancel" on closing dialog. Fixed OpenDialog to loop properly.
TODO: Fix Window Name (Doesn't update properly when opening after previous attempts)

(Hunter) Done: Fixed Filename property to set window name on open or save as.
10/1
(Hunter) Done: Added Help Menu Button, set up so backend spreadsheet uses correct validation and normalization functions, and uses "ps6" version. Added logic so File>New button works and opens new windows. 
Also added "Save As" functionality with Dialog.

Plan: value of cells that reference nonexistant ones is set to 0, but should be FormulaError, we need to fix this. Get REGEX for filename working in Save as.
9/30
(Michael) Done: Added a spreadsheet backend to the window - created a current cell text box and a cell contents text box. Right now the only way to change the contents of a cell is to do so through the contents text box.

Plan: Show FormulaErrors better, ability to edit cell contents in the cell, menu functionality

09/30/2019 PS6 - Added Hunter Schmidt to the repo. We plan to get a functional base working, then create two branches for us to work in and develop features independently.