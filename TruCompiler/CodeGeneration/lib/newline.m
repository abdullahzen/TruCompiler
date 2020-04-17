%-- Procedure newlineWin 
% Will add a new line
% Link: R15
newlineWin  addi R1,R0,13 %
            putc R1       %
            addi R1,R0,10 %
            putc R1       %
            jr	 R15      %