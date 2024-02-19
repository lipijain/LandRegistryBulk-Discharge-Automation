# Land_Registry
Pre-Condition
1.	It’s advised to have data backup somewhere for each month before performing test run in order not to lose them in case, we encounter a bug.
2.	All rows under eDS1 Status must be updated accordingly.
3.	If any row under” HMLR Asset Address” column, displays “Tittle Close” the “eDS1 Status” must be updated with “BB Task Closed” before performing test run. Otherwise, the data will be overwritten.
4.	If any address mismatch in the event of automation test run, the test run will fail, then the “eDS1 Status” must be manually updated with “Referred” before performing another test run.
