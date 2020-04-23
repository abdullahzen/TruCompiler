% Read an integer.
% Exit: R1 contains value of integer read.
% Uses: r1, r2, r3, r4.
% Link: r15.
%
getint   add    r1,r0,r0         % n := 0 (result)
         add    r2,r0,r0         % c := 0 (character)
         add    r3,r0,r0         % s := 0 (sign)
getint1  getc   r2               % read c
         ceqi   r4,r2,32         %
         bnz    r4,getint1       % skip blanks
         ceqi   r4,r2,43         %
         bnz    r4,getint2       % branch if c is '+'
         ceqi   r4,r2,45         %
         bz     r4,getint3       % branch if c is not '-'
         addi   r3,r0,1          % s := 1 (number is negative)
getint2  getc   r2               % read c
getint3  ceqi   r4,r2,10         %
         bnz    r4,getint5       % branch if c is \n
         cgei   r4,r2,48         %
         bz     r4,getint4       % c < 0
         clei   r4,r2,57         %
         bz     r4,getint4       % c > 9
         muli   r1,r1,10         % n := 10 * n
         add    r1,r1,r2         % n := n + c
         subi   r1,r1,48         % n := n - '0'
         j      getint2          %
getint4  addi   r2,r0,63         % c := '?'
         putc   r2               % write c
         j      getint           % Try again
getint5  bz     r3,getint6       % branch if s = 0 (number is positive)
         sub    r1,r0,r1         % n := -n
getint6  jr     r15              % return