﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDP.Remediation.Console
{
    /// <summary>
    /// This class holds all constants used by the program.  No code
    /// </summary>
    public class Constants
    {
        public const string Quote = "\"";
        public const string EscapedQuote = "\"\"";
        public static char[] CharactersThatMustBeQuoted = { ',', '"', '\n' };
        public static readonly string CsvDelimeter = ",";
        public static readonly string NotApplicable = "N/A";
        public static readonly long ExceptionFileSizeinKb = 4096; //4 MB
        public static readonly bool Logging = true;
        public static readonly string Exception = "Exception.csv";
        public static readonly string TraceLogFileSuffix = "TraceLog";
        /// <summary>
        // Excel/CSV Cell CharacterLimit. According to Microsoft's documentation: 
        // https://support.office.com/en-us/article/Excel-specifications-and-limits-1672b34d-7043-467e-8e27-269d656771c3
        // Excel cannot read more than 32767 characters in a single cell
        // Total number of characters that a cell can contain: 32,767 characters
        /// </summary>
        public static readonly int CharacterLimitForCsvCell = 32758;
        public static readonly long OutputFileSizeinKb = 8192; //8 MB


        private static readonly string portalWebAppRoot = "https://contoso.sharePoint.com";
        public static readonly string PortalRootSiteUrl = portalWebAppRoot + "/";

        public static readonly string PropertyBagInheritMaster = "__InheritMasterUrl";
        public static readonly string PropertyBagInheritCustomMaster = "__InheritCustomMasterUrl";

        public static readonly string MASTERPAGE_CONTENT_TYPE = "0x01010500B45822D4B60B7B40A2BFCC0995839404";

        // these are simple Text files and are NOT expected to contain a Header row
        public static readonly string UsageReport_SitesInputFileName = "sites.txt";

        // these are CSV files and are expected to contain a Header row (as described)
        public static readonly string UsageReport_ContentTypesInputFileName = "contentTypes.csv";
        public static readonly string UsageReport_SiteColumnsInputFileName = "siteColumns.csv";

        public static readonly string MissingEventReceiversInputFileName = "PreMT_MissingEventReceiver.csv";
        public static readonly string MissingEventReceiversInputFileHeaders = "Assembly,ContentDatabase,EventName,HostId,HostType,SiteCollection,WebApplication,WebUrl";
        public static readonly int MissingEventReceiversInputFileHeaderCount = 8;

        public static readonly string MissingFeaturesInputFileName = "PreMT_MissingFeature.csv";
        public static readonly string MissingFeaturesInputFileHeaders = "ContentDatabase,FeatureId,FeatureTitle,SiteCollection,Source,UpgradeStatus,WebApplication,WebUrl";
        public static readonly int MissingFeaturesInputFileHeaderCount = 8;

        public static readonly string MissingSetupFilesInputFileName = "PreMT_MissingSetupFile.csv";
        public static readonly string MissingSetupFilesInputFileHeaders = "ContentDatabase,SetupFileDirName,SetupFileExtension,SetupFileName,SetupFilePath,SiteCollection,UpgradeStatus,WebApplication,WebUrl";
        public static readonly int MissingSetupFilesInputFileHeaderCount = 9;

        // OOB Site-level Features IDs of interest...
        public static readonly Guid SharePointEnterpriseFeatures_SiteFeatureID = new Guid("8581a8a7-cf16-4770-ac54-260265ddb0b2");
        public static readonly Guid SharePointStandardFeatures_SiteFeatureID = new Guid("b21b090c-c796-4b0f-ac0f-7ef1659c20ae");
        public static readonly Guid SearchWebPartsAndTemplates_SiteFeatureID = new Guid("9c0834e1-ba47-4d49-812b-7d4fb6fea211");
        public static readonly Guid SitePolicy_SiteFeatureID = new Guid("2fcd5f8a-26b7-4a6a-9755-918566dba90a");

        public static readonly string[] LegacyMasterPageFilesToDelete = 
        { 
            "Sample.master"
        };

    }
}
