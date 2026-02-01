# BSLTours .NET API Deployment Guide

This guide explains how to deploy the BSLTours .NET Core API on Windows hosting (IONOS) and connect it to the frontend.

## Prerequisites

- Windows hosting with IIS
- .NET 8.0 hosting bundle installed on the server
- Access to IONOS hosting control panel
- Visual Studio 2022 (for development and publishing)

## Deployment Steps

### 1. Prepare the API for Deployment

1. Open the solution in Visual Studio 2022
2. Right-click on the BSLTours.API project and select "Publish"
3. Choose "Folder" as the publish target
4. Set the path to a local folder (e.g., `C:\Temp\BSLToursAPI`)
5. Click "Publish" to generate the deployment files

### 2. Configure IIS on IONOS Hosting

1. Log in to your IONOS control panel
2. Go to your hosting package management
3. Create a new subdomain for your API (e.g., `api.travelnip.com`)
4. Configure IIS:
   - Create a new Application Pool:
     - Set the .NET CLR version to "No Managed Code"
     - Set the pipeline mode to "Integrated"
   - Create a new website or application:
     - Set the physical path to the directory where you'll upload the published files
     - Assign it to the Application Pool you created
     - Bind it to your API subdomain

### 3. Upload the Deployment Files

1. Connect to your server using FTP or SFTP
2. Navigate to the directory you specified when setting up the IIS website
3. Upload all files from your local publish folder to this directory

### 4. Configure Web.config

Make sure your web.config file has the correct settings:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\BSLTours.API.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" />
      <cors enabled="true" />
    </system.webServer>
  </location>
</configuration>
```

### 5. Connect the Frontend to the API

1. Update the `API_BASE_URL` in the frontend's `frontend-modifications/netApiClient.ts`:

```typescript
export const API_BASE_URL = "https://api.apitravelnip.com";
```

2. Update your frontend build process to use this new API client instead of the current one

### 6. Troubleshooting

If you encounter issues with the API after deployment:

1. Check the IIS logs in the `logs` directory on your server
2. Ensure the Application Pool identity has sufficient permissions to access the files
3. Verify that the .NET 8.0 Hosting Bundle is installed properly
4. Make sure CORS is properly configured if the frontend and API are on different domains

### 7. SSL Configuration

For secure HTTPS connections:

1. Obtain an SSL certificate for your API subdomain from IONOS control panel
2. Install the certificate in IIS
3. Ensure your API is accessible via HTTPS

## Maintenance

- Keep your .NET runtime updated on the server
- Regularly back up your deployment files
- Monitor IIS logs for any errors or performance issues