# JDP.Remediation.Console #

### Summary ###
This sample shows an JDP Remediation - CSOM application that is used to perform FTC cleanup post to solution retraction.

### Applies to ###
-  Office 365 Dedicated (D)
-  SharePoint 2013 on-premises


### Solution ###
Solution | Author(s)
---------|----------
JDP Remediation - CSOM | Ron Tielke (**Microsoft**), Infosys Ltd

### Version history ###
Version  | Date | Comments
---------| -----| --------
1.0  | January 29th 2016 | Initial release

### Disclaimer ###
**THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.**


----------


## Introduction ##
This is a client-side Console application that leverages the v15 CSOM client SDKs to perform operations against a remote SPO-D 2013 farm.
The primary purpose of this tool is to allow a customer to remediate issues identified by the various reports of the MSO Pre-Migration Scan.  Feel free to extend the console to include additional operations as needed.


## Scope ##
This console application is intended to work against SPO-D 2013 (v15) target environments.  
It has not been tested against SPO-vNext 2013 (v16) target environments.


## Authentication ##
The console will prompt the user for an administration account.  
Be sure to specify an account that has Admin permissions on the target SharePoint environment.  This account will be used to generate Authenticated User Context instances that will be leveraged to access the target environments.  

- If you wish to target an **SPO-D (or On-Prem)** farm:
  *  use the **<domain\>\<alias\>** format for the administrator account.
- If you wish to target an **SPO-MT (or vNext)** farm:
  *  use the **<alias\>@<domain\>.com** format for the administrator account.


## Performance ##
The utility will perform its best when using a client machine that has 8GB RAM and a reliable internet connection with ample bandwidth.  

The utility is currently implemented as a single-threaded application.  As such, it will process the specified input file in a serial fashion, one line at a time.  It can take several hours to process large input files (10,000 rows or more). 
 
In these cases, the easiest and fastest way to improve performance is to partition the input file into smaller files and use multiple instances of the utility (running either on the same machine, or on multiple machines) to process each partition.


## Commands ##
### Operation 1-Generate Site Collection Report (PPE-Only) ###
This operation generates a text file containing a list of all site collections found across all web applications in the target farm. 

#### Input ####
- **None**

#### Output ####
- **GenerateSiteCollectionReport-yyyyMMdd_hhmmss.txt**
  * This is NOT a CSV file, so no header row generated
  * Each line of the file will contain a fully-qualified, absolute site collection URL.

#### NOTES ####
- This operation is intended for use only in PPE; use on PROD at your own risk.  For PROD, it is safer to generate the report via the o365 Self-Service Admin Portal.
- This operation leverages the Search Index; as such, it might take up to 20-minutes for a newly-created site to appear in the report. 


### Operation 2-Generate Non-Default Master Page Usage Report ###
This operation reads a list of site collection URLs from an input file and scans each site collection, looking for any web that is using a non-default SP2013 Master Page (i.e., something other than seattle.master) as either its System or Site master page. 

#### Input ####
- **Sites.txt**
  * This is NOT a CSV file, so no header row expected
  * Each line of this file should contain a fully-qualified, absolute site collection URL.
  * Avoid duplicate entries

#### Output ####
- **GenerateNonDefaultMasterPageUsageReport -yyyyMMdd_hhmmss.log**
  * This is the verbose log file of the scan.
  * Success messages of interest:
      * FOUND: System Master Page setting (Prop=MasterUrl) of web {0} is {1}
      * FOUND: Site Master Page setting (Prop=CustomMasterUrl) of web {0} is {1}
  * Informational messages of interest:
      * <span style="background-color: #FFFF00">TBD</span>
  * Error messages of interest:
      * <span style="background-color: #FFFF00">TBD</span>


### Operation 3-Generate Site Column & Content Type Usage Report ###
This operation reads a list of site collection URLs from an input file and scans each site collection, looking for any web or list that is using either a custom Content Type or custom Site Column of interest.  It also looks for local Content Types that have been derived from the custom Content Types of interest.  
This report is helpful in trying to remediate the Missing Content Type and Missing Site Column reports of the Pre-Migration scan.  This report tells you where within each site collection that the content types and site columns are *still in use*.

**General Remediation:**  

- Visit the affected site collection
- If the data associated with these instances is still needed, migrate it to new content types and site columns, or move it into a spreadsheet.
- Delete all instances and empty BOTH recycle bins. 
- Clean up the definitions themselves via a temporary Sandbox Solution that uses the original Feature ID to re-deploy ONLY the affected content types and site columns.  
- Simply activate, then de-activate, the temporary sandbox feature to remove the definitions. 
- If the definitions remain, you still have some instances to delete.

#### Input ####
- **Sites.txt**
  * This is NOT a CSV file, so no header row expected
  * Each line of this file should contain a fully-qualified, absolute site collection URL.
  * In general, this should contain the list of site collections identified in the following files of the Pre-Migration Scan:
      * **PreMT_MissingSiteColumn.csv**
      * **PreMT_MissingContentType.csv**
  * Avoid duplicate entries

- **SiteColumns.csv**
  * The file defines the custom Site Columns of interest.
  * This is a CSV that follows the format and content of the **PreMT_MissingSiteColumn.csv** file of the Pre-Migration scan.  A header row is expected with the following format:
      * SiteColumnID,SiteColumnName
  * SiteColumnID:
      * This column should contain the GUID of the site column,
          * Take this value from the** PreMT_MissingSiteColumn.csv** file
  * SiteColumnName
      * This column should contain the display name of the site column
          * Take this value from the **PreMT_MissingSiteColumn.csv** file

- **ContentTypes.csv**
  * The file defines the custom Content Types of interest.
  * This is a CSV that follows the format and content of the **PreMT_MissingContentType.csv** file of the Pre-Migration scan. A header row is expected with the following format:
      * ContentTypeId,ContentTypeName
  * ContentTypeId
      * This column should contain the ID of the content type
          * Take this value from the **PreMT_MissingContentType.csv** file
  * ContentTypeName
      * This column should contain the display name of the content type
          * Take this value from the **PreMT_MissingContentType.csv** file

#### Output ####
- **GenerateColumnAndTypeUsageReport-yyyyMMdd_hhmmss.log**
  * This is the verbose log file of the scan.
  * Success messages of interest:
      * FOUND: Site Column [{1}] on WEB: {0}
      * FOUND: Site Column [{2}] on LIST [{0}] of WEB: {1}
      * FOUND: Content Type [{1}] on WEB: {0}
      * FOUND: Child Content Type [{2}] of [{1}] on WEB: {0}
      * FOUND: Content Type [{2}] on LIST [{0}] of WEB: {1}
      * FOUND: Child Content Type [{3}] of [{2}] on LIST [{0}] of WEB: {1}
  * Informational messages of interest:
      * <span style="background-color: #FFFF00">TBD</span>
  * Error messages of interest:
      * <span style="background-color: #FFFF00">TBD</span>


### Operation 4-Delete Missing Setup Files ###
This operation reads a list of setup file definitions from an input file and                <span style="color:red;">deletes</span> the associated setup file from the target SharePoint environment.  

This operation is helpful in trying to remediate the Missing Setup Files report of the Pre-Migration Scan.  It attempts to remove all specified setup files from the target SharePoint environment.

#### Input ####
- **PreMT_MissingSetupFile.csv**
  * This is a CSV that follows the format and content of the PreMT_MissingSetupFile.csv file of the Pre-Migration scan. A header row is expected with the following format:
      * ContentDatabase, SetupFileDirName, SetupFileExtension, SetupFileName, SetupFilePath, SiteCollection, UpgradeStatus, WebApplication, WebUrl

#### Output ####
- **DeleteMissingSetupFiles-yyyyMMdd_hhmmss.log**
  * This is the verbose log file of the scan.
  * Success messages of interest:
      * SUCCESS: Deleted File: {0}
  * Informational messages of interest:
      * none
  * Error messages of interest:
      * Error=File Not Found
          * Cause: The specified file or folder does not exist
          * Remediation: none; the file is gone
      * Error=Cannot remove file
          * Cause: The file is likely being used as the default Master Page of the site
          * Remediation: 
              * Use SPD to open the site containing the locked file
              * Configure **seattle.master** to be both default MPs
              * Delete the locked file
      * Error=This item cannot be deleted because it is still referenced by other pages
          * Cause: the file is being used by other pages
          * Remediation: 
              * Visit the site containing the locked file
              * Go to Site Settings and click Manage Content and Structure
                  * Or hack the URL: /_layouts/15/**siteManager**.aspx
              * Generate a References Report for the locked file
              * Remediate all references
              * Delete the locked file
      * Error=The file is checked out for editing
          * Cause: someone has checked out the file for editing
          * Remediation: 
              * Visit the site containing the locked file
              * Undo the check-out
              * Delete the locked file
      * (404) Not Found
          * Cause: : The specified site collection does not exist
          * Remediation: none; the site collection does not exist
      * Cannot contact site at the specified URL
          * Cause: The specified web (subweb, subsite, etc.) does not exist
          * Remediation: none; the web does not exist


### Operation 5-Delete Missing Features ###
This operation reads a list of feature definitions from an input file and deletes the associated feature from the webs and sites of the target SharePoint environment.  

This operation is helpful in trying to remediate the Missing Feature report of the Pre-Migration Scan.  It attempts to remove all specified features from the target SharePoint environment.


#### Input ####
- **PreMT_MissingFeature.csv **
  * This is a CSV that follows the format and content of the **PreMT_MissingFeature.csv** file of the Pre-Migration scan. A header row is expected with the following format:
      * ContentDatabase, FeatureId, FeatureTitle, SiteCollection, Source, UpgradeStatus, WebApplication, WebUrl

#### Output ####
- **DeleteMissingFeatures-yyyyMMdd_hhmmss.log**
  * This is the verbose log file of the scan.
  * Success messages of interest:
      * SUCCESS: Deleted Feature {0} from web {1}
      * SUCCESS: Deleted Feature {0} from site {1}
  * Informational messages of interest:
      * WARNING: feature was not found in the web-scoped features; trying the site-scoped features...
      * WARNING: Could not delete Feature {0}; feature not found
      * WARNING: Could not delete Feature {0}; feature not found in site.Features
      * WARNING: Could not delete Feature {0}; feature not found in web.Features
  * Error messages of interest:
      * (404) Not Found
          * Cause: : The specified site collection does not exist
          * Remediation: none; the site collection does not exist
      * Cannot contact site at the specified URL
          * Cause: The specified web (subweb, subsite, etc.) does not exist
          * Remediation: none; the web does not exist


### Operation 6-Delete Missing Event Receivers ###
This operation reads a list of event receiver definitions from an input file and deletes the associated event receiver from the sites, webs, and lists of the target SharePoint environment. 

This operation is helpful in trying to remediate the Missing Event Receiver report of the Pre-Migration Scan.  It attempts to remove all specified event receivers from the target SharePoint environment.


#### Input ####
- **PreMT_MissingEventReceiver.csv**
  * This is a CSV that follows the format and content of the **PreMT_MissingEventReceiver.csv** file of the Pre-Migration scan. A header row is expected with the following format:
      * Assembly, ContentDatabase, HostId, HostType, SiteCollection, WebApplication, WebUrl

#### Output ####
- **DeleteMissingEventReceivers-yyyyMMdd_hhmmss.log**
  * This is the verbose log file of the scan.
  * Search the log for instances of the following significant entries:
      * SUCCESS: Deleted SITE Event Receiver [{0}] from site {1}
      * SUCCESS: Deleted WEB Event Receiver [{0}] from web {1}
      * SUCCESS: Deleted LIST Event Receiver [{0}] from list [{1}] on web {2}
  * Informational messages of interest:
      * <span style="background-color: #FFFF00">TBD</span>
  * Error messages of interest:
      * (404) Not Found
          * Cause: : The specified site collection does not exist
          * Remediation: none; the site collection does not exist
      * Cannot contact site at the specified URL
          * Cause: The specified web (subweb, subsite, etc.) does not exist
          * Remediation: none; the web does not exist

