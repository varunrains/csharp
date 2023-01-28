1) Time taken by each file scan would be atleast 30 seconds and there is a chance that it would even further increase if there is increase in the file size	
2) CPU utilization for each scan is high using this approach. This would be a problem in the  because there could be lot of users uploading the files at any given time.	
3) The virus definition file needs to be updated if there is any new update to the definition file. Or else any new virus cannot be caught with old .DAT(definition) file.	
4) As the Definition file grows the CPU% grows over the time	
5) McAfee doesnot recommend running more than 2 parallel scans at a time.	https://kc.mcafee.com/corporate/index?page=content&id=KB82340&locale=en_US
6) CPU usage is pretty high because the  scanner runs as a single-threaded process that uses all available system resources to that thread at the time	https://kc.mcafee.com/corporate/index?page=content&id=KB82340&locale=en_US
7) Definition file needs manual update and there is no auto-update capability	https://download.nai.com/products/commonupdater/current/vscandat1000/dat/0000/

