﻿**Notes**
1. Create a new database and execute all scripts available in the Sql folder of this project to create necessary tables.
2. Add Connection string named 'xDBInteractionDatabase' in ConnectionStrings.config of Processing application when environment is XP1+, or in ConnectionStrings.config if Standalone environment is there.
3. If we are rebuilding the Reporting database and do not want to export data from this processor, set the 'SC.Interactions.Exporter.EnableXDBDataExport' value to 'false'. By default it is true.
4. Truncate all the tables from the database, if you want to export the xDB data during Reporting database rebuilding.
5. After deployment of code changes, hit the processing service app URL to warm up. On local I observed that untill I warm up the processing service, it was not starting the processing.