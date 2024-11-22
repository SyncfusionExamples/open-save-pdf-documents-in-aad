# Open and Save PDF Documents in Azure AD (ASP.NET Core Sample)

This repository provides an example of opening and saving PDF documents in Azure Active Directory (Azure AD) using Syncfusion's PDF Viewer in an ASP.NET Core MVC application.

## Prerequisites

Before you run the sample, ensure you have the following:

- **Visual Studio 2019** or later versions.
- **Azure Active Directory (AAD)** credentials.
- **Azure Storage Account** credentials.
  
## Setting up Azure AD Credentials

You need to configure Azure AD credentials in the sample. In order to connect the Syncfusion PDF Viewer to Azure Blob Storage, update the **Index.cshtml.cs** file with your Azure AD credentials.

### How to update the credentials:

1. Open the `Index.cshtml.cs` file.
2. Update the following fields with your **Azure AD Tenant ID**, **Client ID**, **Client Secret**, and **Blob Storage Endpoint**:

```csharp
string tenantId = "Provide your tenant ID here";
string clientId = "Provide your client ID here";
string clientSecret = "Provide your client secret here";
string blobServiceEndpoint = "https://your-storage-account.blob.core.windows.net";
string containerName = "Provide your container name here";
```

## Deploying and Running the Sample

**Step 1** To deploy and run the Core sample, follow these steps:

**Step 2** Open Visual Studio 2019 or a later version.

**Step 3** Open the solution file (.sln) in Visual Studio.

**Step 4** Build the solution to ensure all the dependencies are resolved.

**Step 5** To run the sample in debugging mode, press F5 or select Debug > Start Debugging from the menu. This will launch the webservice and allow you to debug the application.

**Step 6** To run the sample without debugging, press Ctrl+F5 or select Debug > Start Without Debugging. This will start the webservice without attaching the debugger.

