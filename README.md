# Land_Registry
Pre-Conditions (for data prep)

1.	It’s advised to have data backup somewhere for each month before performing test run in order not to lose them in case, we encounter a bug.

2.	All rows under eDS1 Status must be updated accordingly.

3.	If any row under the” HMLR Asset Address” column, displays “Title Close” the “eDS1 Status” must be updated with “BB Task Closed” before performing test run. Otherwise, the data will be overwritten.

4.	If any address mismatched in the event of the automation test run, the test run will insert “Address does not match” into “eDS1 Status” hence the title number will not be discharged whilst test will continue running.

5.	If any row is empty under the” Full Asset Address” column & HMLR Title Number” column, then the “eDS1 Status” must be updated with “BB Task Closed” before performing test run. Otherwise, automation test run will fail.

6.	If any test run fail while discharging, then user should insert “Manual discharge required” into “eDS1 Status” hence user should perform test re-run.

7.	If there is a space or comma in between the house number address, i.e. (number space/comma number ex: 1 3 Street name or 1,3 Street name) the test run will fail. So, before the test run proceed, the “eDS1 Status” must be manually updated for such row with Manual discharge required.

8.	Once we can connect to the live data directly, changing of data path inside automation framework “Config.resx” file required monthly.

